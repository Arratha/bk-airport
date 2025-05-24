using UnityEngine;
using Interactive.Usables;
using Utils.Observable;
using Utils.SimpleDI;

namespace Check.MainCheck.Appointors
{   
    //Initiates additional check
    //- Removes passenger from processor
    //- Updates processed passenger state
    public class AdditionalCheckAppointor : MonoBehaviour
    {
        [SerializeField] private UsableBehaviour usable;

        private CheckProcessor _processor;

        private IObservableState<ProcessedPassenger> _state;
        
        private void Awake()
        {
            _processor = GetComponent<CheckProcessor>();
            _state = ServiceProvider.instance.Resolve<IObservableState<ProcessedPassenger>>();
            
            usable.OnUsed += HandleUsed;
        }

        private void HandleUsed()
        {
            var processable = _processor.TakeProcessable();
            _state.HandleUpdate(processable);
        }

        private void OnDestroy()
        {
            if (usable != null)
            {
                usable.OnUsed -= HandleUsed;
            }
        }
    }
}