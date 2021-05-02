namespace KasJam.MiniJam79.Unity.Behaviours
{
    using TMPro;
    using UnityEngine;

    [AddComponentMenu("KasJam/CooldownText")]
    [RequireComponent(typeof(TMP_Text))]
    public class CooldownTextBehaviour : BehaviourBase
    {
        [SerializeField] private PlatformerBehaviour _frog;

        private TMP_Text _text;

        protected override void Awake()
        {
            base
                .Awake();

            _text = GetComponent<TMP_Text>();
        }

        protected void Update()
        {
            if (_frog.FlyPower == FlyType.None)
            {
                _text.text = "";
                return;
            }

            _text.text = Mathf
                .RoundToInt(_frog.FlyPowerTimer)
                .ToString();
        }
    }
}