using Chatly.Shared.Constants;
using Chatly.Shared.Messages.Commands;
using FluentValidation;

namespace Chatly.Shared.Messages.Validators;

public class CreateMessageValidator : AbstractValidator<CreateMessageCommand>
{
    public CreateMessageValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
                .WithMessage("Message is required.")
            .MaximumLength(MaxLengths.Messages.Content)
                .WithMessage("Message must not exceed {MaxLength} characters.");
    }
}