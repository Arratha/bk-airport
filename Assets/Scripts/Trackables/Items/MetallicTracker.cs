using UnityEngine;

namespace Trackables.Items
{
    [RequireComponent(typeof(AudioSource))]
    public class MetallicTracker : MetallicTrackerAbstract
    {
        private AudioSource _source;

        [Space, SerializeField] private float pitchRandomFactor = 0.05f;
        private float _pitch;

        protected override void OnInit()
        {
            _source = GetComponent<AudioSource>();
            _pitch = _source.pitch;
        }

        protected override void OnDetected()
        {
            if (_source.isPlaying)
            {
                return;
            }

            _source.pitch = _pitch * (1 + Random.Range(-pitchRandomFactor, pitchRandomFactor));
            _source.Play();
        }
    }
}