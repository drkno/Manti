using System;

namespace MantiCore.Bundle
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BundleInformation : Attribute
    {
        public BundleInformation(string bundleIdentifier)
        {
            BundleIdentifier = bundleIdentifier;
        }

        public string BundleIdentifier { get; protected set; }
        public string Name { get; set; }
        public string Authour { get; set; }
        public string Description { get; set; }
        public int Version { get; set; }
    }
}