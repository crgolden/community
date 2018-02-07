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
    public class AddressesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AddressesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Addresses
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return Json(await _context.Addresses.Select(x => new AddressViewModel(x)).ToArrayAsync());
        }

        // GET: Addresses/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var address = await _context.Addresses.Select(x => new AddressViewModel(x)).SingleOrDefaultAsync(m => m.Id == id);
            if (address == null)
            {
                return NotFound();
            }
            return Json(address);
        }

        //// GET: Addresses/Create
        //public IActionResult Create()
        //{
        //    ViewData["UserId"] = new SelectList(_context.User, "Id", "Id");
        //    return View();
        //}

        // POST: Addresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        //TODO [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody, Bind("Id,Street,Street2,City,State,ZipCode,Latitude,Longitude,Home,UserId")] Address address)
        {
            if (!ModelState.IsValid) return Json(address);

            address.Id = Guid.NewGuid();
            _context.Add(address);
            await _context.SaveChangesAsync();

            return Json(new AddressViewModel(address));
        }

        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses.SingleOrDefaultAsync(m => m.Id == id);
            if (address == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", address.UserId);
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //TODO [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Street,Street2,City,State,ZipCode,Latitude,Longitude,Home,UserId")] Address address)
        {
            if (id != address.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.Id))
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
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", address.UserId);
            return View(address);
        }

        // GET: Addresses/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .Include(a => a.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        //TODO [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var address = await _context.Addresses.SingleOrDefaultAsync(m => m.Id == id);
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressExists(Guid id)
        {
            return _context.Addresses.Any(e => e.Id == id);
        }
    }
}
