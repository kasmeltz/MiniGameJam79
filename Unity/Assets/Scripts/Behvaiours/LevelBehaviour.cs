namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;
    using UnityEngine.Tilemaps;

    [AddComponentMenu("KasJam/Level")]
    public class LevelBehaviour : BehaviourBase
    {
        #region Members

        public GameObject Objects;

        public MovingPlatformBehaviour[] MovingPlatforms;

        public Tilemap Walls;

        public Tilemap Floor;

        public Tilemap OnewayFloor;

        #endregion
    }
}