using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Items.Storages.Placers
{
    //Provide information to modify item placement
    public class AttachmentBounds : MonoBehaviour
    {
        public Vector3 positionOffset => selfPositionOffset;
        [SerializeField] private Vector3 selfPositionOffset;

        public Vector3 rotationOffset => selfRotationOffset;
        [SerializeField] private Vector3 selfRotationOffset;

        public Vector3 size => selfSize;
        [SerializeField] private Vector3 selfSize;

        public Bounds bounds
        {
            get
            {
                var worldPosition = transform.TransformPoint(positionOffset);
                var worldRotation = transform.rotation * Quaternion.Euler(rotationOffset);

                var matrix = Matrix4x4.TRS(worldPosition, worldRotation, Vector3.one);
                var corners = GetCorners(size * 0.5f);

                var min = Vector3.positiveInfinity;
                var max = Vector3.negativeInfinity;

                for (var i = 0; i < corners.Length; i++)
                {
                    var worldCorner = matrix.MultiplyPoint3x4(corners[i]);
                    min = Vector3.Min(min, worldCorner);
                    max = Vector3.Max(max, worldCorner);
                }

                var boundsSize = max - min;
                var boundsCenter = min + boundsSize * 0.5f;

                return new Bounds(boundsCenter, boundsSize);
            }
        }

        private Vector3[] GetCorners(Vector3 extents)
        {
            return new Vector3[]
            {
                new(-extents.x, -extents.y, -extents.z),
                new(extents.x, -extents.y, -extents.z),
                new(-extents.x, extents.y, -extents.z),
                new(extents.x, extents.y, -extents.z),
                new(-extents.x, -extents.y, extents.z),
                new(extents.x, -extents.y, extents.z),
                new(-extents.x, extents.y, extents.z),
                new(extents.x, extents.y, extents.z)
            };
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
            var dimensions = max - min;

            selfSize = transform.InverseTransformDirection(dimensions);
            selfPositionOffset = transform.InverseTransformPoint(center);

            EditorUtility.SetDirty(this);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.5f, 0.5f, 1f, 0.5f);
            DrawRotatedBounds();

            Gizmos.color = new Color(1f, 0.5f, 0.5f, 0.5f);
            Gizmos.DrawWireCube(bounds.center, bounds.size);

            Gizmos.color = new Color(0.5f, 1f, 0.5f, 0.5f);
            Gizmos.DrawWireSphere(transform.TransformPoint(positionOffset), 0.05f);

            DrawAxis(positionOffset, Quaternion.Euler(rotationOffset) * Vector3.right, Color.red);
            DrawAxis(positionOffset, Quaternion.Euler(rotationOffset) * Vector3.up, Color.green);
            DrawAxis(positionOffset, Quaternion.Euler(rotationOffset) * Vector3.forward, Color.blue);
        }

        private void DrawRotatedBounds()
        {
            var worldPosition = transform.TransformPoint(positionOffset);
            var worldRotation = transform.rotation * Quaternion.Euler(rotationOffset);

            var matrix = Matrix4x4.TRS(worldPosition, worldRotation, Vector3.one);
            var corners = GetCorners(size * 0.5f);

            DrawEdge(corners[0], corners[1], matrix);
            DrawEdge(corners[0], corners[2], matrix);
            DrawEdge(corners[0], corners[4], matrix);
            DrawEdge(corners[1], corners[3], matrix);
            DrawEdge(corners[1], corners[5], matrix);
            DrawEdge(corners[2], corners[3], matrix);
            DrawEdge(corners[2], corners[6], matrix);
            DrawEdge(corners[3], corners[7], matrix);
            DrawEdge(corners[4], corners[5], matrix);
            DrawEdge(corners[4], corners[6], matrix);
            DrawEdge(corners[5], corners[7], matrix);
            DrawEdge(corners[6], corners[7], matrix);
        }

        private void DrawEdge(Vector3 a, Vector3 b, Matrix4x4 matrix)
        {
            var worldA = matrix.MultiplyPoint3x4(a);
            var worldB = matrix.MultiplyPoint3x4(b);
            Gizmos.DrawLine(worldA, worldB);
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