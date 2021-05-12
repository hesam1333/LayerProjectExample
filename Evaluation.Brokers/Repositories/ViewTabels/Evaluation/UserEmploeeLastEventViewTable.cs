using Evaluation.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Brokers.Repositories
{
    public class UserEmploeeLastEventViewTable
    {
        public User User { get; set; }
        public Event LastEventAsEvaluatee { get; set; }

    }
}
