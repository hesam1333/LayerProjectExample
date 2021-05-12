using Evaluation.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Brokers.Repositories
{
    public class QuestionsAndAnswersViewTable
    {
        public Question Question { get; set; }

        public EvaluateeEventQuestion Answer { get; set; }

        public int EventQuestionId { get; set; }

    }
}
