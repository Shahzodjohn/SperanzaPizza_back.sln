using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SperanzaPizzaApi.Models;

namespace SperanzaPizzaApi.Infrastructure.Filters
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(params string[] claim) : base(typeof(CustomAuthorizeFilter))
        {
            Arguments = new object[] { claim };
        }
    }

    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        readonly string[] _claim;

        public CustomAuthorizeFilter(params string[] claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {            
            bool isValid = false;
            if (context != null)
            {
                var authToken = context.HttpContext.Request.Headers["kdichr"];  
                // Write your logic here.
                if (string.IsNullOrEmpty(authToken))
                {
                    context.Result = new UnauthorizedObjectResult($"Token is required");
                    return;
                }
                using (SPToCoreContext db = new SPToCoreContext()) {
                    var result = db.EXPOSE_dm_Tokens_CheckAsync(authToken).Result;
                    if (result != null)
                        isValid = result[0].isValid ?? false;
                }   
                if (!isValid)
                {
                    context.Result = new UnauthorizedObjectResult($"Token is invalid");
                    return;
                }
            }           
        }
    }
}