using Azure.Core;
using Cefalo.InfedgeBlog.Service.Interfaces;

namespace Cefalo.InfedgeBlog.Service.Utils
{
    public class PasswordHandler : IPasswordHandler
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string userPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, userPassword);
        }
    }
}
