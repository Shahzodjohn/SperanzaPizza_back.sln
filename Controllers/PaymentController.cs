using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SperanzaPizzaApi.Data;
using SperanzaPizzaApi.Data.DTO.Orders;
using SperanzaPizzaApi.Models;
using SperanzaPizzaApi.Services.Addresses;
using System;
using SperanzaPizzaApi.Services.Payments;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using SperanzaPizzaApi.Infrastructure.Filters;

namespace SperanzaPizzaApi.Controllers
{
    [ApiController]
    [Route ("webapi/payment")]
    public class PaymentController : Controller
    {
        private readonly dbPizzaContext _context;
        private readonly PaymentService _paymentService;

        public PaymentController (dbPizzaContext context, PaymentService paymentService) {
            _context = context;
            _paymentService = paymentService;
        }
        
        [HttpGet("paymentStatus")]
        public async Task GetOrderPaymentStatus()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using WebSocket webSocket = await 
                                HttpContext.WebSockets.AcceptWebSocketAsync();
                await _paymentService.CheckPaymentStatus(HttpContext, webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
        
        //save payment from PayU. Платёжная система будет непрерывно отправлять запрос о статусе платежа пока не получит от нас статускод OK
        [HttpPost("savePayment")]
        [ServiceFilter(typeof(PayuFilter))]
        public async Task<IActionResult> SaveOrderPayment([FromBody] dynamic request)
        {
            try
            {
                var isSaved = await _paymentService.SavePaymentFromPayU(request.ToString());
                return isSaved? Ok() : NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
        
        private bool DmOrdersExists (int id)
        {
            return _context.DmOrders.Any (e => e.Id == id);
        }
    }
}
