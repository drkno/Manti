using System;
using System.Collections.Generic;
using ImpromptuInterface;
using MediaCore.Search;

namespace MediaCore
{
    public class OuroborosMain : IDisposable
    {
        public OuroborosMain(dynamic platform)
        {
            IEnumerable<IMediaSourceSearch> plugins;
            platform.GetPlugins<IMediaSourceSearch>("nz.co.makereti.ouroboros.source", out plugins);
            foreach (var plugin in plugins)
            {
                var results = plugin.Search("Top Gear", (int)SearchCategory.Tv).AllActLike<ISearchResult>();
                foreach (var result in results)
                {
                    platform.Log.Info(result.Title);
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
