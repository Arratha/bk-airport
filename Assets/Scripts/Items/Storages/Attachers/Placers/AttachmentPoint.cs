using UnityEngine;

namespace Items.Storages.Attachers.Placers
{
    //Provide infromation to modify item placement
    public class AttachmentPoint : MonoBehaviour
    {
        public Transform point => selfPoint;
        [SerializeField] private Transform selfPoint;
    }
}