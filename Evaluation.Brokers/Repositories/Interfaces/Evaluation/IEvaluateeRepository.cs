using Evaluation.Domain;
using Evaluation.Brokers.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Evaluation.Brokers.Repositories.Interfaces
{


    public interface IEvaluateeRepository : IGenericRepository<Evaluatee>
    {
        ValueTask<List<UserEmploeeLastEventViewTable>> GetUsersNotEvaluatedFromThisMonthAsync(int formThisMonth);
        ValueTask<List<UserEmploeeTotalPointViewTable>> GetUsersWithTopEvaluateeScoreAsync(int take);
        ValueTask<GroupsForEventViewTable> GetEvaluateeEventGroups(int evaluatorId);

        ValueTask<List<QuestionsAndAnswersViewTable>> 
            GetEvaluateeQuestionsAndAnswersInGroup(int evaluatorId, int questionGroupId);
    }
}