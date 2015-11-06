using System;

namespace TestPlugin
{
    public class PluginDefinition
    {
        public string Name { get; } = "Test Plugin";
        public string Version { get; } = "1.0";
        public string Description { get; } = "This is a test description.";
        public Type Type { get; } = typeof (Class1);
        public string[] Provides { get; } = { "nz.co.makereti.test" };
    }
}
