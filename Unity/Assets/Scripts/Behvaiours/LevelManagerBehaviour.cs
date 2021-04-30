
namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("KasJam/LevelManager")]
    public class LevelManagerBehaviour : BehaviourBase
    {
        #region Members

        public float TransitionStartTime;

        public float TransitionTotalTime;

        protected LevelBehaviour CurrentLevel { get; set; }

        protected LevelBehaviour ToLevel { get; set; }

        protected List<int> CurrentTransitionIndex { get; set; }

        protected List<List<float>> TransitionTimes { get; set; }

        protected float TransitionTimer { get; set; }

        protected int CurrentLevelIndex { get; set; }

        #endregion

        #region Events

        public event EventHandler LevelTransitioned;

        protected void OnLevelTransitioned()
        {
            LevelTransitioned?
                .Invoke(this, EventArgs.Empty);
        }

        #endregion
    
        #region Protected Methods

        protected void TransitionTo(int toIndex)
        {
            if (CurrentLevel == null)
            {
                var prefab = Resources
                    .Load<LevelBehaviour>($"Prefabs/Levels/Level{CurrentLevelIndex}");

                CurrentLevel = Instantiate(prefab);
            }

            var toPrefab = Resources
                .Load<LevelBehaviour>($"Prefabs/Levels/Level{toIndex}");

            ToLevel = Instantiate(toPrefab);
            ToLevel
                .gameObject
                .SetActive(false);

            TransitionTimer = 0;

            CurrentTransitionIndex
                .Clear();

            int transitionCount = Mathf.Max(CurrentLevel.MovingPlatforms.Length, ToLevel.MovingPlatforms.Length);

            CurrentTransitionIndex
                .Add(0);

            CreateTransitionTimes(0, transitionCount);
        }


        protected void CompleteTransition()
        {
            Destroy(ToLevel.gameObject);

            CurrentLevelIndex++;
            TransitionTo(CurrentLevelIndex);
        }

        protected void CreateTransitionTimes(int index, int count)
        {
            var transitionTimes = TransitionTimes[index];

            transitionTimes
                .Clear();

            float stepTime = (TransitionTotalTime - TransitionStartTime) / count;
            float transitionTime = TransitionStartTime;
            for (int i = 0; i < count; i++)
            {
                transitionTimes
                    .Add(transitionTime);

                transitionTime += stepTime;
            }
        }

        protected void DoTransition(int transitionType, int index)
        {
            Debug
                .Log($"DOING TRANSITION Index '{index}' Time '{TransitionTimer}'");



            switch (transitionType)
            {
                case 0:
                    DoMovingPlatformtransition(index);
                    break;
            }
        }

        protected void DoMovingPlatformtransition(int index)
        {
            MovingPlatformBehaviour currentPlatform;
            MovingPlatformBehaviour toPlatform;

            if (index < CurrentLevel.MovingPlatforms.Length)
            {
                currentPlatform = CurrentLevel.MovingPlatforms[index];

                currentPlatform
                    .StartTransition(true);
            }

            if (index < ToLevel.MovingPlatforms.Length)
            {
                toPlatform = ToLevel.MovingPlatforms[index];

                CurrentLevel.MovingPlatforms[index] = toPlatform;

                toPlatform
                    .transform
                    .SetParent(CurrentLevel.Objects.transform);

                toPlatform
                    .gameObject
                    .SetActive(true);

                toPlatform
                    .StartTransition(false);
            }
        }

        #endregion

        #region Unity

        protected override void Awake()
        {
            base
                .Awake();

            CurrentLevelIndex = 0;

            CurrentTransitionIndex = new List<int>();
            TransitionTimes = new List<List<float>>();

            for(int i = 0;i < 4;i++)
            {
                TransitionTimes
                    .Add(new List<float>());
            }

            TransitionTo(1);
        }

        protected void Update()
        {
            if (TransitionTimer >= TransitionTotalTime)
            {
                return;
            }

            if (ToLevel == null)
            {
                return;
            }

            TransitionTimer += Time.deltaTime;

            if (TransitionTimer >= TransitionTotalTime)
            {
                TransitionTimer = TransitionTotalTime;

                CurrentLevel = ToLevel;
                ToLevel = null;

                CompleteTransition();

                OnLevelTransitioned();

                return;
            }

            if (TransitionTimer < TransitionStartTime)
            {
                return;
            }

            for(int i = 0;i < CurrentTransitionIndex.Count;i++)
            {
                var currentIndex = CurrentTransitionIndex[i];
                var transitionTimes = TransitionTimes[currentIndex];

                if (currentIndex < transitionTimes.Count)
                {
                    var plannedTime = transitionTimes[currentIndex];
                    if (TransitionTimer >= plannedTime)
                    {
                        DoTransition(i, currentIndex);
                        CurrentTransitionIndex[i]++;
                    }
                }
            }
        }

        #endregion
    }
}