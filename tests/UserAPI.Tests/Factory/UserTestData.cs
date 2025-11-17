using UserAPI.Models;

namespace UserAPI.Tests.Factory;

internal static class UserTestData
{
    public static List<UserModel> CreateUsers()
    {
        var users = new List<UserModel>
        {
            new()
            {
                Id = "uid-22",
                Code = "check",
            },
            new()
            {
                Id = "uid-23",
                Code = "kiril",
            },
            new()
            {
                Id = "uid-24",
                Code = "dIo0",
            },
            new()
            {
                Id = "uid-25",
                Code = "23245",
            }
        };

        return users;
    }
}
