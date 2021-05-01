namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System.Collections.Generic;
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
                    .SetActive(false);
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

            float x = Random
                .Range(Bounds.min.x, Bounds.max.x);

            float y = Random
                .Range(Bounds.min.y, Bounds.max.y);

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

            LevelManager = FindObjectOfType<LevelManagerBehaviour>(true);

            Flies = new List<GameObject>();
            ToDelete = new List<GameObject>();

            LevelManager.LevelStarted += LevelManager_LevelStarted;
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