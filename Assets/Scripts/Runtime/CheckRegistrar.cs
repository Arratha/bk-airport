using System;
using Check;
using Check.MainCheck;
using Check.Queue;
using Utils.Observable;
using Utils.SimpleDI;

namespace Runtime
{
    [Serializable]
    public class CheckRegistrar
    {
        public void Register(ServiceProvider serviceProvider)
        {   
            //Current check stage
            serviceProvider.Register<IObservableState<CheckType>>(
                new ObservableState<CheckType>(CheckType.MainCheck));
            //Current main check processor state
            serviceProvider.Register<IObservableState<ProcessorState>>(
                new ObservableState<ProcessorState>(ProcessorState.Empty));

            //Passenger transfer from queue
            serviceProvider.Register<IObservableState<DequeuedPassenger>>(
                new ObservableState<DequeuedPassenger>());
            //Passenger transfer from main check
            serviceProvider.Register<IObservableState<ProcessedPassenger>>(
                new ObservableState<ProcessedPassenger>());
        }
    }
}