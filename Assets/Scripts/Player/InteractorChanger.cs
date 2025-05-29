using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Player
{
    public class InteractorChanger : MonoBehaviour
    {
        private XRDirectInteractor _direct;
        private XRRayInteractor _ray;
        private XRPokeInteractor _poke;

        private void Awake()
        {
            _direct = GetComponentInChildren<XRDirectInteractor>(true);
            _ray = GetComponentInChildren<XRRayInteractor>(true);
            _poke = GetComponentInChildren<XRPokeInteractor>(true);

            _direct.selectEntered.AddListener(OnSelected);
            _direct.selectExited.AddListener(OnDeselected);
        }

        private void OnSelected(SelectEnterEventArgs _)
        {
            _ray.enabled = false;
            _poke.enabled = false;
        }

        private void OnDeselected(SelectExitEventArgs _)
        {
            _ray.enabled = true;
            _poke.enabled = true;
        }

        private void OnDestroy()
        {
            if (_direct == null)
            {
                return;
            }

            _direct.selectEntered.RemoveListener(OnSelected);
            _direct.selectExited.RemoveListener(OnDeselected);
        }
    }
}