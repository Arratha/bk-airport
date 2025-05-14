using System.Collections.Generic;
using Check;
using Check.AdditionalCheck;
using Check.MainCheck;
using Check.Queue;
using Player;
using Trackables;
using Trackables.Items;
using Trackables.Usables;
using UnityEngine;
using UnityEngine.SceneManagement;
using Usables;
using Utils;
using Utils.Observable;
using Utils.SimpleDI;

namespace Runtime
{
    public class Bootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            Cursor.visible = false;

            RegisterDependencies();

            SceneManager.LoadSceneAsync(1);
        }

        private void RegisterDependencies()
        {
            var serviceProvider = ServiceProvider.instance;

            var metallicTrackable = new ActiveTrackableState<MetallicTrackable>();
            serviceProvider.Register<IObservable<IReadOnlyCollection<MetallicTrackable>>>(metallicTrackable);
            serviceProvider.Register<IWriteOnlyCollection<MetallicTrackable>>(metallicTrackable);

            var usableTrackable = new ActiveTrackableState<TrackableUsable>();
            serviceProvider.Register<IObservable<IReadOnlyCollection<TrackableUsable>>>(usableTrackable);
            serviceProvider.Register<IWriteOnlyCollection<TrackableUsable>>(usableTrackable);
            serviceProvider.Register<IObservableState<FocusedUsable>>(
                new ObservableState<FocusedUsable>(new FocusedUsable(null)));
            
            serviceProvider.Register<IObservableState<CheckType>>(
                new ObservableState<CheckType>());
            serviceProvider.Register<IObservableState<ProcessorState>>(
                new ObservableState<ProcessorState>());
            
            serviceProvider.Register<IObservableState<DequeuedPassenger>>(
                new ObservableState<DequeuedPassenger>(new DequeuedPassenger()));
            serviceProvider.Register<IObservableState<ProcessedPassenger>>(
                new ObservableState<ProcessedPassenger>());
            
            serviceProvider.Register<IObservableState<PlayerControllable>>(
                new ObservableState<PlayerControllable>(new PlayerControllable {controllable = true}));
            serviceProvider.Register<IObservableState<TeleportInfo>>(
                new ObservableState<TeleportInfo>());
            
            serviceProvider.Register<IObservableState<SearchItems>>(
                new ObservableState<SearchItems>(new SearchItems()));
        }
    }
}