using Evaluation.Domain;
using Evaluation.Brokers.Context; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Evaluation.Brokers.Repositories;

namespace Evaluation.Brokers.Repositories.Interfaces
{
    
    public interface IEventRepository : IGenericRepository<Event>
    {
        Task<List<EventProgressViewTable>> NotFinishedEventsProgressListAsync();

        Task<List<EventEvaluateeEvaluatorsViewTable>> EventEvaluateeEvaluatorListAsync
            (SortAndPagingParameters parameters);

        Task<List<EventQuestion>> GetEventQuestions(int eventId);

        Task<List<Evaluatee>> GetEventEvaluatees(int eventId);

        Task<List<Evaluator>> GetEventEvaluators(int eventId);

    }
}