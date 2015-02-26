namespace MantiCore.Bundles
{
    [BundleSecurity(BundleSecurity.Level.Platform)]
    public abstract class Bundle
    {
        public abstract void InitialiseBundle(Platform platform, BundleArgument[] arguments);
    }
    
    public abstract class StartableBundle : Bundle
    {
        public abstract void StartBundle();
        public abstract void StopBundle();
    }

    public abstract class LoadableBundle : Bundle
    {
        public abstract void UnloadBundle();
    }
}
