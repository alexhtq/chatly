using Chatly.Shared.Constants;
using FluentValidation;

namespace Chatly.Shared.Messages.Validators;

public static class MessageValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ValidMessageContent<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithMessage("Message is required.")
            .MaximumLength(MaxLengths.Messages.Content)
                .WithMessage("Message must not exceed {MaxLength} characters.");
    }
}