namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("KasJam/ParallaxLayer")]
    public class ParallaxLayerBehaviour : BehaviourBase
    {
        #region Members

        public SpriteRenderer Prefab;

        public int LayerNumber;

        public float ScrollSpeed;

        public Sprite[] PossibleSprites;

        public Bounds Bounds;

        public bool IsForeground;

        public Vector2 YRange;

        protected List<SpriteRenderer> Tiles { get; set; }

        #endregion

        
        #region Protected Methods

        public void GenerateTiles()
        {
            float x = Bounds.min.x - 5;

            do
            {
                var tile = Instantiate(Prefab);

                tile
                    .transform
                    .SetParent(transform);

                int tileIndex = Random
                    .Range(0, PossibleSprites.Length);

                tile.sprite = PossibleSprites[tileIndex];

                var pos = tile.transform.localPosition;

                pos.y = Random
                    .Range(YRange.x, YRange.y);

                pos.x = x;

                tile.transform.localPosition = pos;

                if (IsForeground)
                {
                    tile.sortingOrder = LayerNumber + 20;
                }
                else
                {
                    tile.sortingOrder = -LayerNumber - 5;
                }                

                Tiles
                    .Add(tile);

                x += (tile.sprite.bounds.size.x * tile.transform.localScale.x) - 0.01f;
            } while (x <= Bounds.max.x + 5); ;
        }

        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            Tiles = new List<SpriteRenderer>();

            GenerateTiles();
        }

        protected void Update()
        {
            var d = Bounds.center - Camera.main.transform.position;
            d *= (ScrollSpeed);

            d.y = transform.position.y;
            d.z = 0;

            transform.position = d;
        }

        #endregion
    }
}