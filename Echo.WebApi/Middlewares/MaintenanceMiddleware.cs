using Echo.Business.Operations.Settings;

namespace Echo.WebApi.Middlewares
{
    // Blocks requests during maintenance mode except for specific routes or admin users
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MaintenanceMiddleware> _logger;

        public MaintenanceMiddleware(RequestDelegate next, ILogger<MaintenanceMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, ISettingService settingService)
        {
            var isMaintenanceOn = await settingService.IsMaintenanceModeAsync();
            var path = context.Request.Path.Value?.ToLower() ?? "";

            // Allow specific routes
            bool isAllowedPath = path.StartsWith("/api/settings") || path.StartsWith("/api/auth/login");

            // Allow admin users
            bool isAdmin = context.User?.IsInRole("Admin") == true;

            if (isMaintenanceOn && !isAllowedPath && !isAdmin)
            {
                _logger.LogInformation("Blocked by maintenance mode: {Path}", path);

                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = "Echo is under maintenance. Please try again later."
                });

                return;
            }

            await _next(context);
        }
    }
}