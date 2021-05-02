namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    public class ResistDamageUpgradeEffect : UpgradeEffect
    {
        [Space]
        [SerializeField] private int _resistPercentage;

        public override void MakeUpgrade()
        {
            Frog.ReduceDamageTaken(_resistPercentage);
        }
    }
}
