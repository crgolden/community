using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using community.Data;
using community.Models;
using Microsoft.AspNetCore.Authorization;

namespace community.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Events
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return Json(await _context.Events.Select(x => new EventViewModel(x)).ToArrayAsync());
        }

        // GET: Events/Details/5
        [AllowAnonymous]
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

        [HttpPost]
        //TODO [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody, Bind("Id,Name,Details,Date,UserId,AddressId,Street,Street2,City,State,ZipCode")] EventViewModel model)
        {
            if (!ModelState.IsValid) return Json(model);

            var address = await _context.Addresses.FirstOrDefaultAsync(x => x.Street.Equals(model.Street, StringComparison.InvariantCultureIgnoreCase) &&
                                                                            x.City.Equals(model.City, StringComparison.InvariantCultureIgnoreCase) &&
                                                                            x.State.Equals(model.State, StringComparison.InvariantCultureIgnoreCase) &&
                                                                            x.ZipCode.Equals(model.ZipCode, StringComparison.InvariantCultureIgnoreCase));
            if (address == null)
            {
                address = new Address
                {
                    Street = model.Street,
                    Street2 = model.Street2,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode
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

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.SingleOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", @event.AddressId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", @event.UserId);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //TODO [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Details,Date,UserId,AddressId")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", @event.AddressId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", @event.UserId);
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(m => m.Address)
                .Include(m => m.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        //TODO [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var @event = await _context.Events.SingleOrDefaultAsync(m => m.Id == id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(Guid id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
