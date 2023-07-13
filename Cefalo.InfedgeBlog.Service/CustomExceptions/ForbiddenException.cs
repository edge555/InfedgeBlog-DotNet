namespace Cefalo.InfedgeBlog.Service.CustomExceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() { }
        public ForbiddenException(string message) : base(message) { }
    }
}
