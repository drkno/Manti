using System;

namespace CoreLoader.Plugins.Logging
{
    internal class LoggingPluginDefinition : IPluginDefinition
    {
        public string Name { get; } = "Console Logging";
        public string Version { get; } = "1.0";
        public string Description { get; } = "Provides coloured console output for Manti.";
        public Type PluginClass { get; } = typeof(ConsoleLoggingPlugin);
        public string[] PluginProvides { get; } = { "nz.co.makereti.manti.logging" };
    }
}
