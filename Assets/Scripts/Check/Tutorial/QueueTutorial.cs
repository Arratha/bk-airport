using Check.MainCheck;
using Interactive.Usables;
using UnityEngine;
using Utils.Observable;

namespace Check.Tutorial
{
    public class QueueTutorial : TutorialAbstract
    {
        [TextArea(1, 5), SerializeField] private string enabledText;

        [Space, SerializeField] private UsableBehaviour usable;

        private bool _wasCorrectStage;
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
                _wasCorrectStage = true;
            }

            SetText();
        }

        private void Use()
        {
            _wasUsed = true;
            TutorialState.HandleUpdate(stage + 1);

            usable.OnUsed -= Use;
        }

        private void OnEnable()
        {
            if (_wasUsed || usable == null)
            {
                return;
            }

            usable.OnUsed += Use;
        }

        private void OnDisable()
        {
            if (usable != null)
            {
                usable.OnUsed -= Use;
            }
        }

        private void SetText()
        {
            if (!_wasCorrectStage)
            {
                Label.text = string.Empty;
                return;
            }

            if (!_wasUsed)
            {
                Label.text = tutorialText;
                return;
            }

            Label.text = enabledText;
        }
    }
}