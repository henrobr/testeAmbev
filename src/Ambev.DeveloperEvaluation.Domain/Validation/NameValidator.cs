using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class NameValidator : AbstractValidator<string>
{
    public NameValidator(int minSize = 2, int maxSize = 2)
    {
        RuleFor(name => name)
            .NotEmpty()
            .WithMessage("The name cannot be empty.")
            .MinimumLength(minSize)
            .WithMessage($"The name cannot be less than {minSize} characters.")
            .MaximumLength(maxSize)
            .WithMessage($"The name cannot be longer than {maxSize} characters.");
    }
}
