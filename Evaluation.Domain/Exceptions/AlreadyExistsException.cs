using System;

namespace Evaluation.Domain.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(Exception innerException) 
            : base("This  already exists", innerException)
        { }
    }
}
