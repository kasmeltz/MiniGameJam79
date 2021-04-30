namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/FlySpawner")]
    public class FlySpawnerBehaviour : GameObjectPoolBehaviour
    {
        #region Members

        public int ConcurrentFlies;

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
                    .Range(-2f, 2f);

                float y = Random
                    .Range(-2f, 2f);

                fly.transform.position = new Vector3(x, y, 0);
            }
        }

        #endregion
    }
}