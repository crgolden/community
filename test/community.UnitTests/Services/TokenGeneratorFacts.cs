using System;
using System.Collections.Generic;
using System.Security.Claims;
using community.Core.Models;
using community.Core.Services;
using Microsoft.Extensions.Options;
using Xunit;

namespace community.UnitTests.Services
{
    public class TokenGeneratorFacts
    {
        [Fact]
        public void GenerateToken()
        {
            var userEmail = string.Empty;
            var userClaims = new List<Claim>(0);
            var tokenOptions = Options.Create(new JwtOptions {SecretKey = "1234567890ABCDEF"});
            var tokenGenerator = new TokenGenerator(tokenOptions);
            var token = tokenGenerator.GenerateToken(userEmail, userClaims);

            Assert.NotNull(token);
            Assert.IsType<string>(token);
        }

        [Fact]
        public void GenerateToken_Null_SecretKey()
        {
            var userEmail = string.Empty;
            var userClaims = new List<Claim>(0);
            var tokenOptions = Options.Create(new JwtOptions());
            var tokenGenerator = new TokenGenerator(tokenOptions);

            Assert.Throws<ArgumentNullException>(() => tokenGenerator.GenerateToken(userEmail, userClaims));
        }

        [Fact]
        public void GenerateToken_Empty_SecretKey()
        {
            var userEmail = string.Empty;
            var userClaims = new List<Claim>(0);
            var tokenOptions = Options.Create(new JwtOptions {SecretKey = string.Empty});
            var tokenGenerator = new TokenGenerator(tokenOptions);

            Assert.Throws<ArgumentException>(() => tokenGenerator.GenerateToken(userEmail, userClaims));
        }

        [Fact]
        public void GenerateToken_Short_SecretKey()
        {
            var userEmail = string.Empty;
            var userClaims = new List<Claim>(0);
            var tokenOptions = Options.Create(new JwtOptions {SecretKey = "1234567890"});
            var tokenGenerator = new TokenGenerator(tokenOptions);

            Assert.Throws<ArgumentOutOfRangeException>(() => tokenGenerator.GenerateToken(userEmail, userClaims));
        }
    }
}
