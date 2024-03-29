using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    public GameObject _enemyPrefab;
    [SerializeField] private Transform _cam;
    public Dictionary<Vector2, Tile> _tiles;
    public List<Vector2Int> _path;
    [SerializeField] public Vector2Int _start;
    [SerializeField] public Vector2Int _end;
    private bool hasPath;
    [SerializeField] public int numberOfEnemiesToSpawn = 1;
    private List<Enemy> enemies = new List<Enemy>();

    void Start()
    {
        GenerateGrid();
        FindAndShowShortestPath();
        SpawnEnemies();
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                // If tile at position is the start point, activate the start point object
                if (x == _start.x && y == _start.y)
                {
                    spawnedTile._startPoint.SetActive(true);
                }

                // If tile at position is the end point, activate the end point object
                if (x == _end.x && y == _end.y)
                {
                    spawnedTile._endPoint.SetActive(true);
                }

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);

    }

    /**
     * Params: Dictionary<Vector2, Tile> tiles, Vector2Int start, Vector2Int end
     * The method finds the shortest path from a given start to a given end
     * and shows the path on the grid of tiles
     * by changing the color of the tiles in the path.
     * So it does not draw the entire map with the path, but only the path.
     */
    public void FindAndShowShortestPath()
    {


        AStarNode[,] grid = convertTileMapToAStarNodes();

        _path = AStarPathfinding.FindPath(grid, _start, _end);

        if (_path != null)
        {
            hasPath = true;
            if (_path.Count == 0)
            {
                hasPath = false;
            }

            wipeCurrentPath();
            setNewPath();
        }
        else
        {
            hasPath = false;
            Console.WriteLine("No path found");
        }
    }

public AStarNode[,] convertTileMapToAStarNodes()
{
    int width = _width;
    int height = _height;

    AStarNode[,] nodes = new AStarNode[width, height];

    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            Tile tile = GetTileAtPosition(new Vector2Int(x, y));
            nodes[x, y] = new AStarNode
            {
                isWalkable = tile.isWalkable,
                position = new Vector2Int(x, y)
            };
        }
    }
    return nodes;
}

    public void wipeCurrentPath()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _tiles[new Vector2(x, y)]._path.SetActive(false);
            }
        }
    }

    public void setNewPath()
    {
        int[,] gridPattern = new int[_width, _height];
        // set new path
        foreach (Vector2Int Coord in _path)
        {
            gridPattern[Coord.x, Coord.y] = 2;
        }
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (gridPattern[x, y] == 2)
                {
                    _tiles[new Vector2(x, y)].setTileAsCurrentPath();
                }
            }
        }
    }

    public void FindAndShowShortestPathOnClick()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridPosition = new Vector2Int(Mathf.FloorToInt(mousePosition.x), Mathf.FloorToInt(mousePosition.y));
        Tile tile = GetTileAtPosition(gridPosition);
        if (tile != null)
        {
            FindAndShowShortestPath();
            if (!hasPath)
            {
                tile._SetBlock.SetActive(false);
                tile.isWalkable = true;
                FindAndShowShortestPath();

                // Show the cannot set block for a short time
                StartCoroutine(DeactivateCannotSetBlock(tile._CannotSetBlock));
                IEnumerator DeactivateCannotSetBlock(GameObject cannotSetBlock)
                {
                    cannotSetBlock.SetActive(true); // Activate the GameObject
                    yield return new WaitForSeconds(0.1f); // Wait for the specified delay
                    cannotSetBlock.SetActive(false); // Deactivate the GameObject
                }
            }
        }
        else
        {
            Debug.Log("Tile not found");
            Debug.Log(gridPosition);
        }
    }
    

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            GameObject enemyInstance = Instantiate(_enemyPrefab, GetTileAtPosition(_start).transform.position, Quaternion.identity);
            Enemy enemy = enemyInstance.GetComponent<Enemy>();
            enemies.Add(enemy);
        }
    }


}