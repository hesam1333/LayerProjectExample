using System;
using System.Collections.Generic;

#nullable disable

namespace Evaluation.Domain
{
    public partial class EvaluateeEventQuestion
    :Entity{
        public int Id { get; set; }
        public int EventQuestionId { get; set; }
        public int EvaluateeId { get; set; }
        public int Point { get; set; }
        public string DescriptivAnswer { get; set; }

        public virtual Evaluatee Evaluatee { get; set; }
        public virtual EventQuestion EventQuestion { get; set; }
    }
}
