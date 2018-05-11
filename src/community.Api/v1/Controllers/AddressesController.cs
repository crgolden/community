using System;
using System.Linq;
using System.Threading.Tasks;
using community.Api.v1.Extensions;
using community.Api.v1.ViewModels;
using community.Core.Models;
using community.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace community.Api.v1.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]/[action]")]
    public class AddressesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private const string Bind = "Id,Street,Street2,City,State,ZipCode,Latitude,Longitude,Home,UserId";

        public AddressesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Addresses
                .OrderBy(x => x.Street).Select(x => new AddressViewModel(x)).ToArrayAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details([FromRoute] Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var address = await _context.Addresses
                .Select(x => new AddressViewModel(x)).SingleOrDefaultAsync(m => m.Id == id);

            if (address == null)
            {
                return NotFound();
            }
            return Ok(address);
        }

        // TODO [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> Create([FromBody] [Bind(Bind)] AddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var address = await model.ExisitingMatch(_context);
            if (address != null)
            {
                return Json(address.Id);
            }
            address = new Address
            {
                Id = Guid.NewGuid(),
                Street = model.Street,
                Street2 = model.Street2,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                UserId = model.UserId
            };
            _context.Addresses.Add(address);

            await _context.SaveChangesAsync();

            model.Id = address.Id;
            return Ok(model);
        }

        // TODO [ValidateAntiForgeryToken]
        [HttpPut("{id}")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> Edit([FromRoute] Guid? id, [FromBody] [Bind(Bind)] AddressViewModel model)
        {
            if (id == null || id.Value != model.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var address = await _context.Addresses.FindAsync(id.Value);

                address.Street = model.Street;
                address.Street2 = model.Street2;
                address.City = model.City;
                address.State = model.State;
                address.ZipCode = model.ZipCode;

                _context.Update(address);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Addresses.AnyAsync(x => x.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // TODO [ValidateAntiForgeryToken]
        [HttpDelete("{id}")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> Delete([FromRoute] Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var address = await _context.Addresses.FindAsync(id.Value);
            if (address == null)
            {
                return NotFound();
            }
            _context.Addresses.Remove(address);

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
