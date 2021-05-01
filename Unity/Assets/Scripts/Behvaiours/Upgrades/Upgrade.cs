using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private UpgradeButton _upgradeButton;
    [SerializeField] private UpgradeEffect _upgradeEffect;

    private int _currentUpgradeLevel;

    private void Awake()
    {
        _currentUpgradeLevel = 0;
        _upgradeButton.InitializeButton(_upgradeEffect.Label, _upgradeEffect.Cost);
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
        if (_currentUpgradeLevel >= _upgradeEffect.MaxUpgradeLevel)
            return;

        _currentUpgradeLevel++;
        if(_upgradeEffect.TryMakeUpgrade(_currentUpgradeLevel))
        {
            if (_currentUpgradeLevel >= _upgradeEffect.MaxUpgradeLevel)
                _upgradeButton.DisableButton();
        }
    }
}
