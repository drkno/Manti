using CoreLoader.Plugins;

namespace CoreLoader
{
    public interface IITest : IPlugin
    {
        string HelloWorld(int someNumber);
    }
}