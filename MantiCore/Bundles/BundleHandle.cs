using System;
using System.Threading;

namespace MantiCore.Bundles
{
    public class BundleHandle
    {
        private readonly Bundle _bundle;
        private readonly Thread _bundleThread;

        public BundleStatus Status { get; private set; }

        public BundleHandle(Bundle bundle)
        {
            _bundle = bundle;
            _bundleThread = new Thread(Startup);
            Status = BundleStatus.Stopped;
        }

        public void Start()
        {
            if (_bundleThread.IsAlive)
            {
                throw new BundleException(_bundle, "Bundle is already started, it cannot be started again.");
            }
            _bundleThread.Start();
        }

        private void Startup()
        {
            try
            {
                var type = _bundle.GetType();
                if (type == typeof(StartableBundle))
                {
                    ((StartableBundle)_bundle).StartBundle();
                }
                Status = BundleStatus.Started;
            }
            catch (Exception)
            {
                Status = BundleStatus.Error;
            }
        }

        public void Stop()
        {
            try
            {
                var type = _bundle.GetType();
                if (type == typeof(StartableBundle))
                {
                    ((StartableBundle)_bundle).StopBundle();
                }
                else if (type == typeof(LoadableBundle))
                {
                    ((LoadableBundle)_bundle).UnloadBundle();
                }
                Status = BundleStatus.Stopped;
            }
            catch (Exception)
            {
                Status = BundleStatus.Error;
            }
        }
    }
}
