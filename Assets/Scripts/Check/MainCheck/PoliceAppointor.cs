using Commands.Commands;
using Commands.Contexts;
using UnityEngine;
using Usables;

namespace Check.MainCheck
{
    public class PoliceAppointor : MonoBehaviour
    {
        [SerializeField] private UsableBehaviour usable;
        [SerializeField] private Transform point;

        private CheckProcessor _processor;
        private ICompletable _completeCommand;

        private void Awake()
        {
            _processor = GetComponent<CheckProcessor>();

            usable.OnUsed += HandleUsed;
        }

        private void HandleUsed()
        {
            _completeCommand = _processor.AppointCommand(CheckStage.TakeItems);
            _completeCommand.OnComplete += HandleComplete;
        }

        private void HandleComplete(bool isSuccessful)
        {
            var passenger = _processor.TakeProcessable().passenger;
            passenger.EnqueueCommand(new MoveToContext(point.position)).OnComplete +=
                (_) => passenger.gameObject.SetActive(false);

            _completeCommand.OnComplete -= HandleComplete;
        }

        private void OnDestroy()
        {
            if (_completeCommand != null && _completeCommand.isDisposed)
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