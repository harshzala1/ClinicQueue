using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ClinicQueue.Helpers
{
    /// <summary>
    /// Custom action filter that checks if user is authenticated and has the required role.
    /// Redirects to login if unauthenticated, or shows access denied for wrong role.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeRoleAttribute : ActionFilterAttribute
    {
        private readonly string[] _roles;

        public AuthorizeRoleAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Session.GetString(SessionKeys.Token);
            if (string.IsNullOrEmpty(token))
            {
                // Not authenticated — redirect to login
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            if (_roles.Length > 0)
            {
                var userRole = context.HttpContext.Session.GetString(SessionKeys.UserRole);
                if (string.IsNullOrEmpty(userRole) || !_roles.Contains(userRole, StringComparer.OrdinalIgnoreCase))
                {
                    // Wrong role — show access denied
                    context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
