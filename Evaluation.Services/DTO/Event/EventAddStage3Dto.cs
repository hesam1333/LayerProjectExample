using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Services.DTO
{
    public class EventAddStage3Dto
    {
        public int EventId { get; set; }
        public int RatePointFrom { get; set; }
        public int RatePointTo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public short? RepeatMonth { get; set; }
    }

}
