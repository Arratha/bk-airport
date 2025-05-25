using Interactive.Labels;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Trackables.Labels
{
    public class ForcedLabelTrackable : LabelTrackable, IForcedLabel
    {
        public bool forcedShow
        {
            get => shouldForcedShow;
            set
            {
                shouldForcedShow = value;

                UpdateIfEnabled();
            }
        }

        [SerializeField] private bool shouldForcedShow;

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