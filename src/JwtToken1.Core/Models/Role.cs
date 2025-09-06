namespace JwtToken1.Core.Models;

public class Role(string name)
{
    public int Id { get; set; }

    public string Name { get; set; } = name;

    public IList<User> Users { get; set; } = [];
}