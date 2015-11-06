using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using CoreLoader.Core;
using CoreLoader.Exceptions;
using CoreLoader.Logger;
using EdgeJs;
using ImpromptuInterface;

namespace CoreLoader.Plugins.Loader.NodeJsLoader
{
    public class NodeJsLoader : IPluginLoader
    {
        private readonly Log _log;
        public NodeJsLoader(Platform platform)
        {
            _log = platform.Log;
        }

        public string[] HandlesExtensions { get; } = {"nodeplugin"};

        private static dynamic LoadPlugin(Platform platform, string file)
        {
            dynamic obj = Edge.Func(@"
                    return function(data, callback) {
                        try {
                            var module = require(data.Module),
                                instance = new module(data.Platform);
                            callback(null, instance);
                        }
                        catch(e) {
                            callback(e, e);
                        }
                    };
                ").Invoke(new {Module = file, Platform = platform}).Result;
            foreach (KeyValuePair<string, object> kvp in obj)
                Console.WriteLine(kvp.Key);
            Console.WriteLine(obj.HelloWorld.Invoke(42).Result);
            return obj;
        }

        private void LoadFile(string file, ref List<IPluginDefinition> collection)
        {
            try
            {
                var defFile = Path.Combine(file, "plugin.json");
                if (!File.Exists(defFile))
                {
                    throw new FileNotFoundException("Plugins definition file not found.", defFile);
                }
                dynamic definition = Edge.Func(@"
                    return function(data, callback) {
                        try {
                            callback(null, require(data));
                        }
                        catch(e) {
                            callback(e, e);
                        }
                    };
                ").Invoke(defFile).Result;
                definition.CreateInstance = new Func<Platform, dynamic>(platform => LoadPlugin(platform, Path.Combine(file, definition.PluginFile)));
                var def = Impromptu.ActLike<IPluginDefinition>(definition);
                collection.Add(def);
            }
            catch (Exception e)
            {
                _log.Error("Failed to load NodeJS plugin " + file + ". Failed with error: " + e);
            }
        }

        public IEnumerable<IPluginDefinition> GetAvaliblePlugins(string searchLocation = null)
        {
            if (string.IsNullOrWhiteSpace(searchLocation))
            {
                _log.Warning("No search location provided. Defaulting to current directory.");
                searchLocation = Path.Combine(Environment.CurrentDirectory, "plugins");
            }

            _log.Info("Getting Node.js plugins from " + searchLocation);

            if (Directory.Exists(searchLocation))
            {
                var files = Directory.EnumerateDirectories(searchLocation, "*.*", SearchOption.TopDirectoryOnly)
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

            _log.Error("No Node.js plugins at location \"" + searchLocation + "\"");
            throw new NoSuchSearchLocationException(searchLocation);
        }

        public void Dispose()
        {
            _log.Warning("Unloading NodeJS Loader plugin.");
        }
    }
}
