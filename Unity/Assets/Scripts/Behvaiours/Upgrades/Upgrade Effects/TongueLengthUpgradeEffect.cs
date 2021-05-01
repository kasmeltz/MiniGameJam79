namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    public class TongueLengthUpgradeEffect : UpgradeEffect
    {
        [Space]
        [SerializeField] private FrogTongueBehaviour _tongue;
        [SerializeField] private float _tongueLengthMultiplier = 1.5f;

        public override void MakeUpgrade()
        {
            _tongue.MaxLength = (int)(_tongue.MaxLength * _tongueLengthMultiplier);
        }

        private void OnValidate()
        {
            if (_tongueLengthMultiplier < 1)
                _tongueLengthMultiplier = 1;
        }
    }
}