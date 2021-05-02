namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;
    using UnityEngine.Tilemaps;

    [AddComponentMenu("KasJam/Level")]
    public class LevelBehaviour : BehaviourBase
    {
        #region Members

        public GameObject Objects;

        public GameObject Enemies;

        public GameObject FlySpawners;

        public MovingPlatformBehaviour[] MovingPlatforms;

        public Tilemap[] Tilemaps;

        #endregion
    }
}
