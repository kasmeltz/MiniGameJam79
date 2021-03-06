namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    [AddComponentMenu("KasJam/LevelImporter")]
    public class LevelImporterBehaviour : BehaviourBase
    {
        #region Members

        public Texture2D LevelsTexture;

        public MovingPlatformBehaviour MovingPlatformPrefab;

        public PoisonFrogBehaviour PoisonFrogPrefab;

        public FlySpawnerBehaviour FlySpawnerPrefab;

        public LevelBehaviour LevelPrefab;

        public int LevelWidth;

        public int LevelHeight;

        protected Dictionary<LevelPixelType, TileBase> PixelTypeToTileMap { get; set; }

        protected Dictionary<LevelPixelType, FlyBehaviour> PixelTypeToFlyTypeMap { get; set; }

        public const int WATER_MAP = 0;
        public const int FLOOR_MAP = 1;
        public const int ONE_WAY_MAP = 2;
        public const int OVERLAY_MAP = 3;

        public static Dictionary<Color, LevelPixelType> ColorsToPixelTypeMap = new Dictionary<Color, LevelPixelType>
        {
            [new Color32(0, 255, 0, 255)] = LevelPixelType.RegularGround,
            [new Color32(128, 128, 255, 255)] = LevelPixelType.AnimatedWater,
            [new Color32(0, 0, 255, 255)] = LevelPixelType.PureWater,
            [new Color32(0, 128, 0, 255)] = LevelPixelType.BrambleBush,
            [new Color32(255, 227, 0, 255)] = LevelPixelType.LiliPad,
            [new Color32(128, 0, 255, 255)] = LevelPixelType.LotusFlower,
            [new Color32(196, 196, 196, 255)] = LevelPixelType.Boat,
            [new Color32(128, 128, 0, 255)] = LevelPixelType.OneWayFloor,
            [new Color32(98, 85, 101, 255)] = LevelPixelType.PoisonFrog,
            [new Color32(255, 0, 0, 255)] = LevelPixelType.StrawberryFlySpawner,
            [new Color32(128, 0, 0, 255)] = LevelPixelType.CherryFlySpawner,
            [new Color32(255, 196, 32, 255)] = LevelPixelType.LemonFlySpawner,
            [new Color32(90, 50, 99, 255)] = LevelPixelType.PoisonFlySpawner,
        };

        #endregion

        #region Protected Methods

        protected void ImportLevels()
        {
            int index = 0;

            for (int fy = LevelsTexture.height - LevelHeight; fy >= 0; fy -= LevelHeight)
            {
                for (int fx = 0; fx < LevelsTexture.width; fx += LevelWidth)
                {
                    var pixels = LevelsTexture
                        .GetPixels(fx, fy, LevelWidth, LevelHeight);

                    ImportAndSaveLevel(index, pixels);
                    
                    index++;
                }
            }
        }

        protected void ImportAndSaveLevel(int index, Color[] pixels)
        {
            var levelObject = Instantiate(LevelPrefab);

            bool[] platforms = new bool[pixels.Length];

            float hlw = LevelWidth * 0.32f;
            float hlh = LevelHeight * 0.32f;

            int mlw = LevelWidth / 2;
            int mlh = LevelHeight / 2;

            Dictionary<int, MovingPlatformBehaviour> movingPlatforms = new Dictionary<int, MovingPlatformBehaviour>();
            Dictionary<int, int> movingPlatformSizes = new Dictionary<int, int>();
            List<FlySpawnerBehaviour> flySpawners = new List<FlySpawnerBehaviour>();
            List<EnemyBehaviour> enemies = new List<EnemyBehaviour>();

            int pi = 0;
            for (int py = 0; py < LevelHeight; py++)
            {
                for (int px = 0; px < LevelWidth; px++)
                {
                    Color32 p = pixels[pi];

                    if (p.a == 0)
                    {
                        pi++;
                        continue;
                    }

                    float wx = (px * 0.64f) - hlw + 0.32f;
                    float wy = (py * 0.64f) - hlh + 0.32f;
                    var v3i = new Vector3Int(px - mlw, py - mlh, 0);
                    Vector3 pos = new Vector3(wx, wy, 0);

                    if ((p.b == 255) && 
                        (p.r == 255 || p.g == 255))
                    {
                        int platformIndex;
                        int byteValue = p.r;
                        if (byteValue == 255)
                        {
                            byteValue = p.g;
                        }
                        platformIndex = byteValue / 16;

                        MovingPlatformBehaviour platform;
                        if (!movingPlatforms
                            .ContainsKey(platformIndex))
                        {
                            platform = Instantiate(MovingPlatformPrefab);
                            platform
                                .transform
                                .SetParent(levelObject.ObjectParent.transform);
                            movingPlatforms[platformIndex] = platform;
                        }

                        platform = movingPlatforms[platformIndex];

                        if (p.r == 255)
                        {
                            if (!movingPlatformSizes
                                .ContainsKey(platformIndex))
                            {
                                movingPlatformSizes[platformIndex] = 1;
                            }
                            else
                            {
                                movingPlatformSizes[platformIndex]++;
                            }

                            if (platform.Start == Vector2.zero)
                            {
                                platform.Start = new Vector2(wx, wy);
                                platform.transform.position = new Vector3(wx, wy, 0);
                            }
                        }
                        else if (p.g == 255)
                        {
                            if (platform.End == Vector2.zero)
                            {
                                platform.End = new Vector2(wx, wy);
                            }
                        }

                        pi++;
                        continue;
                    }

                    if (!ColorsToPixelTypeMap
                        .ContainsKey(p))
                    {
                        //   throw new InvalidOperationException($"Pixel '{px},{py}' contains invalid Color '{p}'");
                        Debug
                            .Log($"Pixel '{px},{py}' contains invalid Color '{p}'");

                        pi++;

                        continue;
                    }

                    var pixelType = ColorsToPixelTypeMap[p];

                    switch (pixelType)
                    {
                        case LevelPixelType.RegularGround:
                            levelObject
                                .Tilemaps[FLOOR_MAP]
                                .SetTile(v3i, PixelTypeToTileMap[pixelType]);

                            platforms[pi] = true;

                            break;

                        case LevelPixelType.OneWayFloor:
                            levelObject
                                .Tilemaps[ONE_WAY_MAP]
                                .SetTile(v3i, PixelTypeToTileMap[pixelType]);

                            platforms[pi] = true;

                            break;

                        case LevelPixelType.AnimatedWater:
                        case LevelPixelType.PureWater:
                            levelObject
                                .Tilemaps[WATER_MAP]
                                .SetTile(v3i, PixelTypeToTileMap[pixelType]);
                            break;

                        case LevelPixelType.LiliPad:
                        case LevelPixelType.BrambleBush:
                        case LevelPixelType.LotusFlower:
                        case LevelPixelType.Boat:
                            levelObject
                                .Tilemaps[OVERLAY_MAP]
                                .SetTile(v3i, PixelTypeToTileMap[pixelType]);
                            break;

                        case LevelPixelType.PoisonFrog:
                            var frog = Instantiate(PoisonFrogPrefab);
                            frog
                                .transform
                                .SetParent(levelObject.EnemyParent.transform);
                            frog.transform.position = pos;
                            frog.GridCoords = new Vector2Int(px, py);

                            enemies
                                .Add(frog);
                                break;

                        case LevelPixelType.CherryFlySpawner:
                        case LevelPixelType.StrawberryFlySpawner:
                        case LevelPixelType.LemonFlySpawner:
                        case LevelPixelType.PoisonFlySpawner:
                            var flySpawner = Instantiate(FlySpawnerPrefab);
                            flySpawner
                                .transform
                                .SetParent(levelObject.FlySpawnerParent.transform);
                            flySpawner.transform.position = pos;
                            var flyBehaviour = PixelTypeToFlyTypeMap[pixelType];
                            flySpawner.ObjectToPool = flyBehaviour.gameObject;

                            flySpawners
                                .Add(flySpawner);
                            break;
                    }

                    pi++;
                }
            }

            foreach(var kvp in movingPlatforms)
            {
                var platformIndex = kvp.Key;
                var platform = kvp.Value;

                if (platform.Start.x > platform.End.x)
                {
                    platform.Direction = 1;
                }
                else
                {
                    platform.Direction = -1;
                }

                var pixelCount = movingPlatformSizes[platformIndex];
                var sr = platform
                    .GetComponent<SpriteRenderer>();

                var size = new Vector2(pixelCount * 0.32f, 0.16f);
                sr.size = size;

                platform.Collider.size = size;                
            }

            foreach(var enemy in enemies)
            {
                var frog = enemy as PoisonFrogBehaviour;
                if (frog == null)
                {
                    continue;
                }

                var gx = enemy.GridCoords.x;
                var gy = enemy.GridCoords.y;

                if (gy == 0)
                {
                    continue;
                }

                int sx = gx;
                int ex = gx;
                for(int x = gx;x > 0;x--)
                {
                    int idx = (gy - 1) * LevelWidth + x;

                    if (!platforms[idx])
                    {
                        break;
                    }

                    sx = x;
                }

                for (int x = gx; x < LevelWidth; x++)
                {
                    int idx = (gy - 1) * LevelWidth + x;

                    if (!platforms[idx])
                    {
                        break;
                    }

                    ex = x;
                }

                int dx = ex - sx;

                var size = new Vector3(dx * 0.64f, 0, 0);

                float wsx = (sx * 0.64f) - hlw + 0.32f;
                float wex = (ex * 0.64f) - hlw + 0.32f;

                float middle = (wsx + wex) / 2;

                Vector3 center = new Vector3(middle, frog.transform.position.y, 0);

                frog.Bounds = new Bounds(center, size);

                Debug
                    .Log($"Platform bounds? '{frog.Bounds}'");                                
            }

            levelObject.MovingPlatforms = movingPlatforms
                .Values
                .ToArray();

            levelObject.Enemies = enemies
                .ToArray();

            levelObject.FlySpawners = flySpawners
                .ToArray();

#if UNITY_EDITOR
            PrefabUtility.SaveAsPrefabAsset(levelObject.gameObject, $"Assets/Resources/Prefabs/Levels/Level{index}.prefab");
#endif

            Destroy(levelObject);
        }

        protected void LoadTiles()
        {
            PixelTypeToTileMap = new Dictionary<LevelPixelType, TileBase>
            {
                [LevelPixelType.RegularGround] = Resources.Load<TileBase>("Tiles/DirtBlockTile"),
                [LevelPixelType.AnimatedWater] = Resources.Load<TileBase>("Tiles/WaterAnimatedTile"),
                [LevelPixelType.PureWater] = Resources.Load<TileBase>("Tiles/PureWaterTile"),
                [LevelPixelType.BrambleBush] = Resources.Load<TileBase>("Tiles/BrambleTile"),
                [LevelPixelType.LiliPad] = Resources.Load<TileBase>("Tiles/LilipadTile"),
                [LevelPixelType.LotusFlower] = Resources.Load<TileBase>("Tiles/LotusFlowerTile"),
                [LevelPixelType.Boat] = Resources.Load<TileBase>("Tiles/PaperBoatTile"),
                [LevelPixelType.OneWayFloor] = Resources.Load<TileBase>("Tiles/OneWayFloor"),
            };

            PixelTypeToFlyTypeMap = new Dictionary<LevelPixelType, FlyBehaviour>
            {
                [LevelPixelType.CherryFlySpawner] = Resources.Load<FlyBehaviour>("Prefabs/Objects/CherryFly"),
                [LevelPixelType.StrawberryFlySpawner] = Resources.Load<FlyBehaviour>("Prefabs/Objects/StrawBerryFly"),
                [LevelPixelType.LemonFlySpawner] = Resources.Load<FlyBehaviour>("Prefabs/Objects/LemonFly"),
                [LevelPixelType.PoisonFlySpawner] = Resources.Load<FlyBehaviour>("Prefabs/Objects/PoisonFly")
            };
        }

        #endregion

        #region Unity

        protected override void Awake()
        {
            PauseGame(true);

#if UNITY_EDITOR
            AssetDatabase
                .Refresh();
#endif

            LoadTiles();

            ImportLevels();    
        }

        #endregion
    }
}



