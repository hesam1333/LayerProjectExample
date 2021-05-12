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
    public class EventAddStage1DtoValidator : Genericvalidator<EventAddStage1Dto>
    {


        public EventAddStage1DtoValidator()
        {
            RuleFor(item => item).NotNull();


            RuleFor(item => item).Must( (item, cancellation) => IsEmailsUnicAsync(item.evaluatorDtos.Select(i => i.Email)))
                .WithMessage("Each Email must be unic in evaluator list").WithName("evaluatorDtos");


            RuleFor(item => item.EventTitle).NotEmpty();
            RuleFor(item => item.EventTitle).MaximumLength(80);


            RuleFor(item => item.evaluatorDtos).Must((item) => { return item.Count > 0; })
                .WithMessage("There are no evaluators");

            RuleFor(item => item.evaluateeDtos).Must((item) => { return item.Count > 0; })
                .WithMessage("There are no evaluatees");
        }



        private  bool IsEmailsUnicAsync(IEnumerable<String> emails)
        {

            // there is no duplicate
            if (emails.Distinct().Count() == emails.Count())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
