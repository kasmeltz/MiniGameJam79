namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/GameOverPanel")]
    public class GameOverPanelBehaviour : BehaviourBase
    {
        #region Members

        protected MenuMusicLooper MenuLooper;

        protected PlayMusicLooper PlayLooper;

        #endregion

        #region Public Methods

        public void Open()
        {
            gameObject
                .SetActive(true);

            PlayLooper
                .EnsureNotPlaying();

            MenuLooper
                .EnsurePlaying(1.0f);
        }

        #endregion

        #region Unity

        protected override void Awake()
        {
            MenuLooper = FindObjectOfType<MenuMusicLooper>();
            PlayLooper = FindObjectOfType<PlayMusicLooper>();
        }

        #endregion
    }
}
