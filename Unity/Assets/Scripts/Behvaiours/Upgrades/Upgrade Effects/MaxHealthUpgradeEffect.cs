using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthUpgradeEffect : UpgradeEffect
{
    [Space]
    [SerializeField] private int _healthIncreasementPerLevel;

    public override void MakeUpgrade(int currentUpgradeLevel)
    {
        Frog.MaxHealth += _healthIncreasementPerLevel;
    }
}
