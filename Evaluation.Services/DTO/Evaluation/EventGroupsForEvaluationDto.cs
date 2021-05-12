using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Services.DTO.Evaluation
{
    public class EventGroupsForEvaluationDto
    {
        public int EvaluateeId { get; set; }
        public string EvaluateeName { get; set; }
        public string EvaluateePosition { get; set; }
        public string EventName { get; set; }
        public int CompletePercent { get; set; }
        public DateTime? EventDueDate { get; set; }
        public int MinAnswerPoint { get; set; }
        public int MaxAnswerPoint { get; set; }
        public List<EventGroupForEvaluationDto> QuestionGroups { get; set; }

    }

    public class EventGroupForEvaluationDto
    {
        public int order { get; set; }
        public int GroupId { get; set; }
        public string Title { get; set; }
    }

    public class QuestionAnswerForEvaluationDto
    {
        public int EvaluateeQuestionId { get; set; }
        public int QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public bool IsDescriptive { get; set; }
        public string DescriptiveAnswer { get; set; }
        public int? Score { get; set; }

    }

}
