using System;
using MantiCore.Bundles;

namespace MantiCore.Core.Logging
{
    public abstract class LoggingBackend : LoadableBundle
    {
        public abstract bool DisableInternalLogging { get; }
        public abstract bool SupportsColour { get; }
        public abstract ConsoleColor ForegroundColour { get; set; }
        public abstract void Write(params object[] obj);
    }
}
