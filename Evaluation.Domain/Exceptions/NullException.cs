using System;

namespace Evaluation.Domain.Exceptions
{
    public class NullException : Exception
    {
        public NullException() : base(" item is Null") 
        { }
    }
}
