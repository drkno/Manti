using System;

namespace _1337x
{
    public class T1337XPluginDefinition
    {
        public string Name { get; } = "1337x media source plugin.";
        public string Version { get; } = "1.0";
        public string Description { get; } = "Searches 1337x torrents.";
        public Type Type { get; } = typeof(X1337XTorrentSearcher);
        public string[] Provides { get; } = { "nz.co.makereti.ouroboros.source" };
    }
}
