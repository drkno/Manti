using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using CoreLoader.Core;
using CoreLoader.Exceptions;
using CoreLoader.Logger;
using ImpromptuInterface;

namespace CoreLoader.Plugins.Loader.MsilLoader
{
    public class MsilLoader : IPluginLoader
    {
        private readonly Log _log;
        public MsilLoader(Platform platform)
        {
            _log = platform.Log;
        }

        public string[] HandlesExtensions { get; } = {"dll", "exe"};

        private static dynamic LoadPlugin(Platform platform, Type type)
        {
            return Activator.CreateInstance(type, platform);
        }

        private void LoadFile(string file, ref List<IPluginDefinition> collection)
        {
            try
            {
                var assem = Assembly.LoadFrom(file);
                foreach (var type in assem.GetTypes()
                    .Where(t => !t.IsInterface && !t.IsAbstract && t.Name.EndsWith("PluginDefinition")))
                {
                    try
                    {
                        dynamic obj = Activator.CreateInstance(type).MakeExtensable();
                        obj.CreateInstance = new Func<Platform, dynamic>(platform => LoadPlugin(platform, obj.Type));
                        var pluginDefinition = Impromptu.ActLike<IPluginDefinition>(obj);
                        collection.Add(pluginDefinition);
                    }
                    catch (Exception e)
                    {
                        _log.Error("Couldn't load " + file + " as MSIL plugin. Failed with error: " + e);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public IEnumerable<IPluginDefinition> GetAvaliblePlugins(string searchLocation = null)
        {
            if (string.IsNullOrWhiteSpace(searchLocation))
            {
                _log.Warning("No search location provided. Defaulting to current directory.");
                searchLocation = Path.Combine(Environment.CurrentDirectory, "plugins");
            }

            _log.Info("Getting plugins from " + searchLocation);

            if (Directory.Exists(searchLocation))
            {
                var files = Directory.EnumerateFiles(searchLocation, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(f => HandlesExtensions.Any(f.EndsWith));
                var plugins = new List<IPluginDefinition>();
                foreach (var file in files)
                {
                    LoadFile(file, ref plugins);
                }
                return plugins;
            }

            if (File.Exists(searchLocation))
            {
                var plugins = new List<IPluginDefinition>();
                LoadFile(searchLocation, ref plugins);
                return plugins;
            }

            _log.Error("No MSIL plugins at location \"" + searchLocation + "\"");
            throw new NoSuchSearchLocationException(searchLocation);
        }

        public void Dispose()
        {
            _log.Warning("Unloading MSIL Loader plugin.");
        }
    }
}
