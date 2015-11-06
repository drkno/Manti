using System;
using CoreLoader.Core;

namespace CoreLoader.Plugins.Loader.NodeJsLoader
{
    internal class NodeJsPluginLoaderDefinition : IPluginDefinition
    {
        public string Name { get; } = "NodeJS Loader";
        public string Version { get; } = "1.0";
        public string Description { get; } = "Loads plugins from Node.js files (.js).";
        public object[] Provides { get; } = { "nz.co.makereti.manti.pluginloader" };
        public dynamic CreateInstance(Platform platform)
        {
            return new NodeJsLoader(platform);
        }
    }
}
