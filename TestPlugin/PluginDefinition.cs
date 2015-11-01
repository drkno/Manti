using System;

namespace TestPlugin
{
    public class PluginDefinition
    {
        public string Name { get; } = "Test Plugin";
        public string Version { get; } = "1.0";
        public string Description { get; } = "This is a test description.";
        public Type PluginClass { get; } = typeof (Class1);
        public string[] PluginProvides { get; } = { "nz.co.makereti.test" };
    }
}
