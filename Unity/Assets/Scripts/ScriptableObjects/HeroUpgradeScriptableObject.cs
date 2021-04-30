namespace KasJam.MiniJam79.Unity.ScriptableObjects
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "HeroUpgrade", menuName = "KasJam/HeroUpgrade", order = 1)]
    public class HeroUpgradeScriptableObject : ScriptableObject
    {
        #region Members

        public int Cost;

        public int Level;

        public HeroUpgradeType UpgradeType;


        #endregion
    }
}