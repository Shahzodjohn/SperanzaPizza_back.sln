using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using SperanzaPizzaApi.Data.DTO.Orders;
using SperanzaPizzaApi.Data.DTO.Payments;
using SperanzaPizzaApi.Data.DTO.Products;
using SperanzaPizzaApi.Models;
using SperanzaPizzaApi.Services.Payments;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SperanzaPizzaApi.Services.Orders
{
    public class OrderService
    {
        private PaymentService _paymentService;

        public OrderService(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        public async Task<CreateNewOrderResponse> CreateOrder(CreateNewOrderRequest parameters)
        {
            if (parameters.products == null || !parameters.products.Any())
                throw new  HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {ReasonPhrase = "Продукты отсутствуют, невозможно оформить заказ" });
            
            if (parameters.productCost == 0)
                throw new  HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {ReasonPhrase = "Сумма заказа не может равняться 0" });
            
            if (parameters.deliver && parameters.addressId == 0)
                throw new  HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {ReasonPhrase = "Адрес не выбран" });


            var products = JsonSerializer.Serialize(parameters.products);
            TimeSpan.TryParse(parameters.cookingTime, out var cookingTime);

            var orderIdentifier = ""; 
            int? orderId = 0;
            using (SperanzaPizzaApi.Models.SPToCoreContext db = new SperanzaPizzaApi.Models.SPToCoreContext()) {
                db.EXPOSE_dm_Orders_CreateNewOrder(
                    ref orderId,
                    products,
                    parameters.clientName,
                    parameters.clientPhone,
                    parameters.clientComment,
                    parameters.productCost,
                    parameters.deliveryCost,
                    parameters.deliver,
                    parameters.addressId,
                    parameters.postcode,
                    parameters.flatNumber,
                    parameters.gateCode,
                    parameters.asSoonasPossible,
                    cookingTime == new TimeSpan() ? null : cookingTime,
                    parameters.hasInvoice,
                    parameters.clientCompanyName,
                    parameters.nip,
                    parameters.cashPayment,
                    parameters.employeeComment,
                    ref orderIdentifier);
            }
            if (orderId is 0 or null || orderIdentifier == string.Empty)
               throw new  HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                                                {ReasonPhrase = "Не удалось создать заказ"});
            CreateNewOrderResponse response = new CreateNewOrderResponse {
                orderId = (int)orderId,
                isCashPayment = parameters.cashPayment,
                orderIdentifier = orderIdentifier,
                paymentRedirectUrl = null
            };
            if (parameters.cashPayment) return response; 
            
            //creating online payment:
            var paymentRequest = await _paymentService.InitPaymentRequest((int)orderId);
            response.paymentRedirectUrl = paymentRequest.redirectUri;
            return response;
        }

        public async Task<List<GetOrderResponse>> GetAll()
        {
            using (SperanzaPizzaApi.Models.SPToCoreContext db = new SperanzaPizzaApi.Models.SPToCoreContext()) {
                var queryResult =  await db.EXPOSE_dm_Orders_GetAllAsync();
                if (queryResult.Count == 0)
                    throw new  HttpResponseException(new HttpResponseMessage(HttpStatusCode.NoContent)
                    {
                        ReasonPhrase = "Список заказов пуст"
                    });
                var result = queryResult.Select(x => new GetOrderResponse()
                {
                    orderId = x.orderId,
                    orderIdentifier = x.orderIdentifier,
                    orderStatusId = x.orderStatusId,
                    createdDate = x.createdDate,
                    clientComment = x.clientComment,
                    clientCommentIsRead = x.clientCommentIsRead,
                    products = JsonConvert.DeserializeObject<List<OrderProductItem>>(x.products??"") 
                });
                return result.ToList();
            }
        }
        
        public async Task<GetOrderItemResponse> GetById(int orderId)
        {
            using (SperanzaPizzaApi.Models.SPToCoreContext db = new SperanzaPizzaApi.Models.SPToCoreContext()) {
                var result =  await db.EXPOSE_dm_Orders_GetByIdAsync(orderId);
                if (result.Count == 0) 
                    throw new  HttpResponseException(new HttpResponseMessage(HttpStatusCode.NoContent) {ReasonPhrase = $"Заказ с Id = {orderId}  отсутсвтует"});
                var order = result.FirstOrDefault();
                return new GetOrderItemResponse()
                {
                    orderId = order.orderId,
                    orderIdentifier = order.orderIdentifier,
                    productCost = order.productCost,
                    deliveryCost = order.deliveryCost,
                    hasDelivery = order.hasDelivery,
                    cashPayment = order.cashPayment,
                    houseNumber = order.houseNumber,
                    lattitude = order.lattitude,
                    longitude = order.longitude,
                    products = JsonConvert.DeserializeObject<List<OrderProductItem>>(order.products)
                };
            }
        }
    }
}