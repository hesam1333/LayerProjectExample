using System;
using System.Collections.Generic;

#nullable disable

namespace Evaluation.Domain
{
    public partial class QuestionGroup
    :Entity{
        public QuestionGroup()
        {
            Questions = new HashSet<Question>();
        }

        public int Id { get; set; }
        public string GroupTitle { get; set; }
        public bool IsDelete { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
