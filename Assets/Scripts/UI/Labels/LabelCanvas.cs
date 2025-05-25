using System;
using System.Collections.Generic;
using System.Linq;
using Trackables.Labels;
using UnityEngine;
using UnityEngine.Rendering;
using Utils.SimpleDI;

namespace UI.Labels
{
    public class LabelCanvas : MonoBehaviour, Utils.Observable.IObserver<IReadOnlyCollection<LabelTrackable>>
    {
        [SerializeField] private LabelCard prefab;

        private List<LabelTrackable> _labels = new();
        private List<LabelCard> _cards = new();

        private Camera _camera;
        
        public void HandleUpdate(IReadOnlyCollection<LabelTrackable> message)
        {
            _labels = message.ToList();

            for (var i = _cards.Count; i < _labels.Count; i++)
            {
                var newCard = Instantiate(prefab, transform);

                _cards.Add(newCard);
            }

            for (var i = 0; i < _cards.Count; i++)
            {
                if (i < _labels.Count)
                {
                    _cards[i].SetText(_labels[i].text);
                }
                else
                {
                    _cards[i].SetActive(false);
                }
            }
        }

        private void Awake()
        {
            var serviceProvider = ServiceProvider.instance;

            _camera = serviceProvider.Resolve<Camera>();

            var state = serviceProvider.Resolve<Utils.Observable.IObservable<IReadOnlyCollection<LabelTrackable>>>();
            state.RegisterObserver(this, true);
        }

        private void Place(ScriptableRenderContext arg1, Camera[] arg2)
        {
            for (var i = 0; i < _labels.Count; i++)
            {
                var viewportPosition = _camera.WorldToViewportPoint(_labels[i].transform.position);

                if (IsVisible(viewportPosition))
                {
                    _cards[i].SetActive(true);
                    _cards[i].SetViewportPosition(viewportPosition);
                }
                else
                {
                    _cards[i].SetActive(false);
                }


                // if (!(_labels[i] is IForcedLabel forced && forced.forcedShow)
                //     && !IsVisible(viewportPosition))
                // {
                //     return;
                // }

            }
        }

        private bool IsVisible(Vector3 viewportPosition)
        {
            return viewportPosition.x >= 0 && viewportPosition.x <= 1
                                           && viewportPosition.y >= 0 && viewportPosition.y <= 1
                                           && viewportPosition.z >= 0;
        }

        private void OnEnable() => RenderPipelineManager.endFrameRendering += Place;
        
        private void OnDisable() => RenderPipelineManager.endFrameRendering -= Place;

        private void OnDestroy()
        {
            var state = ServiceProvider.instance
                .Resolve<Utils.Observable.IObservable<IReadOnlyCollection<LabelTrackable>>>();
            state.UnregisterObserver(this);
        }
    }
}