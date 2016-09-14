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
        [HttpGet("{id}")]
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
            if (_event.Id == 0)
            {
                _context.Events.Add(_event);
                _context.SaveChanges();
                return new ObjectResult(_event);
            }
            else
            {
                var original = _context.Events.FirstOrDefault(e => e.Id == _event.Id);
                original.Title = _event.Title;
                original.Creator = _event.Creator;
                _context.SaveChanges();
                return new ObjectResult(original);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var _event = _context.Events.FirstOrDefault(e => e.Id == id);
            _context.Events.Remove(_event);
            _context.SaveChanges();
            return new StatusCodeResult(200);
        }
    }
}
