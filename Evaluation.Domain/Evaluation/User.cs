using System;
using System.Collections.Generic;

#nullable disable

namespace Evaluation.Domain
{
    public partial class User
    : Entity
    {
        public User()
        {
            Evaluatees = new HashSet<Evaluatee>();
            Evaluators = new HashSet<Evaluator>();
        }

        public int Id { get; set; }
        public string SureName { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsFirstTime { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Evaluatee> Evaluatees { get; set; }
        public virtual ICollection<Evaluator> Evaluators { get; set; }
    }
}
