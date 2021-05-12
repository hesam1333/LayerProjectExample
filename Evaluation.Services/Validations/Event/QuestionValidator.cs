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
    public class QuestionValidator : Genericvalidator<Question>
    {


        public QuestionValidator()
        {
            RuleFor(item => item).NotNull();


            RuleFor(item => item.QuestionTitle).NotEmpty().NotNull();
            RuleFor(item => item.QuestionTitle).MaximumLength(80);

        }

    }
}
