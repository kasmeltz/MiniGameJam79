using KasJam.MiniJam79.Unity.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class UpgradeEffect : MonoBehaviour
{
    [SerializeField] protected PlatformerBehaviour Frog;
    [SerializeField] private string _label;
    [SerializeField] private int _cost;
    [SerializeField] private int _maxUpgradeLevel;

    public string Label => _label;
    public int Cost => _cost;
    public int MaxUpgradeLevel => _maxUpgradeLevel;

    public bool TryMakeUpgrade(int currentUpgradeLevel)
    {
        if(Frog.TrySpendFlies(_cost))
        {
            MakeUpgrade(currentUpgradeLevel);
            return true;
        }

        return false;
    }
    public abstract void MakeUpgrade(int currentUpgradeLevel);
}
