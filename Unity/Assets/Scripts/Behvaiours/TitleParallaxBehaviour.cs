
namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/TitleParallax")]
    public class TitleParallaxBehaviour : BehaviourBase
    {
        #region Members

        #endregion

        #region Unity

        protected void Update()
        {
            Camera.main.transform.position += new Vector3(1, 0, 0) * Time.deltaTime;
        }

        #endregion
    }
}