using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int castleStartHealth = 100;

    private PlayerManager playerManager;
    private bool playerManagerReady = false;

    private MapManager mapManager;
    private bool mapManagerReady = false;

    private TMP_Text turnCountText;

    private int currentRound = 0;

    public PopUpSystem popUpSystem;
    public TowerUpgrade towerUpgradeSystem;


    // Start is called before the first frame update
    void Start()
    {
        this.mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        this.playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        this.mapManager.mapManagerReady += this.mapManagerReadyEvent;
        this.playerManager.playerManagerReady += this.playerManagerReadyEvent;

        this.turnCountText = GameObject.Find("TurnCount").GetComponent<TMP_Text>();

        this.popUpSystem = GameObject.Find("PopUpSystem").GetComponent<PopUpSystem>();
        this.popUpSystem.closeAll();

        Debug.Log(currentRound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Places the Castles of players at random positions, and clears the grid around them
    /// </summary>
    private void placeCastles()
    {
        for (int i = 0; i < this.playerManager.playerCount; i++)
        {
            if (this.playerManager.players[i].castle != null)
            {
                this.playerManager.players[i].buildings.Remove(this.playerManager.players[i].castle);
            }
            int x = Random.Range(0, this.mapManager.mapSize - 1);
            int y = Random.Range(i % 2 == 0 ? this.mapManager.mapSize - 2 : 0, i % 2 == 0 ? this.mapManager.mapSize - 1 : 1);
            Vector2 castlePosition = this.mapManager.getWorldCoordinateAtGridIndex(x, y);
            this.mapManager.clearGrid(this.mapManager.getGridIndexAtWorldCoordinate(castlePosition.x, castlePosition.y));
            Castle newCastle = new Castle(castlePosition, this.playerManager.players[i]);
            newCastle.health = this.castleStartHealth;
            this.placeBuilding(newCastle);
            newCastle.entityDeadEvent += delegate { this.endGame(newCastle.owner); };
        }
    }


    /// <summary>
    /// Places a unit on the map
    /// </summary>
    /// <param name="unit">Unit to be removed</param>
    public void placeUnit(Unit unit)
    {
        unit.owner.units.Add(unit);
        unit.calculatePath(this.mapManager);
        Instantiate(unit.getTransform(), unit.gameObject.transform);
        unit.refreshHealthText();
        unit.entityDeadEvent += delegate { this.removeUnit(unit); };
    }

    /// <summary>
    /// Removes a unit from the game
    /// </summary>
    /// <param name="unit">Unit to be removed</param>
    public void removeUnit(Unit unit)
    {
        unit.owner.units.Remove(unit);
        Destroy(unit.gameObject);
    }

    /// <summary>
    /// Places a building
    /// </summary>
    /// <param name="building">Building to be placed</param>
    /// <returns>True, if all units have their own route</returns>
    public bool placeBuilding(Building building)
    {
        building.owner.buildings.Add(building);
        this.mapManager.addBuilding(building);
        Castle castleP1 = this.playerManager.players[0].castle;
        Castle castleP2 = this.playerManager.players[1].castle;

        bool hasRoute = castleP1 != null && castleP2 != null ? this.mapManager.findPath(this.mapManager.getGridIndexAtWorldCoordinate(castleP1.position.x, castleP1.position.y), this.mapManager.getGridIndexAtWorldCoordinate(castleP2.position.x, castleP2.position.y)) != null : true;
        for (int i = 0; i < this.playerManager.players.Count && hasRoute; i++)
        {
            for (int j = 0; j < this.playerManager.players[i].units.Count && hasRoute; j++)
            {
                hasRoute = hasRoute && this.playerManager.players[i].units[j].calculatePath(this.mapManager);
            }
        }
        if (!hasRoute)
        {
            building.owner.buildings.Remove(building);
            this.mapManager.removeBuilding(building);
            Destroy(building.gameObject);
            return false;
        }
        Instantiate(building.getTransform(), building.gameObject.transform);
        building.entityDeadEvent += delegate { this.removeBuilding(building); };
        return true;
    }

    /// <summary>
    /// Removes a building from the game
    /// </summary>
    /// <param name="building">Building to be removed</param>
    public void removeBuilding(Building building)
    {
        building.owner.buildings.Remove(building);
        this.mapManager.removeBuilding(building);
        Destroy(building.gameObject);
    }

    /// <summary>
    /// Event Fired after playerManager Start() method finished running.
    /// </summary>
    private void playerManagerReadyEvent()
    {
        //Debug.Log("playerManagerReady");
        this.playerManagerReady = true;

        if (this.playerManagerReady && this.mapManagerReady)
        {
            this.startGame();
        }
    }

    /// <summary>
    /// Event Fired after mapManager Start() method finished running.
    /// </summary>
    private void mapManagerReadyEvent()
    {
        //Debug.Log("mapManagerReady");
        this.mapManagerReady = true;

        if (this.playerManagerReady && this.mapManagerReady)
        {
            this.startGame();
        }
    }

    /// <summary>
    /// Starts the game by placing the castles
    /// </summary>
    private void startGame()
    {
        this.placeCastles();
        this.currentRound++;
        this.turnCountText.text = this.currentRound.ToString();
        this.playerManager.showCurrentPlayerData();
        Castle castleP1 = this.playerManager.players[0].castle;
        Castle castleP2 = this.playerManager.players[1].castle;
        if (castleP1 != null && castleP2 != null)
        {
            List<Vector2> routeBetweenCastles = this.mapManager.findPath(this.mapManager.getGridIndexAtWorldCoordinate(this.playerManager.players[0].castle.position.x, this.playerManager.players[0].castle.position.y), this.mapManager.getGridIndexAtWorldCoordinate(this.playerManager.players[1].castle.position.x, this.playerManager.players[1].castle.position.y));
            if (routeBetweenCastles == null)
            {
                this.currentRound = 0;
                this.mapManager.createGrid();
                this.startGame();
                Debug.Log("Found no path between Castles! Creating a new Grid...");
            }
        }
        else
        {
            this.currentRound = 0;
            this.mapManager.createGrid();
            this.startGame();
            Debug.Log("Found no path between Castles! Creating a new Grid...");
        }
    }

    /// <summary>
    /// Shows endgame message
    /// </summary>
    /// <param name="lostPlayer">Player that lost</param>
    private void endGame(TDPlayer lostPlayer)
    {
        popUpSystem.openPopUpBoxEnd();
    }

    /// <summary>
    /// Starts the current roun by giving gold to players
    /// </summary>
    public void startRound()
    {
        //Debug.Log(currentRound);
        TDPlayer currentPlayer = playerManager.currentPlayer;
        
        if (this.currentRound != 2)
        {
            //Castle gold +5        
            playerManager.giveGold(currentPlayer, 5);
            //Goldmine +10
            playerManager.giveGold(currentPlayer, currentPlayer.goldmineCount*10);
            
            //Debug.Log($"{this.playerManager.currentPlayer.name}'s gold: {this.playerManager.currentPlayer.goldAmount}");
            this.playerManager.showCurrentPlayerData();
        }
    }

    /// <summary>
    /// Returns unit in radius of given position
    /// </summary>
    /// <param name="position">Center position</param>
    /// <param name="radius">Radius to search</param>
    /// <param name="player">Which player's units to search</param>
    /// <returns>List of units that are in radius of given position</returns>
    public List<Unit> getUnitsInRange(Vector2 position, int radius, TDPlayer player)
    {        
        List<Unit> units = new List<Unit>();
        foreach (var unit in player.units)
        {
            Vector2 unitGridIndex = mapManager.getGridIndexAtWorldCoordinate(unit.position.x, unit.position.y);
            if ( Mathf.Abs((int)position.x - (int)unitGridIndex.x)  <= radius && Mathf.Abs((int)position.y - (int)unitGridIndex.y) <= radius)
            {
                units.Add(unit);
            }
        }
        return units;
    }

    /// <summary>
    /// Ends the current round and simulates it, gives the round to player
    /// </summary>
    public void endRound()
    {
        towerHighlightRemove();

        towerUpgradeSystem.closeTowerUpgradePanel();
        TDPlayer currentPlayer = this.playerManager.currentPlayer;
        //Kör vége szimulálása (Minden a current player ilyenkor)
        //1. Egységek lépnek
        foreach (Unit unit in currentPlayer.units)
        {
            unit.moveForward(this.mapManager);
        }
        //2. Egység támad
        List<Unit> deadUnits = new List<Unit>();
        foreach (Unit unit in currentPlayer.units)
        {
            if (unit.position == unit.target.position)
            {
                unit.attack(unit.target);
                deadUnits.Add(unit);
            }
        }
        foreach (Unit unit in deadUnits)
        {
            unit.takeDamage(unit.health);
        }
        Debug.Log($"{currentPlayer.name}'s towers.Count: " + currentPlayer.towers.Count);
        //3. Tornyok támadnak
        foreach (Tower tower in currentPlayer.towers)
        {
            List<Unit> attackableUnits = getUnitsInRange(mapManager.getGridIndexAtWorldCoordinate(tower.position.x, tower.position.y), tower.range, playerManager.otherPlayer);
            Debug.Log($"{currentPlayer.name}'s attackableUnits.Count:" + attackableUnits.Count);
            foreach (Unit unit in attackableUnits)
            {
                tower.attack(unit);
                unit.refreshHealthText();
            }
            if (attackableUnits.Count > 0)
            {
                towerHighlight(tower.position, tower.range, attackableUnits);
            }
        }

        this.currentRound++;
        this.turnCountText.text = this.currentRound.ToString();
        //Váltás a masik játékosra
        this.playerManager.nextPlayer();        
        //A másik játékos körének az elkezdése        
        startRound();
        
    }

    /// <summary>
    /// Highlights MapTiles under units that can be attacked
    /// </summary>
    /// <param name="worldPosition">Worldposition of tower that attacks</param>
    /// <param name="range">Range of tower</param>
    /// <param name="attackableUnits">List of units that can be attacked if in range</param>
    public void towerHighlight(Vector2 worldPosition, int range, List<Unit> attackableUnits)
    {
        Vector2 towerGridIndex = mapManager.getGridIndexAtWorldCoordinate(worldPosition.x, worldPosition.y);
        List<Vector2> highlightTiles = mapManager.towerAttackRange(towerGridIndex,range);
        List<Vector2> unitTiles = new List<Vector2>();
        foreach (Unit unit in attackableUnits)
        {
            Vector2 unitGridIndex = mapManager.getGridIndexAtWorldCoordinate(unit.position.x, unit.position.y);
            unitTiles.Add(unitGridIndex);
        }

        foreach (Vector2 unitPos in unitTiles)
        {
            if (highlightTiles.Contains(unitPos))
            {
                MapTile mt = this.mapManager.getMapTileAtGridIndex((int)unitPos.x, (int)unitPos.y);
                if (mt != null)
                {
                    mt.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 0.27F, 0.27F, 1);
                }
            }
        }
        
        //foreach (Vector2 pos in highlightTiles)
        //{
        //    MapTile mt = this.mapManager.getMapTileAtGridIndex((int)pos.x, (int)pos.y);
        //    if (mt != null)
        //    {
        //        mt.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 0.27F, 0.27F, 1);
        //    }
        //}
    }

    /// <summary>
    /// Removes the highlight
    /// </summary>
    public void towerHighlightRemove()
    { 
        List<Building> currentTowers = playerManager.otherPlayer.towers;
        foreach (Tower tower in currentTowers)
        {
            Vector2 towerGridIndex = mapManager.getGridIndexAtWorldCoordinate(tower.position.x, tower.position.y);
            List<Vector2> highlightTiles = mapManager.towerAttackRange(towerGridIndex, tower.range);
            foreach (Vector2 pos in highlightTiles)
            {
                MapTile mt = this.mapManager.getMapTileAtGridIndex((int)pos.x, (int)pos.y);
                if (mt != null)
                {
                    mt.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
    }

}
