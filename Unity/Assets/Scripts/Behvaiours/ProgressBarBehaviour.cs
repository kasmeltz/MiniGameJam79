namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("KasJam/ProgressBar")]
    public class ProgressBarBehaviour : BehaviourBase
    {
        #region Members

        public SpriteRenderer Background;

        public SpriteRenderer ForeGround;

        #endregion

        #region Public Methods

        public void SetValue(float value, float maxValue)
        {
            float ratio = value / maxValue;

            var size = ForeGround.size;

            size.x = ratio * Background.size.x;

            ForeGround.size = size;
        }

        #endregion
    }
}