using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using community.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace community.Core.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly IList<Claim> _claims;
        private readonly IdentityUser _user;
        private string _secretKey;
        private string _issuer;
        private string _audience;

        public TokenGenerator(IConfiguration configuration, IList<Claim> claims,
            IdentityUser user)
        {
            _configuration = configuration;
            _claims = claims;
            _user = user;
        }

        public string GenerateToken()
        {
            AddClaims();
            SetTokenOptions();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_secretKey);
            var signingKey = new SymmetricSecurityKey(secretKeyBytes);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);
            var payload = new JwtPayload(_issuer, _audience, _claims, null, DateTime.Now.AddMinutes(30));
            var jwtToken = new JwtSecurityToken(header, payload);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return token;
        }

        private void AddClaims()
        {
            _claims.Add(new Claim(JwtRegisteredClaimNames.Sub, _user.Email));
            _claims.Add(new Claim(JwtRegisteredClaimNames.Jti, $"{Guid.NewGuid()}"));
            _claims.Add(new Claim(JwtRegisteredClaimNames.Iat, $"{DateTime.Now}", ClaimValueTypes.Integer64));
        }

        private void SetTokenOptions()
        {
            var tokenOptions = _configuration.GetSection("TokenProviderOptions").GetChildren().ToArray();
            _secretKey = tokenOptions.Single(x => x.Key.Equals("SecretKey")).Value;
            _issuer = tokenOptions.Single(x => x.Key.Equals("Issuer")).Value;
            _audience = tokenOptions.Single(x => x.Key.Equals("Audience")).Value;
        }
    }
}
