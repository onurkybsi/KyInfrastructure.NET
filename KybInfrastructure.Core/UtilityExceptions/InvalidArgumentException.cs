using System;

namespace KybInfrastructure.Core
{
    /// <summary>
    /// Exception that specify the arguments of a method is not valid
    /// </summary>
    public class InvalidArgumentException : Exception
    {
        /// <summary>
        /// Exception that specify the arguments of a method is not valid
        /// </summary>
        /// <param name="invalidArgumentName">Argument name that has invalid value</param>
        /// <param name="argumentValue">Value of invalid argument</param>
        public InvalidArgumentException(string invalidArgumentName, object argumentValue)
            : base(string.Format("Invalid argument given: {0}: {1}", invalidArgumentName, argumentValue)) { }
    }
}
