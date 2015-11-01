namespace CoreLoader.Plugins.Logging
{
    public interface ILoggingPlugin : IPlugin
    {
        void Log(uint logLevel, string prefix, string message);
    }
}