
using Evaluation.Domain;
using Evaluation.Brokers.Context;
using Evaluation.Brokers.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Evaluation.Brokers.Repositories
{

    public class EvaluateeRepository : GenericRepository<Evaluatee>, IEvaluateeRepository
    {
        private readonly EvaluationContext context;

        public EvaluateeRepository(EvaluationContext context) : base(context)
        {
            this.context = context;
        }

        public async ValueTask<GroupsForEventViewTable> GetEvaluateeEventGroups(int evaluatorId)
        {
            var temp = context.Evaluators.Find(evaluatorId);

            var eventEntity = context.Events.Find(temp.EventId);

            var model = new GroupsForEventViewTable()
            {
                EventEntity = eventEntity,
                TotalQuestionCount = context.EventQuestions.Count( i=> i.EventId == eventEntity.Id),
                Groups = await context.QuestionGroups.Where(i => context.EventQuestions.Any(
                    j => j.EventId == eventEntity.Id && j.Question.QuestionGroupId == i.Id
                    )).ToListAsync()
            };

            return model;
        }

        public async ValueTask<List<QuestionsAndAnswersViewTable>> GetEvaluateeQuestionsAndAnswersInGroup
            (int evaluateeId, int questionGroupId)
        {
            var evaluatee = await context.Evaluatees.FindAsync(evaluateeId);
            var evaluator = await context.Evaluators.FindAsync(evaluatee.EvaluatorId);

            var model = await 
                (from eventQuestion in context.EventQuestions.Where(i => i.EventId == evaluator.EventId)
                 from question in context.Questions.Where(i => i.Id == eventQuestion.QuestionId)
                 from questionAnswers in context.EvaluateeEventQuestions
                     .Where(i => i.EvaluateeId == evaluateeId && i.EventQuestionId == eventQuestion.Id)
                     .DefaultIfEmpty()
                 where question.QuestionGroupId == questionGroupId
                 select new QuestionsAndAnswersViewTable()
                 {
                     EventQuestionId = eventQuestion.Id ,
                     Answer = questionAnswers,
                     Question = question

                 }).ToListAsync();


            return model;
                
        }

        public async ValueTask<List<UserEmploeeLastEventViewTable>>
            GetUsersNotEvaluatedFromThisMonthAsync(int formThisMonth)
        {
            var lastAllowedMonth = DateTime.Now.Date.AddMonths(formThisMonth);

            var model = await (from User in context.Users
                               select new UserEmploeeLastEventViewTable()
                               {
                                   LastEventAsEvaluatee =
                                   (from eventEntity in context.Events.Where(i => !i.IsDelete)
                                    from evaluator in context.Evaluators.Where(i => i.EventId == eventEntity.Id)
                                    from evaluatee in context.Evaluatees.Where(i => i.EvaluatorId == evaluator.Id)
                                    where evaluatee.UserId == User.Id &&
                                          eventEntity.IsPublished
                                    select eventEntity).OrderByDescending(i => i.DueDate).FirstOrDefault(),
                                   User = User
                               }).Where(i => i.LastEventAsEvaluatee.DueDate < lastAllowedMonth)
                         .ToListAsync();

            return model;
        }

        public async ValueTask<List<UserEmploeeTotalPointViewTable>>
            GetUsersWithTopEvaluateeScoreAsync(int take)
        {


            var model = await (from User in context.Users
                               select new UserEmploeeTotalPointViewTable()
                               {
                                   TotalScore =
                                   (from eventEntity in context.Events
                                    from evaluator in context.Evaluators.Where(i => i.EventId == eventEntity.Id)
                                    from evaluatee in context.Evaluatees.Where(i => i.EvaluatorId == evaluator.Id)
                                    from evaluateeQuestion in context.EvaluateeEventQuestions
                                    .Where(i => i.EvaluateeId == evaluatee.Id)
                                    where evaluatee.UserId == User.Id
                                    select evaluateeQuestion.Point).Sum(i => i),
                                   User = User
                               }).OrderByDescending(i => i.TotalScore).Take(take)
                         .ToListAsync();

            return model;
        }
    }

}