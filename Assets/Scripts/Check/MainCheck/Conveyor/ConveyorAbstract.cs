using Grid;
using UnityEngine;

namespace Check.MainCheck.Conveyor
{
    [RequireComponent(typeof(ConveyorGrid))]
    public abstract class ConveyorAbstract : MonoBehaviour
    {
        [SerializeField] private Vector2 speed;

        private ConveyorGrid _grid;

        protected bool ShouldMove;
        protected bool ShouldReverse;

        private void Awake()
        {
            _grid = GetComponent<ConveyorGrid>();

            OnInit();
        }

        protected virtual void OnInit()
        {

        }

        private void FixedUpdate()
        {
            if (!ShouldMove && !ShouldReverse)
            {
                return;
            }

            var modifiedSpeed = speed * Time.fixedDeltaTime;
            modifiedSpeed *= ShouldReverse ? -1 : 1;

            _grid.TryMove(modifiedSpeed);
        }
    }
}