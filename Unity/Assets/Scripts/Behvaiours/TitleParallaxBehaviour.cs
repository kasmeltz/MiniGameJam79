
namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/TitleParallax")]
    public class TitleParallaxBehaviour : BehaviourBase
    {
        #region Members

        public MusicLooper MenuMusic;

        protected float Direction { get; set; }
        
        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            Direction = 1;

            MenuMusic
                .EnsurePlaying();
        }

        protected void Update()
        {
            Camera.main.transform.position += new Vector3(1 * Direction, 0, 0) * Time.deltaTime;

            if (Camera.main.transform.position.x >= 5)
            {
                Direction = -1;
            }

            if (Camera.main.transform.position.x <= -5)
            {
                Direction = 1;
            }
        }

        #endregion
    }
}