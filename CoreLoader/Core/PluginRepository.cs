using System.Collections.Generic;
using System.Linq;
using CoreLoader.Plugins;
using ImpromptuInterface;

namespace CoreLoader.Core
{
    internal class PluginRepository
    {
        private class ReferenceManager
        {
            public uint Count { get; set; }
            public dynamic Instance { get; set; }
            public IPluginDefinition Definition { get; set; }
        }

        private readonly Dictionary<string, List<object>> _pluginContainers;
        private readonly Dictionary<string, List<ReferenceManager>> _references;
        private readonly Platform _platform;

        internal PluginRepository(Platform platform)
        {
            _platform = platform;
            _platform.Log?.Write("Starting Plugin Repository");
            _pluginContainers = new Dictionary<string, List<object>>();
            _references = new Dictionary<string, List<ReferenceManager>>();
        }

        internal void Get<T>(string iface, out IEnumerable<T> plugins) where T : class
        {
            _platform.Log?.Write("Plugins for " + iface + " requested.");
            List<object> p;
            if (!_pluginContainers.TryGetValue(iface, out p))
            {
                p = GetAssociatedPlugins(iface);
            }
            plugins = p.AllActLike<T>();
        }

        private List<object> GetAssociatedPlugins(string iface)
        {
            var p = new List<object>();
            _pluginContainers[iface] = p;

            List<ReferenceManager> ifaceRefs;
            if (!_references.TryGetValue(iface, out ifaceRefs)) return p;
                
            foreach (var r in ifaceRefs)
            {
                if (r.Instance == null)
                {
                    r.Instance = r.Definition.CreateInstance(_platform);
                }
                r.Count++;
                p.Add(r.Instance);
            }

            return p;
        }

        internal void Destroy(string iface)
        {
            _platform.Log?.Write("Removing list of " + iface + " plugins.");
            if (!_pluginContainers.Remove(iface)) return;
            foreach (var r in _references[iface])
            {
                r.Count--;
                if (r.Count == 0)
                {
                    r.Instance.Dispose();
                }
                r.Instance = null;
            }
        }

        internal void AddPlugin(IPluginDefinition definition)
        {
            _platform.Log?.Write("Adding to repo \"" + definition.Name + "\" (" + definition.Version + ")");
            var r = new ReferenceManager {Definition = definition};
            foreach (var pl in definition.Provides)
            {
                var p = (string) pl;
                if (!_references.ContainsKey(p))
                {
                    _references[p] = new List<ReferenceManager>();
                }
                _references[p].Add(r);
                if (!_pluginContainers.ContainsKey(p)) continue;
                if (r.Instance == null)
                {
                    r.Instance = r.Definition.CreateInstance(_platform);
                }
                _pluginContainers[p].Add(r.Instance);
                r.Count++;
            }
        }

        internal void RemovePlugin(IPluginDefinition definition)
        {
            _platform.Log?.Info("Removing \"" + definition.Name + "\" from repo.");
            var re = _references[(string) definition.Provides[0]];
            var r = re.First(refe => refe.Definition == definition);
            if (r.Count > 0)
            {
                foreach (var pl in definition.Provides)
                {
                    var p = (string) pl;
                    _pluginContainers[p].Remove(r.Instance);
                }
                r.Instance.Dispose();
                r.Instance = null;
            }
            foreach (var pl in definition.Provides)
            {
                var p = (string)pl;
                _references[p].Remove(r);
                if (_references[p].Count == 0)
                {
                    _references.Remove(p);
                }
            }
        }

        public IEnumerable<string> Interfaces()
        {
            return _references.Keys.ToArray();
        }

        public IEnumerable<IPluginDefinition> Plugins()
        {
            var pluginDefinitions = new List<IPluginDefinition>();
            foreach (var r in from value in _references.Values
                              from r in value where !pluginDefinitions.Contains(r.Definition) select r)
            {
                pluginDefinitions.Add(r.Definition);
            }
            return pluginDefinitions;
        }
    }
}
