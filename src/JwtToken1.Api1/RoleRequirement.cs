using Microsoft.AspNetCore.Authorization;

internal record RoleRequirement(string RoleName) : IAuthorizationRequirement;

internal class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        RoleRequirement requirement)
    {
        if (requirement is null || context.User.Identity is null || !context.User.Identity.IsAuthenticated)
        {
            context.Fail();
            return;
        }

        var roles = context.User.FindFirst("Roles")?.Value.Split(',');

        if (roles is null || !roles.Contains(requirement.RoleName))
        {
            context.Fail();
            return;
        }

        await Task.CompletedTask;
        context.Succeed(requirement);
    }
}