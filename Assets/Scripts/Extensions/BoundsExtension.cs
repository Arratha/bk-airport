using UnityEngine;

namespace Extensions
{
    public static class BoundsExtension
    {
        public static bool FullyIntersects(this Bounds a, Bounds b)
        {
            return (double)a.min.x < (double)b.max.x &&
                   (double)a.max.x > (double)b.min.x &&
                   (double)a.min.y < (double)b.max.y &&
                   (double)a.max.y > (double)b.min.y &&
                   (double)a.min.z < (double)b.max.z &&
                   (double)a.max.z > (double)b.min.z;
        }
    }
}