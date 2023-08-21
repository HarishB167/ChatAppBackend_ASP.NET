using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ChatAppBackend.Filters
{
    public class JwtAuthMiddlewareFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // Check if the JWT token is present in the request header
            if (!actionContext.Request.Headers.Contains("Authorization"))
            {
                // No token provided, return 403 Forbidden with a JSON message
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden,
                    new { message = "Authorization token missing." });
                return;
            }

            // Extract the token from the Authorization header
            string token = actionContext.Request.Headers.GetValues("Authorization").FirstOrDefault();
            
            var validated = Utils.Utils.ValidateJwtToken(token);

            if (!validated)
            {
                // Invalid token, return 401 Unauthorized with a JSON message
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden,
                    new { message = "Invalid token." });
                return;
            }
            

            // You can add additional validation checks if needed, such as checking expiration, issuer, etc.

            // Token is valid, continue with the action
            base.OnActionExecuting(actionContext);


        }
    }
}