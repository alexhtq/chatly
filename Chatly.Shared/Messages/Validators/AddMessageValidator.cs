using Chatly.Shared.Constants;
using Chatly.Shared.Messages.Commands;
using FluentValidation;

namespace Chatly.Shared.Messages.Validators;

public class AddMessageValidator : AbstractValidator<AddMessageCommand>
{
    public AddMessageValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
                .WithMessage("Message is required.")
            .MaximumLength(MaxLengths.Messages.Text)
                .WithMessage("Message must not exceed {MaxLength} characters.");
    }
}