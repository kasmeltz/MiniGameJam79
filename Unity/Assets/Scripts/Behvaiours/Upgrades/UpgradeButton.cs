namespace KasJam.MiniJam79.Unity.Behaviours
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class UpgradeButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _labelText;

        public event UnityAction WasClicked;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            WasClicked?.Invoke();
        }

        public void DisableButton()
        {
            _button.interactable = false;
        }

        public void InitializeButton(string upgradeLabel)
        {
            _labelText.text = upgradeLabel;
        }
    }
}