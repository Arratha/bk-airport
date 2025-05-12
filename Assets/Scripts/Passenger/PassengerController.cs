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
    [RequireComponent(typeof(IMovable))]
    [RequireComponent(typeof(StorageAbstract))]
    public class PassengerController : MonoBehaviour
    {
        private Queue<ICommand> _commands = new();

        private CommandFactory _factory;

        private ICommand _currentCommand;

        private void Awake()
        {
            var movable = GetComponent<IMovable>();
            var storage = GetComponent<StorageAbstract>();

            _factory = new CommandFactory(movable, storage);
        }

        private void FixedUpdate()
        {
            if (_currentCommand != null)
            {
                _currentCommand.Execute(Time.fixedDeltaTime);
            }

            if (_currentCommand != null && !_currentCommand.isCompleted)
            {
                return;
            }

            if (_commands.Any())
            {
                _currentCommand = _commands.Dequeue();
            }
        }

        public void EnqueueCommand(ICommandContext context)
        {
            var command = _factory.CreateCommand(context);
            _commands.Enqueue(command);
        }

        public void StopCommands()
        {
            _currentCommand = null;
            _commands.Clear();
        }
    }
}