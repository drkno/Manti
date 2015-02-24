using System;

namespace MantiCore.Bundles
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class BundleArgument : Attribute
    {
        public BundleArgument(string description, params string[] options)
        {
            Options = options;
            Description = description;
        }

        public BundleArgument(string description, string[] argumentNames, params string[] options)
            : this(description, options)
        {
            ArgumentNames = argumentNames;
        }

        public void SetValue(string[] argumentValues)
        {
            Value = argumentValues;
        }

        public string[] Options { get; protected set; }
        public string[] ArgumentNames { get; protected set; }
        public string Description { get; protected set; }
        public bool IgnoreConflicts { get; set; }
        public string[] Value { get; protected set; }

        public static BundleArgument[] GetBundleArguments(Bundle bundle)
        {
            return (BundleArgument[])bundle.GetType().GetCustomAttributes(typeof(BundleArgument), true);
        }
    }
}