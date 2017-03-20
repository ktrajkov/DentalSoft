using System;

namespace DentalSoft.Data.Services.Exceptions
{
    public class NonUniqueEntityException : Exception
    {
        public NonUniqueEntityException() { }

        public NonUniqueEntityException(string message)
            : base(message) { }

        public NonUniqueEntityException(string message, Exception inner)
            : base(message, inner) { }
    }
}
