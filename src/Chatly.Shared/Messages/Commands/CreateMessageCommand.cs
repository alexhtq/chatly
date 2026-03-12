namespace Chatly.Shared.Messages.Commands;

public class CreateMessageCommand : IMessageCommand
{
    public string Content { get; set; } = null!;
}