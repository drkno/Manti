using CoreLoader.Plugins.Loader.MsilLoader;
using CoreLoader.Plugins.Logging;

namespace CoreLoader.Plugins
{
    internal static class DefaultPluginDefinitions
    {
        public static IPluginDefinition[] DefaultDefinitions { get; } = {
            new LoggingPluginDefinition(),
            new MsilPluginLoaderDefinition()
        };
    }
}
