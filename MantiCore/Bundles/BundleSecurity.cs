using System;

namespace MantiCore.Bundles
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class BundleSecurity : Attribute
    {
        public Level SecurityLevel { get; private set; }

        public enum Level
        {
            Server,
            Platform,
            Self
        }

        public BundleSecurity(Level securityLevel)
        {
            SecurityLevel = securityLevel;
        }
    }
}
