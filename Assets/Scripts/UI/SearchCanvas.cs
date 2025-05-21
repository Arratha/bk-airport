using UnityEngine;
using Items.Base;
using Check.AdditionalCheck;
using Utils.Observable;
using Extensions;
using Items.Storages.Attachers.Placers;
using Player;
using Utils.SimpleDI;

namespace UI
{
    //Creates preview for given item ids
    //Disables player input while active
    [RequireComponent(typeof(Canvas))]
    public class SearchCanvas : MonoBehaviour, IObserver<PreviewItems>
    {
        [SerializeField] private Transform attachmentPoint;
        [SerializeField] private float radius = 1f;
        
        private ItemIdentifier[] _currentItems;
        private int _currentItemIndex;

        private GameObject _currentInstance;
        [SerializeField] private float rotationSpeed = 500;

        private Canvas _selfCanvas;
        
        private void Awake()
        {
            _selfCanvas = GetComponent<Canvas>();
            
            var state = ServiceProvider.instance.Resolve<IObservableState<PreviewItems>>();
            state.RegisterObserver(this);
            HandleUpdate(state.GetState());
        }

        public void HandleUpdate(PreviewItems message)
        {
            _currentItems = message.items;
            _currentItemIndex = 0;

            if (_currentItems == null || _currentItems.Length == 0)
            {
                EndSearch();
                return;
            }

            _selfCanvas.enabled = true;

            var state = ServiceProvider.instance.Resolve<IObservableState<PlayerControllable>>();
            state.HandleUpdate(new PlayerControllable { controllable = false });

            ShowCurrentItem();
        }

        private void Update()
        {
            if (_currentInstance == null)
            {
                return;
            }

            attachmentPoint.Rotate(Vector3.up, Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime, 
                Space.World);
            attachmentPoint.Rotate(Vector3.right, Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime,
                Space.World);

            if (Input.GetMouseButtonDown(0))
            {
                NextItem();
            }
        }   

        private void EndSearch()
        {
            _selfCanvas.enabled = false;

            var state = ServiceProvider.instance.Resolve<IObservableState<PlayerControllable>>();
            state.HandleUpdate(new PlayerControllable { controllable = true });
            
            if (_currentInstance != null)
            {
                Destroy(_currentInstance);
                _currentInstance = null;
            }
        }

        private void ShowCurrentItem()
        {
            if (_currentInstance != null)
            {
                Destroy(_currentInstance);
            }

            attachmentPoint.rotation = Quaternion.identity;

            var definition = _currentItems[_currentItemIndex].GetDefinition();
            _currentInstance = Instantiate(definition.prefab, attachmentPoint).gameObject;

            var bounds = CalculateBounds(definition.prefab.gameObject);
            var scaleFactor = radius / (Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z) / 2);

            _currentInstance.transform.localScale = _currentInstance.transform.localScale * scaleFactor;
            _currentInstance.transform.localPosition = -bounds.center * scaleFactor;
            SetLayerRecursively(_currentInstance.transform, attachmentPoint.gameObject.layer);
        }

        private void NextItem()
        {
            _currentItemIndex++;

            if (_currentItemIndex >= _currentItems.Length)
            {
                EndSearch();
                return;
            }

            ShowCurrentItem();
        }

        private Bounds CalculateBounds(GameObject targetObject)
        {
            var bounds = new Bounds();

            if (targetObject.TryGetComponent<AttachmentBounds>(out var attachmentBounds))
            {
                bounds = new Bounds(attachmentBounds.centralPoint.position - targetObject.transform.position,
                    attachmentBounds.size);
            }
            else
            {
                var renderers = targetObject.GetComponentsInChildren<Renderer>();

                foreach (var rnd in renderers)
                {
                    bounds.Encapsulate(rnd.bounds);
                }
            }

            return bounds;
        }

        private void SetLayerRecursively(Transform obj, LayerMask layer)
        {
            obj.gameObject.layer = layer;

            for (var i = 0; i < obj.childCount; i++)
            {
                SetLayerRecursively(obj.GetChild(i), layer);
            }
        }

        private void OnDestroy()
        {
            var state = ServiceProvider.instance.Resolve<IObservableState<PreviewItems>>();
            state.RegisterObserver(this);
        }
    }
}