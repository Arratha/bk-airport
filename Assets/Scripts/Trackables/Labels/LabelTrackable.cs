using Interactive.Labels;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Trackables.Labels
{
    public class LabelTrackable : TrackableAbstract<LabelTrackable>, ILabel
    {
        public string text
        {
            get => selfText;
            set
            {
                selfText = value;

                UpdateIfEnabled();
            }
        }

        [SerializeField] private string selfText;

        protected IObserver<LabelTrackable> Observer;

        private void UpdateIfEnabled()
        {
            if (!enabled)
            {
                return;
            }

            if (Observer == null)
            {
                Observer = ServiceProvider.instance.Resolve<IObserver<LabelTrackable>>();
            }

            Observer.HandleUpdate(this);
        }
    }
}