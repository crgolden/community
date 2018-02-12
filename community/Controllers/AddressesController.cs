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
    public class AddressesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AddressesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return Json(await _context.Addresses.OrderBy(x => x.Street).Select(x => new AddressViewModel(x)).ToArrayAsync());
        }

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

        // TODO [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> Create([FromBody, Bind("Id,Street,Street2,City,State,ZipCode,Latitude,Longitude,Home,UserId")] AddressViewModel model)
        {
            if (!ModelState.IsValid) return Json(model);
            var address = await model.ExisitingMatch(_context);
            if (address != null)
            {
                model.Id = address.Id;
            }
            else
            {
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
                await _context.Addresses.AddAsync(address);

                if (await _context.SaveChangesAsync() <= 0) return Json(model);

                model.Id = address.Id;
            }
            return Json(model);
        }

        // TODO [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> Edit(Guid id, [FromBody, Bind("Id,Street,Street2,City,State,ZipCode,Latitude,Longitude,Home,UserId")] AddressViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid) return Json(model);

            try
            {
                var address = await _context.Addresses.FindAsync(model.Id);

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
                if (!AddressExists(model.Id)) return NotFound();

                throw;
            }
            return Json(model);
        }

        // TODO [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var address = await _context.Addresses.FindAsync(id);
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return Json(true);
        }

        private bool AddressExists(Guid id)
        {
            return _context.Addresses.Any(e => e.Id == id);
        }
    }
}
