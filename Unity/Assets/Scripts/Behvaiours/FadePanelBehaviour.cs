namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("KasJam/FadePanel")]
    public class FadePanelBehaviour : BehaviourBase
    {
        #region Members

        public Image ToFade;

        public float FadeSpeed;

        public bool IsFadingIn { get; protected set; }

        public bool IsFadingOut { get; protected set; }

        protected float Progress { get; set; }

        #endregion

        #region Events

        public event EventHandler FadeInComplete;
        protected void OnFadeInComplete()
        {
            FadeInComplete?
                .Invoke(this, EventArgs.Empty);
        }

        public event EventHandler FadeOutComplete;
        protected void OnFadeOutComplete()
        {
            FadeOutComplete?
                .Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Public Methods

        public void FadeIn()
        {
            if (IsFadingIn || IsFadingOut)
            {
                return;
            }

            ToFade.color = new Color(0, 0, 0, 255);
            IsFadingIn = true;
            Progress = 0;
        }

        public void FadeOut()
        {
            if (IsFadingIn || IsFadingOut)
            {
                return;
            }

            ToFade.color = new Color(0, 0, 0, 0);
            IsFadingOut = true;
            Progress = 0;
        }

        #endregion

        #region Unity

        protected void Update()
        {
            if (!IsFadingIn && !IsFadingOut)
            {
                return;
            }

            if (Progress >= 1)
            {
                return;
            }

            Progress += Time.deltaTime * FadeSpeed;

            if (IsFadingIn)
            {
                ToFade.color = new Color(0f, 0f, 0f, 1 - Progress);
            }
            else if (IsFadingOut)
            {
                ToFade.color = new Color(0f, 0f, 0f, Progress);
            }

            if (Progress >= 1)
            {
                Progress = 1;

                if (IsFadingIn)
                {
                    IsFadingIn = false;
                    OnFadeInComplete();
                }

                if (IsFadingOut)
                {
                    IsFadingOut = false;
                    OnFadeOutComplete();
                }
            }
        }

        protected override void Awake()
        {
            base
                .Awake();

            ToFade.color = new Color(0, 0, 0, 1f);
        }

        #endregion
    }
}