namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class ButtonSoundEffect : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        protected virtual void OnEnable()
        {
            _button.onClick.AddListener(PlayMenuSelectSFX);
        }

        protected virtual void OnDisable()
        {
            _button.onClick.RemoveListener(PlayMenuSelectSFX);
        }

        protected void PlayMenuSelectSFX()
        {
            SoundEffects.Instance.MenuSelect();
        }
    }
}
