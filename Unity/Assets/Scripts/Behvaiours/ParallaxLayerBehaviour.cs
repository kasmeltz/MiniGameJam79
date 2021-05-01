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

        #region Public Methods

        public void Scroll(float amount)
        {

        }

        #endregion

        #region Protected Methods

        public void GenerateTiles()
        {
            float x = Bounds.min.x;

            do
            {
                var tile = Instantiate(Prefab);

                tile
                    .transform
                    .SetParent(transform);

                tile.sprite = PossibleSprites[0];
                var pos = tile.transform.localPosition;
                pos.x = x;
                tile.transform.localPosition = pos;
                tile.sortingOrder = -1 * LayerNumber;

                x += tile.sprite.bounds.size.x;
            } while (x <= Bounds.max.x);
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

        #endregion
    }
}