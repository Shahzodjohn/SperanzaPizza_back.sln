using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SperanzaPizzaApi.Data;
using SperanzaPizzaApi.Data.DTO.Products;
using SperanzaPizzaApi.Models;
using SperanzaPizzaApi.Services.Products;

namespace SperanzaPizzaApi.Controllers
{
    [ApiController]
    [Route ("webapi/product")]
    public class ProductsController :Controller
    {
        private readonly dbPizzaContext _context;
        private readonly ProductService _productService;
        public ProductsController (dbPizzaContext context, ProductService productService) {
            _context = context;
            _productService = productService;
        } 
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDmProducts(int id)
        {
            var dmProducts = await _context.DmProducts.FindAsync(id);
            if (dmProducts == null)
            {
                return NotFound();
            }

            _context.DmProducts.Remove(dmProducts);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // EXPOSE_dm_Products_GetByCategory

        [HttpGet("getByCategory")]
        public async Task<ActionResult<List<GetProductsResponse>>> PostDmProductsGetByCategory([FromQuery]GetByCategoryParams parameters)
        {
            var result = await _productService.GetProductsByCategory(parameters);
            return Ok(result);
        }
        
        // EXPOSE_dm_Products_GetAll
        [HttpGet("getAll")]
        public async Task<ActionResult<GetProductsResponse>> PostDmProductsGetAll([FromQuery]GetAllParams parameters)
        {
            var result = await _productService.GetAllProducts(parameters);
            return Ok(result);
        }

        private bool DmProductsExists (int id)
        {
            return _context.DmProducts.Any (e => e.Id == id);
        }            
    }
}