using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KasJam.MiniJam79.Unity.Behaviours;
public class TongueLengthUpgradeEffect : UpgradeEffect
{
    [Space]
    [SerializeField] private FrogTongueBehaviour _tongue;
    [SerializeField] private int _lengthPerLevel;

    public override void MakeUpgrade(int currentUpgradeLevel)
    {
        _tongue.MaxLength += _lengthPerLevel;
    }
}
