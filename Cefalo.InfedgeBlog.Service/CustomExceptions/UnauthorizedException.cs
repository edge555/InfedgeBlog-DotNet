namespace Cefalo.InfedgeBlog.Service.CustomExceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() { }
        public UnauthorizedException(string message) : base(message) { }
    }
}
