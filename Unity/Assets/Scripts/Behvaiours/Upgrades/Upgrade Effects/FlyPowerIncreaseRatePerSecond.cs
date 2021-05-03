namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    public class FlyPowerIncreaseRatePerSecond : UpgradeEffect
    {
        [Space]
        [SerializeField] private int _rateUpgrade;
        [SerializeField] private FlyType _flyPowerType;

        public override void MakeUpgrade()
        {
            Frog.IncreaseFlyPowerRate(_rateUpgrade, _flyPowerType);
        }
    }
}