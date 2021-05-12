using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Services.DTO
{
    public class EventListDto
    {
        public int Id { get; set; }
        public string EventTitle { get; set; }
        public string Evaluatees { get; set; }
        public string Evaluators { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
