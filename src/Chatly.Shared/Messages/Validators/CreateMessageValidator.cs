using Chatly.Shared.Messages.Commands;
using FluentValidation;

namespace Chatly.Shared.Messages.Validators;

public class CreateMessageValidator : AbstractValidator<CreateMessageCommand>
{
    public CreateMessageValidator()
    {
        RuleFor(x => x.Content).ValidMessageContent();
    }
}