using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Services.DTO
{
    public class EvaluateeQuestionAnswersDto
    {
        public List<QuestionAnswersDto> questionAnswersDtos { get; set; } = new List<QuestionAnswersDto>();
        public int EvaluateeId { get; set; }
    }

    public class QuestionAnswersDto
{
        public int EventQuestionId { get; set; }
        public int? Point { get; set; }
        public string DescriptiveAnswer { get; set; }

    }
}
