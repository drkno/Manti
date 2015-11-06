using CoreLoader.Plugins.Loader.MsilLoader;
using CoreLoader.Plugins.Logging;
using CoreLoader.Plugins.Logging.Console;
using CoreLoader.Plugins.Logging.Disk;

namespace CoreLoader.Plugins
{
    internal static class DefaultPluginDefinitions
    {
        public static IPluginDefinition[] DefaultDefinitions { get; } = {
            new ConsoleLoggingPluginDefinition(),
            new DiskLoggingPluginDefinition(),
            new MsilPluginLoaderDefinition()
        };
    }
}
