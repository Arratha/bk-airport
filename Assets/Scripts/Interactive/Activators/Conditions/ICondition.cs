using System;
using Utils.Initializable;

namespace Interactive.Activators.Conditions
{
    public interface ICondition : IInitializable
    {
        public bool isSatisfied { get; }

        public event Action OnChanged;
    }
}