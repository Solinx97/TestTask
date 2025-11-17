using Microsoft.EntityFrameworkCore;
using UserAPI.Data;

namespace UserAPI.Tests;

public class ServiceTestsBase
{
    protected static UserContext CreateInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<UserContext>()
            .UseInMemoryDatabase(databaseName: dbName + Guid.NewGuid().ToString())
            .Options;

        return new UserContext(options);
    }
}
