using Chatly.Messages.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chatly.Messages.Api.Database;

public class MessagesContext(DbContextOptions<MessagesContext> options) : DbContext(options)
{
    public DbSet<Message> Messages => Set<Message>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = TruncateToSeconds(DateTimeOffset.UtcNow);
        
        foreach (var entry in ChangeTracker.Entries<Message>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }

    private static DateTimeOffset TruncateToSeconds(DateTimeOffset value)
    {
        return new DateTimeOffset(value.Year, value.Month, value.Day,
            value.Hour, value.Minute, value.Second, value.Offset);
    }
}