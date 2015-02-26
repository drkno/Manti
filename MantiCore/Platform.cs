using System.Collections.Generic;
using MantiCore.Bundles;
using MantiCore.Core.Logging;
using MantiCore.Startup;
using MantiCore.Startup.Arguments;
using MantiCore.Startup.Load;

namespace MantiCore
{
    public class Platform
    {
        private List<Bundle> _stoppedBundles;
        private List<Bundle> _runningBundles;
        public Logging Log { get; private set; }
        private StartupManager _startupManager;

        public Platform(string[] args)
        {
            Log = new Logging();

            _stoppedBundles = new List<Bundle>();
            _runningBundles = new List<Bundle>();
            _startupManager = new StartupManager(args);
        }

        public void Startup()
        {
            BundleLoader loader = new BundleLoader();
            //_startupManager.StartBundle(loader);
        }

        public void Shutdown()
        {
            
        }

        public void PrintBundleArgumentsHelp()
        {
            //HelpPrinter.ShowHelp(_bundles);
        }

        internal void LoadBundle()
        {
            
        }

        internal void UnloadBundle()
        {
            
        }
    }
}
