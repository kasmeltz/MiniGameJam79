namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    public class MaxHealthUpgradeEffect : UpgradeEffect
    {
        [Space]
        [SerializeField] private float[] _maxHealthMultiplier;

        public int MaxHealthBase;

        public override void MakeUpgrade()
        {
            Frog.MaxHealth = MaxHealthBase * _maxHealthMultiplier[Level];
        }

        protected override string DescriptionForLevel()
        {
            int level = Level;
            if (level >= Levels)
            {
                level--;
            }

            float percentage = _maxHealthMultiplier[level] - 1;

            var text = _description
                .Replace("{0}", percentage.ToString("p"));

            if (Level >= Levels)
            {
                text = "FULLY UPGRADED - " + text;
            }

            return text;
        }
    }
}