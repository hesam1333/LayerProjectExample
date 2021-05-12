using System;

namespace Evaluation.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(int Id)
            : base($"Could not find item with ID: {Id}")
        { }
    }
}
