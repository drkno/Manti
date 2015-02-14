namespace MantiCore.Bundle
{
    public abstract class Bundle
    {
        public abstract void InitialiseBundle(BundleArgument[] arguments);
        public abstract void StartBundle();
        public abstract void StopBundle();
    }
}
