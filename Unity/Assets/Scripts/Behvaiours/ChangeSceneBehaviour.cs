namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [AddComponentMenu("KasJam/ChangeScene")]
    public class ChangeSceneBehaviour : BehaviourBase
    {
        #region Public Methods

        public void ChangeScene(string sceneName)
        {
            SceneManager
                .LoadSceneAsync(sceneName);
        }

        #endregion
    }
}