using System.Collections.Generic;
using System.Linq;
using Commands;
using Commands.Commands;
using Commands.Contexts;
using Items.Storages;
using Passenger.Movables;
using UnityEngine;

namespace Passenger
{
    //Executes enqueued commands
    [RequireComponent(typeof(IMovable))]
    [RequireComponent(typeof(StorageAbstract))]
    public class PassengerController : MonoBehaviour, ICommandExecutor
    {
        public float accuracy
        {
            get
            {
                var result = _accuracy;

                _accuracy = 0.5f + _accuracy / 2;

                return result;
            }
            set => _accuracy = value;
        }

        private float _accuracy;

        private Queue<ICommand> _commands = new();

        private CommandFactory _factory;

        private ICommand _currentCommand;

        public ICompletable EnqueueCommand(ICommandContext context)
        {
            var command = _factory.CreateCommand(context);
            _commands.Enqueue(command);

            return command;
        }

        public void StopCommands()
        {
            _currentCommand = null;
            _commands.Clear();
        }

        private void Awake()
        {
            var movable = GetComponent<IMovable>();
            var storage = GetComponent<StorageAbstract>();

            _factory = new CommandFactory(movable, storage);
        }

        private void FixedUpdate()
        {
            if (_currentCommand == null)
            {
                if (_commands.Any())
                {
                    _currentCommand = _commands.Dequeue();
                    Debug.Log($"New command started {_currentCommand}", gameObject);
                }

                return;
            }

            _currentCommand.Execute(Time.fixedDeltaTime);

            if (_currentCommand.isCompleted)
            {
                _currentCommand = null;
            }
        }
    }
}