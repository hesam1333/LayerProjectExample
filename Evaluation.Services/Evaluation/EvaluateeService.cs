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
    public class EvaluateeService : AplicationService
    {
        private readonly ILoggingBroker loggingBroker;

        private readonly IUserRepository userRepository;
        private readonly IEvaluateeRepository evaluateeRepository;
        private readonly IEvaluateeEventQuestionRepository evaluateeEventQuestionRepository;
        private readonly IEvaluatorRepository evaluatorRepository;

        private readonly IUserClaims userClaims;

        public EvaluateeService(ILoggingBroker loggingBroker,
                                IUserClaims userClaims,
                                IUserRepository userRepository,
                                IEvaluatorRepository evaluatorRepository,
                                IEvaluateeEventQuestionRepository evaluateeEventQuestionRepository,
                                IEvaluateeRepository evaluateeRepository)
                                : base(loggingBroker)
        {
            this.loggingBroker = loggingBroker;
            this.userClaims = userClaims;

            this.userRepository = userRepository;
            this.evaluateeRepository = evaluateeRepository;
            this.evaluateeEventQuestionRepository = evaluateeEventQuestionRepository;
            this.evaluatorRepository = evaluatorRepository;

            this.userClaims = userClaims;
        }



        public async ValueTask<EventGroupsForEvaluationDto> GetEventGroupsForEvaluation(int evaluateeId)
        => await TryCatchAsync(async () =>
        {

            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.evaluator.ToString());


            var evaluatee = await evaluateeRepository.GetAsync(evaluateeId);
            var evaluator = await evaluatorRepository.GetAsync(evaluatee.EvaluatorId);
            var currentUser = await userRepository.GetAsync(userClaims.GetUserId());


            bool isForThisUser = evaluator.UserId == currentUser.Id;

            if (!isForThisUser)
            {
                throw new NotAuthorizedException("you cant evaluate this event");
            }

            var rowData = await
                evaluateeRepository.GetEvaluateeEventGroups(evaluatee.EvaluatorId);



            var mappedData = new EventGroupsForEvaluationDto()
            {
                CompletePercent = (evaluatee.AnsweredQuestionCount / rowData.TotalQuestionCount) * 100,
                EvaluateeId = evaluateeId,
                EvaluateeName = currentUser.SureName,
                EvaluateePosition = currentUser.Position,
                EventDueDate = rowData.EventEntity.DueDate,
                EventName = rowData.EventEntity.EventTitle,
                MaxAnswerPoint = rowData.EventEntity.RatePointTo,
                MinAnswerPoint = rowData.EventEntity.RatePointFrom,
                QuestionGroups = rowData.Groups.Select(i => new EventGroupForEvaluationDto()
                {
                    GroupId = i.Id,
                    order = rowData.Groups.IndexOf(i),
                    Title = i.GroupTitle

                }).ToList()
            };

            return mappedData;
        });


        public async ValueTask<List<QuestionForEvaluateeWhitAnswerDto>> getEvaluateeQuestionsForGroup
            (int evaluateeId, int questionGroupId)
        => await TryCatchAsync(async () =>
        {

            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.evaluator.ToString());


            var evaluatee = await evaluateeRepository.GetAsync(evaluateeId);
            var evaluator = await evaluatorRepository.GetAsync(evaluatee.EvaluatorId);


            bool isForThisUser = evaluator.UserId == userClaims.GetUserId();

            if (!isForThisUser)
            {
                throw new NotAuthorizedException("you cant evaluate this evaluatee");
            }

            var rowData = await
                evaluateeRepository.GetEvaluateeQuestionsAndAnswersInGroup(evaluatee.Id, questionGroupId);



            var mappedData = rowData.Select(i =>
                new QuestionForEvaluateeWhitAnswerDto()
                {
                    QuestionTitle = i.Question.QuestionTitle,
                    QuestionGroupId = i.Question.QuestionGroupId,
                    DescriptivAnswer = i.Answer?.DescriptivAnswer,
                    IsDescriptive = i.Question.IsDescriptive,
                    Point = i.Answer?.Point,
                    EventQuestionId = i.EventQuestionId
                }).ToList();

            return mappedData;
        });


        public async ValueTask<int> AddQuestionAnswersForEvaluatee
            (EvaluateeQuestionAnswersDto answersDto)
        => await TryCatchAsync(async () =>
        {

            userClaims.HaseClaimAndThrow(ConstantVaribels.ServiceUserRoles.evaluator.ToString());


            var evaluatee = await evaluateeRepository.GetAsync(answersDto.EvaluateeId);
            var evaluator = await evaluatorRepository.GetAsync(evaluatee.EvaluatorId);

            bool isForThisUser = evaluator.UserId == userClaims.GetUserId();

            if (!isForThisUser)
            {
                throw new NotAuthorizedException("you cant evaluate this evaluatee");
            }

            List<EvaluateeEventQuestion> oldAnswers = await
            evaluateeEventQuestionRepository.GetQuestionAnswers(
                answersDto.questionAnswersDtos.Select(i => i.EventQuestionId), answersDto.EvaluateeId);

            await evaluateeEventQuestionRepository.RemoveRangeAsync(oldAnswers);

            evaluatee.AnsweredQuestionCount -= oldAnswers.Count;


            List<EvaluateeEventQuestion> newAnswers = new List<EvaluateeEventQuestion>();

            foreach (var item in answersDto.questionAnswersDtos)
            {
                var temp = new EvaluateeEventQuestion()
                {
                    DescriptivAnswer = item.DescriptiveAnswer,
                    EvaluateeId = answersDto.EvaluateeId,
                    EventQuestionId = item.EventQuestionId,
                    Point = item.Point ?? 0,

                };

                newAnswers.Add(temp);
            }


            int result = await evaluateeEventQuestionRepository.AddRangeAsync(newAnswers);


            evaluatee.AnsweredQuestionCount += result;

            await evaluateeRepository.EditAsync(evaluatee);

            return result;

        });

    }
}
