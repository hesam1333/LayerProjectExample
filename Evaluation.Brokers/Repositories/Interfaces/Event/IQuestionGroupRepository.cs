using Evaluation.Domain;
using Evaluation.Brokers.Context; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Evaluation.Brokers.Repositories.Interfaces
{
    
    public interface IQuestionGroupRepository : IGenericRepository<QuestionGroup>
    {
        ValueTask<List<QuestionGroupViewTable>> GetQuestionGroupViewTablesForEvent(int eventId);
    }
}