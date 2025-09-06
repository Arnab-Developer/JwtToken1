namespace JwtToken1.Core.Tokens;

public interface ITokenGenerator
{
    public string GenerateToken(int id, string name, params IEnumerable<string> roleNames);
}
