using System.Linq;
using System.Threading.Tasks;
using community.Api.v1.ViewModels;
using community.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace community.Api.v1.Controllers
{
    //[EnableCors("<YourCorsPolicyName>")]
    [Produces("application/json")]
    [Route("api/v1/[controller]/[action]")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _userManager.Users
                .OrderBy(x => x.FirstName)
                .Select(x => new UserViewModel(x))
                .ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(new UserViewModel(user));
        }
    }
}
