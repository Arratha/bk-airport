using Check.Queue;
using Commands.Commands;
using Commands.Contexts;
using UnityEngine;
using Interactive.Usables;
using Utils.Observable;
using Utils.SimpleDI;

namespace Check.MainCheck.Appointors
{
    //Initiates end of main check:
    //- Returns items to passenger
    //- Removes passenger from processor
    //- Move passenger out of introscope area and then disables them
    //Attaches interaction point to dequeued passenger
    public class EndCheckAppointor : MonoBehaviour, IObserver<DequeuedPassenger>
    {
        [SerializeField] private CheckProcessor processor;
        [Space, SerializeField] private UsableBehaviour usable;
        [SerializeField] private Transform point;

        private ICompletable _completeCommand;

        private IObservableState<DequeuedPassenger> _passengerState;

        private Transform _passengerTransform;
        
        public void HandleUpdate(DequeuedPassenger message)
        {
            if (message.passenger != null)
            {
                usable.gameObject.SetActive(true);
                _passengerTransform = message.passenger.transform;
            }
            else
            {
                usable.gameObject.SetActive(false);
            }
        }

        private void Awake()
        {
            usable.OnUsed += HandleUsed;

            _passengerState = ServiceProvider.instance.Resolve<IObservableState<DequeuedPassenger>>();
            _passengerState.RegisterObserver(this, true);
        }

        private void Update()
        {
            if (_passengerTransform == null)
            {
                return;
            }

            var passengerPosition = _passengerTransform.position;

            var usableTransform = usable.transform;
            usableTransform.position = new Vector3(passengerPosition.x,
                usableTransform.position.y, passengerPosition.z);
        }

        private void HandleUsed()
        {
            _completeCommand = processor.AppointTakeItems();
            _completeCommand.OnComplete += HandleComplete;

            usable.gameObject.SetActive(false);
            usable.transform.SetParent(null);
        }

        private void HandleComplete(bool isSuccessful)
        {
            var passenger = processor.TakeProcessable().passenger;
            passenger.EnqueueCommand(new MoveToContext(point.position)).OnComplete +=
                (_) => passenger.gameObject.SetActive(false);
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
            
            _passengerState.UnregisterObserver(this);
        }
    }
}