using Interactive.Labels;
using UnityEngine;

namespace Trackables.Labels
{
    public class LabeledTrackable : TrackableAbstract<LabeledTrackable>, ILabeled
    {
        public string label => selfLabel;
        [SerializeField] private string selfLabel;
    }
}