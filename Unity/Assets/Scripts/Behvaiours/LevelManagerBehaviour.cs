
namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    [AddComponentMenu("KasJam/LevelManager")]
    public class LevelManagerBehaviour : BehaviourBase
    {
        #region Members

        public float TransitionStartTime;

        public float TransitionTotalTime;

        protected LevelBehaviour CurrentLevel { get; set; }

        protected LevelBehaviour ToLevel { get; set; }

        protected List<int> CurrentTransitionIndex { get; set; }

        protected List<List<int>> CurrentTileTransitionIndex { get; set; }

        protected List<List<float>> TransitionTimes { get; set; }

        protected List<TileBase[]> FromTileArrays { get; set; }

        protected List<TileBase[]> ToTileArrays { get; set; }

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
        }


        protected void CompleteTransition()
        {
            Destroy(ToLevel.gameObject);
            CurrentLevelIndex++;
            TransitionTo(CurrentLevelIndex);            
        }

        protected void CreateTransitionTimes(int index, Tilemap from, Tilemap to)
        {
            var tileTransitionIndex = CurrentTileTransitionIndex[index];

            tileTransitionIndex
                .Clear();

            TileBase[] fromTiles = from
                .GetTilesBlock(from.cellBounds);

            TileBase[] toTiles = to
                .GetTilesBlock(to.cellBounds);

            int transitionCount = 0;
            int idx = 0;
            for (int y = from.cellBounds.min.y; y < from.cellBounds.max.y; y++)
            {
                for (int x = from.cellBounds.min.x; x < from.cellBounds.max.x; x++)
                {
                    /*
                    if (fromTiles[idx] != null)
                    {
                        Debug
                            .Log($"IDX: '{idx}' TILE: {fromTiles[idx]}");
                    }
                    
                    if (toTiles[idx] != null)
                    {
                        Debug
                            .Log($"IDX: '{idx}' TILE: {toTiles[idx]}");
                    }
                    */

                    if (fromTiles[idx] != toTiles[idx])
                    {
                        tileTransitionIndex
                            .Add(idx);
                        
                        transitionCount++;
                    }

                    idx++;
                }
            }

            FromTileArrays[index] = fromTiles;
            ToTileArrays[index] = toTiles;

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
            Debug
                .Log($"DOING TRANSITION Index '{index}' Time '{TransitionTimer}'");

            switch (transitionType)
            {
                case 0:
                    DoMovingPlatformtransition(index);
                    break;
                case 1:
                case 2:
                case 3:
                    DoTileTransition(transitionType, index);
                    break;
            }
        }

        protected void DoTileTransition(int tilemapIndex, int transitionIndex)
        {
            TileBase[] fromTiles = FromTileArrays[tilemapIndex];
            TileBase[] toTiles = ToTileArrays[tilemapIndex];

            var tileTransitionIndex = CurrentTileTransitionIndex[tilemapIndex];
            if (tilemapIndex >= tileTransitionIndex.Count)
            {
                return;                
            }

            var tileIndex = tileTransitionIndex[transitionIndex];
            var fromTile = fromTiles[tileIndex];
            var toTile = toTiles[tileIndex];

            var f = 1;

            //CurrentLevel.flo
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
            FromTileArrays = new List<TileBase[]>();
            ToTileArrays = new List<TileBase[]>();
            CurrentTileTransitionIndex = new List<List<int>>();

            for (int i = 0;i < 4;i++)
            {
                TransitionTimes
                    .Add(new List<float>());

                FromTileArrays
                    .Add(new TileBase[1]);

                ToTileArrays
                    .Add(new TileBase[1]);

                CurrentTileTransitionIndex
                    .Add(new List<int>());
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