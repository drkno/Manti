using System;

namespace KickassTorrents
{
    public class KickassPluginDefinition
    {
        public string Name { get; } = "Kickass Torrents media source plugin.";
        public string Version { get; } = "1.0";
        public string Description { get; } = "Searches kickass torrents.";
        public Type Type { get; } = typeof(KickassTorrentSearcher);
        public string[] Provides { get; } = { "nz.co.makereti.ouroboros.source" };
    }
}
