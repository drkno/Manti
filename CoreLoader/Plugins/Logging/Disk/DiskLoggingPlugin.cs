using System;
using System.IO;
using CoreLoader.Core;
using CoreLoader.Logger;

namespace CoreLoader.Plugins.Logging.Disk
{
    internal class DiskLoggingPlugin : ILoggingPlugin
    {
        private const uint Write = 0;
        private const uint Info = 1;
        private const uint Warning = 2;
        private const uint Error = 3;

        private readonly Log _logger;
        private readonly bool _enabled;
        private readonly StreamWriter _writer;

        public DiskLoggingPlugin(Platform platform)
        {
            _enabled = File.Exists("DiskOutput.conf");
            if (_enabled)
            {
                _writer = new StreamWriter("manti.log");
            }
            _logger = platform.Log;
            _logger.Info("Disk Logging Plugin Loaded" + (!_enabled ? " (disabled)" : ""));
        }

        public void Log(uint logLevel, string prefix, string message)
        {
            if (_enabled)
            {
                _writer.WriteLine(prefix + " " + message);
            }
        }

        public void Dispose()
        {
            if (_enabled)
            {
                _writer.Close();
            }
            _logger.Info("Disk Logging Plugin Unloaded");
        }
    }
}
