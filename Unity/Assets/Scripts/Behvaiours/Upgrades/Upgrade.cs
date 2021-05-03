namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    public class Upgrade : MonoBehaviour
    {
        [SerializeField] private UpgradeButton _upgradeButton;
        [SerializeField] private UpgradeEffect _upgradeEffect;

        private UpgradeShop _upgradeShop;

        public string Description => _upgradeEffect.DescriptionForCurrentLevel;

        public int Price => _upgradeEffect.PriceForCurentLevel;

        private void Awake()
        {
            _upgradeButton.InitializeButton(_upgradeEffect.Label, _upgradeEffect.Level);

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
                if (_upgradeEffect.Level > _upgradeEffect.Levels)
                {
                    _upgradeButton.DisableButton();
                }
                else
                {
                    _upgradeShop.SelectUpgrade(this);                    
                }

                _upgradeButton.InitializeButton(_upgradeEffect.Label, _upgradeEffect.Level);

                return true;
            }

            return false;
        }
    }
}