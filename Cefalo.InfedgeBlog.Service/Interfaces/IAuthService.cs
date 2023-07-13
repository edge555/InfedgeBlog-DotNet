using Cefalo.InfedgeBlog.Service.Dtos;

namespace Cefalo.InfedgeBlog.Service.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> SignupAsync(SignupDto request);
        Task<UserWithTokenDto> LoginAsync(LoginDto request);
    }
}
