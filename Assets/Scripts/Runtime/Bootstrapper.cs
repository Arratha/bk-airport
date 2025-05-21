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

            //Tracking active metal objects. Used by metal detector.
            //Ideally, it is worth replacing collections with a separate class.
            var metallicTrackable = new ActiveTrackableState<MetallicTrackable>();
            serviceProvider.Register<IObservable<IReadOnlyCollection<MetallicTrackable>>>(metallicTrackable);
            serviceProvider.Register<IWriteOnlyCollection<MetallicTrackable>>(metallicTrackable);

            //Tracking active usable objects.
            //Ideally, it is worth replacing collections with a separate class.
            var usableTrackable = new ActiveTrackableState<TrackableUsable>();
            serviceProvider.Register<IObservable<IReadOnlyCollection<TrackableUsable>>>(usableTrackable);
            serviceProvider.Register<IWriteOnlyCollection<TrackableUsable>>(usableTrackable);

            // Currently focused usable object - tracks what the player is looking on
            serviceProvider.Register<IObservableState<FocusedUsable>>(
                new ObservableState<FocusedUsable>(new FocusedUsable(null)));

            //Current check stage
            serviceProvider.Register<IObservableState<CheckType>>(
                new ObservableState<CheckType>());
            //Current main check processor state
            serviceProvider.Register<IObservableState<ProcessorState>>(
                new ObservableState<ProcessorState>());

            //Passenger transfer from queue
            serviceProvider.Register<IObservableState<DequeuedPassenger>>(
                new ObservableState<DequeuedPassenger>(new DequeuedPassenger()));
            //Passenger transfer from main check
            serviceProvider.Register<IObservableState<ProcessedPassenger>>(
                new ObservableState<ProcessedPassenger>());

            //Enable/disable player input
            serviceProvider.Register<IObservableState<PlayerControllable>>(
                new ObservableState<PlayerControllable>(new PlayerControllable { controllable = true }));
            //Teleport player to given transform
            serviceProvider.Register<IObservableState<TeleportInfo>>(
                new ObservableState<TeleportInfo>());

            //Items that will be shown in close-up
            serviceProvider.Register<IObservableState<PreviewItems>>(
                new ObservableState<PreviewItems>(new PreviewItems()));
        }
    }
}