//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using AssetTrackApi.Data;
//using AssetTrackApi.Models;

//namespace AssetTrackApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class SubLocationsController : ControllerBase
//    {
//        private readonly AssetTrackContext _context;

//        public SubLocationsController(AssetTrackContext context)
//        {
//            _context = context;
//        }

//        // GET: api/SubLocations
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<SubLocation>>> GetSubLocations()
//        {
//            return await _context.SubLocations.ToListAsync();
//        }

//        // GET: api/SubLocations/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<SubLocation>> GetSubLocation(int id)
//        {
//            var subLocation = await _context.SubLocations.FindAsync(id);

//            if (subLocation == null)
//            {
//                return NotFound();
//            }

//            return subLocation;
//        }

//        // PUT: api/SubLocations/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutSubLocation(int id, SubLocation subLocation)
//        {
//            if (id != subLocation.SubLocationId)
//            {
//                return BadRequest();
//            }

//            _context.Entry(subLocation).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!SubLocationExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // POST: api/SubLocations
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<SubLocation>> PostSubLocation(SubLocation subLocation)
//        {
//            _context.SubLocations.Add(subLocation);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetSubLocation), new { id = subLocation.SubLocationId }, subLocation);
//        }

//        // DELETE: api/SubLocations/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteSubLocation(int id)
//        {
//            var subLocation = await _context.SubLocations.FindAsync(id);
//            if (subLocation == null)
//            {
//                return NotFound();
//            }

//            _context.SubLocations.Remove(subLocation);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool SubLocationExists(int id)
//        {
//            return _context.SubLocations.Any(e => e.SubLocationId == id);
//        }
//    }
//}
