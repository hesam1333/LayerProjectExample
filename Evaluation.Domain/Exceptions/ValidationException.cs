using System;

namespace Evaluation.Domain.Exceptions
{
    public class ValidationException : Exception
    {

        public ValidationException(Exception innerException = null, string Message = null)
            : base(Message ?? " validation error occurred, correct your request and try again", innerException)
        { }

    }
}
