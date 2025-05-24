using UnityEngine;
using Interactive.Usables;

namespace Check.MainCheck.Appointors
{
    //Initiates pass through detector and back
    [RequireComponent(typeof(CheckProcessor))]
    public class SecondPassAppointor : MonoBehaviour
    {
        [SerializeField] private UsableBehaviour usable;
      
        private CheckProcessor _processor;

        private void Awake()
        {
            _processor = GetComponent<CheckProcessor>();

            usable.OnUsed += HandleUsed;
        }

        private void HandleUsed()
        {
            _processor.AppointCommand(CheckStage.PlaceMetallic);
            _processor.AppointCommand(CheckStage.PassDetector);
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