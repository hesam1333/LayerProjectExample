using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Brokers.Repositories
{
    public class QuestionGroupViewTable
    {
        public int QuestionGroupId { get; set; }
        public string QuestionGroupTitle { get; set; }
        public IEnumerable<QuestiontDto> Questions { get; set; }

    }

    public class QuestiontDto
    {
        public int QuestionId { get; set; }
        public int QuestionGroupId { get; set; }
        public string QuestionTitle { get; set; }
        public bool IsDescriptive { get; set; }
        public bool IsInEvent { get; set; }
    }
}
