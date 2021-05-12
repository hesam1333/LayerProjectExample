using Common.Utilities;
using Evaluation.Brokers.Context;
using Evaluation.Brokers.Logging;
using Evaluation.Brokers.Repositories;
using Evaluation.Brokers.Repositories.Interfaces;
using Evaluation.Domain;
using Evaluation.Domain.Exceptions;
using Evaluation.Services.DTO;
using Evaluation.Services.DTO.Evaluation;
using Evaluation.Services.Validations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services
{
    public class EvaluatorService : AplicationService
    {
        private readonly ILoggingBroker loggingBroker;

        private readonly IUserRepository userRepository;
        private readonly IEvaluatorRepository evaluatorRepository;

        private readonly IUserClaims userClaims;
        private readonly UserValidator userValidator;

        public EvaluatorService(ILoggingBroker loggingBroker,
                                IUserClaims userClaims,
                                IUserRepository userRepository,
                                IEvaluatorRepository evaluatorRepository)
                                : base(loggingBroker)
        {
            this.loggingBroker = loggingBroker;
            this.userClaims = userClaims;

            this.userRepository = userRepository;
            this.evaluatorRepository = evaluatorRepository;

            this.userValidator = new UserValidator(userRepository);
        }


        public async ValueTask<List<OnGoingEventDto>> RetrieveCurrentEvaluatorOngoingEvents() => await
        TryCatchAsync(async () =>
        {

            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.evaluator.ToString());

            List<EventProgressViewTable> rowData = await
                evaluatorRepository.GetNotFinishedEventsListAsync(userClaims.GetUserId());



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


        public async ValueTask<List<EvaluateeProgressDto>> GetCurrentEvaluatorEvaluateesWhitProgres(int eventId) => await
        TryCatchAsync(async () =>
        {

            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.evaluator.ToString());

            int userId = userClaims.GetUserId();

            bool isForThisUser = await evaluatorRepository.AnyAsync(
                i => i.UserId == userId && i.EventId == eventId);

            if (!isForThisUser)
            {
                throw new NotAuthorizedException("you cant evaluate this event");
            }

            var rowData = await
                evaluatorRepository.GetEvaluatorEvaluatees(userId, eventId);



            var mappedData = rowData.Select(i => new EvaluateeProgressDto()
            {
                CompletPercent = (i.TotalAnswred / i.TotalQuestion) * 100,
                EvaluateeId = i.EvaluateeId,
                EvaluateeName = i.EvaluateeName,
            }).ToList();

            return mappedData;
        });


    }
}
