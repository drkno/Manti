using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MantiCore.Bundles;

namespace MantiCore.Startup.Load
{
    internal class DllBundleLoader
    {
        public Bundle[] GetBundles(string directory, string dllBundleExtension = "dll", SearchOption option = SearchOption.TopDirectoryOnly)
        {
            var dlls = Directory.EnumerateFiles(directory, "*." + dllBundleExtension, option);
            foreach (var dll in dlls)
            {
                
            }
            return null;
        }

        public Bundle GetBundle(string bundleDll)
        {
            return null;
        }
    }
}
