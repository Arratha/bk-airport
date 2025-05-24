using Interactive.Usables;
using Interactive.Teleport;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Check
{
    //Change current check type by interacting with usable
    //Teleports player to given point
    public class CheckChanger : MonoBehaviour
    {
        [SerializeField] private CheckType newType;

        [Space, SerializeField] private Teleport teleport;
        [SerializeField] private UsableBehaviour[] usables;

        private IObservableState<CheckType> _state;

        private void Awake()
        {
            foreach (var usable in usables)
            {
                usable.OnUsed += HandleUsed;
            }

            _state = ServiceProvider.instance.Resolve<IObservableState<CheckType>>();
        }

        private void HandleUsed()
        {
            teleport.Invoke();

            _state.HandleUpdate(newType);
        }

        private void OnDestroy()
        {
            foreach (var usable in usables)
            {
                if (usable != null)
                {
                    usable.OnUsed += HandleUsed;
                }
            }
        }
    }
}