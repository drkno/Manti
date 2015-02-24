using System;

namespace MantiCore.Bundles
{
    public class BundleException : Exception
    {
        public Bundle Bundle { get; private set; }

        public BundleException(Bundle caller, string message, Exception innerException = null)
            : base(message, innerException)
        {
            Bundle = caller;
        }
    }
}