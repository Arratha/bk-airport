using System;
using System.Collections.Generic;
using Commands.Commands;
using Commands.Contexts;
using Items.Storages;
using Passenger.Movables;

namespace Commands
{
    public class CommandFactory
    {
        private readonly IMovable _movable;
        private readonly StorageAbstract _storage;

        private readonly Dictionary<Type, Func<ICommandContext, ICommand>> _commandBuilders;

        public CommandFactory(IMovable movable, StorageAbstract storage)
        {
            _movable = movable ?? throw new ArgumentNullException(nameof(movable));
            _storage = storage;

            _commandBuilders = new Dictionary<Type, Func<ICommandContext, ICommand>>()
            {
                { typeof(MoveToContext), CreateMoveToCommand },
                { typeof(TransferItemToContext), CreateTransferToCommand },
                { typeof(TransferItemFromContext), CreateTransferFromCommand },
                { typeof(GetItemsContext), CreateGetItemsCommand },
                { typeof(WaitContext), CreateWaitCommand }
            };
        }

        public ICommand CreateCommand(ICommandContext context)
        {
            if (_commandBuilders.TryGetValue(context.GetType(), out var builder))
            {
                return builder(context);
            }

            throw new ArgumentException($"Invalid context type {context.GetType()}");
        }

        private ICommand CreateMoveToCommand(ICommandContext context)
        {
            if (!(context is MoveToContext moveToContext))
            {
                throw new ArgumentException($"Invalid context type {context.GetType()}");
            }

            return new MoveToCommand(moveToContext, _movable);
        }

        private ICommand CreateTransferToCommand(ICommandContext context)
        {
            if (!(context is TransferItemToContext transferItemToContext))
            {
                throw new ArgumentException($"Invalid context type {context.GetType()}");
            }

            return new TransferItemToCommand(transferItemToContext, _storage);
        }

        private ICommand CreateTransferFromCommand(ICommandContext context)
        {
            if (!(context is TransferItemFromContext transferItemFromContext))
            {
                throw new ArgumentException($"Invalid context type {context.GetType()}");
            }

            return new TransferItemFromCommand(transferItemFromContext, _storage);
        }

        private ICommand CreateGetItemsCommand(ICommandContext context)
        {
            if (!(context is GetItemsContext getItemsContext))
            {
                throw new ArgumentException($"Invalid context type {context.GetType()}");
            }

            return new GetItemsCommand(getItemsContext, _storage);
        }

        private ICommand CreateWaitCommand(ICommandContext context)
        {
            if (!(context is WaitContext waitContext))
            {
                throw new ArgumentException($"Invalid context type {context.GetType()}");
            }

            return new WaitCommand(waitContext);
        }
    }
}