using UnityEngine;
using Interactive.Usables;

namespace Check.MainCheck.Appointors
{
    //Initiates pass through detector and back
    public class SecondPassAppointor : MonoBehaviour
    {
        [SerializeField] private CheckProcessor processor;
        [Space, SerializeField] private UsableBehaviour usable;

        private void Awake()
        {
            processor = GetComponent<CheckProcessor>();

            usable.OnUsed += HandleUsed;
        }

        private void HandleUsed()
        {
            processor.AppointPlaceMetallic();
            processor.AppointPassDetector();
        }

        private void OnDestroy()
        {
            if (usable != null)
            {
                usable.OnUsed -= HandleUsed;
            }
        }
    }
}