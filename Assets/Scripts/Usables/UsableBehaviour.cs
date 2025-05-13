using System;
using UnityEngine;

namespace Usables
{
    public class UsableBehaviour : MonoBehaviour, IUsable
    {
        public event Action OnUsed;

        public void Use()
        {
            if (!TryUse())
            {
                return;
            }

            OnUsed?.Invoke();
        }

        protected virtual bool TryUse()
        {
            return true;
        }
    }
}