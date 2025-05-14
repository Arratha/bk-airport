using System;
using System.Collections.Generic;
using Commands.Commands;
using Commands.Contexts;
using Extensions;
using Items.Base;
using Items.Storages;
using Passenger;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

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

        public PassengerController TakePassenger()
        {
            if (_processedPassenger == null)
            {
                throw new NullReferenceException(nameof(_processedPassenger));
            }

            var passenger = _processedPassenger;
            _processedPassenger = null;
         
            _processorState.HandleUpdate(ProcessorState.Empty);
            
            return passenger;
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
                (identifier) => (identifier.GetDefinition().tag & ItemTag.Bag) != 0));

            return result;
        }

        private List<ICommandContext> GetMetallicContexts()
        {
            var result = new List<ICommandContext>();

            result.Add(new MoveToContext(pointIntroscope.position));
            result.Add(new WaitContext(0.5f));
            result.Add(new TransferItemToContext(metallicStorage,
                (identifier) =>
                {
                    var tags = identifier.GetDefinition().tag;

                    return (tags & ItemTag.Bag) == 0 && (tags & ItemTag.Illegal) == 0 &&
                           (tags & ItemTag.Metallic) != 0;
                }));

            return result;
        }

        private List<ICommandContext> GetDetectorContexts()
        {
            var result = new List<ICommandContext>();

            result.Add(new MoveToContext(pointIntroscope.position));
            result.Add(new MoveToContext(pointDetector.position));

            return result;
        }

        private List<ICommandContext> GetTakeItemsContexts()
        {
            var result = new List<ICommandContext>();

            result.Add(new MoveToContext(pointDetector.position));
            result.Add(new WaitContext(0.5f));
            result.Add(new TransferItemFromContext(introscopeStorage, (_) => true));
            result.Add(new WaitContext(0.5f));
            result.Add(new TransferItemFromContext(metallicStorage, (_) => true));

            return result;
        }

        private void SetLastCommand(ICompletable command)
        {
            if (command == null)
            {
                return;
            }

            _processorState.HandleUpdate(ProcessorState.Processing);

            if (_lastIssuedCommand != null && !_lastIssuedCommand.isDisposed)
            {
                _lastIssuedCommand.OnComplete -= HandleAppointmentCompletion;
            }

            _lastIssuedCommand = command;
            _lastIssuedCommand.OnComplete += HandleAppointmentCompletion;
        }

        private void HandleAppointmentCompletion(bool isSuccessful)
        {
            _processorState.HandleUpdate(ProcessorState.Idle);

            if (_lastIssuedCommand == null || !_lastIssuedCommand.isDisposed)
            {
                return;
            }

            _lastIssuedCommand.OnComplete -= HandleAppointmentCompletion;
        }

        private void OnDestroy()
        {
            if (_lastIssuedCommand != null && _lastIssuedCommand.isDisposed)
            {
                _lastIssuedCommand.OnComplete -= HandleAppointmentCompletion;
            }
        }
    }
}