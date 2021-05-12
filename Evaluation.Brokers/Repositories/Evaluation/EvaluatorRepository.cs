
using Evaluation.Domain;
using Evaluation.Brokers.Context;
using Evaluation.Brokers.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Brokers.Repositories
{

    public class EvaluatorRepository : GenericRepository<Evaluator>, IEvaluatorRepository
    {
        private readonly EvaluationContext context;

        public EvaluatorRepository(EvaluationContext context) : base(context)
        {
            this.context = context;
        }

        public Task<List<EvaluateeProgressViewTable>> GetEvaluatorEvaluatees(int userId, int eventId)
        {

            var items = (from evaluatee in context.Evaluatees
                         from evaluators in context.Evaluators.Where(i => i.Id == evaluatee.EvaluatorId)
                         from evet in context.Events.Where(i => i.Id == evaluators.EventId)
                         from user in context.Users.Where(i => i.Id == evaluatee.UserId)
                         where evaluators.UserId == userId && evaluators.EventId == eventId
                         select new EvaluateeProgressViewTable()
                         {
                             EvaluateeId = evaluatee.Id,
                             EvaluateeName = user.SureName,
                             TotalAnswred = evaluatee.AnsweredQuestionCount,
                             TotalQuestion = evet.EventQuestions.Count
                         }).ToListAsync();

            return items;

        }

        public async Task<List<EventProgressViewTable>> GetNotFinishedEventsListAsync(int userId)
        {
            var today = DateTime.Now.Date;

            List<EventProgressViewTable> model =
               await (from eventEntity in context.Events.Where(i => !i.IsDelete)
                      from evaluator in context.Evaluators.Where(
                          i => i.EventId == eventEntity.Id && i.UserId == userId)
                       .Include(i => i.Evaluatees)
                      where eventEntity.StartDate < today && eventEntity.DueDate > today

                      select new EventProgressViewTable()
                      {
                          Event = eventEntity,
                          TotalEvaluate = evaluator.Evaluatees.Count,
                          EvaluateDone = evaluator.Evaluatees.Count(
                              i => i.AnsweredQuestionCount == context.EventQuestions.Count(
                                    j => j.EventId == eventEntity.Id
                                  )
                              )
                      }).ToListAsync();

            return model;
        }
    }
}