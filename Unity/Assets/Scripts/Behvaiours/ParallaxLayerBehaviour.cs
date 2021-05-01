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

                tile.sprite = PossibleSprites[0];

                var pos = tile.transform.localPosition;

                pos.y = 0;
                pos.x = x;

                tile.transform.localPosition = pos;

                tile.sortingOrder = -LayerNumber - 5;

                Tiles
                    .Add(tile);

                x += tile.sprite.bounds.size.x;
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
            d *= (ScrollSpeed / 512f);

            d.y = -5;
            d.z = 0;

            transform.position = d;
        }

        #endregion
    }
}