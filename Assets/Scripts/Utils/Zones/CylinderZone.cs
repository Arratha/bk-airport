using System;
using UnityEngine;

namespace Utils.Zones
{
    [Serializable]
    public class CylinderZone : ZoneAbstract
    {
        [Space, SerializeField] private float radius;
        [SerializeField] private float height;

        public override bool IsInside(Vector3 pointPosition)
        {
            if (!isInitialized)
            {
                throw new Exception($"The object {this} is not initialized.");
            }
            
            var localPoint = Quaternion.Inverse(rotation) * (pointPosition - position);
            var distXZ = new Vector2(localPoint.x, localPoint.z).magnitude;
            
            if (distXZ > radius)
            {
                return false;
            }

            if (Mathf.Abs(localPoint.y) > height / 2f)
            {
                return false;
            }

            return true;
        }

        public override void Draw()
        {
            if (!isInitialized)
            {
                throw new Exception($"The object {this} is not initialized.");
            }

            Gizmos.color = drawColor;

            var segments = 32;

            var topCircle = new Vector3[segments];
            var bottomCircle = new Vector3[segments];

            for (var i = 0; i < segments; i++)
            {
                var angle = (float)i / segments * Mathf.PI * 2f;
                var x = Mathf.Cos(angle) * radius;
                var z = Mathf.Sin(angle) * radius;

                var offset = new Vector3(x, 0f, z);
                topCircle[i] = position + ModifyPositionByRotation(offset + Vector3.up * height / 2);
                bottomCircle[i] = position + ModifyPositionByRotation(offset - Vector3.up * height / 2);
            }

            for (var i = 0; i < segments; i++)
            {
                var next = (i + 1) % segments;

                Gizmos.DrawLine(topCircle[i], topCircle[next]);
                Gizmos.DrawLine(bottomCircle[i], bottomCircle[next]);
                Gizmos.DrawLine(topCircle[i], bottomCircle[i]);
            }
        }
    }
}