
using Evaluation.Domain;
using Evaluation.Brokers.Context;
using Evaluation.Brokers.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Evaluation.Brokers.Repositories;

namespace Evaluation.Brokers.Repositories
{

    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        private readonly EvaluationContext context;

        public EventRepository(EvaluationContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<EventEvaluateeEvaluatorsViewTable>> EventEvaluateeEvaluatorListAsync
            (SortAndPagingParameters parameters)
        {
            var result =
               await (from eventEntity in context.Events.Where(i => !i.IsDelete)
                 select new EventEvaluateeEvaluatorsViewTable()
                 {
                     Id  = eventEntity.Id ,
                     DueDate = eventEntity.DueDate,
                     EntryDatetime = eventEntity.EntryDatetime,
                     EventTitle = eventEntity.EventTitle,
                     NextStartDate = eventEntity.NextStartDate,
                     RatePointFrom = eventEntity.RatePointFrom,
                     RatePointTo = eventEntity.RatePointTo,
                     RepeatMonth = eventEntity.RepeatMonth,
                     StartDate = eventEntity.StartDate,

                     Evaluators = eventEntity.Evaluators.Select(i => i.User),
                     Evaluetees = context.Evaluatees.Where(i => eventEntity.Evaluators.Any(
                            j => j.Id == i.EvaluatorId
                         )).Select(i => i.User).AsEnumerable()
                 }).ToListAsync();

            return result;
        }

        public async Task<List<EventProgressViewTable>> NotFinishedEventsProgressListAsync()
        {
            var today = DateTime.Now.Date;

            List<EventProgressViewTable> model =
               await (from eventEntity in context.Events.Where(i => !i.IsDelete).Include(i => i.Evaluators)
                      where eventEntity.StartDate < today && eventEntity.DueDate > today
                      select new EventProgressViewTable()
                      {
                          Event = eventEntity,
                          TotalEvaluate = eventEntity.Evaluators.Count,
                          EvaluateDone = eventEntity.Evaluators
                          .Count(i => i.EvaluateeCompletCount == i.Evaluatees.Count)
                      }).ToListAsync();

            return model;
        }

        public async Task<List<Evaluator>> GetEventEvaluators(int eventId)
        {
            var today = DateTime.Now.Date;

            List<Evaluator> model =
               await context.Evaluators.Where(i => i.EventId == eventId).ToListAsync();

            return model;
        }


        public async Task<List<Evaluatee>> GetEventEvaluatees(int eventId)
        {
            var today = DateTime.Now.Date;

            List<Evaluatee> model =
               await context.Evaluatees.Include(i => i.EvaluateeEventQuestions).Where(i => i.Evaluator.EventId == eventId).ToListAsync();

            return model;
        }


        public async Task<List<EventQuestion>> GetEventQuestions(int eventId)
        {
            var today = DateTime.Now.Date;

            List<EventQuestion> model =
               await context.EventQuestions.Where(i => i.EventId == eventId).ToListAsync();

            return model;
        }

    }
}