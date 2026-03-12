namespace Chatly.Shared.Messages.Commands;

public class UpdateMessageCommand : IMessageCommand
{
    public string Content { get; set; } = null!;
}