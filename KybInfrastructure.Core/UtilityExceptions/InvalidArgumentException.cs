using System;

namespace KybInfrastructure.Core
{
    public class InvalidArgumentException : Exception
    {
        public InvalidArgumentException(string invalidArgumentName, object argumentValue)
            : base(string.Format("Invalid argument given: {0}: {1}", invalidArgumentName, argumentValue)) { }
    }
}
