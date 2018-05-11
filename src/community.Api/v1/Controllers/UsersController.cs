using System.Linq;
using System.Threading.Tasks;
using community.Api.v1.ViewModels;
using community.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace community.Api.v1.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]/[action]")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.User
                .OrderBy(x => x.FirstName).Select(x => new UserViewModel(x)).ToArrayAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details([FromRoute] string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var user = await _context.User
                .Select(x => new UserViewModel(x)).SingleOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
