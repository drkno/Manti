using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoreLoader.Exceptions;
using CoreLoader.Logger;
using CoreLoader.Plugins;
using CoreLoader.Plugins.Loader;

namespace CoreLoader.Core
{
    public class Platform : IDisposable
    {
        public Log Log { get; }
        private readonly PluginRepository _pluginRepository;

        public Platform()
        {
            _pluginRepository = new PluginRepository(this);
            Log = new Log(this);
        }

        private void CorrectAndVerifyInterfaceName(ref string name)
        {
            name = name.ToLower().Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Invalid plugin interface name.", nameof(name));
            }
        }

        public void GetPlugins<T>(string name, out IEnumerable<T> plugins) where T : class, IPlugin
        {
            try
            {
                CorrectAndVerifyInterfaceName(ref name);
                _pluginRepository.Get(name, out plugins);
            }
            catch (Exception e)
            {
                throw new NoRegisteredPluginsException(e);
            }
        }

        public void Dispose()
        {
            Log.Info("Shutting down platform.");
            foreach (var plugin in _pluginRepository.Plugins())
            {
                _pluginRepository.RemovePlugin(plugin);
            }
            Log.Info("Plugin shutdown complete.");
            Log.Warning("Core shutdown in progress. Have a nice day!");
            Log.Dispose();
        }

        public void Start()
        {
            Log.Info("Platform Starting Up");
            Log.Write("Loading Core Plugins");
            foreach (var pluginDefinition in DefaultPluginDefinitions.DefaultDefinitions)
            {
                InternalLoadPlugin(pluginDefinition);
            }
            Log.Write("Core Plugins Complete");

            LoadPlugin();

            IEnumerable<IITest> e;
            GetPlugins("nz.co.makereti.test", out e);
            Console.WriteLine(e.First().HelloWorld(133333337));
        }

        private void InternalLoadPlugin(IPluginDefinition pluginDefinition)
        {
            _pluginRepository.AddPlugin(pluginDefinition);
        }

        public void LoadPlugin(string searchLocation = "", string pluginName = "")
        {
            IEnumerable<IPluginLoader> loaderPlugins;
            GetPlugins("nz.co.makereti.manti.pluginloader", out loaderPlugins);
            var ext = Path.GetExtension(searchLocation)?.ToLower();
            var definitions = new List<IPluginDefinition>();
            foreach (var loaderPlugin in loaderPlugins)
            {
                if (!string.IsNullOrWhiteSpace(ext))
                {
                    if (!loaderPlugin.HandlesExtensions.Contains(ext)) continue;
                    definitions.AddRange(loaderPlugin.GetAvaliblePlugins(searchLocation));
                    break;
                }
                definitions.AddRange(loaderPlugin.GetAvaliblePlugins(searchLocation));
            }

            if (!string.IsNullOrWhiteSpace(pluginName))
            {
                definitions = definitions.Where(pluginDefinition => pluginDefinition.Name == pluginName).ToList();
            }

            foreach (var pluginDefinition in definitions)
            {
                InternalLoadPlugin(pluginDefinition);
            }
        }
    }
}