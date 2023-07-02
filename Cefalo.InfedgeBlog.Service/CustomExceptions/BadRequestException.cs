namespace Cefalo.InfedgeBlog.Service.CustomExceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException() { }
        public BadRequestException(string message) : base(message) { }
    }
}
