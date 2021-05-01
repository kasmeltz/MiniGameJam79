namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    public class MaxHealthUpgradeEffect : UpgradeEffect
    {
        [Space]
        [SerializeField] private float _maxHealthMultiplier = 1.2f;

        public override void MakeUpgrade()
        {
            Frog.MaxHealth *= _maxHealthMultiplier;
        }

        private void OnValidate()
        {
            if (_maxHealthMultiplier < 1)
                _maxHealthMultiplier = 1;
        }
    }
}