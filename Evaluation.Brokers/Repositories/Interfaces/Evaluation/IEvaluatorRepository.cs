using Evaluation.Domain;
using Evaluation.Brokers.Context; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Evaluation.Brokers.Repositories.Interfaces
{

    public interface IEvaluatorRepository : IGenericRepository<Evaluator>
    {
        Task<List<EventProgressViewTable>> GetNotFinishedEventsListAsync(int userId);

        Task<List<EvaluateeProgressViewTable>> GetEvaluatorEvaluatees(int userId, int eventId);
    }
}