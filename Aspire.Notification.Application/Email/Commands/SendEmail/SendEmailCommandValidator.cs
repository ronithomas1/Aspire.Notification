using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Notification.Application.Email.Commands.SendEmail
{
    public  class SendEmailCommandValidator: AbstractValidator<SendEmailCommand>
    {
        public SendEmailCommandValidator()
        {
            RuleFor(v => v.subject)
            .NotEmpty()
            .MaximumLength(100);

            RuleFor(v => v.To)
                .Must(HaveAtLeastOneRecepient)
                .WithMessage("'{PropertyName}' must have atleast one recepient.")
                .WithErrorCode("AtleastOne");

        }

        public bool HaveAtLeastOneRecepient(List<string> recepients)
        {
            return recepients.Any();
        }
    }
}
