using Check.MainCheck;
using Interactive.Usables;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Check.Tutorial
{
    public class QueueTutorial : TutorialAbstract, IObserver<ProcessorState>
    {
        [TextArea(1, 5), SerializeField] private string enabledText;
        [TextArea(1, 5), SerializeField] private string disabledText;

        [Space, SerializeField] private UsableBehaviour usable;

        private ProcessorState _processState;

        private bool _wasActivated;
        private bool _wasUsed;

        private IObservableState<ProcessorState> _processorState;

        public override void HandleUpdate(TutorialStage message)
        {
            if (message == TutorialStage.None)
            {
                Destroy(gameObject);
                return;
            }

            if (message == stage)
            {
                _wasActivated = true;
            }

            SetText();
        }

        public void HandleUpdate(ProcessorState message)
        {
            if (!_wasActivated)
            {
                return;
            }

            _processState = message;

            SetText();
        }

        private void Use()
        {
            _wasUsed = true;
            TutorialState.HandleUpdate(stage + 1);

            usable.OnUsed -= Use;
        }

        protected override void HandleInit()
        {
            var serviceProvider = ServiceProvider.instance;

            _processorState = serviceProvider.Resolve<IObservableState<ProcessorState>>();
            _processorState.RegisterObserver(this, true);
        }

        protected override void HandleDestroy()
        {
            _processorState.UnregisterObserver(this);
        }

        private void OnEnable() => usable.OnUsed += Use;

        private void OnDisable()
        {
            if (usable != null)
            {
                usable.OnUsed -= Use;
            }
        }

        private void SetText()
        {
            if (!_wasActivated)
            {
                Label.text = string.Empty;
                return;
            }

            if (!_wasUsed)
            {
                Label.text = tutorialText;
                return;
            }

            Label.text = _processState == ProcessorState.Empty ? enabledText : disabledText;
        }
    }
}