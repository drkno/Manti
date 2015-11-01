using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using CoreLoader.Core;
using CoreLoader.Plugins.Logging;

namespace CoreLoader.Logger
{
    public class Log : IDisposable
    {
        private readonly IEnumerable<ILoggingPlugin> _loggingPlugins;
        private readonly BlockingCollection<LogEntry> _logQueue;
        private bool _shouldLog;
        private readonly CancellationTokenSource _cancellationToken;
        private readonly Thread _loggingThread;

        public bool ConsoleFallback { get; set; } = true;

        private struct LogEntry
        {
            public string Message { get; set; }
            public string Prefix { get; set; }
            public uint LogLevel { get; set; }
        }

        public Log(Platform platform)
        {
            platform.GetPlugins("nz.co.makereti.manti.logging", out _loggingPlugins);
            _shouldLog = true;
            _logQueue = new BlockingCollection<LogEntry>();
            _cancellationToken = new CancellationTokenSource();
            _loggingThread = new Thread(InternalLogThread) { IsBackground = true };
            _loggingThread.Start();
        }

        public void WriteLine(string message) => Write(message);

        public void Write(string message)
        {
            InternalLog(message, 0);
        }

        public void Info(string message)
        {
            InternalLog(message, 1);
        }

        public void Warning(string message)
        {
            InternalLog(message, 2);
        }

        public void Error(string message)
        {
            InternalLog(message, 3);
        }

        private void InternalLog(string message, uint logLevel)
        {
            if (!_shouldLog) return;
            var d = DateTime.Now - Process.GetCurrentProcess().StartTime;
            var prefix = "[" + d.TotalSeconds.ToString("0.0").PadLeft(11) + "]";
            _logQueue.Add(new LogEntry {Message = message, Prefix = prefix, LogLevel = logLevel});
        }

        private void InternalLogThread()
        {
            try
            {
                while (_shouldLog)
                {
                    try
                    {
                        var log = _logQueue.Take(_cancellationToken.Token);
                        if (!_loggingPlugins.Any())
                        {
                            if (ConsoleFallback)
                            {
                                Console.WriteLine(log.Prefix + " " + log.Message);
                            }
                        }
                        else
                        {
                            foreach (var loggingPlugin in _loggingPlugins)
                            {
                                loggingPlugin.Log(log.LogLevel, log.Prefix, log.Message);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (_shouldLog && ConsoleFallback) Console.WriteLine("Failed log: " + e.Message);
                        break;
                    }
                }
            }
            catch (Exception)
            {
                if (_shouldLog && ConsoleFallback) Console.WriteLine("Logger has died.");
            }
        }

        public void Dispose()
        {
            _shouldLog = false;
            _cancellationToken.Cancel(true);
            _loggingThread.Abort();
            _loggingThread.Join();
            if (!ConsoleFallback) return;
            while (_logQueue.Count > 0)
            {
                var log = _logQueue.Take();
                Console.WriteLine(log.Prefix + " " + log.Message);
            }
        }
    }
}
