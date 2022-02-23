using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SperanzaPizzaApi.Data;
using SperanzaPizzaApi.Data.DTO.ProductCategories;
using SperanzaPizzaApi.Models;

namespace SperanzaPizzaApi.Controllers
{
    [ApiController]
    [Route ("webapi/category")]
    public class ProductCategoriesController : Controller
    {
        private readonly dbPizzaContext _context;

        public ProductCategoriesController (dbPizzaContext context) {
            _context = context;
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDmProductCategories(int id)
        {
            var dmProductCategories = await _context.DmProductCategories.FindAsync(id);
            if (dmProductCategories == null)
            {
                return NotFound();
            }

            _context.DmProductCategories.Remove(dmProductCategories);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // EXPOSE_dm_ProductCategories_GetAll

        [HttpGet("getAll")]
        public async Task<ActionResult<List<GetProductCategoriesResponse>>> PostDmProductCategoriesGetAll()
        {
            using (SperanzaPizzaApi.Models.SPToCoreContext db = new SperanzaPizzaApi.Models.SPToCoreContext()) {
                var queryResult = await db.EXPOSE_dm_ProductCategories_GetAllAsync();
                var result = queryResult.Select(x => new GetProductCategoriesResponse()
                {
                    categoryId = x.categoryId,
                    categoryName = x.categoryName,
                    sizes = JsonConvert.DeserializeObject<List<SizeResponse>>(x.sizes)
                });
                return result.ToList();
            }
        }
        
        private bool DmProductCategoriesExists (int id)
        {
                return _context.DmProductCategories.Any (e => e.Id == id);
        }         
    }
}