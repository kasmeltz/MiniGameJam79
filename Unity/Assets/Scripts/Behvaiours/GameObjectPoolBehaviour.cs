namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    [AddComponentMenu("KasJam/GameObjectPool")]
    public class GameObjectPoolBehaviour : BehaviourBase
    {
        #region Members

        public GameObject ObjectToPool;

        public int AmountToPool;

        protected List<GameObject> PooledObjects { get; set; }

        #endregion

        #region Public Methods

        public GameObject GetPooledObject()
        {
            if (PooledObjects == null || 
                !PooledObjects.Any())
            {
                CreatePool();
            }

            for(int i = 0;i < AmountToPool;i++)
            {
                var pooledObject = PooledObjects[i];
                if (!pooledObject.activeInHierarchy)
                {
                    pooledObject
                        .SetActive(true);

                    return pooledObject;
                }
            }

            return null;
        }

        public T GetPooledObject<T>() where T : BehaviourBase
        {
            var obj = GetPooledObject();
            if (obj == null)
            {
                return null;
            }

            return obj
                .GetComponent<T>();
        }

        protected void CreatePool()
        {
            GameObject tmp;
            for (int i = 0; i < AmountToPool; i++)
            {
                tmp = Instantiate(ObjectToPool);
                tmp
                    .SetActive(false);

                PooledObjects
                    .Add(tmp);
            }
        }

        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            PooledObjects = new List<GameObject>(AmountToPool);
        }

        #endregion
    }
}