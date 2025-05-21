using Check.MainCheck;
using UnityEngine;
using Usables;
using Utils.Observable;
using Utils.SimpleDI;

namespace Check.Queue
{
    //Controls interactions with queue based on remaining passengers and emptiness of processor
    [RequireComponent(typeof(IUsable))]
    public class QueueUsableActivator : MonoBehaviour, IObserver<ProcessorState>
    {
        private PassengersQueue _queue;
        private IUsable _usable;

        private IObservableState<ProcessorState> _processorState;

        private bool _isActiveQueue;
        private bool _isActiveState;

        public void HandleUpdate(ProcessorState message)
        {
            _isActiveState = message == ProcessorState.Empty;

            SetUsableActive();
        }

        private void Awake()
        {
            _queue = GetComponent<PassengersQueue>();
            _usable = GetComponent<IUsable>();

            _usable.OnUsed += HandleUsed;
            
            _processorState = ServiceProvider.instance.Resolve<IObservableState<ProcessorState>>();
        }

        private void Start()
        {
            _isActiveQueue = _queue.count > 0;

            _processorState.RegisterObserver(this);
            HandleUpdate(_processorState.GetState());
        }

        private void HandleUsed()
        {
            _isActiveQueue = _queue.count > 0;
            
            SetUsableActive();
        }

        private void SetUsableActive()
        {
            _usable.enabled = _isActiveState && _isActiveQueue;
        }

        private void OnDestroy()
        {
            _processorState.UnregisterObserver(this);
                
            if (_usable == null)
            {
                return;
            }

            _usable.OnUsed -= HandleUsed;
        }
    }
}