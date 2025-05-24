using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Utils.SimpleDI;

namespace Interactive.Teleport
{
    public class Teleport : MonoBehaviour
    {
        public void Invoke()
        {
            var provider = ServiceProvider.instance.Resolve<TeleportationProvider>();

            var teleportRequest = new TeleportRequest();
            teleportRequest.destinationPosition = transform.position;
            teleportRequest.destinationRotation = transform.rotation;
            teleportRequest.matchOrientation = MatchOrientation.TargetUpAndForward;
            provider.QueueTeleportRequest(teleportRequest);
        }
    }
}