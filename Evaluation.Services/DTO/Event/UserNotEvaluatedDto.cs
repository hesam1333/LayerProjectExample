using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Services.DTO
{
    public class UserNotEvaluatedDto
    {
        public string SureName { get; set; }
        public string Position { get; set; }
        public DateTime? LastEvaluationDateTime { get; set; }
    }
}
