using CelebRateApi.Authorization.Requirements;
using CelebRateApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CelebRateApi.Authorization.Handlers
{
    public class IsOwnerHandler : AuthorizationHandler<IsOwnerRequirement, ApplicationUser>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            IsOwnerRequirement requirement, 
            ApplicationUser resource)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (resource.Id == userId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
