namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    public class ResistDamageUpgradeEffect : UpgradeEffect
    {
        [Space]
        [SerializeField] private float[] _resistPercentage;

        public override void MakeUpgrade()
        {
            Frog.ReduceDamageTaken(_resistPercentage[Level]);
        }

        protected override string DescriptionForLevel()
        {
            int level = Level;
            if (level >= Levels)
            {
                level--;
            }

            var text = _description.Replace("{0}", _resistPercentage[level].ToString("p"));

            if (Level >= Levels)
            {
                text = "FULLY UPGRADED - " + text;
            }

            return text;
        }
    }
}