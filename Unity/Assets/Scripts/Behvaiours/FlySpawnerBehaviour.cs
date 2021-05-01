namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/FlySpawner")]
    public class FlySpawnerBehaviour : GameObjectPoolBehaviour
    {
        #region Members

        public int ConcurrentFlies;

        public Bounds Bounds;

        #endregion

        #region Public Methods

        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            for (int i = 0; i < ConcurrentFlies; i++)
            {
                var fly = GetPooledObject();

                float x = Random
                    .Range(Bounds.min.x, Bounds.max.x);

                float y = Random
                    .Range(Bounds.min.y, Bounds.max.y);

                fly.transform.position = new Vector3(x, y, 0);
            }
        }

        #endregion
    }
}