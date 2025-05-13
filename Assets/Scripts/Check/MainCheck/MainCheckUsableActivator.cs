using UnityEngine;
using Usables;
using Utils.Observable;
using Utils.SimpleDI;

namespace Check.MainCheck
{
    [RequireComponent(typeof(IUsable))]
    public class MainCheckUsableActivator : MonoBehaviour, IObserver<ProcessorState>
    {
        private IUsable _usable;

        private IObservableState<ProcessorState> _processorState;

        public void HandleUpdate(ProcessorState message)
        {
            _usable.enabled = message == ProcessorState.Idle;
        }

        private void Awake()
        {
            _usable = GetComponent<IUsable>();

            _processorState = ServiceProvider.instance.Resolve<IObservableState<ProcessorState>>();
        }

        private void Start()
        {
            _processorState.RegisterObserver(this);
            HandleUpdate(_processorState.GetState());
        }

        private void OnDestroy()
        {
            _processorState.UnregisterObserver(this);
        }
    }
}