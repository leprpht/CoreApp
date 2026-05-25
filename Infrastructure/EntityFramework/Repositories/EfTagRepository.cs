using AppCore.Entities;
using AppCore.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfTagRepository(ContactsDbContext context)
    : EfGenericRepository<Tag>(context.Tags), ITagRepository
{
    public async Task<IEnumerable<Tag>> FindByNameAsync(string name) =>
        await context.Tags
            .Where(t => t.Name.Contains(name))
            .ToListAsync();

    public async Task<IEnumerable<Tag>> GetAllAsync() =>
        await context.Tags.ToListAsync();

    public async Task CreateAsync(Tag tag)
    {
        await context.Tags.AddAsync(tag);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tag tag)
    {
        context.Tags.Update(tag);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var tag = await context.Tags.FindAsync(id);
        if (tag != null)
        {
            context.Tags.Remove(tag);
            await context.SaveChangesAsync();
        }
    }
}