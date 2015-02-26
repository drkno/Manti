using System;

namespace MantiCore.Bundles
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BundleInformation : Attribute
    {
        public BundleInformation(string bundleIdentifier)
        {
            if (bundleIdentifier.ToLower().StartsWith("manti"))
            {
                throw new InvalidBundleInformationException("Only internal bundle identifiers can start with \"manti\".");
            }
            BundleIdentifier = bundleIdentifier;
        }

        public string BundleIdentifier { get; protected set; }
        public string Name { get; set; }
        public string Authour { get; set; }
        public string Description { get; set; }
        public int Version { get; set; }
    }

    public class InvalidBundleInformationException : Exception
    {
        public InvalidBundleInformationException(string message, Exception innerException = null)
            : base(message, innerException) { }
    }
}