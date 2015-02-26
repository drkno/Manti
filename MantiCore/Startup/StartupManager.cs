using System.Threading;
using MantiCore.Bundles;
using MantiCore.Startup.Arguments;

namespace MantiCore.Startup
{
    internal class StartupManager
    {
        private uint _referenceCounter;
        private readonly OptionSet _optionSet;
        public StartupManager(string[] args)
        {
            _optionSet = new OptionSet();
            _optionSet.Parse(args);
        }

        internal void StartBundle(Bundle bundle)
        {
            var arguments = BundleArgument.GetBundleArguments(bundle);
            _optionSet.SetBundleArguments(arguments);
            var thread = new Thread(Start);
            thread.Start(bundle);
        }

        private void Start(object obj)
        {
            var bundle = (Bundle) obj;
            bundle.StartBundle();
        }
    }
}
