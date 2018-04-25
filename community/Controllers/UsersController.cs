using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using community.Data;
using community.Models;

namespace community.Controllers
{
    [Route("[controller]/[action]")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return Json(await _context.User
                .OrderBy(x => x.FirstName).Select(x => new UserViewModel(x)).ToArrayAsync());
        }

        public async Task<IActionResult> Details(string id)
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
            return Json(user);
        }
    }
}
