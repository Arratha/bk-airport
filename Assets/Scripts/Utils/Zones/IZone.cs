using UnityEngine;
using Utils.Initializable;

namespace Utils.Zones
{
    public interface IZone : IInitializable<ZoneInitArgs>
    {
        public Vector3 position { get; }

        public Quaternion rotation { get; }

        public bool IsInside(Vector3 pointPosition);
    }
}