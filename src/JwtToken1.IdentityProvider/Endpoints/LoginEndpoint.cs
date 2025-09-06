using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace JwtToken1.IdentityProvider.Endpoints;

internal record LoginRequest(string Name, string Password);

internal record LoginResponse(string AccessToken);

internal static class LoginEndpoint
{
    public static IEndpointConventionBuilder MapLoginEndpoint(this IEndpointRouteBuilder builder)
        => builder.MapPost("login", LoginEndpointHandler.Handle);
}

internal static class LoginEndpointHandler
{
    public static async Task<Results<Ok<LoginResponse>, BadRequest<string>>> Handle(
        LoginRequest request,
        AuthContext context,
        ITokenGenerator tokenGenerator,
        CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Name);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Password);

        var user = await context.Users
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Name == request.Name && u.Password == request.Password && u.IsConfirm, ct)
            .ConfigureAwait(false);

        if (user is null)
        {
            return TypedResults.BadRequest("Invalid user");
        }

        var token = tokenGenerator.GenerateToken(user.Id, user.Name, user.Roles.Select(r => r.Name));
        var response = new LoginResponse(token);
        return TypedResults.Ok(response);
    }
}