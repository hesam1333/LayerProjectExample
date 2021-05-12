using Evaluation.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Brokers.Repositories
{
    public class GroupsForEventViewTable
    {
        public Event  EventEntity { get; set; }
        public int TotalQuestionCount { get; set; }
        public List<QuestionGroup> Groups { get; set; }

    }

}
