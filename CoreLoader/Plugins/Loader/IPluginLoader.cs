using System.Collections.Generic;

namespace CoreLoader.Plugins.Loader
{
    public interface IPluginLoader : IPlugin
    {
        string[] HandlesExtensions { get; }
        IEnumerable<IPluginDefinition> GetAvaliblePlugins(string searchLocation = null);
    }
}