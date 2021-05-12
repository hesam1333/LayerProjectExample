using System;

namespace Evaluation.Domain.Exceptions
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException(string message = null)
            : base(message ?? "You are not allowed to access here")
        { }
    }
}
