namespace Chatly.Messages.Api.Entities;

public class Message
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; internal set; }
    public DateTimeOffset? UpdatedAt { get; internal set; }
}