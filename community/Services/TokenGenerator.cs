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

namespace community.Services
{
    public class TokenGenerator
    {
        private readonly UserManager<User> _userManager;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenGenerator(IConfiguration configuration,
            UserManager<User> userManager)
        {
            _userManager = userManager;

            var tokenOptions = configuration.GetSection("TokenProviderOptions").GetChildren().ToArray();

            _secretKey = tokenOptions.Single(x => x.Key.Equals("SecretKey")).Value;
            _issuer = tokenOptions.Single(x => x.Key.Equals("Issuer")).Value;
            _audience = tokenOptions.Single(x => x.Key.Equals("Audience")).Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<string> GenerateToken(User user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, $"{Guid.NewGuid()}"));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, $"{DateTime.Now}", ClaimValueTypes.Integer64));

            var secretKeyBytes = Encoding.UTF8.GetBytes(_secretKey);
            var signingKey = new SymmetricSecurityKey(secretKeyBytes);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(_issuer, _audience, claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return token;
        }
    }
}
