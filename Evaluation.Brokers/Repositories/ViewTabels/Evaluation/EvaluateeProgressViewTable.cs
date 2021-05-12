using Evaluation.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Brokers.Repositories
{
    public class EvaluateeProgressViewTable
    {
        public int EvaluateeId { get; set; }

        public string EvaluateeName { get; set; }

        public int TotalQuestion { get; set; }

        public int TotalAnswred { get; set; }
    }
}
