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
    public class EventAddService : AplicationService
    {
        private readonly ILoggingBroker loggingBroker;

        private readonly IEventRepository eventRepository;
        private readonly IEvaluateeRepository evaluateeRepository;
        private readonly IEvaluatorRepository evaluatorRepository;
        private readonly IUserRepository userRepository;
        private readonly IEventQuestionRepository eventQuestionRepository;

        private readonly IUserClaims userClaims;
        private readonly EventAddStage1DtoValidator eventAddStage1DtoValidator;
        private readonly EventAddStage2DtoValidator eventAddStage2DtoValidator;
        private readonly EventAddStage3DtoValidator eventAddStage3DtoValidator;
        private readonly UserValidator userValidator;
        public EventAddService(ILoggingBroker loggingBroker,
                                IEventRepository eventRepository,
                                IEvaluateeRepository evaluateeRepository,
                                IEvaluatorRepository evaluatorRepository,
                                IUserRepository userRepository,
                                IEventQuestionRepository eventQuestionRepository,
                                IUserClaims userClaims)
                                : base(loggingBroker)
        {

            this.loggingBroker = loggingBroker;
            this.userClaims = userClaims;


            this.eventRepository = eventRepository;
            this.evaluateeRepository = evaluateeRepository;
            this.evaluatorRepository = evaluatorRepository;
            this.userRepository = userRepository;
            this.eventQuestionRepository = eventQuestionRepository;

            eventAddStage1DtoValidator = new EventAddStage1DtoValidator();
            eventAddStage2DtoValidator = new EventAddStage2DtoValidator();
            eventAddStage3DtoValidator = new EventAddStage3DtoValidator();
            userValidator = new UserValidator(userRepository);

        }

        public async ValueTask<Event> EventStage1AddAsync(EventAddStage1Dto eventAddStage1Dto)
        => await TryCatchAsync(async () =>
        {

            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.admin.ToString());

            eventAddStage1DtoValidator.ValidateEntity(eventAddStage1Dto);

            List<int> evaluatorUserIds = await HandelEvaluatorUsers(eventAddStage1Dto.evaluatorDtos);

            List<int> evaluateeUserIds = await HandelEvaluateeUsers(eventAddStage1Dto.evaluateeDtos);

            Event @event = new Event()
            {
                EntryDatetime = DateTime.Now,
                EventTitle = eventAddStage1Dto.EventTitle,
                IsDelete = false,
                IsPublished = false
            };

            await eventRepository.AddAsync(@event);
            await AddEventEvaluatorEvaluatees(evaluatorUserIds, evaluateeUserIds, @event);

            return @event;
        });

        public async ValueTask<List<int>> EventStage2AddAsync(EventAddStage2Dto eventAddStage2Dto)
        => await TryCatchAsync(async () =>
        {

            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.admin.ToString());

            eventAddStage2DtoValidator.ValidateEntity(eventAddStage2Dto);


            var oldEventQuestions = await eventQuestionRepository.
                FindAsync(i => i.EventId == eventAddStage2Dto.EventId);

            await eventQuestionRepository.RemoveRangeAsync(oldEventQuestions);

            var newEventQuestions = new List<EventQuestion>();

            foreach (var questionId in eventAddStage2Dto.QuestionIds)
            {
                var item = new EventQuestion();
                item.EventId = eventAddStage2Dto.EventId;
                item.QuestionId = questionId;

                newEventQuestions.Add(item);
            }

            await eventQuestionRepository.AddRangeAsync(newEventQuestions);

            return newEventQuestions.Select(i => i.Id).ToList();
        });


        public async ValueTask<Event> EventStage3AddAsync(EventAddStage3Dto eventStage3Dto)
        => await TryCatchAsync(async () =>
        {

            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.admin.ToString());

            eventAddStage3DtoValidator.ValidateEntity(eventStage3Dto);

            var @event = await eventRepository.GetAsync(eventStage3Dto.EventId);

            @event.DueDate = eventStage3Dto.DueDate;
            @event.StartDate = eventStage3Dto.StartDate;
            @event.RatePointFrom = eventStage3Dto.RatePointFrom;
            @event.RatePointTo = eventStage3Dto.RatePointTo;
            @event.RepeatMonth = eventStage3Dto.RepeatMonth;
            @event.IsPublished = true;
            @event.NextStartDate = eventStage3Dto.RepeatMonth == null ? null :
                (DateTime?)eventStage3Dto.DueDate.AddMonths(eventStage3Dto.RepeatMonth ?? 0);


            await eventRepository.EditAsync(@event);

            return @event;
        });

        private async Task AddEventEvaluatorEvaluatees(
            List<int> evaluatorUserIds, List<int> evaluateeUserIds, Event @event)
        {
            foreach (var item in evaluatorUserIds)
            {
                var evaluator = new Evaluator();
                evaluator.EventId = @event.Id;
                evaluator.UserId = item;
                evaluator.EvaluateeCompletCount = 0;

                await evaluatorRepository.AddAsync(evaluator);

                foreach (var evaluatee in evaluateeUserIds)
                {
                    var evaluaee = new Evaluatee();
                    evaluaee.EvaluatorId = evaluator.Id;
                    evaluaee.UserId = evaluatee;
                    evaluaee.AnsweredQuestionCount = 0;

                    evaluateeRepository.Add(evaluaee);
                }

                await evaluateeRepository.SaveChangeAsync();
            }
        }

        private async Task<List<int>> HandelEvaluatorUsers(List<EventAddEvaluatorDto> evaluatorDtos)
        {
            List<User> evaluatorUse = new List<User>();
            foreach (var evaluator in evaluatorDtos)
            {
                if (evaluator.UserId != null)
                {
                    var user = await userRepository.GetAsync(evaluator.UserId.Value);

                    user.Email = evaluator.Email;
                    user.Position = evaluator.position;
                    user.SureName = evaluator.SureName;

                    await userValidator.ValidateEntityAsync(user);

                    userRepository.Edit(user);

                    evaluatorUse.Add(user);
                }
                else
                {
                    var user = new User();

                    user.Email = evaluator.Email;
                    user.Position = evaluator.position;
                    user.SureName = evaluator.SureName;
                    user.IsFirstTime = true;
                    user.IsActive = true;
                    user.IsAdmin = false;

                    await userValidator.ValidateEntityAsync(user);

                    userRepository.Add(user);

                    evaluatorUse.Add(user);
                }


            }

            await userRepository.SaveChangeAsync();

            return evaluatorUse.Select(i => i.Id).ToList();
        }

        private async Task<List<int>> HandelEvaluateeUsers(List<EventAddEvaluateeDto> evaluateeDtos)
        {
            List<User> evaluateeUser = new List<User>();
            foreach (var evaluator in evaluateeDtos)
            {
                if (evaluator.UserId != null)
                {
                    var user = await userRepository.GetAsync(evaluator.UserId.Value);

                    user.Position = evaluator.position;
                    user.SureName = evaluator.SureName;

                    await userValidator.ValidateEntityAsync(user);

                    userRepository.Edit(user);

                    evaluateeUser.Add(user);
                }
                else
                {
                    var user = new User();

                    user.Position = evaluator.position;
                    user.SureName = evaluator.SureName;
                    user.IsFirstTime = true;
                    user.IsActive = true;
                    user.IsAdmin = false;

                    await userValidator.ValidateEntityAsync(user);

                    userRepository.Add(user);

                    evaluateeUser.Add(user);
                }

            }

            await userRepository.SaveChangeAsync();


            return evaluateeUser.Select(i => i.Id).ToList();
        }
    }
}
