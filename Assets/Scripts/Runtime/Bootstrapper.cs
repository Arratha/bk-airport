using System.Collections.Generic;
using Check;
using Check.MainCheck;
using Check.Queue;
using Trackables;
using Trackables.Items;
using UnityEngine;
using Utils;
using Utils.Observable;
using Utils.SimpleDI;

namespace Runtime
{
    [DefaultExecutionOrder(-100)]
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private PlayerRegistrar playerRegistrar;

        private void Awake()
        {
            RegisterDependencies();
        }

        private void RegisterDependencies()
        {
            var serviceProvider = ServiceProvider.instance;

            RegisterBaseDependencies(serviceProvider);

            playerRegistrar.Register(serviceProvider);
        }

        private void RegisterBaseDependencies(ServiceProvider serviceProvider)
        {
            //Tracking active metal objects. Used by metal detector.
            //Ideally, it is worth replacing collections with a separate class.
            var metallicTrackable = new ActiveTrackableState<MetallicTrackable>();
            serviceProvider.Register<IObservable<IReadOnlyCollection<MetallicTrackable>>>(metallicTrackable);
            serviceProvider.Register<IWriteOnlyCollection<MetallicTrackable>>(metallicTrackable);

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