using UserAPI.Exceptions;
using UserAPI.Models;
using UserAPI.Services;
using UserAPI.Tests.Factory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserAPI.Tests;

public class UserServiceTests : ServiceTestsBase
{
    [Fact]
    public async Task CreateAsync_Entity_ShouldCreateAndReturnCreatedEntity()
    {
        // Arrange
        const string code = "oleg";

        using var context = CreateInMemoryContext(nameof(CreateAsync_Entity_ShouldCreateAndReturnCreatedEntity));
        await context.Set<UserModel>().AddRangeAsync(UserTestData.CreateUsers());
        await context.SaveChangesAsync();

        var service = new UserService(context);

        // Act
        var result = await service.CreateAsync(code);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(code, result.Code);
        Assert.Equal(5, context.Set<UserModel>().Count());
    }

    [Fact]
    public async Task CreateAsync_ThrowSecureException_ShouldNotCreateAndThrowSecureExceptionAsUserExist()
    {
        // Arrange
        const string code = "check";

        using var context = CreateInMemoryContext(nameof(CreateAsync_ThrowSecureException_ShouldNotCreateAndThrowSecureExceptionAsUserExist));
        await context.Set<UserModel>().AddRangeAsync(UserTestData.CreateUsers());
        await context.SaveChangesAsync();

        var service = new UserService(context);

        // Act and Assert
        await Assert.ThrowsAsync<SecureException>(() => service.CreateAsync(code));
    }

    [Fact]
    public async Task GetAsync_Entity_ShouldReturnUserByCode()
    {
        // Arrange
        const string code = "check";

        using var context = CreateInMemoryContext(nameof(GetAsync_Entity_ShouldReturnUserByCode));
        await context.Set<UserModel>().AddRangeAsync(UserTestData.CreateUsers());
        await context.SaveChangesAsync();

        var service = new UserService(context);

        // Act
        var result = await service.GetAsync(code);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(code, result.Code);
    }

    [Fact]
    public async Task GetAsync_Entity_ShouldReturnNoAnyUserByCode()
    {
        // Arrange
        const string code = "oleg";

        using var context = CreateInMemoryContext(nameof(GetAsync_Entity_ShouldReturnNoAnyUserByCode));
        await context.Set<UserModel>().AddRangeAsync(UserTestData.CreateUsers());
        await context.SaveChangesAsync();

        var service = new UserService(context);

        // Act
        var result = await service.GetAsync(code);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_ThrowArgumentException_ShouldReturnNotAnyUserAndThrowArgumentExceptionAsCodeIsEmpty()
    {
        // Arrange
        const string code = "";

        using var context = CreateInMemoryContext(nameof(GetAsync_ThrowArgumentException_ShouldReturnNotAnyUserAndThrowArgumentExceptionAsCodeIsEmpty));
        await context.Set<UserModel>().AddRangeAsync(UserTestData.CreateUsers());
        await context.SaveChangesAsync();

        var service = new UserService(context);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.GetAsync(code));
    }
}
