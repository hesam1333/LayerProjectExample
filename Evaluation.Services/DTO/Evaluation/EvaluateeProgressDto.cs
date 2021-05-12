using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Services.DTO
{
    public class EvaluateeProgressDto
    {
        public int EvaluateeId { get; set; }
        public string EvaluateeName { get; set; }
        public int CompletPercent { get; set; }
    }
}
