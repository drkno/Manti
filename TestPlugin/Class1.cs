using System;

namespace TestPlugin
{
    public class Class1 : IDisposable
    {
        public string HelloWorld(int someNumber)
        {
            return "Hello World " + someNumber;
        }

        public Class1(dynamic platform)
        {
            platform.Log.Error("IT LOADED!");
        }

        public void Dispose()
        {
        }
    }
}
