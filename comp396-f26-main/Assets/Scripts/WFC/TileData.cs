using System.Collections.Generic;
using UnityEngine;

namespace Core.WFC
{
    [CreateAssetMenu(fileName = "NewTileData", menuName = "WFC/Tile Data")]
    public class TileData : ScriptableObject
    {
        public string tileName;
        public GameObject prefab;

        [Header("Neighbour Rules")]
        public List<TileData> validBottom;
        public List<TileData> validLeft;
        public List<TileData> validRight;
        public List<TileData> validTop;

        private void OnValidate()
        {
            if (prefab != null) tileName = prefab.name;
        }
    }
}