using Ej.Application.Models;
using FluentValidation;

namespace Ej.Application.Validators;

public class ContactViewModelValidator : AbstractValidator<ContactViewModel>
{
    private const string REQUIRED = "This field is required.";

    public ContactViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage(REQUIRED)
            .Length(2, 100)
                .WithMessage("Your name should be between 2 and 100 characters long.");


        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage(REQUIRED)
            .EmailAddress()
                .WithMessage("This is not valid email address.");


        RuleFor(x => x.Message)
            .NotEmpty()
                .WithMessage(REQUIRED)
            .Length(2, 2000)
                .WithMessage("Your message should be between 2 and 2000 characters long.");
    }
}
