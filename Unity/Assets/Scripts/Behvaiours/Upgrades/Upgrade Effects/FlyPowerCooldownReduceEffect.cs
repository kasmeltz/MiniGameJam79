namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class FlyPowerCooldownReduceEffect : UpgradeEffect
    {
        [Space]
        [SerializeField] private int _reducePercentage;
        [SerializeField] private FlyType _flyPowerType;

        public override void MakeUpgrade()
        {
            Frog.ReduceFlyPowerCooldown(_reducePercentage, _flyPowerType);
        }
    }
}