using Chatly.Messages.Api.Entities;
using Chatly.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chatly.Messages.Api.Database.Configurations;

public class MessageConfguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey (m => m.Id);

        builder.Property(m => m.Text)
            .IsRequired()
            .HasMaxLength(MaxLengths.Messages.Text);
    }
}