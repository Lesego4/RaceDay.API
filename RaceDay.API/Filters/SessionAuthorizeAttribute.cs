using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RaceDay.API.Filters
{
    public class SessionAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string _requiredRole;

        // Pass the role you want to check for (e.g., "Organiser")
        public SessionAuthorizeAttribute(string requiredRole = null)
        {
            _requiredRole = requiredRole;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 1. Check if UserId exists in the session
            var userId = context.HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                // If no user ID, they are not logged in
                context.Result = new UnauthorizedObjectResult(new { Message = "You are not logged in." });
                return;
            }

            // 2. Check if the role matches (if a specific role was required)
            if (_requiredRole != null)
            {
                var userRole = context.HttpContext.Session.GetString("UserRole");

                if (userRole != _requiredRole)
                {
                    // If the role doesn't match, forbid access
                    context.Result = new ObjectResult(new { Message = $"Access denied. Requires {_requiredRole} role." })
                    {
                        StatusCode = 403
                    };
                    return;
                }
            }

            // If both checks pass, the request continues to the endpoint
            base.OnActionExecuting(context);
        }
    }
}