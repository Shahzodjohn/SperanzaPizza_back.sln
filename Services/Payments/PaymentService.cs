using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SperanzaPizzaApi.Data.Common;
using SperanzaPizzaApi.Data.DTO.Payments;
using SperanzaPizzaApi.Infrastructure.Helpers;
using SperanzaPizzaApi.Models;

namespace SperanzaPizzaApi.Services.Payments
{
    public class PaymentService
    {
        private HttpClient _httpClient;
        private PayuSecretsHelperModel _config;
        public PaymentService(HttpClient client, PayuSecretsHelperModel config)
        {
            _httpClient = client;
            _config = config;
        }
        
        //create payment from Payu System
        public async Task<InitPaymentOrderResponse> InitPaymentRequest(int orderId)
        {
            //get Order Info:
            await using var db = new SperanzaPizzaApi.Models.SPToCoreContext();
            //get order full info from database:
            var query = await db.EXPOSE_dm_Orders_GetInfoForPaymentAsync(orderId);
            InitPaymentOrderRequest request = new InitPaymentOrderRequest()
            {
                notifyUrl = "http://lab500.nc-one.com:5002/webapi/payment/savePayment",
                merchantPosId = _config.MerchantPosId,
                customerIp = "127.0.0.1",
                currencyCode = "PLN",
                description = "Food order payment",
                buyer = (JsonConvert.DeserializeObject<List<BuyerModel>>(query[0].buyer))?[0],
                products = JsonConvert.DeserializeObject<List<ProductItemModel>>(query[0].products),
                totalAmount = (int)(query[0].totalAmount ?? 0),
                extOrderId = query[0].orderId.ToString(),
                continueUrl = $"http://lab500.nc-one.com:3000/#/ordersuccess/{orderId}"
            };
            
            //create paymentOrderRequest:
            var url = "https://secure.snd.payu.com/api/v2_1/orders"; //поменять когда подключите основной платёжный шлюз
            var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")); //ACCEPT header
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", PaymentToken.Token);
            using var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8,
                "application/json");
            
            var result = await _httpClient.PostAsync(url, content);
            var returnValue = result.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<InitPaymentOrderResponse>(returnValue);
            
            if (response.status.statusCode != "SUCCESS" || response.extOrderId != orderId.ToString())
                throw new Exception("Не удалось создать платёж");
            // create OrderPayment:
            await db.DmOrderPayments.AddAsync(new DmOrderPayment
            {
                OrderId = (int)orderId,
                ExtOrderId = response.orderId,
                CreatedDate = DateTime.Now
            });
            await db.SaveChangesAsync();
            return response;
        }

        public async Task<bool> SavePaymentFromPayU(string inputResult)
        {
            await using var db = new SperanzaPizzaApi.Models.SPToCoreContext();
            var payUPaymentModel = JsonConvert.DeserializeObject<SavePaymentRequest>(inputResult);
            var orderId = int.Parse(payUPaymentModel.order.extOrderId); // Id заказа из нашей бд приходит от системы как параметр extOrderId;
            var status = db.DmPaymentStatuses.FirstOrDefault(x => x.StatusName == payUPaymentModel.order.status);
            
            if (orderId == 0)
                throw new  HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) {ReasonPhrase = $"Заказ с Id = {orderId} отсутсвтует"});
            
            var orderPayment = db.DmOrderPayments.FirstOrDefault(x => x.ExtOrderId == payUPaymentModel.order.orderId && x.OrderId == orderId);
            
            
            if (orderPayment == null || status == null)
                return false;
            orderPayment.StatusId = status.Id;
            db.DmOrderPayments.Update(orderPayment);
            await db.SaveChangesAsync();
            return true;
        }
        
        private void GetPaymentStatus(int orderId, ref GetPaymentStatusResponse paymentStatus)
        {
            using SperanzaPizzaApi.Models.SPToCoreContext db = new SperanzaPizzaApi.Models.SPToCoreContext();
            var orderPayment =
                db.DmOrderPayments.FirstOrDefault(x => x.OrderId == orderId && x.StatusId != null);
            if (orderPayment == null) return;
            var status = db.DmPaymentStatuses.FirstOrDefault(x => x.Id == orderPayment.StatusId);
            paymentStatus.paymentStatus = status?.StatusName;
        }
        
        public async Task CheckPaymentStatus(HttpContext httpContext, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                var request = Encoding.UTF8.GetString(buffer);
                var response = Array.Empty<byte>();
                if (Int32.TryParse(request, out int orderId) || orderId > 0)
                {
                    var paymentStatus = new GetPaymentStatusResponse
                    {
                        orderId = orderId
                    };
                    GetPaymentStatus(orderId, ref paymentStatus);
                    response = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(paymentStatus));
                }
                else
                {
                    response = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new GetPaymentStatusResponse()
                    {
                        paymentStatus = null,
                        orderId = 0
                    }));
                }
                await webSocket.SendAsync(new ArraySegment<byte>(response, 0, response.Length), result.MessageType, true, CancellationToken.None);
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);        
        }
    }
}