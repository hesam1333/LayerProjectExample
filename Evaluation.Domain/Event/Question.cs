using System;
using System.Collections.Generic;

#nullable disable

namespace Evaluation.Domain
{
    public partial class Question
    :Entity{
        public Question()
        {
            EventQuestions = new HashSet<EventQuestion>();
        }

        public int Id { get; set; }
        public int QuestionGroupId { get; set; }
        public string QuestionTitle { get; set; }
        public bool IsDescriptive { get; set; }
        public bool IsDelete { get; set; }

        public virtual QuestionGroup QuestionGroup { get; set; }
        public virtual ICollection<EventQuestion> EventQuestions { get; set; }
    }
}
