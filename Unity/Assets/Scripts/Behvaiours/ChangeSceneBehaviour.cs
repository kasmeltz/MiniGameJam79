namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [AddComponentMenu("KasJam/ChangeScene")]
    public class ChangeSceneBehaviour : BehaviourBase
    {
        #region Members

        protected string SceneName { get; set; }

        #endregion

        #region Public Methods

        public void ChangeScene(string sceneName)
        {
            SceneName = sceneName;

            var fadePanel = FindObjectOfType<FadePanelBehaviour>();

            if (fadePanel == null)
            {
                SceneManager
                    .LoadSceneAsync(sceneName);
            }
            else 
            {
                fadePanel.FadeOutComplete += FadePanel_FadeOutComplete;

                fadePanel
                    .FadeOut();
            }
        }

        #endregion

        #region Event Handlers

        private void FadePanel_FadeOutComplete(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(SceneName))
            {
                SceneManager
                       .LoadSceneAsync(SceneName);
            }
        }

        #endregion
    }
}