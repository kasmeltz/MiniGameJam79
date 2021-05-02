
namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/TitleParallax")]
    public class TitleParallaxBehaviour : BehaviourBase
    {
        #region Members

        protected MenuMusicLooper MenuMusic { get; set; }

        protected float Direction { get; set; }
        
        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            Direction = 1;

            MenuMusic = FindObjectOfType<MenuMusicLooper>();

            MenuMusic
                .EnsurePlaying();

            var fadePanel = FindObjectOfType<FadePanelBehaviour>();
            
            fadePanel
                .FadeIn();
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