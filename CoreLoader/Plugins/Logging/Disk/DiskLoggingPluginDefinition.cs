using CoreLoader.Core;

namespace CoreLoader.Plugins.Logging.Disk
{
    internal class DiskLoggingPluginDefinition : IPluginDefinition
    {
        public string Name { get; } = "Disk Logging";
        public string Version { get; } = "1.0";
        public string Description { get; } = "Provides persistent output for Manti.";
        public object[] Provides { get; } = { "nz.co.makereti.manti.logging" };
        public dynamic CreateInstance(Platform platform)
        {
            return new DiskLoggingPlugin(platform);
        }
    }
}
