using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AEC.Filters
{
    public class AuthenticatedUserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetInt32("UserId") == null)
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
                context.HttpContext.Session.SetString("ErrorMessage", "Você deve estar autenticado para acessar essa página.");
            }
            base.OnActionExecuting(context);
        }
    }
}
