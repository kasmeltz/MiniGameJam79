using UnityEngine;

namespace KasJam.MiniJam79.Unity.Behaviours
{
    public abstract class EnemyBehaviour : PlatformerBehaviourBase
    {
        #region Members

        public int Level;

        public Vector2Int GridCoords { get; set; }

        public float DeathTime { get; set; }

        #endregion
    }
}
