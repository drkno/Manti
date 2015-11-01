using System;

namespace CoreLoader.Exceptions
{
    internal class PlatformSecurityException : Exception
    {
        public PlatformSecurityException(string onlyTheCorePlatformCanAccessAndAlterCorePlugins)
        {
            throw new NotImplementedException();
        }
    }
}