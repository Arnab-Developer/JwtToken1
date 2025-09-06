using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JwtToken1.Core.Data;

/// <summary>This is to create migrations.</summary>
internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AuthContext>
{
    public AuthContext CreateDbContext(string[] args)
    {
        var constr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JwtToken1;Integrated Security=True";

        var context = new AuthContext(new DbContextOptionsBuilder<AuthContext>()
            .UseSqlServer(constr)
            .Options);

        return context;
    }
}