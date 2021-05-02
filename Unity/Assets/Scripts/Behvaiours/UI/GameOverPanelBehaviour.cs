namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/GameOverPanel")]
    public class GameOverPanelBehaviour : BehaviourBase
    {
        #region Members

        public MusicLooper MenuLooper;

        public MusicLooper LevelLoper;

        #endregion

        #region Public Methods

        public void Open()
        {
            gameObject
                .SetActive(true);

            MenuLooper.gameObject.SetActive(true);
            LevelLoper.EnsureNotPlaying();
            MenuLooper.EnsurePlaying(1.0f);
        }

        #endregion
    }
}
