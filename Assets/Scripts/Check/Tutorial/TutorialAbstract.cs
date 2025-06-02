using Interactive.Labels;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Check.Tutorial
{
    public enum TutorialStage
    {
        None,
        Queue,
        ConveyorBelt
    }

    public abstract class TutorialAbstract : MonoBehaviour, IObserver<TutorialStage>
    {
        [SerializeField] protected TutorialStage stage;

        [TextArea(1, 5), SerializeField] protected string tutorialText;

        protected ILabel Label;
        protected IObservableState<TutorialStage> TutorialState;

        public virtual void HandleUpdate(TutorialStage message)
        {
            if (message == TutorialStage.None)
            {
                Destroy(gameObject);
                return;
            }

            Label.text = stage == message ? tutorialText : string.Empty;
        }

        private void Awake()
        {
            Label = GetComponent<ILabel>();

            TutorialState = ServiceProvider.instance.Resolve<IObservableState<TutorialStage>>();
            TutorialState.RegisterObserver(this, true);

            HandleInit();
        }

        protected virtual void HandleInit()
        {

        }

        private void OnDestroy()
        {
            TutorialState.UnregisterObserver(this);

            HandleDestroy();
        }

        protected virtual void HandleDestroy()
        {

        }
    }
}