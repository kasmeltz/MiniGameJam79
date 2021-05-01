namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("KasJam/FlySpawner")]
    public class FlySpawnerBehaviour : GameObjectPoolBehaviour
    {
        #region Members

        public LevelManagerBehaviour LevelManager;

        public int ConcurrentFlies;

        public Bounds Bounds;

        #endregion

        #region Event Handlers
        
        private void LevelManager_LevelStarted(object sender, System.EventArgs e)
        {
            SpawnFlies();
        }

        #endregion

        #region Protected Methods

        protected  void SpawnFlies()
        {
            foreach (var fly in PooledObjects)
            {
                if (fly == null)
                {
                    continue;
                }

                fly
                    .SetActive(false);
            }

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

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            LevelManager.LevelStarted += LevelManager_LevelStarted;
        }

       
        #endregion
    }
}