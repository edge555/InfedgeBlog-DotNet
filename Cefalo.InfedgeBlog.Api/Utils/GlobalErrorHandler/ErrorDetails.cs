using System.Text.Json;

namespace Cefalo.InfedgeBlog.Api.Utils.GlobalErrorHandler
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
