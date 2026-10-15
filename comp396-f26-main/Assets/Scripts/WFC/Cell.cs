using System.Collections.Generic;

namespace Core.WFC
{
    [System.Serializable]
    public class Cell
    {
        public int x, y; // Coordinate in the Grid Map
        public List<TileData> possibleTiles;
        public bool isCollapsed;

        public Cell(int x, int y, List<TileData> allTiles)
        {
            this.x = x;
            this.y = y;
            possibleTiles = new List<TileData>(allTiles);
            isCollapsed = false;
        }

        public int Entropy => possibleTiles.Count; // The lower the entropy, higher priority on Generation Algorithm
    }
}