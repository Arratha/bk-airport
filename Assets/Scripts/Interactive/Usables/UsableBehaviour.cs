using System;
using UnityEngine;

namespace Interactive.Usables
{
    public class UsableBehaviour : MonoBehaviour, IUsable
    {
        public event Action OnUsed;
        public event Action OnCancelled;

        public void Use()
        {
            if (!TryUse())
            {
                return;
            }

            OnUsed?.Invoke();
        }

        public void Cancel()
        {
            if (!TryCancel())
            {
                return;
            }

            OnCancelled?.Invoke();
        }

        protected virtual bool TryUse()
        {
            return enabled;
        }
        
        protected virtual bool TryCancel()
        {
            return enabled;
        }
    }
}