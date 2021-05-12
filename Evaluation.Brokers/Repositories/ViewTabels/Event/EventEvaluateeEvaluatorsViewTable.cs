using Evaluation.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Brokers.Repositories
{
    public class EventEvaluateeEvaluatorsViewTable
    {
        public int Id { get; set; }
        public DateTime EntryDatetime { get; set; }
        public string EventTitle { get; set; }
        public int RatePointFrom { get; set; }
        public int RatePointTo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public short? RepeatMonth { get; set; }
        public DateTime? NextStartDate { get; set; }
        public IEnumerable<User> Evaluetees { get; set; } 
        public IEnumerable<User> Evaluators { get; set; } 
    }
}
