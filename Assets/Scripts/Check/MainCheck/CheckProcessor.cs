using System;
using System.Collections.Generic;
using System.Linq;
using Check.MainCheck.Conveyor;
using Commands.Commands;
using Commands.Contexts;
using Extensions;
using Items.Base;
using Items.Storages;
using Passenger;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;
using Random = UnityEngine.Random;

namespace Check.MainCheck
{
    public enum ProcessorState
    {
        Empty,
        Idle,
        Processing
    }

    //Enqueues standard commands to passenger
    public class CheckProcessor : MonoBehaviour
    {
        [SerializeField] private Transform pointIntroscope;
        [SerializeField] private Transform pointDetector;

        [Space, SerializeField] private StorageAbstract introscopeStorage;
        [SerializeField] private StorageAbstract metallicStorage;

        [Space, SerializeField] private InteractableConveyor introscopeConveyor;
        
        private PassengerController _processedPassenger;

        private ICompletable _lastIssuedCommand;

        private IObservableState<ProcessorState> _processorState;

        public void AppointPassenger(PassengerController passenger)
        {
            if (passenger == null)
            {
                throw new ArgumentNullException(nameof(passenger));
            }

            if (_processedPassenger != null)
            {
                throw new Exception("Passenger is already appointed.");
            }

            _processedPassenger = passenger;
            _processorState.HandleUpdate(ProcessorState.Idle);
        }

        public ProcessedPassenger TakeProcessable()
        {
            if (_processedPassenger == null)
            {
                throw new NullReferenceException(nameof(_processedPassenger));
            }

            var passenger = _processedPassenger;
            _processedPassenger = null;

            var bags = introscopeStorage.items.ToList();
            bags.ForEach(x => introscopeStorage.TryRemoveItem(x));

            var items = metallicStorage.items.ToList();
            items.ForEach(x => metallicStorage.TryRemoveItem(x));
            
            _processorState.HandleUpdate(ProcessorState.Empty);

            return new ProcessedPassenger
            {
                passenger = passenger,
                bags = bags,
                items = items
            };
        }

        public ICompletable AppointPlaceBaggage()
        {
            var contexts = new List<ICommandContext>();

            contexts.Add(new MoveToContext(pointIntroscope.position));
            contexts.Add(new WaitContext(0.5f));
            contexts.Add(new TransferItemToContext(introscopeStorage,
                (storage) => storage.items.Where(item => (item.GetDefinition().tag & ItemTag.Bag) != 0).ToList()));
                
            return AppointCommand(contexts);
        }

        public ICompletable AppointPlaceMetallic()
        {
            var contexts = new List<ICommandContext>();

            contexts.Add(new MoveToContext(pointIntroscope.position));
            contexts.Add(new WaitContext(0.5f));
            contexts.Add(new TransferItemToContext(metallicStorage,
                (storage) =>
                {
                    var accuracy = _processedPassenger.accuracy;
                    return storage.items.Where(item =>
                    {
                        var tags = item.GetDefinition().tag;

                        var notBag = (tags & ItemTag.Bag) == 0;

                        var notIllegal = Random.value < accuracy
                            ? (tags & ItemTag.Illegal) == 0
                            : true;

                        var isMetallic = Random.value < accuracy
                            ? (tags & ItemTag.Metallic) != 0
                            : (tags & ItemTag.Metallic) == 0;

                        return notBag && notIllegal && isMetallic;
                    }).ToList();
                }));

            return AppointCommand(contexts);
        }

        public ICompletable AppointMoveBaggage(List<Item> itemsToMove)
        {
            var contexts = new List<ICommandContext>();

            var introscopeBatch = new List<ICommandContext>();
            var detectorBatch = new List<ICommandContext>();

            itemsToMove.ForEach(item =>
            {
                var itemPosition = item.transform.position;

                if (Vector3.Distance(itemPosition, pointIntroscope.position) <
                    Vector3.Distance(itemPosition, pointDetector.position))
                {
                    introscopeBatch.Add(new MoveConveyorItemContext(item, introscopeConveyor));
                }
                else
                {
                    detectorBatch.Add(new MoveConveyorItemContext(item, introscopeConveyor));
                }
            });

            if (introscopeBatch.Any())
            {
                introscopeBatch.Insert(0, new MoveToContext(pointIntroscope.position));
            }

            if (detectorBatch.Any())
            {
                detectorBatch.Insert(0, new MoveToContext(pointDetector.position));
            }

            var passengerPosition = _processedPassenger.transform.position;

            if (Vector3.Distance(passengerPosition, pointIntroscope.position) <
                Vector3.Distance(passengerPosition, pointDetector.position))
            {
                contexts.AddRange(introscopeBatch);
                contexts.AddRange(detectorBatch);
            }
            else
            {
                contexts.AddRange(detectorBatch);
                contexts.AddRange(introscopeBatch);
            }

            return AppointCommand(contexts);
        }

        public ICompletable AppointPassDetector()
        {
            var contexts = new List<ICommandContext>();

            contexts.Add(new MoveToContext(pointIntroscope.position));
            contexts.Add(new WaitContext(0.5f));
            contexts.Add(new MoveToContext(pointDetector.position));

            return AppointCommand(contexts);
        }

        public ICompletable AppointTakeItems()
        {
            var contexts = new List<ICommandContext>();

            contexts.Add(new MoveToContext(pointDetector.position));
            contexts.Add(new WaitContext(0.5f));
            contexts.Add(new TransferItemFromContext(introscopeStorage, (storage) => storage.items.ToList()));
            contexts.Add(new WaitContext(0.5f));
            contexts.Add(new TransferItemFromContext(metallicStorage, (storage) => storage.items.ToList()));

            return AppointCommand(contexts);
        }

        private void Awake()
        {
            _processorState = ServiceProvider.instance.Resolve<IObservableState<ProcessorState>>();
            _processorState.HandleUpdate(ProcessorState.Empty);
        }
        
        private ICompletable AppointCommand(List<ICommandContext> contexts)
        {
            if (_processedPassenger == null)
            {
                throw new NullReferenceException(nameof(_processedPassenger));
            }

            ICompletable lastCommand = null;

            contexts.ForEach(x =>
                lastCommand = _processedPassenger.EnqueueCommand(x));

            SetLastCommand(lastCommand);

            return lastCommand;
        }

        private void SetLastCommand(ICompletable command)
        {
            if (command == null)
            {
                return;
            }

            _processorState.HandleUpdate(ProcessorState.Processing);

            if (_lastIssuedCommand != null)
            {
                _lastIssuedCommand.OnComplete -= HandleAppointmentCompletion;
            }

            _lastIssuedCommand = command;
            _lastIssuedCommand.OnComplete += HandleAppointmentCompletion;
        }

        private void HandleAppointmentCompletion(bool isSuccessful)
        {
            _processorState.HandleUpdate(ProcessorState.Idle);

            if (_lastIssuedCommand == null)
            {
                return;
            }

            _lastIssuedCommand.OnComplete -= HandleAppointmentCompletion;
        }

        private void OnDestroy()
        {
            if (_lastIssuedCommand != null)
            {
                _lastIssuedCommand.OnComplete -= HandleAppointmentCompletion;
            }
        }
    }
}