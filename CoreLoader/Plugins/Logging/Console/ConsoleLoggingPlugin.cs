using System;
using CoreLoader.Core;
using CoreLoader.Logger;

namespace CoreLoader.Plugins.Logging.Console
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
            System.Console.Write(prefix + " ");
            switch (logLevel)
            {
                case Write: break;
                case Info: System.Console.ForegroundColor = ConsoleColor.Cyan; break;
                case Warning: System.Console.ForegroundColor = ConsoleColor.Yellow; break;
                case Error: System.Console.ForegroundColor = ConsoleColor.Red; break;
                default: goto case Write;
            }
            System.Console.WriteLine(message);
            System.Console.ResetColor();
        }

        public void Dispose()
        {
            _logger.Info("Console Logging Plugin Unloaded");
        }
    }
}
