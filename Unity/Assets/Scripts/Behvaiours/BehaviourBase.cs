namespace KasJam.MiniJam79.Unity.Behaviours
{
    using KasJam.MiniJam79.Unity.Managers;
    using System;
    using System.Collections;
    using UnityEngine;

    public class BehaviourBase : MonoBehaviour
    {
        #region Protected Methods

        protected Coroutine DoAfter(float seconds, Action toPerform)
        {
            var coroutine = StartCoroutine(ExecuteAfterTimeCoroutine(seconds, toPerform));

            return coroutine;
        }

        protected IEnumerator ExecuteAfterTimeCoroutine(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);

            action();
        }

        protected void PauseGame(bool isPaused)
        {
            GameManager
                .Instance
                .IsPaused = isPaused;
        }

        #endregion

        #region Unity

        protected virtual void Awake()
        {
        }

        #endregion
    }
}