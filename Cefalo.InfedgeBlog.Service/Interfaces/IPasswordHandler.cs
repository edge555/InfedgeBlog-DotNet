namespace Cefalo.InfedgeBlog.Service.Interfaces
{
    public interface IPasswordHandler
    {
        string HashPassword(string password);
        Boolean Verify(string password, string userPassword);
    }
}
