using System.Collections.Generic;
using UnityEngine;

namespace Interactive.Usables
{
    public class UsableBatch : UsableBehaviour
    {
        [SerializeField] private List<UsableBehaviour> usables;

        protected override bool TryUse()
        {
            if (!enabled)
            {
                return false;
            }

            usables.ForEach(x => x.Use());

            return true;
        }

        protected override bool TryCancel()
        {
            if (!enabled)
            {
                return false;
            }

            usables.ForEach(x => x.Cancel());

            return true;
        }
    }
}