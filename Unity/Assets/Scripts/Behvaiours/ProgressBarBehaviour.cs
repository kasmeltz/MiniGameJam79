namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("KasJam/ProgressBar")]
    public class ProgressBarBehaviour : BehaviourBase
    {
        #region Members

        public Image BackgroundImage;

        public Image ForegroundImage;

        public SpriteRenderer Background;

        public SpriteRenderer ForeGround;

        #endregion

        #region Public Methods

        public void SetValue(float value, float maxValue)
        {
            float ratio = value / maxValue;

            if (ForeGround != null)
            {
                var size = ForeGround.size;

                size.x = ratio * Background.size.x;

                ForeGround.size = size;
            }

            if (ForegroundImage != null)
            {
                ForegroundImage.fillAmount = ratio;
            }
        }

        #endregion
    }
}