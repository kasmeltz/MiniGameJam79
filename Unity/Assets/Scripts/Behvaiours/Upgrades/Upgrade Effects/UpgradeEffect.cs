namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    public abstract class UpgradeEffect : MonoBehaviour
    {
        [SerializeField] protected PlatformerBehaviour Frog;
        [SerializeField] private string _label;
        [SerializeField] private int _price;
        [SerializeField] [TextArea(3, 15)] private string _description;

        public string Label => _label;
        public int Price => _price;
        public string Description => _description;

        public bool TryMakeUpgrade()
        {
            if (Frog.TrySpendFlies(_price))
            {
                MakeUpgrade();
                return true;
            }

            return false;
        }
        public abstract void MakeUpgrade();
    }
}