using System;
using System.Collections.Generic;
using System.Linq;
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
    public enum CheckStage
    {
        PlaceBaggage,
        PlaceMetallic,
        PassDetector,
        TakeItems
    }

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

        public ICompletable AppointCommand(CheckStage stage)
        {
            if (_processedPassenger == null)
            {
                throw new NullReferenceException(nameof(_processedPassenger));
            }

            var contexts = new List<ICommandContext>();

            switch (stage)
            {
                case CheckStage.PlaceBaggage:
                    contexts = GetBaggageContexts();
                    break;
                case CheckStage.PlaceMetallic:
                    contexts = GetMetallicContexts();
                    break;
                case CheckStage.PassDetector:
                    contexts = GetDetectorContexts();
                    break;
                case CheckStage.TakeItems:
                    contexts = GetTakeItemsContexts();
                    break;
            }

            ICompletable lastCommand = null;

            contexts.ForEach(x =>
                lastCommand = _processedPassenger.EnqueueCommand(x));

            SetLastCommand(lastCommand);

            return lastCommand;
        }

        private void Awake()
        {
            _processorState = ServiceProvider.instance.Resolve<IObservableState<ProcessorState>>();
            _processorState.HandleUpdate(ProcessorState.Empty);
        }

        private List<ICommandContext> GetBaggageContexts()
        {
            var result = new List<ICommandContext>();

            result.Add(new MoveToContext(pointIntroscope.position));
            result.Add(new WaitContext(0.5f));
            result.Add(new TransferItemToContext(introscopeStorage,
                (storage) => storage.items.Where(item => (item.GetDefinition().tag & ItemTag.Bag) != 0).ToList()));
                
            return result;
        }

        private List<ICommandContext> GetMetallicContexts()
        {
            var result = new List<ICommandContext>();

            result.Add(new MoveToContext(pointIntroscope.position));
            result.Add(new WaitContext(0.5f));
            result.Add(new TransferItemToContext(metallicStorage,
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

            return result;
        }

        private List<ICommandContext> GetDetectorContexts()
        {
            var result = new List<ICommandContext>();

            result.Add(new MoveToContext(pointIntroscope.position));
            result.Add(new WaitContext(0.5f));
            result.Add(new MoveToContext(pointDetector.position));

            return result;
        }

        private List<ICommandContext> GetTakeItemsContexts()
        {
            var result = new List<ICommandContext>();

            result.Add(new MoveToContext(pointDetector.position));
            result.Add(new WaitContext(0.5f));
            result.Add(new TransferItemFromContext(introscopeStorage, (storage) => storage.items.ToList()));
            result.Add(new WaitContext(0.5f));
            result.Add(new TransferItemFromContext(metallicStorage, (storage) => storage.items.ToList()));

            return result;
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