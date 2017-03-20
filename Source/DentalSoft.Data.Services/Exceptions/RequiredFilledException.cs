using System;

namespace DentalSoft.Data.Services.Exceptions
{
    public class RequiredFilledException : Exception
    {
        public RequiredFilledException() { }

        public RequiredFilledException(string message)
            : base(message) { }

        public RequiredFilledException(string message, Exception inner)
            : base(message, inner) { }
    }
}
