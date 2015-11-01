using System;

namespace CoreLoader.Plugins.Loader.MsilLoader
{
    internal class MsilPluginLoaderDefinition : IPluginDefinition
    {
        public string Name { get; } = "MSIL Loader";
        public string Version { get; } = "1.0";
        public string Description { get; } = "Loads plugins from MSIL assembly files (.dll, .exe).";
        public Type PluginClass { get; } = typeof(MsilLoader);
        public string[] PluginProvides { get; } = { "nz.co.makereti.manti.pluginloader" };
    }
}