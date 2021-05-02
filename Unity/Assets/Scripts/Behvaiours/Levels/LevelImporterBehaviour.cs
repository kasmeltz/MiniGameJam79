namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System;
    using System.Collections.Generic;
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
            [new Color32(255, 255, 0, 255)] = LevelPixelType.LiliPad,
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

            for (int fy = 0; fy < LevelsTexture.height; fy += LevelHeight)
            {
                for (int fx = 0; fx < LevelsTexture.width; fx += LevelWidth)
                {
                    var pixels = LevelsTexture
                        .GetPixels(fx, fy, LevelWidth, LevelHeight);

                    ImportAndSaveLevel(index, pixels);
                }
            }
        }

        protected void ImportAndSaveLevel(int index, Color[] pixels)
        {
            var levelObject = Instantiate(LevelPrefab);

            float hlw = LevelWidth * 0.32f;
            float hlh = LevelHeight * 0.32f;

            int mlw = LevelWidth / 2;
            int mlh = LevelHeight / 2;

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

                    if (!ColorsToPixelTypeMap
                        .ContainsKey(p))
                    {
                        throw new InvalidOperationException($"Pixel '{px},{py}' contains invalid Color '{p}'");
                    }

                    var pixelType = ColorsToPixelTypeMap[p];
                    float wx = (px * 0.64f) - hlw + 0.32f;
                    float wy = (py * 0.64f) - hlh + 0.32f;
                    var v3i = new Vector3Int(px - mlw, py - mlh, 0);
                    Vector3 pos = new Vector3(wx, wy, 0);

                    /*
                     * 
        MovingPlatformStart = 12,
        MovingPlatformEnd = 13,
                     * */

                    switch (pixelType)
                    {
                        case LevelPixelType.RegularGround:
                            levelObject
                                .Tilemaps[FLOOR_MAP]
                                .SetTile(v3i, PixelTypeToTileMap[pixelType]);
                            break;

                        case LevelPixelType.OneWayFloor:
                            levelObject
                                .Tilemaps[ONE_WAY_MAP]
                                .SetTile(v3i, PixelTypeToTileMap[pixelType]);
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
                                .SetParent(levelObject.Enemies.transform);
                            frog.transform.position = pos;
                            frog.Bounds = new Bounds(pos, new Vector3(5, 0, 0));
                                break;

                        case LevelPixelType.CherryFlySpawner:
                        case LevelPixelType.StrawberryFlySpawner:
                        case LevelPixelType.LemonFlySpawner:
                        case LevelPixelType.PoisonFlySpawner:
                            var flySpawner = Instantiate(FlySpawnerPrefab);
                            flySpawner
                                .transform
                                .SetParent(levelObject.FlySpawners.transform);
                            flySpawner.transform.position = pos;
                            var flyBehaviour = PixelTypeToFlyTypeMap[pixelType];
                            flySpawner.ObjectToPool = flyBehaviour.gameObject;
                            break;

                    }

                    pi++;
                }
            }

            PrefabUtility.SaveAsPrefabAsset(levelObject.gameObject, $"Assets/Resources/Prefabs/Levels/Level{index}.prefab");
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

            LoadTiles();

            ImportLevels();    
        }

        #endregion
    }
}



