using AppCore.Entities;
using AppCore.Repositories;
using Infrastructure.Memory;

namespace UnitTests;

public class MemoryGenericRepositoryTests
{
    private readonly IGenericRepositoryAsync<Person> _repo;

    public MemoryGenericRepositoryTests()
    {
        _repo = new MemoryGenericRepository<Person>();
    }

    [Fact]
    public async Task AddPersonTestAsync()
    {
        var person = new Person { FirstName = "Adam" };

        var added = await _repo.AddAsync(person);
        Assert.NotNull(added);
        Assert.Equal("Adam", added.FirstName);

        var found = await _repo.FindByIdAsync(added.Id);
        Assert.NotNull(found);
        Assert.Equal("Adam", found.FirstName);
    }

    [Fact]
    public async Task FindAllTestAsync()
    {
        var p1 = new Person { FirstName = "Adam" };
        var p2 = new Person { FirstName = "Eva" };

        await _repo.AddAsync(p1);
        await _repo.AddAsync(p2);

        var result = await _repo.FindAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateTestAsync()
    {
        var person = new Person { FirstName = "Adam" };
        await _repo.AddAsync(person);

        person.FirstName = "Adam Updated";

        await _repo.UpdateAsync(person);
        var result = await _repo.FindByIdAsync(person.Id);

        Assert.Equal("Adam Updated", result?.FirstName);
    }

    [Fact]
    public async Task RemoveTestAsync()
    {
        var person = new Person { FirstName = "Adam" };
        await _repo.AddAsync(person);

        await _repo.RemoveByIdAsync(person.Id);
        var result = await _repo.FindByIdAsync(person.Id);

        Assert.Null(result);
    }

    [Fact]
    public async Task FindPagedTestAsync()
    {
        for (var i = 0; i < 10; i++) await _repo.AddAsync(new Person { FirstName = $"Person{i}" });

        var page = await _repo.FindPagedAsync(1, 5);

        Assert.Equal(5, page.Items.Count);
        Assert.Equal(10, page.TotalCount);
        Assert.True(page.HasNext);
    }

    [Fact]
    public async Task RemoveNonExistingShouldThrow()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _repo.RemoveByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task UpdateNonExistingShouldThrow()
    {
        var person = new Person
        {
            FirstName = "Ghost"
        };

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _repo.UpdateAsync(person));
    }
}