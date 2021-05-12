using Common.Utilities;
using Evaluation.Brokers.Context;
using Evaluation.Brokers.Logging;
using Evaluation.Brokers.Repositories;
using Evaluation.Brokers.Repositories.Interfaces;
using Evaluation.Domain;
using Evaluation.Domain.Exceptions;
using Evaluation.Services.DTO;
using Evaluation.Services.Validations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services
{
    public class EventListService : AplicationService
    {
        private readonly ILoggingBroker loggingBroker;

        private readonly IEventRepository eventRepository;

        private readonly IUserClaims userClaims;

        public EventListService(ILoggingBroker loggingBroker,
                                IEventRepository eventRepository,
                                IUserClaims userClaims)
                                : base(loggingBroker)
        {

            this.loggingBroker = loggingBroker;
            this.userClaims = userClaims;

            this.eventRepository = eventRepository;

        }


        public async ValueTask<List<EventListDto>> EventlistAsync(SortAndPagingParameters sortAndPagingParas) => await
        TryCatchAsync(async () =>
        {

            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.admin.ToString());

            List<EventEvaluateeEvaluatorsViewTable> rowData =
               await eventRepository.EventEvaluateeEvaluatorListAsync(sortAndPagingParas);


            var mappedData = rowData.Select(i => new EventListDto()
            {
                EndDate = i.DueDate,
                StartDate = i.StartDate,
                Id = i.Id,
                EventTitle = i.EventTitle,
                Status = GetEventStatus(i.DueDate),
                Evaluatees = string.Join(',', i.Evaluetees.Select(i => i.SureName)),
                Evaluators = string.Join(',', i.Evaluators.Select(i => i.SureName))
            }).ToList();

            return mappedData;
        });


        public async ValueTask<List<OnGoingEventDto>> OnGoingEventListAsync() => await
        TryCatchAsync(async () =>
        {

            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.admin.ToString());

            List<EventProgressViewTable> rowData = await
                eventRepository.NotFinishedEventsProgressListAsync();



            var mappedData = rowData.Select(i => new OnGoingEventDto()
            {
                EndDate = i.Event.DueDate,
                StartDate = i.Event.StartDate,
                Id = i.Event.Id,
                EventTitle = i.Event.EventTitle,
                CompletPercent = (i.EvaluateDone / i.TotalEvaluate) * 100
            }).ToList();

            return mappedData;
        });


        public async ValueTask<bool> RemoveEvent(int eventId) => await
        TryCatchAsync(async () =>
        {

            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.admin.ToString());

            Event Item = await eventRepository.GetAsync(eventId);

            Item.IsDelete = true;

            await eventRepository.EditAsync(Item);

            return true;
        });

        public string GetEventStatus(DateTime? dueDate)
        {
            var today = DateTime.Now.Date;
            if (dueDate < today)
                return "finished";


            if (dueDate >= today && dueDate <= today)
                return "on going";


            if (dueDate > today)
                return "finished";

            return "";
        }


    }
}
