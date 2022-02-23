using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SperanzaPizzaApi.Data;
using SperanzaPizzaApi.Data.DTO.Cities;
using SperanzaPizzaApi.Infrastructure.Filters;
using SperanzaPizzaApi.Models;

namespace SperanzaPizzaApi.Controllers
{
    [ApiController]
    [Route ("webapi/city")]
    public class CitiesController: Controller
    {
        private readonly dbPizzaContext _context;

        public CitiesController (dbPizzaContext context) {
            _context = context;
        }
        
        // [ServiceFilter(typeof(PayuFilter))]
        [HttpGet("getAll")]
        public async Task<ActionResult<List<SperanzaPizzaApi.Models.SPToCoreContext.EXPOSE_dm_Cities_GetAllResult>>> PostDmCitiesGetAll([FromQuery]GetAllParams parameters)
        {
            using (SperanzaPizzaApi.Models.SPToCoreContext db = new SperanzaPizzaApi.Models.SPToCoreContext()) {
                return await db.EXPOSE_dm_Cities_GetAllAsync (parameters.query);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDmCities(int id)
        {
            var dmCities = await _context.DmCities.FindAsync(id);
            if (dmCities == null)
            {
                return NotFound();
            }
            _context.DmCities.Remove(dmCities);
            await _context.SaveChangesAsync();
            return NoContent();
        }    
        
        
        private bool DmCitiesExists (int id)
        {
                return _context.DmCities.Any (e => e.Id == id);
        }         
    }
}