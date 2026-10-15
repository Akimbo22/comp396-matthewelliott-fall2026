using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.WFC
{
    public class WaveFunctionCollapseGenerator : MonoBehaviour
    {
        [Header("Grid Config")]
        [SerializeField] private int width = 10;
        [SerializeField] private int height = 10;
        [SerializeField] private int cellWidth = 5;
        [SerializeField] private int cellHeight = 5;

        [Header("Tile Set")] 
        [SerializeField] private List<TileData> allTiles;

        private Cell[,] grid;
        private List<GameObject> spawnedTiles = new List<GameObject>();

        private void Start()
        {
            ClearMap();
            InitializeGrid();
            //int maxIteration = width * height * 10;
            //int iteration = 0;
            //while (iteration < maxIteration)
            //{
            //    iteration++;
            //    Cell cellToCollapse = GetCellWithLowestEntropy();
            //    if (cellToCollapse == null) { continue; }

            //    CollapseCell(cellToCollapse);
            //    Propagate(cellToCollapse);
            //}
            ////SpawnGameObjects();
            SpawnMapOverTime();
        }

        private void SpawnMapOverTime()
        {
            //yield return new WaitForEndOfFrame();
            int maxIteration = width * height * 10;
            int iteration = 0;
            while (iteration < maxIteration)
            {
                iteration++;
                Cell cellToCollapse = GetCellWithLowestEntropy();
                if (cellToCollapse == null) { continue; }

                CollapseCell(cellToCollapse);
                Propagate(cellToCollapse);
                //yield return new WaitForSeconds(0.5f);
            }
        }

        private void InitializeGrid()
        {
            grid = new Cell[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grid[x, y] = new Cell(x, y, allTiles);
                }
            }
        }

        private void ClearMap() 
        {
            for (int i =0; i< spawnedTiles.Count; i++)
            {
                GameObject tile = spawnedTiles[i];
                if(Application.isEditor && !Application.isPlaying)
                {
                    DestroyImmediate(tile);
                }
                else
                {
                    Destroy(tile);
                }
            }
            spawnedTiles.Clear();
        }

        private void SpawnGameObjects() 
        {
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height;y++)
                {
                    Cell cell = grid[x, y];
                    if(cell.possibleTiles.Count == 1)
                    {
                        TileData tile = cell.possibleTiles[0];
                        if (tile.prefab == null) continue;
                        Vector3 position = new Vector3(x * cellWidth, 0f, y * cellHeight);
                        GameObject obj = Instantiate(tile.prefab, position, Quaternion.identity);
                        spawnedTiles.Add(obj);
                    }
                    else if (cell.possibleTiles.Count == 0)
                    {
                        Debug.LogWarning("Contradiction");
                    }
                }
            }
        }

        private Cell GetCellWithLowestEntropy()
        {
            List<Cell> lowestEntropy = new List<Cell>();
            int minEntropy = int.MaxValue;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Cell cell = grid[x, y];
                    if (cell.isCollapsed) { continue; }
                    if(cell.Entropy < minEntropy)
                    {
                        minEntropy = cell.Entropy;
                        lowestEntropy.Clear();
                        lowestEntropy.Add(cell);
                    }
                    else if (cell.Entropy == minEntropy)
                    {
                        lowestEntropy.Add(cell);
                    }
                }
            }

            if (lowestEntropy.Count == 0) return null;

            int randomIndex = Random.Range(0, lowestEntropy.Count);
            Debug.Log($"lowestEntropy Selected - {lowestEntropy[randomIndex].x},{lowestEntropy[randomIndex].y}");

            return lowestEntropy[randomIndex];
        }

        private void CollapseCell(Cell cell)
        {
            if (cell.possibleTiles.Count == 0)
            {
                Debug.LogWarning("Contradiction");
                return;
            }

            int randomIndex = Random.Range(0, cell.possibleTiles.Count);
            TileData selectedTile = cell.possibleTiles[randomIndex];

            cell.possibleTiles.Clear();
            cell.possibleTiles.Add(selectedTile);
            cell.isCollapsed = true;

            TileData tile = cell.possibleTiles[0];
            if (tile.prefab == null) return;
            Vector3 position = new Vector3(cell.x * cellWidth, 0f, cell.y * cellHeight);
            GameObject obj = Instantiate(tile.prefab, position, Quaternion.identity);
            spawnedTiles.Add(obj);
        }

        private void Propagate(Cell startCell)
        {
            Stack<Cell> stack = new Stack<Cell>();
            stack.Push(startCell);

            while (stack.Count > 0)
            {
                Cell current = stack.Pop();

                foreach (Vector2Int direction in GetDirection())
                {
                    Cell neighbour = GetNeighbour(current, direction);

                    if (neighbour == null || neighbour.isCollapsed) continue;

                    bool optionsRemoved = ConstraintNeighbour(current, neighbour, direction);
                    if(!optionsRemoved) continue;

                    if(neighbour.Entropy == 1)
                    {
                        neighbour.isCollapsed = true;
                    }

                    stack.Push(neighbour);
                }
            }
        }

        private Vector2Int[] GetDirection()
        {
            return new Vector2Int[] {Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};
        }

        private Cell GetNeighbour(Cell current, Vector2Int direction) // Direction means left (-1,0), up (0,1), right (1,0), down (0,-1)
        {
            int neighbourX = current.x + direction.x;
            int neighbourY = current.y + direction.y;

            if(neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
            {
                return grid[neighbourX, neighbourY];
            }

            return null;
        }

        private bool ConstraintNeighbour(Cell current, Cell neighbour, Vector2Int direction)
        {
            bool optionsRemoved = false;

            List<TileData> allowedNeighbour = new List<TileData>();
            foreach(TileData possibleTile in current.possibleTiles)
            {
                List<TileData> validDirection = GetValidTilesInDirection(possibleTile, direction);
                foreach (TileData tile in validDirection)
                {
                    if(!allowedNeighbour.Contains(tile))
                    {
                        allowedNeighbour.Add(tile);
                    }
                }
            }

            for (int i = neighbour.possibleTiles.Count - 1; i >= 0; i--)
            {
                TileData candidate = neighbour.possibleTiles[i];
                if (allowedNeighbour.Contains(candidate)) continue;
                neighbour.possibleTiles.RemoveAt(i);
                optionsRemoved = true;
            }

            return optionsRemoved;
        }

        private List<TileData> GetValidTilesInDirection(TileData tile, Vector2Int direction)
        {
            if (direction == Vector2Int.up) return tile.validTop;
            if (direction == Vector2Int.down) return tile.validBottom;
            if (direction == Vector2Int.left) return tile.validLeft;
            if (direction == Vector2Int.right) return tile.validRight;

            return null;
        }
    }
}