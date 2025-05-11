using Items.Trackable;
using Items.Trackable.Observables;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Observable;
using Utils.SimpleDI;

namespace Runtime
{
    public class Bootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            RegisterDependencies();

            SceneManager.LoadSceneAsync(1);
        }

        private void RegisterDependencies()
        {
            var serviceProvider = ServiceProvider.instance;

            serviceProvider.Register<IObserverMediator<StorageTrackable>>(() =>
                new TrackableMediator<StorageTrackable>());
            serviceProvider.Register<IObserverMediator<MetallicTrackable>>(() =>
                new TrackableMediator<MetallicTrackable>());
        }
    }
}