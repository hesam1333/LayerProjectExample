using Evaluation.Brokers.Context;
using Evaluation.Brokers.Repositories.Interfaces;
using Evaluation.Domain;
using Evaluation.Domain.Exceptions;
using Evaluation.Services.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.Validations
{
    public class SignUpSIngInDtoValidator : Genericvalidator<SignUpSingInDto>
    {

        public SignUpSIngInDtoValidator()
        {
            RuleFor(item => item.Email).EmailAddress();
            RuleFor(item => item.Email).MaximumLength(80);

            RuleFor(item => item.Password).MaximumLength(80);
            RuleFor(item => item.Password).Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$")
                .WithMessage("Minimum eight characters, at least one letter," +
                " one number and" +
                " one special '@$!%*#?' character");
        }
    }
}

