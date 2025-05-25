using TMPro;
using UnityEngine;

namespace UI.Labels
{
    public class LabelCard : MonoBehaviour
    {
        [SerializeField] private RectTransform attachmentPoint;
        [SerializeField] private TextMeshProUGUI textField;

        private RectTransform _selfRt;
        private RectTransform _canvasRt;

        public void SetViewportPosition(Vector3 position)
        {
            var canvasHalfSize = _canvasRt.sizeDelta / 2;
            var newPosition = new Vector2(Mathf.Lerp(-canvasHalfSize.x, canvasHalfSize.x, position.x),
                Mathf.Lerp(-canvasHalfSize.y, canvasHalfSize.y, position.y));
            
            _selfRt.anchoredPosition = newPosition;
            
            PlaceInsideViewRect();
        }

        public void SetText(string text)
        {
            textField.text = text;

            PlaceInsideViewRect();
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void Awake()
        {
            _selfRt = (RectTransform)transform;
            _canvasRt = GetComponentInParent<Canvas>(true).GetComponent<RectTransform>();
        }

        private void PlaceInsideViewRect()
        {
            // var selfPosition = _selfRt.anchoredPosition;
            // var attachmentSize = attachmentPoint.sizeDelta;
            //
            // var canvasSize = _canvasRt.sizeDelta;
            //
            // var minPoint = selfPosition - attachmentSize / 2;
            // var maxPoint = selfPosition + attachmentSize / 2;
            //
            // var offset = Vector2.zero;
            //
            // if (minPoint.x < 0)
            // {
            //     offset.x = -minPoint.x;
            // }
            //
            // if (minPoint.y < 0)
            // {
            //     offset.y = -minPoint.y;
            // }
            //
            // if (maxPoint.x > canvasSize.x)
            // {
            //     offset.x = canvasSize.x - maxPoint.x;
            // }
            //
            // if (maxPoint.y > canvasSize.y)
            // {
            //     offset.y = canvasSize.y - maxPoint.y;
            // }
            //
            // attachmentPoint.anchoredPosition = offset;
        }
    }
}