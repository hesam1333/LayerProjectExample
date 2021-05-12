using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Services.DTO
{
    public class QuestionForEvaluateeWhitAnswerDto
    {
        public int EventQuestionId { get; set; }
        public int QuestionGroupId { get; set; }
        public string QuestionTitle { get; set; }
        public bool IsDescriptive { get; set; }
        public int? Point { get; set; }
        public string DescriptivAnswer { get; set; }
    }
}
