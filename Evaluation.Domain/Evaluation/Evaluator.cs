using System;
using System.Collections.Generic;

#nullable disable

namespace Evaluation.Domain
{
    public partial class Evaluator
    :Entity{
        public Evaluator()
        {
            Evaluatees = new HashSet<Evaluatee>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int EvaluateeCompletCount { get; set; }

        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Evaluatee> Evaluatees { get; set; }
    }
}
