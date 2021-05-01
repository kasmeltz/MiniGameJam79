namespace KasJam.MiniJam79.Unity.Behaviours
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;

    public class UpgradeShop : MonoBehaviour
    {
        #region Members
        [SerializeField] private TMP_Text _selectedDescription;
        [SerializeField] private TMP_Text _selectedPriceText;
        public MusicLooper music;

        private Upgrade _selectedUpgrade;

        #endregion

        #region PublicMethods
        public void Open() {
            music.EnsurePlaying();
            music.MoveToLoop(1);
        }

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

            if (_selectedUpgrade.TryBuy())
            {
                _selectedUpgrade = null;
            }
        }
        #endregion
    }
}
