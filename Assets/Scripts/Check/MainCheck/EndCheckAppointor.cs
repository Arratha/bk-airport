using Check.Queue;
using Commands.Commands;
using Commands.Contexts;
using UnityEngine;
using Usables;
using Utils.Observable;
using Utils.SimpleDI;

namespace Check.MainCheck
{
    [RequireComponent(typeof(CheckProcessor))]
    public class EndCheckAppointor : MonoBehaviour, IObserver<DequeuedPassenger>
    {
        [SerializeField] private UsableBehaviour usable;
        [SerializeField] private Transform point;

        private CheckProcessor _processor;
        private ICompletable _completeCommand;

        private IObservableState<DequeuedPassenger> _passengerState;

        public void HandleUpdate(DequeuedPassenger message)
        {
            if (message.passenger != null)
            {
                usable.gameObject.SetActive(true);

                var usableTransform = usable.transform;
                usableTransform.SetParent(message.passenger.transform);
                usableTransform.localPosition = new Vector3(0, usableTransform.localPosition.y, 0);
            }
            else
            {
                usable.gameObject.SetActive(false);
            }
        }

        private void Awake()
        {
            _processor = GetComponent<CheckProcessor>();

            usable.OnUsed += HandleUsed;

            _passengerState = ServiceProvider.instance.Resolve<IObservableState<DequeuedPassenger>>();
            _passengerState.RegisterObserver(this);
            HandleUpdate(_passengerState.GetState());
        }

        private void HandleUsed()
        {
            _completeCommand = _processor.AppointCommand(CheckStage.TakeItems);
            _completeCommand.OnComplete += HandleComplete;
            
            usable.gameObject.SetActive(false);
            usable.transform.SetParent(null);
        }

        private void HandleComplete(bool isSuccessful)
        {
            var passenger = _processor.TakeProcessable().passenger;
            passenger.EnqueueCommand(new MoveToContext(point.position)).OnComplete +=
                (_) => passenger.gameObject.SetActive(false);
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
            
            _passengerState.UnregisterObserver(this);
        }
    }
}