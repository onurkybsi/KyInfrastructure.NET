using System;

namespace KybInfrastructure.Core.UtilityExceptions
{
    /// <summary>
    /// Represents exceptions which are occurred when a module is loading
    /// </summary>
    public class ModuleLoadingException : Exception
    {
        public ModuleLoadingException() : base() { }
        public ModuleLoadingException(string errorMessage) : base(errorMessage) { }
    }
}
