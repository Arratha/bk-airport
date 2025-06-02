using Check.Queue;
using UnityEngine;
using Utils.Observable;

namespace Check.Tutorial
{
    public class ChangingTutorialAbstract : TutorialAbstract
    {
        [TextArea(1, 5), SerializeField] private string enabledText;

        private IObservableState<DequeuedPassenger> _state;

        private bool _wasCorrectStage;
        protected bool WasShown;
        protected bool IsHidden;

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
                SetText();
            }
        }
        
        protected void SetText()
        {
            if (IsHidden || !_wasCorrectStage)
            {
                Label.text = string.Empty;
                return;
            }

            Label.text = WasShown ? enabledText : tutorialText;
        }
    }
}