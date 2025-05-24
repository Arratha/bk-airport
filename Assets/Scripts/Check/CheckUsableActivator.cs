using Check.MainCheck;
using UnityEngine;
using Interactive.Usables;
using Utils.Observable;
using Utils.SimpleDI;

namespace Check
{
    //Manages whether usable is interactable based on current check type
    [RequireComponent(typeof(IUsable))]
    public class CheckUsableActivator : MonoBehaviour, Utils.Observable.IObserver<CheckType>, Utils.Observable.IObserver<ProcessorState>
    {
        [SerializeField] private CheckType targetType;

        private IUsable _usable;

        private CheckType _currentType;
        private ProcessorState _currentProcessor;

        private IObservableState<CheckType> _checkState;
        private IObservableState<ProcessorState> _processorState;

        public void HandleUpdate(CheckType message)
        {
            _currentType = message;
            _usable.enabled = _currentType == targetType &&
                              (_currentType != CheckType.MainCheck || _currentProcessor == ProcessorState.Idle);
        }

        public void HandleUpdate(ProcessorState message)
        {
            _currentProcessor = message;
            _usable.enabled = _currentType == targetType &&
                              (_currentType != CheckType.MainCheck || _currentProcessor == ProcessorState.Idle);
        }

        private void Awake()
        {
            _usable = GetComponent<IUsable>();

            _checkState = ServiceProvider.instance.Resolve<IObservableState<CheckType>>();
            _processorState = ServiceProvider.instance.Resolve<IObservableState<ProcessorState>>();
        }

        private void Start()
        {
            _checkState.RegisterObserver(this, true);
            _processorState.RegisterObserver(this, true);
        }

        private void OnDestroy()
        {
            _checkState.UnregisterObserver(this);
            _processorState.UnregisterObserver(this);
        }
    }
}