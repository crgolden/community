using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using community.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace community.Controllers
{
    public class TokenController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private const string BadToken = "Could not create token";

        public TokenController(IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            var tokenOptions = configuration.GetSection("TokenProviderOptions").GetChildren().ToList();

            _secretKey = tokenOptions.Single(x => x.Key.Equals("SecretKey")).Value;
            _issuer = tokenOptions.Single(x => x.Key.Equals("Issuer")).Value;
            _audience = tokenOptions.Single(x => x.Key.Equals("Audience")).Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GenerateToken(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return BadRequest(BadToken);

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest(BadToken);

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                return BadRequest(BadToken);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, $"{Guid.NewGuid()}")
            };
            var secretKeyByets = Encoding.UTF8.GetBytes(_secretKey);
            var signingKey = new SymmetricSecurityKey(secretKeyByets);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(_issuer, _audience, claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return Ok(new {token});
        }
    }
}