using System;
using System.Collections.Generic;

#nullable disable

namespace Evaluation.Domain
{
    public partial class EventQuestion
    :Entity{
        public EventQuestion()
        {
            EvaluateeEventQuestions = new HashSet<EvaluateeEventQuestion>();
        }

        public int Id { get; set; }
        public int EventId { get; set; }
        public int QuestionId { get; set; }

        public virtual Event Event { get; set; }
        public virtual Question Question { get; set; }
        public virtual ICollection<EvaluateeEventQuestion> EvaluateeEventQuestions { get; set; }
    }
}
