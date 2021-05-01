namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    public class Upgrade : MonoBehaviour
    {
        [SerializeField] private UpgradeButton _upgradeButton;
        [SerializeField] private UpgradeEffect _upgradeEffect;

        private UpgradeShop _upgradeShop;

        public string Description => _upgradeEffect.Description;
        public int Price => _upgradeEffect.Price;

        private void Awake()
        {
            _upgradeButton.InitializeButton(_upgradeEffect.Label);

            _upgradeShop = GetComponentInParent<UpgradeShop>();
        }

        private void OnEnable()
        {
            _upgradeButton.WasClicked += OnUpgradeButtonClick;
        }

        private void OnDisable()
        {
            _upgradeButton.WasClicked -= OnUpgradeButtonClick;
        }

        private void OnUpgradeButtonClick()
        {
            _upgradeShop.SelectUpgrade(this);
        }

        public bool TryBuy()
        {
            if (_upgradeEffect.TryMakeUpgrade())
            {
                _upgradeButton.DisableButton();
                return true;
            }

            return false;
        }
    }
}