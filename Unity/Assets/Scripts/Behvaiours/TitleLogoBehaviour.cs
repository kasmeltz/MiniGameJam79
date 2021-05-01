
namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine.UI;

    public class TitleLogoBehaviour
    {
        #region Members

        public Image Logo { get; set; }

        public Image Tongue { get; set; }

        #endregion

        #region Public Methods

        public void ShowTongue(bool show)
        {
            Tongue
                .gameObject
                .SetActive(show);
        }

        #endregion
    }
}