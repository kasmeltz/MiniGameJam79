namespace KasJam.MiniJam79.Unity.Behaviours
{
    using TMPro;
    using UnityEngine;

    public class UpgradeShop : MonoBehaviour
    {
        #region Members
        [SerializeField] private TMP_Text _selectedDescription;
        [SerializeField] private TMP_Text _selectedPriceText;

        private Upgrade _selectedUpgrade;

        #endregion

        #region PublicMethods
        public void SelectUpgrade(Upgrade upgradeEffect)
        {
            _selectedUpgrade = upgradeEffect;
            _selectedDescription.text = _selectedUpgrade.Description;
            _selectedPriceText.text = _selectedUpgrade.Price.ToString();
        }

        public void ConfirmUpgradePurchase()
        {
            if (_selectedUpgrade == null)
                return;

            _selectedUpgrade.TryBuy();
            if (_selectedUpgrade.IsUpgradedToMax)
                _selectedUpgrade = null;
        }
        #endregion
    }
}