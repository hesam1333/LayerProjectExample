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
    public class EventAddStage3DtoValidator : Genericvalidator<EventAddStage3Dto>
    {


        public EventAddStage3DtoValidator()
        {
            RuleFor(item => item).NotNull();

            RuleFor(item => item).Must((item) => { return item.StartDate < item.DueDate; })
                .WithMessage("Due Date must be greater than Start Date");


            RuleFor(item => item).Must((item) => { return item.RatePointFrom < item.RatePointTo; })
                .WithMessage("Max rate must be greater than min rate");

            RuleFor(item => item.RatePointFrom).GreaterThan(0);

        }

    }
}
