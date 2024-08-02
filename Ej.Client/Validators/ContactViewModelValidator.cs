using Ej.Application.Models;
using Ej.Client.Controllers;
using Ekzakt.Utilities.Validation.Regex;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Ej.Client.Validators;

public class ContactViewModelValidator : AbstractValidator<ContactViewModel>
{
    public ContactViewModelValidator(IStringLocalizer<ContactController> localizer)
    {
        var x = CultureInfo.CurrentUICulture.Name;

        RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage(x => localizer["__View_Index_Form_Name_Required"])
            .Length(2, 100)
                .WithMessage(x => localizer["__View_Index_Form_Name_InvalidLength"]);

        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage(x => localizer["__View_Index_Form_Email_Required"])
            .Matches(Internet.EMAIL_ADDRESS)
                .WithMessage(x => localizer["__View_Index_Form_Email_Invalid"]);

        RuleFor(x => x.Message)
            .NotEmpty()
                .WithMessage(x => localizer["__View_Index_Form_Message_Required"])
            .Length(2, 2000)
                .WithMessage(x => localizer["__View_Index_Form_Message_InvalidLength"]);
    }
}
