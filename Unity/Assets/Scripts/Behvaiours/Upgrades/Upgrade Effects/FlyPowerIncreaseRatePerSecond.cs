namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System.Linq;
    using UnityEngine;

    public class FlyPowerIncreaseRatePerSecond : UpgradeEffect
    {
        [Space]
        [SerializeField] private int[] _rateUpgrade;
        [SerializeField] private FlyType _flyPowerType;

        public override void MakeUpgrade()
        {
            Frog.IncreaseFlyPowerRate(_rateUpgrade[Level], _flyPowerType);
        }

        protected override string DescriptionForLevel()
        {
            int level = Level;
            if (level >= Levels)
            {
                level--;
            }

            var amount = _rateUpgrade.Take(level + 1).Sum();

            var text = _description.Replace("{0}", amount.ToString());

            if (Level >= Levels)
            {
                text = "FULLY UPGRADED - " + text;
            }

            return text;
        }
    }
}