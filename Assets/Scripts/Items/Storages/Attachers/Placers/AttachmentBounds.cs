using UnityEngine;

namespace Items.Storages.Attachers.Placers
{
    public class AttachmentBounds : MonoBehaviour
    {
        public Transform centralPoint => selfCentralPoint;
        [SerializeField] private Transform selfCentralPoint;

        public Vector3 size => selfSize;
        [SerializeField] private Vector3 selfSize;

        public void OnDrawGizmos()
        {
            if (selfCentralPoint == null)
            {
                return;
            }

            Gizmos.color = Color.yellow;

            Gizmos.DrawWireCube(selfCentralPoint.position, selfSize);
        }
    }
}