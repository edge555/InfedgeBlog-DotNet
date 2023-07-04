using Cefalo.InfedgeBlog.Database.Models;

namespace Cefalo.InfedgeBlog.Service.Interfaces
{
    public interface IJwtTokenHandler
    {
        string GenerateJwtToken(User user);
    }
}
