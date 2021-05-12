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
    public class EventAddStage2DtoValidator : Genericvalidator<EventAddStage2Dto>
    {


        public EventAddStage2DtoValidator()
        {
            RuleFor(item => item).NotNull();

            RuleFor(item => item.QuestionIds).Must((item) => { return item.Count > 0; })
                .WithMessage("no Question selected");

        }

    }
}
