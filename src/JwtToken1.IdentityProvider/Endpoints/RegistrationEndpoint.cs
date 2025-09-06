using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace JwtToken1.IdentityProvider.Endpoints;

internal record RegistrationRequest(string Name, string Password, string RoleName);

internal static class RegistrationEndpoint
{
    public static IEndpointConventionBuilder MapRegistrationEndpoint(this IEndpointRouteBuilder builder)
        => builder.MapPost("registration", RegistrationEndpointHandler.Handle);
}

internal static class RegistrationEndpointHandler
{
    public static async Task<Results<Ok<string>, BadRequest<string>>> Handle(
        RegistrationRequest request,
        AuthContext context,
        CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Name);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Password);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.RoleName);

        var role = await context.Roles // Roles has been added in the db manually.
            .SingleOrDefaultAsync(r => r.Name == request.RoleName, ct)
            .ConfigureAwait(false);

        if (role is null)
        {
            return TypedResults.BadRequest("Invalid role");
        }

        var user = new User(request.Name, request.Password); // IsConfirm should be mark as true from the backend only.
        role.Users.Add(user);
        await context.SaveChangesAsync(ct).ConfigureAwait(false);

        return TypedResults.Ok("Success");
    }
}