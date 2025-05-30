using UnityEngine;
using Interactive.Usables;

namespace Check.MainCheck.Conveyor
{
    //Defines conveyor move condition by usable activation
    public class UsableDrivenConveyor : ConveyorAbstract
    {
        [Space, SerializeField] private UsableBehaviour moveButton;
        [SerializeField] private UsableBehaviour reverseButton;

        private bool _isMovePressed;
        private bool _isReversePressed;

        private Vector2 _originalSpeed;

        protected override void OnInit()
        {
            moveButton.OnUsed += HandleMoveUsed;
            reverseButton.OnUsed += HandleReverseUsed;

            _originalSpeed = speed;
        }

        private void HandleMoveUsed()
        {
            _isMovePressed = !_isMovePressed;
            _isReversePressed = false;

            speed = _originalSpeed;

            shouldMove = _isMovePressed || _isReversePressed;
        }

        private void HandleReverseUsed()
        {
            _isReversePressed = !_isReversePressed;
            _isMovePressed = false;

            speed = _originalSpeed * -1;

            shouldMove = _isMovePressed || _isReversePressed;
        }

        protected override void HandleDestroy()
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