namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    public class TongueLengthUpgradeEffect : UpgradeEffect
    {
        [Space]
        [SerializeField] private FrogTongueBehaviour _tongue;
        [SerializeField] private int _lengthPerLevel;

        public override void MakeUpgrade(int currentUpgradeLevel)
        {
            _tongue.MaxLength += _lengthPerLevel;
        }
    }
}