namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    public abstract class UpgradeEffect : BehaviourBase
    {
        [SerializeField] protected PlatformerBehaviour Frog;
        [SerializeField] private string _label;
        [SerializeField] private int[] _price;
        [SerializeField] [TextArea(3, 15)] protected string _description;

        public string Label => _label;

        public int PriceForCurentLevel 
        {   
            get
            {
                if (Level >= Levels)
                {
                    return 0;
                }

                return _price[Level];
            }
        }

        public string DescriptionForCurrentLevel
        {
            get
            {
                return DescriptionForLevel();
            }
        }

        public int Level { get; protected set; }

        public int Levels
        {
            get
            {
                return _price.Length;
            }
        }

        public bool TryMakeUpgrade()
        {
            if (Level >= Levels)
            {
                return false;
            }

            if (Frog
                .TrySpendFlies(_price[Level]))
            {
                MakeUpgrade();

                Level++;

                SoundEffects.Instance.BuyUpgrade();

                return true;
            }

            return false;
        }

        public abstract void MakeUpgrade();

        protected abstract string DescriptionForLevel();

        protected override void Awake()
        {
            base
                .Awake();

            Level = 0;
        }
    }
}
