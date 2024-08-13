using Microsoft.AspNetCore.Identity;

namespace WalksAPI.Interfaces.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> Roles);
    }
}
