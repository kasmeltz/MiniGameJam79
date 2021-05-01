
namespace KasJam.MiniJam79.Unity.Behaviours
{
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public class TileTransition
    {
        #region Members
        
        public Vector3Int Coords { get; set; }

        public TileBase FromTile { get; set; }

        public TileBase ToTile { get; set; }

        #endregion
    }
}