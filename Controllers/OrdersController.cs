using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SperanzaPizzaApi.Data;
using SperanzaPizzaApi.Data.DTO.Orders;
using SperanzaPizzaApi.Services.Addresses;
using System;
using System.Web.Http;
using SperanzaPizzaApi.Infrastructure.Filters;
using OrderService = SperanzaPizzaApi.Services.Orders.OrderService;

namespace SperanzaPizzaApi.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route ("webapi/order")]
    public class OrdersController : Controller
    {
        private readonly dbPizzaContext _context;
        private readonly OrderService _orderService;
        
     //   private readonly IPaymentService _payment;

        public OrdersController (dbPizzaContext context, OrderService orderService) {
            _context = context;
            _orderService = orderService;
            //    _payment = paymentService;
        }
        
        [Microsoft.AspNetCore.Mvc.HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDmOrders(int id)
        {
            var dmOrders = await _context.DmOrders.FindAsync(id);
            if (dmOrders == null)
            {
                return NotFound();
            }
            _context.DmOrders.Remove(dmOrders);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // EXPOSE_dm_Orders_CreateNewOrder

        [Microsoft.AspNetCore.Mvc.HttpPost("createOrder")]
        public async Task<ActionResult> PostDmOrdersCreateNewOrder([Microsoft.AspNetCore.Mvc.FromBody]CreateNewOrderRequest parameters)
        {
            try
            {
                var response = await _orderService.CreateOrder(parameters);
                return Ok(response);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode((int)ex.Response.StatusCode, ex.Response.ReasonPhrase);
            }
        }

        // [TypeFilter(typeof(CustomAuthorizeFilter))]
        //[Authorize]
        [Microsoft.AspNetCore.Mvc.HttpGet("getAll")]
        public async Task<IActionResult> DmOrdersGetAll()
        {
            try
            {
                var response = await _orderService.GetAll();
                return Ok(response);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode((int)ex.Response.StatusCode, ex.Response.ReasonPhrase);
            }
        }
        
        [Microsoft.AspNetCore.Mvc.HttpGet("getById")]
        public async Task<ActionResult<GetOrderResponse>> DmOrdersGetById( int orderId)
        {
            try
            {
                var response = await _orderService.GetById(orderId);
                return Ok(response);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode((int)ex.Response.StatusCode, ex.Response.ReasonPhrase);
            }
        }
        
        private bool DmOrdersExists (int id)
        {
            return _context.DmOrders.Any (e => e.Id == id);
        }
    }
}
