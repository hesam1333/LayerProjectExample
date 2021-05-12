using System;

namespace Evaluation.Domain.Exceptions
{
    public class ServiceException : Exception
    {
        public ServiceException(Exception innerException)
            : base("System error occurred, contact support.", innerException)
        { }
    }
}
