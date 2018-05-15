using System.Collections.Generic;
using System.Security.Claims;

namespace community.Core.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateToken(string userEmail, IList<Claim> userClaims);
    }
}
