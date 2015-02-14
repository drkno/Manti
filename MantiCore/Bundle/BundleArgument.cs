using System;

namespace MantiCore.Bundle
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class BundleArgument : Attribute
    {
        public BundleArgument(string description, params string[] arguments)
        {
            Arguments = arguments;
            Description = description;
        }

        public void SetValue(string[] argumentValues)
        {
            Value = argumentValues;
        }

        public string[] Arguments { get; protected set; }
        public string Description { get; protected set; }
        public bool IgnoreConflicts { get; set; }
        public string[] Value { get; protected set; }
    }
}