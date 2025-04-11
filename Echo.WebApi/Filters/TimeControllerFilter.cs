using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Echo.WebApi.Filters
{
    // Filters requests based on time range
    public class TimeControllerFilter : ActionFilterAttribute
    {
        // Set allowed time range (24-hour format)
        public string StartTime { get; set; } = "22:00";
        public string EndTime { get; set; } = "23:59";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var now = DateTime.Now.TimeOfDay;

            if (now >= TimeSpan.Parse(StartTime) && now <= TimeSpan.Parse(EndTime))
            {
                context.Result = new ContentResult
                {
                    Content = "Echo is quiet during this time. Please come back later :)",
                    StatusCode = 403
                };
                return; // Stop execution
            }

            base.OnActionExecuting(context);
        }
    }
}