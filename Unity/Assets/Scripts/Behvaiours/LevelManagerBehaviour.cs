
namespace KasJam.MiniJam79.Unity.Behaviours
{
    using KasJam.MiniJam79.Unity.Managers;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    [AddComponentMenu("KasJam/LevelManager")]
    public class LevelManagerBehaviour : BehaviourBase
    {
        #region Members

        public PlatformerBehaviour Platformer;

        public float TransitionStartTime;

        public float TransitionTotalTime;

        protected LevelBehaviour CurrentLevel { get; set; }

        protected LevelBehaviour ToLevel { get; set; }

        protected List<int> CurrentTransitionIndex { get; set; }

        protected List<List<TileTransition>> TileTransitions { get; set; }

        protected List<List<float>> TransitionTimes { get; set; }

        protected float TransitionTimer { get; set; }

        protected int CurrentLevelIndex { get; set; }

        #endregion

        #region Events

        public event EventHandler LevelStarted;

        protected void OnLevelStarted()
        {
            LevelStarted?
                .Invoke(this, EventArgs.Empty);
        }

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

                var collider = CurrentLevel
                    .Tilemaps[2]
                    .GetComponent<CompositeCollider2D>();

                Platformer.OneWayCollider = collider;
            }

            /*
            var toPrefab = Resources
                .Load<LevelBehaviour>($"Prefabs/Levels/Level{toIndex}");

            ToLevel = null;
            if (toPrefab == null)
            {
                return;
            }

            ToLevel = Instantiate(toPrefab);
            ToLevel
                .gameObject
                .SetActive(false);

            TransitionTimer = 0;

            CurrentTransitionIndex
                .Clear();

            int transitionCount = Mathf
                .Max(CurrentLevel.MovingPlatforms.Length, ToLevel.MovingPlatforms.Length);

            CurrentTransitionIndex
                .Add(0);

            CreateTransitionTimes(0, transitionCount);

            for (int i = 0; i < 3; i++)
            {
                CurrentTransitionIndex
                    .Add(0);

                CreateTransitionTimes(i + 1, CurrentLevel.Tilemaps[i], ToLevel.Tilemaps[i]);
            }
            
            CurrentLevelIndex = toIndex;
            */

            OnLevelStarted();
        }


        protected void CompleteTransition()
        {
            Destroy(ToLevel.gameObject);
            CurrentLevelIndex++;
            TransitionTo(CurrentLevelIndex);            
        }

        protected void CreateTransitionTimes(int index, Tilemap from, Tilemap to)
        {
            var tileTransitions = TileTransitions[index];

            tileTransitions
                .Clear();

            int transitionCount = 0;
            int idx = 0;
            for (int y = from.cellBounds.min.y; y < from.cellBounds.max.y; y++)
            {
                for (int x = from.cellBounds.min.x; x < from.cellBounds.max.x; x++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);

                    var fromTile = from
                        .GetTile(pos);

                    var toTile = to
                        .GetTile(pos);

                    if (fromTile != toTile)
                    {
                        tileTransitions.Add(new TileTransition
                        {
                            FromTile = fromTile,
                            ToTile = toTile,
                            Coords = new Vector3Int(x, y, 0)
                        });

                        transitionCount++;
                    }

                    idx++;
                }
            }

            CreateTransitionTimes(index, transitionCount);
        }

        protected void CreateTransitionTimes(int index, int count)
        {
            var transitionTimes = TransitionTimes[index];

            transitionTimes
                .Clear();

            if (count > 0)
            {
                float stepTime = (TransitionTotalTime - TransitionStartTime) / count;
                float transitionTime = TransitionStartTime;
                for (int i = 0; i < count; i++)
                {
                    transitionTimes
                        .Add(transitionTime);

                    transitionTime += stepTime;
                }
            }
        }

        protected void DoTransition(int transitionType, int index)
        {
            switch (transitionType)
            {
                case 0:
                    DoMovingPlatformtransition(index);
                    break;
                case 1:
                case 2:
                case 3:
                    DoTileTransition(transitionType - 1, index);
                    break;
            }
        }

        protected void DoTileTransition(int tilemapIndex, int transitionIndex)
        {
            var tileTransitions = TileTransitions[tilemapIndex + 1];
            if (transitionIndex >= tileTransitions.Count)
            {
                return;                
            }

            var tileTransition = tileTransitions[transitionIndex];

            Debug
                .Log($"Coords '{tileTransition.Coords}' From '{tileTransition.FromTile}' To '{tileTransition.ToTile}'");

            var fromTilemap = CurrentLevel.Tilemaps[tilemapIndex];

            fromTilemap
                .SetTile(tileTransition.Coords, tileTransition.ToTile);
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

        public void Reset()
        {
            CurrentLevelIndex = 0;
            TransitionTo(1);
            
            PauseGame(false);
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
            TileTransitions = new List<List<TileTransition>>();
            
            for (int i = 0;i < 4;i++)
            {
                TransitionTimes
                    .Add(new List<float>());

                TileTransitions
                    .Add(new List<TileTransition>());
            }

            TransitionTo(1);
        }

        protected void Update()
        {
            if (GameManager
                .Instance
                .IsPaused)
            {
                return;
            }

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
                var transitionTimes = TransitionTimes[i];
                var currentIndex = CurrentTransitionIndex[i];                                
                
                if (currentIndex < transitionTimes.Count)
                {
                    var plannedTime = transitionTimes[currentIndex];

                    //Debug
                        //.Log($"i '{i}' currentIndex '{currentIndex}' plannedTime '{plannedTime}' TransitionTimer '{TransitionTimer}'");

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
