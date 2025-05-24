using System;

namespace Interactive.Usables
{
    public interface IUsable
    {
        public bool enabled { get; set; }

        public event Action OnUsed;

        public event Action OnCancelled;

        public void Use();

        public void Cancel();
    }
}