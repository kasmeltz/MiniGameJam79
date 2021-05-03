namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    public class TongueLengthUpgradeEffect : UpgradeEffect
    {
        [Space]
        [SerializeField] private FrogTongueBehaviour _tongue;
        [SerializeField] private float[] _tongueLengthMultiplier;

        public int TongueLengthBase;

        public override void MakeUpgrade()
        {
            _tongue.MaxLength = (int)(TongueLengthBase * _tongueLengthMultiplier[Level]);
        }

        protected override void Awake()
        {
            base
                .Awake();
        }

        protected override string DescriptionForLevel()
        {
            int level = Level;
            if (level >= Levels)
            {
                level--;
            }

            var percentage = 1 - _tongueLengthMultiplier[level];

            var text = _description.Replace("{0}", percentage.ToString("p"));

            if (Level >= Levels)
            {
                text = "FULLY UPGRADED - " + text;
            }

            return text;
        }
    }
}