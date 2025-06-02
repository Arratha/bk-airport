using Commands.Commands;
using Commands.Contexts;
using UnityEngine;
using Interactive.Usables;

namespace Check.MainCheck.Appointors
{
    //Initiates end of main check:
    //- Returns items to passenger
    //- Removes passenger from processor
    //- Move passenger out of introscope area and then disables them
    public class PoliceAppointor : MonoBehaviour
    {
        [SerializeField] private CheckProcessor processor;
        [Space, SerializeField] private UsableBehaviour usable;
        [SerializeField] private Transform point;

        private ICompletable _completeCommand;

        private void Awake()
        {
            usable.OnUsed += HandleUsed;
        }

        private void HandleUsed()
        {
            _completeCommand = processor.AppointTakeItems();
            _completeCommand.OnComplete += HandleComplete;
        }

        private void HandleComplete(bool isSuccessful)
        {
            var passenger = processor.TakeProcessable().passenger;
            passenger.EnqueueCommand(new MoveToContext(point.position)).OnComplete +=
                (_) => passenger.gameObject.SetActive(false);

            _completeCommand.OnComplete -= HandleComplete;
        }

        private void OnDestroy()
        {
            if (_completeCommand != null)
            {
                _completeCommand.OnComplete -= HandleComplete;
            }

            if (usable != null)
            {
                usable.OnUsed -= HandleUsed;
            }
        }
    }
}