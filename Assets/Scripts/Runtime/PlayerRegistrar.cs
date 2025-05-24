using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using Utils.Observable;
using Utils.SimpleDI;

namespace Runtime
{
    public enum Side
    {
        Left,
        Right
    }

    public enum PlayerActions
    {
        Select,
        Activate
    }

    public enum PlayerControlMode
    {
        Direct,
        Ray
    }

    [Serializable]
    public class PlayerRegistrar
    {
        [SerializeField] private TeleportationProvider teleportationProvider;

        [Space, SerializeField] private XRDirectInteractor leftInteractor;
        [SerializeField] private XRDirectInteractor rightInteractor;

        [Space, SerializeField] private InputActionReference leftSelectReference;
        [SerializeField] private InputActionReference rightSelectReference;
        [SerializeField] private InputActionReference leftActivateReference;
        [SerializeField] private InputActionReference rightActivateReference;

        public void Register(ServiceProvider serviceProvider)
        {
            serviceProvider.Register(teleportationProvider);

            serviceProvider.Register((Side.Left, PlayerActions.Select), leftSelectReference);
            serviceProvider.Register((Side.Right, PlayerActions.Select), rightSelectReference);
            serviceProvider.Register((Side.Left, PlayerActions.Activate), leftActivateReference);
            serviceProvider.Register((Side.Right, PlayerActions.Activate), rightActivateReference);

            serviceProvider.Register(Side.Left, leftInteractor);
            serviceProvider.Register(Side.Right, rightInteractor);

            serviceProvider.Register<IObservableState<PlayerControlMode>>(Side.Left,
                new ObservableState<PlayerControlMode>(PlayerControlMode.Direct));
            serviceProvider.Register<IObservableState<PlayerControlMode>>(Side.Right,
                new ObservableState<PlayerControlMode>(PlayerControlMode.Direct));
        }
    }
}