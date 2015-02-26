using System;
using System.Threading;

namespace MantiCore.Bundles
{
    public class BundleHandle
    {
        private Bundle _bundle;
        private Thread _bundleThread;

        public BundleStatus Status { get; private set; }

        public BundleHandle(Bundle bundle)
        {
            _bundle = bundle;
            Status = BundleStatus.Stopped;

        }

        public void Start()
        {
            _bundleThread = new Thread(Startup);
        }

        private void Startup()
        {
            try
            {

            }
            catch (Exception)
            {
                Status.
            }
        }

        public void Stop()
        {
            
        }
    }
}
