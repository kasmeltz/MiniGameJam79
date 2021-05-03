namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    [AddComponentMenu("KasJam/FlySpawner")]
    public class FlySpawnerBehaviour : GameObjectPoolBehaviour
    {
        #region Members        

        public int ConcurrentFlies;

        public Bounds Bounds;

        public float RefreshDelay;

        protected List<GameObject> Flies { get; set; }

        protected List<GameObject> ToDelete { get; set; }

        protected float RefreshTimer { get; set; }

        protected LevelManagerBehaviour LevelManager { get; set; }

        #endregion

        #region Event Handlers

        private void LevelManager_LevelStarted(object sender, System.EventArgs e)
        {
            if (this == null)
                return;

            SpawnFlies();
        }

        #endregion

        #region Protected Methods

        protected void SpawnFlies()
        {
            foreach (var fly in PooledObjects)
            {
                if (fly == null)
                {
                    continue;
                }

                fly
                    .gameObject
                    .SetActive(false);

                fly
                    .transform
                    .SetParent(transform);
            }

            Flies
                .Clear();

            for (int i = 0; i < ConcurrentFlies; i++)
            {
                SpawnFly();
            }
        }

        protected void SpawnFly()
        {
            var fly = GetPooledObject();

            var pos = transform.position;

            float x = Random
                .Range(Bounds.min.x + pos.x, Bounds.max.x + pos.x);

            float y = Random
                .Range(Bounds.min.y + pos.y, Bounds.max.y + pos.y);

            fly.transform.position = new Vector3(x, y, 0);

            Flies
                .Add(fly);
        }

        protected void RefreshFlies()
        {
            ToDelete
                .Clear();

            foreach(var fly in Flies)
            {
                if (fly == null)
                {
                    continue;
                }

                if (!fly.gameObject.activeInHierarchy)
                {
                    ToDelete
                        .Add(fly);
                }
            }

            foreach(var fly in ToDelete)
            {
                Flies
                    .Remove(fly);
            }

            ToDelete
                .Clear();

            int required = ConcurrentFlies - Flies.Count;

            for(int i = 0;i < required;i++)
            {
                SpawnFly();
            }

        }


        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            Flies = new List<GameObject>();
            ToDelete = new List<GameObject>();

            LevelManager = FindObjectOfType<LevelManagerBehaviour>(true);
            if (LevelManager != null)
            {
                LevelManager.LevelStarted += LevelManager_LevelStarted;
            }
        }

        protected void Update()
        {
            RefreshTimer += Time.deltaTime;
            if (RefreshTimer >= RefreshDelay)
            {
                RefreshTimer -= RefreshDelay;
                RefreshFlies();
            }
        }

        #endregion
    }
}