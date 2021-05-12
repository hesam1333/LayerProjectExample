using Evaluation.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Brokers.Repositories
{
    public class EventProgressViewTable
    {
        public Event Event { get; set; }
        public int EvaluateDone { get; set; }
        public int TotalEvaluate { get; set; }

    }
}
