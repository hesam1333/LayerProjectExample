using System;
using System.Collections.Generic;

#nullable disable

namespace Evaluation.Domain
{
    public partial class Event
    :Entity{
        public Event()
        {
            Evaluators = new HashSet<Evaluator>();
            EventQuestions = new HashSet<EventQuestion>();
        }

        public int Id { get; set; }
        public DateTime EntryDatetime { get; set; }
        public string EventTitle { get; set; }
        public int RatePointFrom { get; set; }
        public int RatePointTo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public short? RepeatMonth { get; set; }
        public DateTime? NextStartDate { get; set; }
        public bool IsPublished { get; set; }
        public bool IsDelete { get; set; }

        public virtual ICollection<Evaluator> Evaluators { get; set; }
        public virtual ICollection<EventQuestion> EventQuestions { get; set; }
    }
}
