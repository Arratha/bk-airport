using UnityEngine;

namespace Items.Storages.Attachers.Placers
{
    public class AttachmentPoint : MonoBehaviour
    {
        public Transform point => selfPoint;
        [SerializeField] private Transform selfPoint;
    }
}