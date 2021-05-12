
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

    public class EvaluateeEventQuestionRepository : GenericRepository<EvaluateeEventQuestion>, IEvaluateeEventQuestionRepository
    {
        private readonly EvaluationContext context;

        public EvaluateeEventQuestionRepository(EvaluationContext context) : base(context)
        {
            this.context = context ;
        }

        public async Task<List<EvaluateeEventQuestion>> GetQuestionAnswers(IEnumerable<int> eventQuestionIds, int evaluateeId)
        {
            var model = await context.EvaluateeEventQuestions.Where(i =>

                        eventQuestionIds.Contains(i.EventQuestionId) &&
                        i.EvaluateeId == evaluateeId

                ).ToListAsync();

            return model;
        }
    }
}