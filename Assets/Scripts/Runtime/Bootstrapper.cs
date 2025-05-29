using UnityEngine;
using Utils.SimpleDI;

namespace Runtime
{
    [DefaultExecutionOrder(-105)]
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private CheckRegistrar checkRegistrar;
        [SerializeField] private TrackableRegistrar trackableRegistrar;
        [SerializeField] private PlayerRegistrar playerRegistrar;
        
        private void Awake()
        {
            Application.targetFrameRate = 0;
            
            RegisterDependencies();
        }

        private void RegisterDependencies()
        {
            var serviceProvider = ServiceProvider.instance;

            checkRegistrar.Register(serviceProvider);
            trackableRegistrar.Register(serviceProvider);
            playerRegistrar.Register(serviceProvider);
        }
    }
}