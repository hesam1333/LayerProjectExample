
using Evaluation.Domain;
using Evaluation.Brokers.Context;
using Evaluation.Brokers.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Evaluation.Brokers.Repositories
{

    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        private readonly EvaluationContext context;

        public QuestionRepository(EvaluationContext context) : base(context)
        {
            this.context = context ;
        }


    }
}