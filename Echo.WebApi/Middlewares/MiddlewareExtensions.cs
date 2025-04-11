using Microsoft.AspNetCore.Builder;

namespace Echo.WebApi.Middlewares
{
    public static class MiddlewareExtensions
    {
        // Shortcut method for using the maintenance middleware
        public static IApplicationBuilder UseMaintenanceMode(this IApplicationBuilder app)
            => app.UseMiddleware<MaintenanceMiddleware>();
    }
}