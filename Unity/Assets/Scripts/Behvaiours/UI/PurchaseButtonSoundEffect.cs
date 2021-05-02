namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PurchaseButtonSoundEffect : ButtonSoundEffect
    {
        [SerializeField] private UpgradeShop _upgradeShop;

        protected override void OnEnable()
        {
            _upgradeShop.SuccessfullyPurchased += PlayMenuSelectSFX;
        }

        protected override void OnDisable()
        {
            _upgradeShop.SuccessfullyPurchased -= PlayMenuSelectSFX;
        }
    }
}
