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

            LevelLoper.EnsureNotPlaying();
            MenuLooper.gameObject.SetActive(true);

            DoAfter(0.75f, StartMenuMusic);
        }

        public void StartMenuMusic() {
            //Debug.Log("callback");
            MenuLooper.EnsurePlaying();
        }


        #endregion
    }
}
