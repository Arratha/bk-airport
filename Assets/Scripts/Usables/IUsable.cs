using System;

namespace Usables
{
    public interface IUsable
    {
        public bool enabled { get; set; }

        public event Action OnUsed;

        public void Use();
    }
}