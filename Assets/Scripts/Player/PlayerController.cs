using Trackables.Usables;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Player
{
    [RequireComponent(typeof(PlayerMoveController))]
    [RequireComponent(typeof(PlayerCameraController))]
    [RequireComponent(typeof(PlayerUsableDetector))]
    public class PlayerController : MonoBehaviour, IObserver<TeleportInfo>, IObserver<PlayerControllable>
    {
        private PlayerMoveController _moveController;
        private PlayerCameraController _cameraController;
        private PlayerUsableDetector _usableDetector;

        private IObservableState<PlayerControllable> _controllableState;
        private IObservableState<TeleportInfo> _teleportState;

        public void HandleUpdate(TeleportInfo message)
        {
            _moveController.SetPosition(message.position);
            _cameraController.SetRotation(message.rotation);
        }
        
        public void HandleUpdate(PlayerControllable message)
        {
            _moveController.enabled = message.controllable;
            _cameraController.enabled = message.controllable;
            _usableDetector.enabled = message.controllable;
        }
        
        private void Awake()
        {
            _moveController = GetComponent<PlayerMoveController>();
            _cameraController = GetComponent<PlayerCameraController>();
            _usableDetector = GetComponent<PlayerUsableDetector>();

            _controllableState = ServiceProvider.instance.Resolve<IObservableState<PlayerControllable>>();
            _controllableState.RegisterObserver(this);
            HandleUpdate(_controllableState.GetState());
            
            _teleportState = ServiceProvider.instance.Resolve<IObservableState<TeleportInfo>>();
            _teleportState.RegisterObserver(this);
        }


        private void OnDestroy()
        {
            _teleportState.UnregisterObserver(this);
        }
    }
}