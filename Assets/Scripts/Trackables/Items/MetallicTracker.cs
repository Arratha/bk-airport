using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Trackables.Items
{
    [RequireComponent(typeof(AudioSource))]
    public class MetallicTracker : TrackerBehaviour<IReadOnlyCollection<MetallicTrackable>>
    {
        [SerializeField] private float height;
        [SerializeField] private float radius;

        private AudioSource _source;
        
        [Space, SerializeField] private float pitchRandomFactor = 0.05f;
        private float _pitch;

        private HashSet<Transform> _metallicTrackable = new();
        private HashSet<Transform> _metallicInside = new();

        public override void HandleUpdate(IReadOnlyCollection<MetallicTrackable> message)
        {
            _metallicTrackable = message.Select(x => x.transform).ToHashSet();
        }

        protected override void OnInit()
        {
            _source = GetComponent<AudioSource>();
            _pitch = _source.pitch;
        }

        private void FixedUpdate()
        {
            var newMetallicInside = Enumerable.ToHashSet(_metallicTrackable
                .Where(x => IsInside(x.position)));

            if (!_source.isPlaying
                && newMetallicInside.Any(x => !_metallicInside.Contains(x)))
            {
                _source.pitch = _pitch * (1 + Random.Range(-pitchRandomFactor, pitchRandomFactor));
                _source.Play();
            }

            _metallicInside = newMetallicInside;
        }

        private bool IsInside(Vector3 position)
        {
            var selfPosition = transform.position;

            return Vector2.Distance(new Vector2(selfPosition.x, selfPosition.z),
                new Vector2(position.x, position.z)) <= radius;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            var segments = 32;
            
            var bottom = transform.position;
            
            var topCircle = new Vector3[segments];
            var bottomCircle = new Vector3[segments];
            
            for (var i = 0; i < segments; i++)
            {
                var angle = (float)i / segments * Mathf.PI * 2f;
                var x = Mathf.Cos(angle) * radius;
                var z = Mathf.Sin(angle) * radius;
            
                var offset = new Vector3(x, 0f, z);
                topCircle[i] = bottom + offset + Vector3.up * height;
                bottomCircle[i] = bottom + offset;
            }
            
            for (var i = 0; i < segments; i++)
            {
                var next = (i + 1) % segments;
                Gizmos.DrawLine(topCircle[i], topCircle[next]);
                Gizmos.DrawLine(bottomCircle[i], bottomCircle[next]);
                Gizmos.DrawLine(topCircle[i], bottomCircle[i]);
            }
        }
    }
}