using System;

namespace MediaCore
{
    // ReSharper disable UnusedMember.Global
    public class OuroborosCorePluginDefinition
    {
        public string Name { get; } = "Ouroboros Core Plugin";
        public string Version { get; } = "1.0";
        public string Description { get; } = "Core of the media server.";
        public Type Type { get; } = typeof(OuroborosMain);
        public string[] Provides { get; } = { "nz.makereti.manti.pluginmain" };
    }
}
