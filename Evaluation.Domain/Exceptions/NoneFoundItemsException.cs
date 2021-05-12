using System;

namespace Evaluation.Domain.Exceptions
{
    public class NoneFoundItemsException : Exception
    {
        public NoneFoundItemsException()
            : base($"Could not find any items")
        { }
    }
}
