using Microsoft.AspNetCore.Builder;

namespace Echo.WebApi.Middlewares
{
    public static class MiddlewareExtensions
    {
        // Shortcut method for using the maintenance middleware
        public static IApplicationBuilder UseMaintenanceMode(this IApplicationBuilder app)
            => app.UseMiddleware<MaintenanceMiddleware>();

        // Shortcut method for using the exception middleware
        public static IApplicationBuilder UseGlobalException(this IApplicationBuilder app)
            => app.UseMiddleware<ExceptionMiddleware>();
    }
}