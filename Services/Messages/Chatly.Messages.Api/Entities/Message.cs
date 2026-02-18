namespace Chatly.Messages.Api.Entities;

public class Message
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
}