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

        private SoundEffects _soundEffects;

        private void Awake()
        {
            _soundEffects = FindObjectOfType<SoundEffects>();
        }

        public bool TryMakeUpgrade()
        {
            if (Frog.TrySpendFlies(_price))
            {
                MakeUpgrade();
                _soundEffects.BuyUpgrade();
                return true;
            }

            return false;
        }
        public abstract void MakeUpgrade();
    }
}
