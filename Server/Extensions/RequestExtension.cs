using Microsoft.AspNetCore.Http;

namespace Server.Extensions
{
    public static class RequestExtension
    {
        public static string ToIP(this HttpRequest request)
        {
            return request.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        public static string ToIP(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
