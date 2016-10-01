using System.Collections.Generic;
using System.Linq;
using Community.Data;
using Community.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Community.API.Controllers
{
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Event> Get()
        {
            return _context.Events;
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetEvent")]
        public IActionResult Get(int id)
        {
            var _event = _context.Events.FirstOrDefault(e => e.Id == id);
            if (_event == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return new ObjectResult(_event);
            }
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Event _event)
        {
            if (_event == null)
            {
                return BadRequest();
            }
            _context.Events.Add(_event);
            _context.SaveChanges();
            return CreatedAtRoute("GetEvent", new { id = _event.Id }, _event);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Event _event)
        {
            if (_event == null || _event.Id != id)
            {
                return BadRequest();
            }
            _context.Events.Update(_event);
            _context.SaveChanges();
            return new NoContentResult();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var _event = _context.Events.FirstOrDefault(e => e.Id == id);
            if (_event.Equals(null))
            {
                return NotFound();
            }
            _context.Events.Remove(_event);
            _context.SaveChanges();
            return new StatusCodeResult(200);
        }
    }
}
