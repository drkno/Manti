using System;
using System.Collections.Concurrent;
using System.Threading;
using MantiCore.Bundles;
using MantiCore.Interop;

namespace MantiCore.Core.Logging
{
    public class Logging
    {
        private ConsoleColor _consoleColour;
        private bool _loggingThreadShouldRun;
        private readonly Thread _loggingThread;
        private readonly BlockingCollection<string> _writeQueue;
        private readonly ObservableServiceList<LoggingBackend> _loggingBackends;
        private readonly LoggingFallback _fallback;

        public bool DisableInternalLogging { get; set; } 

        private class LoggingFallback : LoggingBackend
        {
            public override void InitialiseBundle(Platform platform, BundleArgument[] arguments)
            {
                throw new NotImplementedException("This internal bundle should never be initialised.");
            }

            public override void UnloadBundle()
            {
                throw new NotImplementedException("This internal bundle should never be unloaded.");
            }

            public override bool DisableInternalLogging
            {
                get { throw new NotImplementedException("This internal bundle cannot disable internal logging."); }
            }

            public override bool SupportsColour
            {
                get { return true; }
            }

            public override ConsoleColor ForegroundColour
            {
                get { return Console.ForegroundColor; }
                set { Console.ForegroundColor = value; }
            }

            public override void Write(params object[] obj)
            {
                foreach (var o in obj)
                {
                    Console.Write(o);
                }
            }
        }

        public Logging(InteropManager interopManager)
        {
            _loggingThreadShouldRun = true;
            _fallback = new LoggingFallback();
            _loggingBackends = interopManager.GetServices(typeof (LoggingBackend));
            _writeQueue = new BlockingCollection<string>();
            _loggingThread = new Thread(
                () =>
                {
                    while (_loggingThreadShouldRun) InternalWrite(_writeQueue.Take());
                });
            _loggingThread.IsBackground = true;
            _loggingThread.Start();
        }

        public ConsoleColor ForegroundColor
        {
            get { return _consoleColour; }
            set
            {
                var disableInternal = DisableInternalLogging;
                foreach (var loggingBackend in _loggingBackends)
                {
                    if (loggingBackend.SupportsColour)
                    {
                        loggingBackend.ForegroundColour = value;
                    }
                    disableInternal || = loggingBackend.DisableInternalLogging;
                }
                if (!disableInternal)
                {
                    _fallback.ForegroundColour = value;
                }
                _consoleColour = value;
            }
        }

        public void StopLogging(string message)
        {
            _loggingThreadShouldRun = false;
            WriteLine(message);
            _loggingThread.Abort();
            _loggingThread.Join();
        }

        public void Write(params object[] obj)
        {
            _writeQueue.Add(obj);
        }

        private void InternalWrite(params object[] obj)
        {
            var disableInternal = DisableInternalLogging;
            foreach (var loggingBackend in _loggingBackends)
            {
                loggingBackend.Write(obj);
                disableInternal || = loggingBackend.DisableInternalLogging;
            }
            if (!disableInternal)
            {
                _fallback.Write(obj);
            }
        }

        public void WriteLine(params object[] obj)
        {
            Array.Resize(ref obj, obj.Length + 1);
            obj[obj.Length - 1] = Environment.NewLine;
            Write(obj);
        }
    }
}
