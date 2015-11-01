using System;

namespace CoreLoader.Plugins
{
    public interface IPluginDefinition
    {
        string Name { get; }
        string Version { get; }
        string Description { get; }
        Type PluginClass { get; }
        string[] PluginProvides { get; }
    }
}
