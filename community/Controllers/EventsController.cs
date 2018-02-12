using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using community.Data;
using community.Extensions;
using community.Models;
using Microsoft.AspNetCore.Authorization;

namespace community.Controllers
{
    [Route("[controller]/[action]")]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return Json(await _context.Events.OrderBy(x => x.Name).Select(x => new EventViewModel(x)).ToArrayAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var @event = await _context.Events.Select(x => new EventViewModel(x)).SingleOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }
            return Json(@event);
        }

        // TODO [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> Create([FromBody, Bind("Id,Name,Details,Date,UserId,AddressId,Street,Street2,City,State,ZipCode")] EventViewModel model)
        {
            if (!ModelState.IsValid) return Json(model);

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
                await _context.Addresses.AddAsync(address);
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
            await _context.Events.AddAsync(@event);

            if (await _context.SaveChangesAsync() <= 0) return Json(model);

            model.Id = @event.Id;
            model.AddressId = @event.AddressId;
            return Json(model);
        }

        // TODO [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> Edit(Guid id, [FromBody, Bind("Id,Name,Details,Date,UserId,AddressId")] EventViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid) return Json(model);

            try
            {
                var @event = await _context.Events.FindAsync(model.Id);

                @event.AddressId = model.AddressId;
                @event.Date = model.Date;
                @event.Details = model.Details;
                @event.Name = model.Name;

                _context.Update(@event);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(model.Id)) return NotFound();

                throw;
            }
            return Json(model);
        }

        // TODO [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return Json(true);
        }

        private bool EventExists(Guid id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
