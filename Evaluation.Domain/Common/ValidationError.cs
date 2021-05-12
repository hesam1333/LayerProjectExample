using System;
using System.Collections.Generic;
using System.Text;

namespace Evaluation.Domain
{
    public class ValidationError
    {
        public List<VaidationErrorItem> Errors { get; set; } = new List<VaidationErrorItem>();
    }

    public class VaidationErrorItem
    {
        public string FieldName { get; set; }
        public string FieldErrorMessage { get; set; }
    }
}
