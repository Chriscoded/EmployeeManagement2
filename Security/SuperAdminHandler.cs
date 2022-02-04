using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement2.Security
{
    public class SuperAdminHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirements>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirements requirement)
        {
            if(context.User.IsInRole("Super Admin"))
            {
                context.Succeed(requirement);  
            }
            return Task.CompletedTask;
        }
    }
}
