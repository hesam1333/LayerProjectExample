using System;

namespace Evaluation.Domain.Exceptions
{
    public class InvalidException : Exception
    {
        public InvalidException(string Message )
            : base(Message ?? $"Invalid  error occurred")
        { }
    }

}
