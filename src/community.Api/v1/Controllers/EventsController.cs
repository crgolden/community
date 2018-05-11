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
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private const string Bind = "Id,Name,Details,Date,UserId,AddressId,Street,Street2,City,State,ZipCode";

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Events
                .OrderBy(x => x.Name).Select(x => new EventViewModel(x)).ToArrayAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details([FromRoute] Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var @event = await _context.Events
                .Select(x => new EventViewModel(x)).SingleOrDefaultAsync(m => m.Id == id.Value);

            if (@event == null)
            {
                return NotFound();
            }
            return Ok(@event);
        }

        // TODO [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> Create([FromBody] [Bind(Bind)] EventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var address = await model.ExisitingMatch(_context);
            if (address == null)
            {
                address = new Address
                {
                    Street = model.Street,
                    Street2 = model.Street2,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode,
                    UserId = model.UserId
                };
                _context.Addresses.Add(address);
            }
            var @event = new Event
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Date = model.Date,
                Details = model.Details,
                UserId = model.UserId,
                Address = address
            };
            _context.Events.Add(@event);

            await _context.SaveChangesAsync();

            model.Id = @event.Id;
            return Ok(model);
        }

        // TODO [ValidateAntiForgeryToken]
        [HttpPut("{id}")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> Edit([FromRoute] Guid? id, [FromBody] [Bind(Bind)] EventViewModel model)
        {
            if (id == null || id.Value != model.Id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var @event = await _context.Events.FindAsync(id.Value);

                @event.AddressId = model.AddressId;
                @event.Date = model.Date;
                @event.Details = model.Details;
                @event.Name = model.Name;

                _context.Update(@event);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Events.AnyAsync(x => x.Id == id.Value))
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
            var @event = await _context.Events.FindAsync(id.Value);
            if (@event == null)
            {
                return NotFound();
            }
            _context.Events.Remove(@event);

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
