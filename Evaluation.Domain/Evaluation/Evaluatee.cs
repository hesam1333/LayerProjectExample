using System;
using System.Collections.Generic;

#nullable disable

namespace Evaluation.Domain
{
    public partial class Evaluatee
    :Entity{
        public Evaluatee()
        {
            EvaluateeEventQuestions = new HashSet<EvaluateeEventQuestion>();
        }

        public int Id { get; set; }
        public int EvaluatorId { get; set; }
        public int UserId { get; set; }
        public int AnsweredQuestionCount { get; set; }

        public virtual Evaluator Evaluator { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<EvaluateeEventQuestion> EvaluateeEventQuestions { get; set; }
    }
}
