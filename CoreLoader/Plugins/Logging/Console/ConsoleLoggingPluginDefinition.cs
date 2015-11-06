using CoreLoader.Core;

namespace CoreLoader.Plugins.Logging.Console
{
    internal class ConsoleLoggingPluginDefinition : IPluginDefinition
    {
        public string Name { get; } = "Console Logging";
        public string Version { get; } = "1.0";
        public string Description { get; } = "Provides coloured console output for Manti.";
        public object[] Provides { get; } = { "nz.co.makereti.manti.logging" };
        public dynamic CreateInstance(Platform platform)
        {
            return new ConsoleLoggingPlugin(platform);
        }
    }
}
