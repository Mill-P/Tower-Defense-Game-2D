using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class MapManager : MonoBehaviour
{
    
    public int mapSize = 15;
    public int tileSize = 10;
    public int gridOffset = 1;

    [SerializeField] 
    private Transform groundTile;
    [SerializeField] 
    private Transform waterTile;
    [SerializeField] 
    private Transform mountainTile;
    [SerializeField] 
    private Transform treeTile;

    [SerializeField]
    private int walkableChance = 85;

    private MapTile[,] grid;
    private List<Building> buildings = new List<Building>();

    private GameObject mapTilesGroup;
    private GameObject buildingTilesGroup;

    public UnityAction mapManagerReady;

    // Start is called before the first frame update
    void Start()
    {
        this.mapSize = GameObject.Find("passingObj")?.GetComponent<PassMapSize>()?.mapSize ?? this.mapSize;
        this.grid = new MapTile[this.mapSize, this.mapSize];
        this.mapTilesGroup = this.transform.Find("MapTiles").gameObject;
        this.buildingTilesGroup = this.transform.Find("BuildingTiles").gameObject;
        this.createGrid();

        this.mapManagerReady?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Finds a path between start and end.
    /// </summary>
    /// <param name="startIndex">Start position in GridIndex</param>
    /// <param name="endIndex">End position in GridIndex</param>
    /// <returns>Shortest path in GridIndexes</returns>
    public List<Vector2> findPath(Vector2 startIndex, Vector2 endIndex, Unit unit = null)
    {
        bool isCreativeUnit = unit != null && unit.GetType() == typeof(CreativeUnit);
        if (isCreativeUnit) Debug.Log("Működik a CreativeUnit");

        PathFindingTile startNode = new PathFindingTile(startIndex.x, startIndex.y, true);
        PathFindingTile endNode = new PathFindingTile(endIndex.x, endIndex.y, true);

        List<PathFindingTile> allTiles = new List<PathFindingTile>();

        if (startNode == null || endNode == null)
        {
            // Invalid Path
            return null;
        }

        List<PathFindingTile> openList = new List<PathFindingTile> { startNode };
        List<PathFindingTile> closedList = new List<PathFindingTile>();

        for (int x = 0; x < this.mapSize; x++)
        {
            for (int y = 0; y < this.mapSize; y++)
            {
                PathFindingTile pathNode = new PathFindingTile(x, y, this.isWalkableAtIndex(x, y) || isCreativeUnit);
                pathNode.gCost = 99999999;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
                allTiles.Add(pathNode);
            }
        }

        startNode.gCost = 0;
        startNode.hCost = 0; //(int)Mathf.Min(Mathf.Abs(startIndex.x - endIndex.x), Mathf.Abs(startIndex.y - endIndex.y)) * (int)(Mathf.Abs(startIndex.x - endIndex.x) - Mathf.Abs(startIndex.y - endIndex.y));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            int minFCost = openList.Min((pathFindingTile) => { return pathFindingTile.fCost; });
            PathFindingTile currentNode = openList.Find((pathFindingTile) => { return pathFindingTile.fCost == minFCost; }) ?? openList[0];
            if (currentNode.x == endNode.x && currentNode.y == endNode.y)
            {
                List<PathFindingTile> path = new List<PathFindingTile>();
                path.Add(currentNode);
                PathFindingTile prevNode = currentNode;
                while (prevNode.cameFromNode != null)
                {
                    path.Add(prevNode.cameFromNode);
                    prevNode = prevNode.cameFromNode;
                }
                path.Reverse();
                //Debug.Log($"PathLength between Castles: {path.Count}");
                return path.Select((pathFindingTile) => { return new Vector2(pathFindingTile.x, pathFindingTile.y); }).ToList();
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            List<PathFindingTile> neighbourList = new List<PathFindingTile>();
            int[][] gridOffsets = new int[][] { new int[] { 0, -1 }, new int[] { -1, 0 }, new int[] { 1, 0 }, new int[] { 0, 1 } };
            for (int i = 0; i < gridOffsets.Length; i++)
            {
                MapTile mapTile = this.getMapTileAtGridIndex((int)currentNode.x + gridOffsets[i][0], (int)currentNode.y + gridOffsets[i][1]);
                if (mapTile != null)
                {
                    Vector2 mapTilePosition = this.getGridIndexAtWorldCoordinate(mapTile.position.x, mapTile.position.y);
                    PathFindingTile pf = allTiles.Find((pathFindingTile) => { return pathFindingTile.x == mapTilePosition.x && pathFindingTile.y == mapTilePosition.y; });
                    if (pf != null)
                    {
                        neighbourList.Add(pf);
                    }
                }
            }

            foreach (PathFindingTile neighbourNode in neighbourList)
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable && (neighbourNode.x != endNode.x || neighbourNode.y != endNode.y))
                {
                    closedList.Add(neighbourNode);
                    // MapTile mt = this.getMapTileAtGridIndex((int)neighbourNode.x, (int)neighbourNode.y);
                    // SpriteRenderer sr = mt.gameObject.transform.Find($"{mt.gameObject.transform.GetChild(0).name}/Square")?.GetComponent<SpriteRenderer>();
                    // if (sr != null) sr.color = Color.black;
                    continue;
                }

                int tentativeGCost = currentNode.gCost + ((int)Mathf.Min(Mathf.Abs(currentNode.x - neighbourNode.x), Mathf.Abs(currentNode.y - neighbourNode.y)) * (int)(Mathf.Abs(currentNode.x - neighbourNode.x) - Mathf.Abs(currentNode.y - neighbourNode.y)));
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = 0; // (int)Mathf.Min(Mathf.Abs(neighbourNode.x - endIndex.x), Mathf.Abs(neighbourNode.y - endIndex.y)) * (int)(Mathf.Abs(neighbourNode.x - endIndex.x) - Mathf.Abs(neighbourNode.y - endIndex.y));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        // MapTile mt = this.getMapTileAtGridIndex((int)neighbourNode.x, (int)neighbourNode.y);
                        // mt.gameObject.transform.Find($"{mt.gameObject.transform.GetChild(0).name}/Square").GetComponent<SpriteRenderer>().color = Color.yellow;
                        openList.Add(neighbourNode);
                    }
                }
                /* else
                {
                    MapTile mt = this.getMapTileAtGridIndex((int)neighbourNode.x, (int)neighbourNode.y);
                    mt.gameObject.transform.Find($"{mt.gameObject.transform.GetChild(0).name}/Square").GetComponent<SpriteRenderer>().color = Color.red;
                } */
            }
        }

        // Out of nodes on the openList
        return null;

    }

    /// <summary>
    /// Creates the grid and places random MapTiles
    /// </summary>
    public void createGrid()
    {
        if (this.buildings.Count > 0)
        {
            for (int i = 0; i < this.buildings.Count; i++)
            {
                Destroy(this.buildings[i].gameObject);
            }
            this.buildings.Clear();
        }
        for (int i = 0; i < this.mapSize; i++)
        {
            for (int j = 0; j < this.mapSize; j++)
            {
                if (this.grid[i, j] != null)
                {
                    Destroy(this.grid[i, j].gameObject);
                    this.grid[i, j] = null;
                }
                MapTile newTile = new MapTile(new Vector2((i + this.gridOffset) * this.tileSize, (j + this.gridOffset) * this.tileSize), Random.value * 100 < this.walkableChance);
                newTile.gameObject.transform.SetParent(this.mapTilesGroup.transform);
                this.grid[i, j] = newTile;
                Instantiate(this.getTileTypeTransform(newTile), newTile.gameObject.transform);
            }
        }
    }

    /// <summary>
    /// Returns true if GridIndex contains castle or Barrack
    /// </summary>
    /// <param name="gridIndex">Searched Index</param>
    /// <param name="player">Player that owns the buildings</param>
    /// <returns>True, if unit is placeable there</returns>
    public bool isUnitPlaceableAtGridIndex(Vector2 gridIndex, TDPlayer player)
    {
        for (int i = 0; i < player.buildings.Count; i++)
        {
            Vector2 buildingGridIndex = this.getGridIndexAtWorldCoordinate(player.buildings[i].position.x, player.buildings[i].position.y);
            if (buildingGridIndex.x == gridIndex.x && buildingGridIndex.y == gridIndex.y)
            {
                if (player.buildings[i].GetType() == typeof(Barrack) || this.buildings[i].GetType() == typeof(Castle)) 
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Converts World Coordinates to Grid Indexes
    /// </summary>
    /// <param name="x">X position in World Coordinate</param>
    /// <param name="y">Y position in World Coordinate</param>
    /// <returns>Grid Index unless out of grid</returns>
    public Vector2 getGridIndexAtWorldCoordinate(float x, float y)
    {
        if (x % 10 >= 5) x += 10 - (x % 10);
        if (y % 10 >= 5) y += 10 - (y % 10);
        int xIndex = Mathf.RoundToInt(x) / this.tileSize - this.gridOffset;
        int yIndex = Mathf.RoundToInt(y) / this.tileSize - this.gridOffset;

        if (xIndex < 0 || xIndex >= this.mapSize || yIndex < 0 || yIndex >= this.mapSize) throw new System.IndexOutOfRangeException();

        return new Vector2(xIndex, yIndex);
    }

    /// <summary>
    /// Returns Vector2 World Coordinates of any MapTile at indexes
    /// </summary>
    /// <param name="xIndex">xIndex on the Grid</param>
    /// <param name="yIndex">yIndex on the Grid</param>
    /// <returns>Vector2 position of the middle of the tile</returns>
    public Vector2 getWorldCoordinateAtGridIndex(int xIndex, int yIndex)
    {
        return this.getMapTileAtGridIndex(xIndex, yIndex)?.position ?? throw new System.Exception("Ilyen GridIndex alatt nincs semmi.");
    }

    /// <summary>
    /// Returns true if MapTile is walkable and no buildings are placed at index
    /// </summary>
    /// <param name="xIndex">xIndex on the grid</param>
    /// <param name="yIndex">yIndex on the grid</param>
    /// <returns>Boolean indicating if the tile is walkable(buildable as well) or not</returns>
    public bool isWalkableAtIndex(int xIndex, int yIndex)
    {
        if (xIndex < 0 || xIndex >= this.mapSize || yIndex < 0 || yIndex >= this.mapSize) return false;
        for (int i = 0; i < this.buildings.Count; i++)
        {
            Vector2 gridIndex = this.getGridIndexAtWorldCoordinate(this.buildings[i].position.x, this.buildings[i].position.y);
            if (gridIndex.x == xIndex && gridIndex.y == yIndex) return false;
        }
        return this.grid[xIndex, yIndex].isWalkable;
    }

    /// <summary>
    /// Sets MapTiles to walkable at position in given radius
    /// </summary>
    /// <param name="gridIndex">Vector2 of center of area which is to be cleared</param>
    /// <param name="radius">Radius in which the tiles are cleared</param>
    public void clearGrid(Vector2 gridIndex, int radius = 2)
    {
        for (int i = (int)gridIndex.x - radius; i <= (int)gridIndex.x + radius; i++)
        {
            for (int j = (int)gridIndex.y - radius; j <= (int)gridIndex.y + radius; j++)
            {
                if (i > -1 && i < this.mapSize && j > -1 && j < this.mapSize)
                {
                    Destroy(this.grid[i, j].gameObject);
                    this.grid[i, j] = new MapTile(new Vector2((i + this.gridOffset) * this.tileSize, (j + this.gridOffset) * this.tileSize), true);
                    this.grid[i, j].gameObject.transform.SetParent(this.mapTilesGroup.transform);
                    //Debug.Log($"Cleared MapTile at {this.grid[i, j].position.x}, {this.grid[i, j].position.y}");
                    Instantiate(this.getTileTypeTransform(this.grid[i,j]), this.grid[i, j].gameObject.transform);
                }
            }
        }
    }

    /// <summary>
    /// Finds Gridindexes where a tower could attack
    /// </summary>
    /// <param name="gridIndex">Center</param>
    /// <param name="radius">Radius</param>
    /// <returns>Returns Gridindexes where a tower could attack</returns>
    public List<Vector2> towerAttackRange(Vector2 gridIndex, int radius)
    {
        List<Vector2> possibleIndexes = new List<Vector2>();
        for (int i = (int)gridIndex.x - radius; i <= (int)gridIndex.x + radius; i++)
        {
            for (int j = (int)gridIndex.y - radius; j <= (int)gridIndex.y + radius; j++)
            {
                if (i > -1 && i < this.mapSize && j > -1 && j < this.mapSize)
                {
                    possibleIndexes.Add(new Vector2(i, j));
                }
            }
        }
        return possibleIndexes;
    }

    /// <summary>
    /// Returns GridIndexes where a player can place buildings
    /// </summary>
    /// <param name="player">Player to look for</param>
    /// <param name="radius">Radius of center and other buildings in which the player can build</param>
    /// <returns>List of Indexes where a player can build</returns>
    public List<Vector2> getBuildibleGridIndexesForPlayer(TDPlayer player, int radius = 2)
    {
        List<Vector2> possibleIndexes = new List<Vector2>();
        for (int i = 0; i < player.buildings.Count; i++)
        {
            Vector2 gridIndex = this.getGridIndexAtWorldCoordinate(player.buildings[i].position.x, player.buildings[i].position.y);
            for (int j = (int)gridIndex.x - radius; j <= (int)gridIndex.x + radius; j++)
            {
                for (int k = (int)gridIndex.y - radius; k <= (int)gridIndex.y + radius; k++)
                {
                    if (j > -1 && j < this.mapSize && k > -1 && k < this.mapSize)
                    {
                        if (this.isWalkableAtIndex(j, k) && !possibleIndexes.Exists((pe) => { return (int)pe.x == j && (int)pe.y == k; }))
                        {
                            possibleIndexes.Add(new Vector2(j, k));
                        }
                    }
                }
            }
        }
        return possibleIndexes;
    }

    /// <summary>
    /// Adds the building to the list of buildings on the map and sets the correct parent for it
    /// </summary>
    /// <param name="building">The building to be added to the list</param>
    public void addBuilding(Building building)
    {
        this.buildings.Add(building);
        building.gameObject.transform.SetParent(this.buildingTilesGroup.transform);
    }

    /// <summary>
    /// Removes a building from the map
    /// </summary>
    /// <param name="building">Building to remove</param>
    public void removeBuilding(Building building)
    {
        this.buildings.Remove(building);
    }

    /// <summary>
    /// Gets MapTile at world coordinates
    /// </summary>
    /// <param name="x">x World Coordinate of MapTile</param>
    /// <param name="y">y World Coordinate of MapTile</param>
    /// <returns>Maptile</returns>
    public MapTile getMapTileAtWorldCoordinate(float x, float y)
    {
        if (x % 10 >= 5) x += 10 - (x % 10);
        if (y % 10 >= 5) y += 10 - (y % 10);
        int xIndex = Mathf.RoundToInt(x) / this.tileSize - this.gridOffset;
        int yIndex = Mathf.RoundToInt(y) / this.tileSize - this.gridOffset;

        if (xIndex < 0 || xIndex >= this.mapSize || yIndex < 0 || yIndex >= this.mapSize) return null;

        return this.grid[xIndex, yIndex];
    }

    /// <summary>
    /// Gets MapTile at Grid Index
    /// </summary>
    /// <param name="xIndex">xIndex of MapTile</param>
    /// <param name="yIndex">yIndex of MapTile</param>
    /// <returns>MapTile</returns>
    public MapTile getMapTileAtGridIndex(int xIndex, int yIndex)
    {
        if (xIndex < 0 || xIndex >= this.mapSize || yIndex < 0 || yIndex >= this.mapSize) return null;

        return this.grid[xIndex, yIndex];
    }

    /// <summary>
    /// Returns the GameObject.Transform (in this case, a prefab) for the given mapTile
    /// </summary>
    /// <param name="mapTile">mapTile to get the Transform</param>
    /// <returns>Transform (prefab)</returns>
    private Transform getTileTypeTransform(MapTile mapTile)
    {
        switch (mapTile.tileType)
        {
            case TileType.GRASS:
                return this.groundTile;
            case TileType.MOUNTAIN:
                return this.mountainTile;
            case TileType.WATER:
                return this.waterTile;
            case TileType.TREE:
                return this.treeTile;
            default:
                return null;
        }
    }
}

public enum TileType
{
    GRASS,
    WATER,
    MOUNTAIN,
    TREE
}

public class MapTile
{
    public Vector2 position { get; private set; }
    public TileType tileType { get; private set; }
    public bool isWalkable { get; set; }

    public GameObject gameObject;

    public MapTile(Vector2 _position, bool _isWalkable)
    {
        this.position = _position;
        this.isWalkable = _isWalkable;

        int rnd = Mathf.FloorToInt(Random.value * 100);

        if (this.isWalkable)
        {
            this.tileType = TileType.GRASS;
        }
        else
        {
            if (rnd > 50)
            {
                if (rnd > 75)
                {
                    this.tileType = TileType.MOUNTAIN;
                }
                else
                {
                    this.tileType = TileType.TREE;
                }
            }
            else
            {
                this.tileType = TileType.WATER;
            }
        }

        this.gameObject = new GameObject($"MapTile [{this.tileType}] ({this.position.x},{this.position.y})");
        this.gameObject.transform.position = new Vector3(this.position.x, this.position.y, 0);
    }
}

public class PathFindingTile
{
    public float x;
    public float y;

    public int gCost;
    public int hCost = 0;
    public int fCost;

    public bool isWalkable;
    public PathFindingTile cameFromNode;

    public PathFindingTile(float _x, float _y, bool _isWalkable)
    {
        this.x = _x;
        this.y = _y;
        this.isWalkable = _isWalkable;
    }
    public void CalculateFCost()
    {
        this.fCost = this.gCost + this.hCost;
    }
}