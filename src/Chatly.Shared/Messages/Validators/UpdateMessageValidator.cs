using Chatly.Shared.Messages.Commands;
using FluentValidation;

namespace Chatly.Shared.Messages.Validators;

public class UpdateMessageValidator : AbstractValidator<UpdateMessageCommand>
{
    public UpdateMessageValidator()
    {
        RuleFor(x => x.Content).ValidMessageContent();
    }
}