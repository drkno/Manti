using System;
using CoreLoader.Core;
using CoreLoader.Logger;

namespace CoreLoader.Plugins.Logging
{
    internal class ConsoleLoggingPlugin : ILoggingPlugin
    {
        private const uint Write = 0;
        private const uint Info = 1;
        private const uint Warning = 2;
        private const uint Error = 3;

        private readonly Log _logger;

        public ConsoleLoggingPlugin(Platform platform)
        {
            _logger = platform.Log;
            _logger.Info("Console Logging Plugin Loaded");
        }

        public void Log(uint logLevel, string prefix, string message)
        {
            Console.Write(prefix + " ");
            switch (logLevel)
            {
                case Write: break;
                case Info: Console.ForegroundColor = ConsoleColor.Cyan; break;
                case Warning: Console.ForegroundColor = ConsoleColor.Yellow; break;
                case Error: Console.ForegroundColor = ConsoleColor.Red; break;
                default: goto case Write;
            }
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Dispose()
        {
            _logger.Info("Console Logging Plugin Unloaded");
        }
    }
}
