using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Zones;

namespace Trackables.Items
{
    public abstract class MetallicTrackerAbstract : TrackerBehaviour<IReadOnlyCollection<MetallicTrackableAbstract>>
    {
        public IZone zone
        {
            get => Zone;
            set
            {
                if (!value.isInitialized)
                {
                    value.Initialize(new ZoneInitArgs
                    {
                        transform = transform
                    });

                    Zone = value;
                }
            }
        }

        [SerializeReference] protected IZone Zone;

        private HashSet<Transform> _metallicTrackable = new();
        private HashSet<Transform> _metallicInside = new();

        public override void HandleUpdate(IReadOnlyCollection<MetallicTrackableAbstract> message)
        {
            _metallicTrackable = message.Select(x => x.transform).ToHashSet();
        }

        private void Update()
        {
            var newMetallicInside = Enumerable.ToHashSet(_metallicTrackable
                .Where(x => Zone.IsInside(x.position)));

            if (!newMetallicInside.SetEquals(_metallicInside))
            {
                OnDetected();
            }

            _metallicInside = newMetallicInside;
        }

        protected abstract void OnDetected();

        private void OnDrawGizmosSelected()
        {
            if (Zone is IDrawable drawable)
            {
                drawable.Draw();
            }
        }
    }
}