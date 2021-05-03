
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

        public float EnemyRespawnTime;

        protected LevelBehaviour CurrentLevel { get; set; }

        protected LevelBehaviour ToLevel { get; set; }

        protected List<int> CurrentTransitionIndex { get; set; }

        protected List<List<TileTransition>> TileTransitions { get; set; }

        protected List<List<float>> TransitionTimes { get; set; }

        protected float TransitionTimer { get; set; }

        protected int CurrentLevelIndex { get; set; }


        protected float LastUpgradeTime { get; set; }

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

        #region Public Methods

        public void Reset()
        {
            CurrentLevelIndex = 0;
            LastUpgradeTime = Time.time;

            TransitionTo(1);

            PauseGame(false);
        }

        #endregion

        #region Protected Methods

        protected void LoadNextLevel(int toIndex)
        {
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

            // PREPARE TILE TRANSITION TIMES
            for (int i = 0; i < 4; i++)
            {
                CurrentTransitionIndex
                    .Add(0);

                CreateTransitionTimes(i, CurrentLevel.Tilemaps[i], ToLevel.Tilemaps[i]);
            }
        }

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

            LoadNextLevel(toIndex);

            CurrentLevelIndex = toIndex;

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

            Debug
                .Log($"{transitionCount} tile changes for layer {index}");
                
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
            DoTileTransition(transitionType, index);
        }

        protected void DoTileTransition(int tilemapIndex, int transitionIndex)
        {
            var tileTransitions = TileTransitions[tilemapIndex];
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

        protected void CheckEnemies()
        {
            bool doUpdgrade = false;

            if (Time.time - LastUpgradeTime >= 60)
            {
                LastUpgradeTime = Time.time;
                doUpdgrade = true;
            }

            foreach (var enemy in CurrentLevel.Enemies)
            {
                if (enemy.IsDead)
                {
                    if (Time.time - enemy.DeathTime >= EnemyRespawnTime)
                    {
                        enemy.Health = enemy.MaxHealth;
                        enemy.IsDead = false;
                        enemy
                            .gameObject
                            .SetActive(true);
                    }
                }

                if (doUpdgrade)
                {
                    enemy
                        .SetLevel(enemy.Level + 1);
                }
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

            for (int i = 0; i < CurrentTransitionIndex.Count; i++)
            {
                var transitionTimes = TransitionTimes[i];
                var currentIndex = CurrentTransitionIndex[i];

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

            CheckEnemies();
        }

        #endregion
    }
}
