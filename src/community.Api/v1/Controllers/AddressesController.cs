using System;
using System.Linq;
using System.Threading.Tasks;
using community.Api.v1.Extensions;
using community.Api.v1.ViewModels;
using community.Core.Interfaces;
using community.Core.Models;
using community.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace community.Api.v1.Controllers
{
    //[EnableCors("<YourCorsPolicyName>")]
    [Produces("application/json")]
    [Route("api/v1/[controller]/[action]")]
    public class AddressesController : Controller
    {
        private readonly IManager<Address> _addressManager;

        private const string Bind = "Id,Street,Street2,City,State,ZipCode,Latitude,Longitude,Home,UserId";

        public AddressesController(IManager<Address> addressManager)
        {
            _addressManager = addressManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var addresses = await _addressManager.Index();

            return Ok(addresses.Select(x => new AddressViewModel(x)).ToArray());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details([FromRoute] Guid? id)
        {
            try
            {
                var address = await _addressManager.Details(id);
                if (address == null) return NotFound();
                return Ok(new AddressViewModel(address));
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
        }

        // TODO [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> Create([FromBody] [Bind(Bind)] AddressViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var address = await _addressManager.Create(new Address
                {
                    Street = model.Street.Trim(),
                    Street2 = model.Street2?.Trim(),
                    City = model.City.Trim(),
                    State = model.State.Trim(),
                    ZipCode = model.ZipCode.Trim(),
                    UserId = model.UserId
                });
                model.Id = address.Id;
                return Ok(model);
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
        }

        // TODO [ValidateAntiForgeryToken]
        [HttpPut("{id}")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> Edit([FromRoute] Guid? id, [FromBody] [Bind(Bind)] AddressViewModel model)
        {
            if (id == null || id.Value != model.Id || !ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                await _addressManager.Edit(new Address
                {
                    Id = model.Id,
                    Street = model.Street.Trim(),
                    Street2 = model.Street2?.Trim(),
                    City = model.City.Trim(),
                    State = model.State.Trim(),
                    ZipCode = model.ZipCode.Trim(),
                    UserId = model.UserId
                });
                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
        }

        // TODO [ValidateAntiForgeryToken]
        [HttpDelete("{id}")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> Delete([FromRoute] Guid? id)
        {
            try
            {
                await _addressManager.Delete(id);
                return Ok();
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
        }
    }
}
