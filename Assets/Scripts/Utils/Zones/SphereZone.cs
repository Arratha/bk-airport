using System;
using UnityEngine;

namespace Utils.Zones
{
    [Serializable]
    public class SphereZone : ZoneAbstract
    {
        [Space, SerializeField] private float radius;

        public override bool IsInside(Vector3 pointPosition)
        {
            if (!isInitialized)
            {
                throw new Exception($"The object {this} is not initialized.");
            }

            return Vector3.Distance(position, pointPosition) <= radius;
        }

        public override void Draw()
        {
            if (!isInitialized)
            {
                throw new Exception($"The object {this} is not initialized.");
            }

            Gizmos.color = drawColor;
            Gizmos.DrawWireSphere(position, radius);
        }
    }
}