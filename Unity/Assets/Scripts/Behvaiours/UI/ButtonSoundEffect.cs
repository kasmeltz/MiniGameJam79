namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class ButtonSoundEffect : MonoBehaviour
    {
        [SerializeField] private UpgradeShop _upgradeShop;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            SoundEffects.Instance.MenuSelect();
        }
    }
}
