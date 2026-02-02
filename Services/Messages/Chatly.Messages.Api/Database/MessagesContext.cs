using Chatly.Messages.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chatly.Messages.Api.Database;

public class MessagesContext(DbContextOptions<MessagesContext> options)
    : DbContext(options)
{
    public DbSet<Message> Messages => Set<Message>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}