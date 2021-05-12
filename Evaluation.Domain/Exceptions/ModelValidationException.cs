using System;

namespace Evaluation.Domain.Exceptions
{
    public class ModelValidationException : Exception
    {
        public ValidationError validationError;

        public ModelValidationException(Exception innerException , ValidationError validationError )
            : base(" validation error occurred, correct your request and try again", innerException)
        {
            this.validationError = validationError;
        }

    }
}
