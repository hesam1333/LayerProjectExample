using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Services.DTO
{
    public class EventAddStage1Dto
    {
        public String EventTitle { get; set; }
        public List<EventAddEvaluatorDto> evaluatorDtos { get; set; } =
            new List<EventAddEvaluatorDto>();
        public List<EventAddEvaluateeDto> evaluateeDtos { get; set; } =
            new List<EventAddEvaluateeDto>();
    }

    public class EventAddEvaluatorDto
    {
        public int? UserId { get; set; }
        public string SureName { get; set; }
        public string position { get; set; }
        public string Email { get; set; }
    }

    public class EventAddEvaluateeDto
    {
        public int? UserId { get; set; }
        public string SureName { get; set; }
        public string position { get; set; }
    }

}
