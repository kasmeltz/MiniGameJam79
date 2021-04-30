namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System.Collections.Generic;
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
        
        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            PooledObjects = new List<GameObject>(AmountToPool);

            GameObject tmp;
            for(int i = 0;i < AmountToPool;i++)
            {
                tmp = Instantiate(ObjectToPool);
                tmp
                    .SetActive(false);

                PooledObjects
                    .Add(tmp);
            }

        }

        #endregion
    }
}