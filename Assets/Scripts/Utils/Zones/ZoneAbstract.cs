using System;
using UnityEngine;

namespace Utils.Zones
{
    [Serializable]
    public abstract class ZoneAbstract : IZone, IDrawable
    {
        public Vector3 position
        {
            get
            {
                if (!isInitialized)
                {
                    throw new Exception($"The object {this} is not initialized.");
                }

                return transform.position + ModifyPositionByRotation(offset);
            }
        }

        public Quaternion rotation
        {
            get
            {
                if (!isInitialized)
                {
                    throw new Exception($"The object {this} is not initialized.");
                }

                return transform.rotation;
            }
        }

        [HideInInspector, SerializeField] private Transform transform;

        public bool isInitialized => selfIsInitialized;
        [HideInInspector, SerializeField] private bool selfIsInitialized;

        [SerializeField] protected Color drawColor;

        [Space, SerializeField] private Vector3 offset;

        public void Initialize(ZoneInitArgs args)
        {
            if (isInitialized)
            {
                throw new Exception($"The object {this} is already initialized.");
            }

            selfIsInitialized = true;

            transform = args.transform;
        }

        public void Deinitialize()
        {
            if (!isInitialized)
            {
                throw new Exception($"The object {this} is not initialized.");
            }

            selfIsInitialized = false;

            transform = null;
        }

        public abstract bool IsInside(Vector3 pointPosition);

        public abstract void Draw();

        protected Vector3 ModifyPositionByRotation(Vector3 pos)
        {
            return rotation * pos;
        }
    }
}