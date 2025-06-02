using System;
using UnityEngine;

namespace Interactive.Usables
{
    public class UsableBehaviour : MonoBehaviour, IUsable
    {
        public virtual event Action OnUsed
        {
            add => _onUsed += value;
            remove => _onUsed -= value;
        }

        private Action _onUsed;

        public virtual event Action OnCancelled
        {
            add => _onCancelled += value;
            remove => _onCancelled -= value;
        }

        private Action _onCancelled;

        [ContextMenu(nameof(Use))]
        public void Use()
        {
            if (!TryUse())
            {
                return;
            }

            _onUsed?.Invoke();
        }

        [ContextMenu(nameof(Cancel))]
        public void Cancel()
        {
            if (!TryCancel())
            {
                return;
            }

            _onCancelled?.Invoke();
        }

        protected bool HasListeners()
        {
            return _onUsed != null || _onCancelled != null;
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