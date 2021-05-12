using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Services.DTO
{
    public class OnGoingEventDto
    {
        public int Id { get; set; }
        public string EventTitle { get; set; }
        public int CompletPercent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
