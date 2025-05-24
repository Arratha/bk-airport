using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Utils.Observable;

namespace Interactive.Usables
{
    [RequireComponent(typeof(XRBaseInteractable))]
    public class XRUsable : UsableBehaviour
    {
        private XRBaseInteractable _interactable;

        private void Awake()
        {
            _interactable = GetComponent<XRBaseInteractable>();
            
            _interactable.selectEntered.AddListener(_ => Use());
            _interactable.selectExited.AddListener(_ => Cancel());
        }

        private void OnDestroy()
        {
            if (_interactable == null)
            {
                return;
            }

            _interactable.selectEntered.RemoveAllListeners();
            _interactable.selectExited.RemoveAllListeners();
        }
        
        private void OnEnable()
        {
            if (_interactable == null)
            {
                return;
            }

            _interactable.enabled = true;
        }

        private void OnDisable()
        {
            if (_interactable == null)
            {
                return;
            }
            
            _interactable.enabled = false;
        }
    }
}