using CoreLoader.Core;

namespace CoreLoader.Plugins
{
    public interface IPluginDefinition
    {
        string Name { get; }
        string Version { get; }
        string Description { get; }
        object[] Provides { get; }
        dynamic CreateInstance(Platform platform);
    }
}
