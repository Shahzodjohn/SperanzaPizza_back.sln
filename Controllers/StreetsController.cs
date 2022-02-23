using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SperanzaPizzaApi.Data;
using SperanzaPizzaApi.Data.DTO.Streets;
using SperanzaPizzaApi.Models;

namespace SperanzaPizzaApi.Controllers
{
    [ApiController]
    [Route ("webapi/street")]
    public class StreetsController: Controller
    {
        private readonly dbPizzaContext _context;

        public StreetsController (dbPizzaContext context) {
            _context = context;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDmStreets(int id)
        {
            var dmStreets = await _context.DmStreets.FindAsync(id);
            if (dmStreets == null)
            {
                return NotFound();
            }

            _context.DmStreets.Remove(dmStreets);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // EXPOSE_dm_Streets_GetByCityId

        [HttpGet("getByCityId")]
        public async Task<ActionResult<List<SperanzaPizzaApi.Models.SPToCoreContext.EXPOSE_dm_Streets_GetByCityIdResult>>> PostDmStreetsGetByCityId([FromQuery]GetByCityIdParams parameters)
        {
            using (SperanzaPizzaApi.Models.SPToCoreContext db = new SperanzaPizzaApi.Models.SPToCoreContext()) {
                return await db.EXPOSE_dm_Streets_GetByCityIdAsync(parameters.cityId, parameters.query );
            }
        }
        
        private bool DmStreetsExists (int id)
        {
            return _context.DmStreets.Any (e => e.Id == id);
        }  
    }
}
