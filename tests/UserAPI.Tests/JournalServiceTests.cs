using UserAPI.Models;
using UserAPI.Services;
using UserAPI.Tests.Factory;

namespace UserAPI.Tests;

public class JournalServiceTests : ServiceTestsBase
{
    [Fact]
    public async Task CreateAsync_Entity_ShouldCreateAndReturnCreatedEntity()
    {
        // Arrange
        const int journalId = 5;
        const string data = "{\"message\": \"The parent with the specified ID does not exist\"}";

        using var context = CreateInMemoryContext(nameof(CreateAsync_Entity_ShouldCreateAndReturnCreatedEntity));
        await context.Set<JournalModel>().AddRangeAsync(JournalTestData.CreateJournals());
        await context.SaveChangesAsync();

        var service = new JournalService(context);

        // Act
        var result = await service.CreateAsync(Enums.ExceptionType.Secure, data);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(journalId, result.Id);
        Assert.Equal(5, context.Set<JournalModel>().Count());
    }

    [Fact]
    public async Task CreateAsync_ThrowArgumentException_ShouldNotCreateAndThrowArgumentExceptionAsDataIsEmpty()
    {
        // Arrange
        const string data = "";

        using var context = CreateInMemoryContext(nameof(CreateAsync_ThrowArgumentException_ShouldNotCreateAndThrowArgumentExceptionAsDataIsEmpty));
        await context.Set<JournalModel>().AddRangeAsync(JournalTestData.CreateJournals());
        await context.SaveChangesAsync();

        var service = new JournalService(context);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(nameof(data), () => service.CreateAsync(Enums.ExceptionType.Secure, data));
    }

    [Fact]
    public async Task GetRangeAsync_Collection_ShouldReturnCollectionOfJournalsInRange()
    {
        // Arrange
        const int skip = 1;
        const int take = 3;

        using var context = CreateInMemoryContext(nameof(GetRangeAsync_Collection_ShouldReturnCollectionOfJournalsInRange));
        await context.Set<JournalModel>().AddRangeAsync(JournalTestData.CreateJournals());
        await context.SaveChangesAsync();

        var service = new JournalService(context);

        // Act
        var result = await service.GetRangeAsync(skip, take);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(skip, result.Skip);
        Assert.Equal(take, result.Count);
        Assert.NotEmpty(result.Items);
        Assert.Equal(take, result.Items.Length);
    }

    [Fact]
    public async Task GetRangeAsync_Collection_ShouldReturnEmptyCollection()
    {
        // Arrange
        const int skip = 1;
        const int take = 3;

        using var context = CreateInMemoryContext(nameof(GetRangeAsync_Collection_ShouldReturnEmptyCollection));

        var service = new JournalService(context);

        // Act
        var result = await service.GetRangeAsync(skip, take);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(skip, result.Skip);
        Assert.Equal(0, result.Count);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task GetRangeAsync_ThrowArgumentOutOfRangeException_ShouldThrowArgumentOutOfRangeExceptionAsSkipIsNegative()
    {
        // Arrange
        const int skip = -1;
        const int take = 3;

        using var context = CreateInMemoryContext(nameof(GetRangeAsync_ThrowArgumentOutOfRangeException_ShouldThrowArgumentOutOfRangeExceptionAsSkipIsNegative));
        await context.Set<JournalModel>().AddRangeAsync(JournalTestData.CreateJournals());
        await context.SaveChangesAsync();

        var service = new JournalService(context);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(skip), () => service.GetRangeAsync(skip, take));
    }

    [Fact]
    public async Task GetRangeAsync_ThrowArgumentOutOfRangeException_ShouldThrowArgumentOutOfRangeExceptionAsTakeIsNegative()
    {
        // Arrange
        const int skip = 1;
        const int take = -3;

        using var context = CreateInMemoryContext(nameof(GetRangeAsync_ThrowArgumentOutOfRangeException_ShouldThrowArgumentOutOfRangeExceptionAsTakeIsNegative));
        await context.Set<JournalModel>().AddRangeAsync(JournalTestData.CreateJournals());
        await context.SaveChangesAsync();

        var service = new JournalService(context);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(nameof(take), () => service.GetRangeAsync(skip, take));
    }

    [Fact]
    public async Task GetSingleAsync_Entity_ShouldReturnJournalById()
    {
        // Arrange
        const int id = 2;

        using var context = CreateInMemoryContext(nameof(GetSingleAsync_Entity_ShouldReturnJournalById));
        await context.Set<JournalModel>().AddRangeAsync(JournalTestData.CreateJournals());
        await context.SaveChangesAsync();

        var service = new JournalService(context);

        // Act
        var result = await service.GetSingleAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetSingleAsync_Entity_ShouldReturnNoAnyJournalById()
    {
        // Arrange
        const int id = 22;

        using var context = CreateInMemoryContext(nameof(GetSingleAsync_Entity_ShouldReturnNoAnyJournalById));
        await context.Set<JournalModel>().AddRangeAsync(JournalTestData.CreateJournals());
        await context.SaveChangesAsync();

        var service = new JournalService(context);

        // Act
        var result = await service.GetSingleAsync(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetSingleAsync_ThrowArgumentOutOfRangeException_ShouldNotReturnJournalAndThrowArgumentOutOfRangeException()
    {
        // Arrange
        const int id = 0;

        using var context = CreateInMemoryContext(nameof(GetSingleAsync_ThrowArgumentOutOfRangeException_ShouldNotReturnJournalAndThrowArgumentOutOfRangeException));
        await context.Set<JournalModel>().AddRangeAsync(JournalTestData.CreateJournals());
        await context.SaveChangesAsync();

        var service = new JournalService(context);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetSingleAsync(id));
    }
}
