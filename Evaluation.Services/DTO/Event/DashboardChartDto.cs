using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Services.DTO
{
    public class DashboardChartDto
    {
        public int TotalEvents { get; set; }
        public int OnGoingEvents { get; set; }
        public int NotStartedEvents { get; set; }
        public int CompletedEvents { get; set; }


    }
}
