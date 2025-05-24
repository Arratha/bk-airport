using UnityEngine;
using Interactive.Usables;

namespace Check.MainCheck.Conveyor
{
    //Defines conveyor move condition by usable activation
    public class UsableDrivenConveyor : ConveyorAbstract
    {
        [SerializeField] private UsableBehaviour moveButton;
        [SerializeField] private UsableBehaviour reverseButton;
        
        protected override void OnInit()
        {
            moveButton.OnUsed += HandleMoveUsed;
            reverseButton.OnUsed += HandleReverseUsed;
        }

        private void HandleMoveUsed()
        {
            ShouldMove = !ShouldMove;
            ShouldReverse = false;
        }

        private void HandleReverseUsed()
        {
            ShouldReverse = !ShouldReverse;
            ShouldMove = false;
        }

        private void OnDestroy()
        {
            if (moveButton != null)
            {
                moveButton.OnUsed -= HandleMoveUsed;
            }
            
            if (reverseButton != null)
            {
                reverseButton.OnUsed -= HandleReverseUsed;
            }
        }
    }
}