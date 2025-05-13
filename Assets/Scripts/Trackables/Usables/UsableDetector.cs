using System.Collections.Generic;
using Unity.VisualScripting;

namespace Trackables.Usables
{
    public class UsableDetector : TrackerBehaviour<IReadOnlyCollection<TrackableUsable>>
    {
        protected HashSet<TrackableUsable> Usables = new();

        public override void HandleUpdate(IReadOnlyCollection<TrackableUsable> message)
        {
            Usables = message.ToHashSet();
        }
    }
}