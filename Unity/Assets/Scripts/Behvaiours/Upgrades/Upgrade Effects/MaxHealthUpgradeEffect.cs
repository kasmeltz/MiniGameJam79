namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    public class MaxHealthUpgradeEffect : UpgradeEffect
    {
        [Space]
        [SerializeField] private int _healthIncreasementPerLevel;

        public override void MakeUpgrade(int currentUpgradeLevel)
        {
            Frog.MaxHealth += _healthIncreasementPerLevel;
        }
    }
}