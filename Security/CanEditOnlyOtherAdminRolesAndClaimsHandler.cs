using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace EmployeeManagement2.Security
{
    public class CanEditOnlyOtherAdminRolesAndClaimsHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirements>
    {


        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirements requirement)
        {
            var authFilterContext = context.Resource as AuthorizationFilterContext;
            if(authFilterContext == null)
            {
                return Task.CompletedTask;
            }
            //get Id of logged in Admin
            string loggedInAdminId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            //id of admin being edited
            string adminbeingedited = authFilterContext.HttpContext.Request.Query["userId"];
            if (context.User.IsInRole("Admin") &&
                context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") &&
                    loggedInAdminId.ToLower() != adminbeingedited.ToLower())
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;  
        }
    }
}
