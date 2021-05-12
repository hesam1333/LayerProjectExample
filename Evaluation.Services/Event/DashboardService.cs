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
    public class DashboardService : AplicationService
    {
        private readonly ILoggingBroker loggingBroker;

        private readonly IEventRepository eventRepository;
        private readonly IEvaluateeRepository evaluateeRepository;

        private readonly IUserClaims userClaims;

        public DashboardService(ILoggingBroker loggingBroker,
                                IUserClaims userClaims,
                                IEventRepository eventRepository,
                                IEvaluateeRepository evaluateeRepository)
                                : base(loggingBroker)
        {

            this.loggingBroker = loggingBroker;
            this.userClaims = userClaims;


            this.eventRepository = eventRepository;
            this.evaluateeRepository = evaluateeRepository;

        }


        public async ValueTask<List<EventListForSearchDto>>  SearchEventsAsync(string searchString) => await
        TryCatchAsync(async () =>
        {
            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.admin.ToString());

            List<Event> Events = new List<Event>();
            var mayBeDateTime = new DateTime();
            bool isADateTime = DateTime.TryParse(searchString, out mayBeDateTime);

            if (isADateTime)
            {
                Events = await this.eventRepository.FindAsync(i => !i.IsDelete && i.DueDate > mayBeDateTime && i.StartDate < mayBeDateTime);
            }
            else
            {
                Events = await this.eventRepository.FindAsync(i => !i.IsDelete && i.EventTitle.Contains(searchString));
            }

            List<EventListForSearchDto> cleanEvent = Events.Select(i => new EventListForSearchDto()
            {
                Id = i.Id,
                DueDate = i.DueDate,
                EntryDatetime = i.EntryDatetime,
                EventTitle = i.EventTitle,
                StartDate = i.StartDate
            }).ToList();

            return cleanEvent;
        });

        public async ValueTask<DashboardChartDto> DashBoardChartValuesAsync() => await
        TryCatchAsync(async () =>
        {
            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.admin.ToString());

            DateTime today = DateTime.Now.Date;
            List<Event> ThisYearEvents = await eventRepository.
                FindAsync(i =>!i.IsDelete && i.IsPublished && i.StartDate.Value.Year == today.Year);

            DashboardChartDto dashboardChart = new DashboardChartDto()
            {
                CompletedEvents = ThisYearEvents.Count(i => i.DueDate < today),
                OnGoingEvents = ThisYearEvents.Count(i => i.DueDate > today && i.StartDate < today),
                NotStartedEvents = ThisYearEvents.Count(i => i.StartDate > today),
                TotalEvents = ThisYearEvents.Count()
            };

            return dashboardChart;

        });


        public async ValueTask<List<UserNotEvaluatedDto>> GetUsersNotEvaluatedAsync() => await
        TryCatchAsync(async () =>
        {
            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.admin.ToString());

            var rawData = await evaluateeRepository.GetUsersNotEvaluatedFromThisMonthAsync(6);

            var mappedData = rawData.Select(i => new UserNotEvaluatedDto()
            {
                LastEvaluationDateTime = i.LastEventAsEvaluatee.DueDate,
                Position = i.User.Position,
                SureName = i.User.SureName
            }).ToList();

            return mappedData;

        });


        public async ValueTask<List<TopStarUsersDto>> GetTopStarAsyncs() => await
        TryCatchAsync(async () =>
        {

            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.admin.ToString());

            var rawData = await evaluateeRepository.GetUsersWithTopEvaluateeScoreAsync(12);

            var mappedData = rawData.Select(i => new TopStarUsersDto()
            {
                Score = i.TotalScore,
                Position = i.User.Position,
                SureName = i.User.SureName
            }).ToList();

            return mappedData;

        });

    }
}
