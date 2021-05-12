using System;

namespace Evaluation.Domain.Exceptions
{
    public class DependencyException : Exception
    {
        public DependencyException(Exception innerException)
            : base("Service dependency error occurred, contact support", innerException)
        { }
    }
}
