namespace KasJam.MiniJam79.Unity.Behaviours
{
    using System;
    using System.Collections.Generic;
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

        public const int WATER_MAP = 0;
        public const int FLOOR_MAP = 1;        
        public const int ONE_WAY_MAP  = 2;
        public const int OVERLAY_MAP = 3;

        public static Dictionary<Color, LevelPixelType> ColorsToPixelTypeMap = new Dictionary<Color, LevelPixelType>
        {
            [new Color32(0, 255, 0, 255)] = LevelPixelType.RegularGround,
            [new Color32(128, 128, 255, 255)] = LevelPixelType.AnimatedWater,
            [new Color32(0, 0, 255, 255)] = LevelPixelType.PureWater
        };

        protected TileBase RegularGroundTile { get; set; }

        protected TileBase AnimatedWaterTile { get; set; }

        protected TileBase PureWaterTile { get; set; }

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
            for (int py = LevelHeight - 1; py >= 0; py--)
            {
                for (int px = LevelWidth - 1; px >= 0; px--)
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

                    float wx = (px * 0.64f) - hlw;
                    float wy = (py * 0.64f) - hlh;
                    var v3i = new Vector3Int(px - mlw, py - mlh, 0);

                    switch (ColorsToPixelTypeMap[p])
                    {
                        case LevelPixelType.RegularGround:                            
                            levelObject
                                .Tilemaps[FLOOR_MAP]
                                .SetTile(v3i, RegularGroundTile);

                            break;

                        case LevelPixelType.AnimatedWater:
                            levelObject
                                .Tilemaps[WATER_MAP]
                                .SetTile(v3i, AnimatedWaterTile);

                            break;

                        case LevelPixelType.PureWater:
                            levelObject
                                .Tilemaps[WATER_MAP]
                                .SetTile(v3i, PureWaterTile);

                            break;
                    }

                    pi++;
                }
            }
        }

        protected void LoadTiles()
        {
            RegularGroundTile = Resources
                .Load<TileBase>("Tiles/DirtBlockTile");

            AnimatedWaterTile = Resources
                .Load<TileBase>("Tiles/WaterAnimatedTile");

            PureWaterTile = Resources
                .Load<TileBase>("PureWaterTile");
        }

        #endregion


        #region Unity

        protected override void Awake()
        {
            LoadTiles();

            ImportLevels();    
        }

        #endregion
    }
}

