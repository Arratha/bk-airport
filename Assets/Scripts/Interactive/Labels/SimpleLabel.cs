using TMPro;
using UnityEngine;

namespace Interactive.Labels
{
    public class SimpleLabel : MonoBehaviour, ILabel
    {
        public string text
        {
            get => selfText;
            set => SetText(value);
        }

        private bool isActive => !string.IsNullOrEmpty(selfText) && enabled;

        [TextArea(1, 5), SerializeField] private string selfText;

        [Space, SerializeField] private GameObject anchor;
        [SerializeField] private TextMeshProUGUI textField;

        private void Awake() => SetText(selfText);

        private void SetText(string input)
        {
            selfText = input;

            if (anchor == null || textField == null)
            {
                return;
            }

            anchor.SetActive(isActive);
            textField.text = input;
        }

        private void OnValidate() => SetText(selfText);

        private void OnEnable() => anchor.SetActive(isActive);

        private void OnDisable() => anchor.SetActive(isActive);
    }
}