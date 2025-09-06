namespace JwtToken1.Core.Tokens;

public record TokenOptions
{
    public required string Key { get; init; }

    public required string Issuer { get; init; }

    public required string Audience { get; init; }

    public required int ExpiresInMinutes { get; init; }
}