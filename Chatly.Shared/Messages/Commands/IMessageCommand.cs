namespace Chatly.Shared.Messages.Commands;

public interface IMessageCommand
{
    public string Content { get; set; }
}