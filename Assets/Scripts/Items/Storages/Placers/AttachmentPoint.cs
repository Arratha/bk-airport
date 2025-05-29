using System.Linq;
using UnityEngine;

namespace Items.Storages.Placers
{
    public class AttachmentPoint : MonoBehaviour
    {
        public Vector3 positionOffset => selfPositionOffset;
        [SerializeField] private Vector3 selfPositionOffset;

        public Vector3 rotationOffset => selfRotationOffset;
        [SerializeField] private Vector3 selfRotationOffset;

        public void Attach(Transform attachPoint)
        {
            var selfTransform = transform.transform;

            selfTransform.SetParent(attachPoint);

            selfTransform.localPosition = positionOffset;
            selfTransform.localRotation = Quaternion.Euler(rotationOffset);
        }

        [ContextMenu(nameof(Centralize))]
        private void Centralize()
        {
            selfRotationOffset = Vector3.zero;

            var renderers = GetComponentsInChildren<Renderer>(true);

            if (!renderers.Any())
            {
                return;
            }

            var max = Vector3.negativeInfinity;
            var min = Vector3.positiveInfinity;

            foreach (var rend in renderers)
            {
                max.x = Mathf.Max(max.x, rend.bounds.max.x);
                max.y = Mathf.Max(max.y, rend.bounds.max.y);
                max.z = Mathf.Max(max.z, rend.bounds.max.z);
                
                min.x = Mathf.Min(min.x, rend.bounds.min.x);
                min.y = Mathf.Min(min.y, rend.bounds.min.y);
                min.z = Mathf.Min(min.z, rend.bounds.min.z);
            }

            var center = (max + min) / 2;

            selfPositionOffset = transform.InverseTransformPoint(center);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.5f, 1f, 0.5f, 0.5f);
            Gizmos.DrawWireSphere(transform.TransformPoint(positionOffset), 0.05f);

            DrawAxis(positionOffset, Quaternion.Euler(rotationOffset) * Vector3.right, Color.red);
            DrawAxis(positionOffset, Quaternion.Euler(rotationOffset) * Vector3.up, Color.green);
            DrawAxis(positionOffset, Quaternion.Euler(rotationOffset) * Vector3.forward, Color.blue);
        }

        private void DrawAxis(Vector3 position, Vector3 direction, Color color)
        {
            Gizmos.color = color;

            var worldPos = transform.TransformPoint(position);
            var worldDir = transform.TransformDirection(direction);

            Gizmos.DrawLine(worldPos, worldPos + worldDir * 0.2f);
        }
    }
}