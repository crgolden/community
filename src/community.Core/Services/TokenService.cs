using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using community.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace community.Core.Services
{
    public class TokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenService(IConfiguration configuration, UserManager<User> userManager)
        {
            _userManager = userManager;

            var tokenOptions = configuration.GetSection("TokenProviderOptions").GetChildren().ToArray();

            _secretKey = tokenOptions.Single(x => x.Key.Equals("SecretKey")).Value;
            _issuer = tokenOptions.Single(x => x.Key.Equals("Issuer")).Value;
            _audience = tokenOptions.Single(x => x.Key.Equals("Audience")).Value;
        }

        public async Task<string> GenerateToken(User user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, $"{Guid.NewGuid()}"));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, $"{DateTime.Now}", ClaimValueTypes.Integer64));

            var secretKeyBytes = Encoding.UTF8.GetBytes(_secretKey);
            var signingKey = new SymmetricSecurityKey(secretKeyBytes);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);
            var payload = new JwtPayload(_issuer, _audience, claims, null, DateTime.Now.AddMinutes(30));
            var jwtToken = new JwtSecurityToken(header, payload);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return token;
        }
    }
}
