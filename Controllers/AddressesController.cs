using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SperanzaPizzaApi.Data;
using SperanzaPizzaApi.Data.DTO.Addresses;
using SperanzaPizzaApi.Models;

namespace SperanzaPizzaApi.Controllers
{
    [ApiController]
    [Route ("webapi/address")]
    public class AddressesController: Controller
    {
        private readonly dbPizzaContext _context;

        public AddressesController (dbPizzaContext context) {
            _context = context;
        }
        
        
        [HttpPost]
        public async Task<ActionResult<DmAddress>> PostDmAddresses(DmAddress dmAddresses)
        {
            _context.DmAddresses.Add(dmAddresses);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDmAddresses", new { id = dmAddresses.Id }, dmAddresses);
        }


        [HttpGet("all/{linkString?}")]
        public async Task<ActionResult<IEnumerable<DmAddress>>> GetAllDmAddresses (string linkString = null) {
            IQueryable<DmAddress> finalContext = _context.DmAddresses;

            if (!string.IsNullOrWhiteSpace (linkString)) {
                foreach (var es in (linkString ?? "").Split (";")) {
                    finalContext = finalContext.Include (es);
                }
            }

            return await finalContext.ToListAsync().ConfigureAwait (false);
        }

        [HttpGet("{id}/{linkString?}")]
        public async Task<ActionResult<DmAddress>> GetDmAddresses(int id, string linkString = null)
        {
            IQueryable<DmAddress> finalContext = _context.DmAddresses;

            if (!string.IsNullOrWhiteSpace (linkString)) {
                foreach (var es in (linkString ?? "").Split (";")) {
                    finalContext = finalContext.Include (es);
                }
            }

            var dmAddresses = await finalContext.FirstOrDefaultAsync (e => e.Id == id);

            if (dmAddresses == null)
            {
                return NotFound();
            }

            return dmAddresses;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutDmAddresses(int id, DmAddress dmAddresses)
        {
            if (id != dmAddresses.Id)
            {
                return BadRequest();
            }

            _context.Entry(dmAddresses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DmAddressesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDmAddresses(int id)
        {
            var dmAddresses = await _context.DmAddresses.FindAsync(id);
            if (dmAddresses == null)
            {
                return NotFound();
            }

            _context.DmAddresses.Remove(dmAddresses);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        // EXPOSE_dm_Addresses_GetByStreetId
        [HttpGet("GetByStreetId")]
        public async Task<ActionResult<List<SperanzaPizzaApi.Models.SPToCoreContext.EXPOSE_dm_Addresses_GetByStreetIdResult>>> PostDmAddressesGetByStreetId([FromQuery]GetByStreetIdParams parameters)
        {
            using (SperanzaPizzaApi.Models.SPToCoreContext db = new SperanzaPizzaApi.Models.SPToCoreContext()) {
                return await db.EXPOSE_dm_Addresses_GetByStreetIdAsync(parameters.streetid, parameters.query);
                
            }
        }
        private bool DmAddressesExists (int id)
        { 
            return _context.DmAddresses.Any (e => e.Id == id);
        }         
    }
}