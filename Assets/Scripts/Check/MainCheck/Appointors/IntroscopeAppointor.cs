using System.Collections.Generic;
using System.Linq;
using Check.Queue;
using Commands.Commands;
using Interactive.Usables;
using Items.Base;
using Items.Storages.Attachers;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Check.MainCheck.Appointors
{
    //Sets dequeued passenger as new processed passenger
    //Initiates check
    public class IntroscopeAppointor : MonoBehaviour, IObserver<DequeuedPassenger>
    {
        [SerializeField] private CheckProcessor processor;
        [SerializeField] private AttacherStorageAbstract introscopeStorage;

        private IObservableState<DequeuedPassenger> _passengerState;

        private ICompletable _completeCommand;

        public void HandleUpdate(DequeuedPassenger message)
        {
            processor.AppointPassenger(message.passenger);

            _completeCommand = processor.AppointPlaceBaggage();
            _completeCommand.OnComplete += HandleComplete;
        }

        private void Awake()
        {
            _passengerState = ServiceProvider.instance.Resolve<IObservableState<DequeuedPassenger>>();
            _passengerState.RegisterObserver(this, true);

            introscopeStorage.OnItemObjectAdded += HandleItemAdded;
        }

        private void HandleComplete(bool isSuccessful)
        {
            processor.AppointMoveBaggage(introscopeStorage.itemObjects.ToList());
        }

        private void HandleItemAdded(Item item)
        {
            var usable = item.GetComponentInChildren<IUsable>();

            if (usable == null)
            {
                return;
            }

            usable.OnUsed += () => processor.AppointMoveBaggage(new List<Item>() { item });
        }

        private void OnDestroy()
        {
            _passengerState.UnregisterObserver(this);

            if (_completeCommand != null)
            {
                _completeCommand.OnComplete += HandleComplete;
            }

            if (introscopeStorage != null)
            {
                introscopeStorage.OnItemObjectAdded -= HandleItemAdded;
            }
        }
    }
}