using ApiSkeleton.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace ApiSkeleton.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute, IAuthorizationFilter
    {
        public bool Optional { get; set; } = false;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var id = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id");
            if (id == null)
            {
                if (Optional && !context.HttpContext.Request.Headers.ContainsKey(Constants.HEADER_USER_ID))
                    context.HttpContext.Request.Headers.Add(Constants.HEADER_USER_ID, default);
                else // not logged in
                    context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else
            {
                if (!context.HttpContext.Request.Headers.ContainsKey(Constants.HEADER_USER_ID))
                    context.HttpContext.Request.Headers.Add(Constants.HEADER_USER_ID, id.Value);
            }
        }
    }
}