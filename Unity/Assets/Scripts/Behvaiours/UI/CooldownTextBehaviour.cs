namespace KasJam.MiniJam79.Unity.Behaviours
{
    using TMPro;
    using UnityEngine;

    [AddComponentMenu("KasJam/CooldownText")]
    [RequireComponent(typeof(TMP_Text))]
    public class CooldownTextBehaviour : BehaviourBase
    {
        public ProgressBarBehaviour ProgressBar;

        [SerializeField] private PlatformerBehaviour _frog;

        private TMP_Text _text;

        protected override void Awake()
        {
            base
                .Awake();

            _frog.AbilityRemaining += _frog_AbilityRemaining;
            _frog.AbiltyChanged += _frog_AbiltyChanged;
            _text = GetComponent<TMP_Text>();

           UpdateUI();
        }

        private void _frog_AbiltyChanged(float arg0)
        {
            UpdateUI();   
        }

        private void _frog_AbilityRemaining(float arg0)
        {
            UpdateUI();
        }

        protected void UpdateUI()
        {
            _text.text = Mathf
                .RoundToInt(_frog.AbilityRateAvailable)
                .ToString();

            ProgressBar
                .SetValue(_frog.AbilityRateAvailable, _frog.MaximumAbilityRate);
        }
    }
}