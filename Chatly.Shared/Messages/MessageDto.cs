namespace Chatly.Shared.Messages;

public class MessageDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
}