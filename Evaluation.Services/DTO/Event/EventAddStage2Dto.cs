using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Services.DTO
{
    public class EventAddStage2Dto
    {
        public int EventId { get; set; }
        public List<int> QuestionIds { get; set; } = new List<int>();
    }

}
