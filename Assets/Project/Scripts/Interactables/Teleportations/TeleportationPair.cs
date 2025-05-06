using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Airport.Interactables.Teleportations
{
    public class TeleportationPair : MonoBehaviour
    {
        [SerializeField] private TeleportationStation first;
        [SerializeField] private TeleportationStation second;

        private TeleportationProvider _provider;

        private Transform _activeAnchor;

        private void Awake()
        {
            InitializeProvider();
            InitializeStations();
        }

        private void InitializeProvider()
        {
            var player = GameObject.FindWithTag("Player");

            _provider = player.GetComponentInChildren<TeleportationProvider>(true);
        }

        private void InitializeStations()
        {
            first.tpButton.selectEntered.AddListener(_ => ForceTeleport(second.anchor));
            second.tpButton.selectEntered.AddListener(_ => ForceTeleport(first.anchor));
        }

        private void ForceTeleport(Transform anchor)
        {
            var teleportRequest = new TeleportRequest();
            teleportRequest.destinationPosition = anchor.position;
            teleportRequest.destinationRotation = anchor.rotation;
            teleportRequest.matchOrientation = MatchOrientation.TargetUpAndForward;
            _provider.QueueTeleportRequest(teleportRequest);
        }
    }
}