using System;
using CoreLoader.Core;

namespace CoreLoader.Plugins.Loader.MsilLoader
{
    internal class MsilPluginLoaderDefinition : IPluginDefinition
    {
        public string Name { get; } = "MSIL Loader";
        public string Version { get; } = "1.0";
        public string Description { get; } = "Loads plugins from MSIL assembly files (.dll, .exe).";
        public object[] Provides { get; } = { "nz.co.makereti.manti.pluginloader" };
        public dynamic CreateInstance(Platform platform)
        {
            return new MsilLoader(platform);
        }
    }
}