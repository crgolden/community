using System;
using System.Linq;
using System.Threading.Tasks;
using community.Api.v1.ViewModels;
using community.Core.Interfaces;
using community.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace community.Api.v1.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]/[action]")]
    public class EventsController : Controller
    {
        private readonly IManager<Event> _eventManager;
        private readonly IManager<Address> _addressManager;

        private const string Bind = "Id,Name,Details,Date,UserId,AddressId,Street,Street2,City,State,ZipCode";

        public EventsController(IManager<Event> eventManager, IManager<Address> addressManager)
        {
            _eventManager = eventManager;
            _addressManager = addressManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var events = await _eventManager.Index();

            return Ok(events.Select(x => new EventViewModel(x)).ToArray());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details([FromRoute] Guid? id)
        {
            try
            {
                var @event = await _eventManager.Details(id);
                if (@event == null) return NotFound();
                return Ok(new EventViewModel(@event));
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
        }

        // TODO [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> Create([FromBody] [Bind(Bind)] EventViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var address = await _addressManager.Create(new Address
                {
                    Street = model.Street.Trim(),
                    Street2 = model.Street2.Trim(),
                    City = model.City.Trim(),
                    State = model.State.Trim(),
                    ZipCode = model.ZipCode.Trim(),
                    UserId = model.UserId
                });
                var @event = await _eventManager.Create(new Event
                {
                    Name = model.Name.Trim(),
                    Date = model.Date,
                    Details = model.Details.Trim(),
                    UserId = model.UserId,
                    AddressId = address.Id
                });
                model.Id = @event.Id;
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
        public async Task<IActionResult> Edit([FromRoute] Guid? id, [FromBody] [Bind(Bind)] EventViewModel model)
        {
            if (id == null || id.Value != model.Id || !ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                await _eventManager.Edit(new Event
                {
                    Id = model.Id,
                    Name = model.Name.Trim(),
                    Date = model.Date,
                    Details = model.Details.Trim(),
                    AddressId = model.AddressId,
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
                await _eventManager.Delete(id);
                return Ok();
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
        }
    }
}
