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
    public class PassengerController : MonoBehaviour, ICommandExecutor
    {
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
            if (_currentCommand != null)
            {
                _currentCommand.Dispose();
                _currentCommand = null;
            }

            while (_commands.Any())
            {
                _commands.Dequeue().Dispose();
            }
        }

        public void Dispose()
        {
            StopCommands();
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
                }

                return;
            }

            _currentCommand.Execute(Time.fixedDeltaTime);

            if (_currentCommand.isCompleted)
            {
                _currentCommand.Dispose();
                _currentCommand = null;
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}