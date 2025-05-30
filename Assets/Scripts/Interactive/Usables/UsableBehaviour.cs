using System;
using UnityEngine;

namespace Interactive.Usables
{
    public class UsableBehaviour : MonoBehaviour, IUsable
    {
        public event Action OnUsed;
        public event Action OnCancelled;

        [ContextMenu(nameof(Use))]
        public void Use()
        {
            if (!TryUse())
            {
                return;
            }

            OnUsed?.Invoke();
        }

        [ContextMenu(nameof(Cancel))]
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