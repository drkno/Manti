using System;
using MantiCore.Core.Logging;

namespace MantiCore.Interop
{
    public class InteropManager : IObservable<Type>, IObservable<string>
    {
        public IDisposable Subscribe(IObserver<Type> observer)
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            throw new NotImplementedException();
        }

        public ObservableServiceList<LoggingBackend> GetServices(Type type)
        {
            throw new NotImplementedException();
        }

        public ObservableServiceList<LoggingBackend> GetServices(string identifier)
        {
            throw new NotImplementedException();
        }
    }
}
