using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class DragAndDropBehavior : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    //Dani
    private MapManager mapManager;
    private PlayerManager playerManager;
    private GameManager gameManager;

    private enum EntityType
    {
        BARRACK,
        MINE,
        DEADLYFIRETOWER,
        LONGRANGETOWER,
        SIMPLETOWER,
        FIGHTERUNIT,
        HEAVYARMORUNIT,
        HEAVYDAMAGEUNIT,
        BUILDINGDESTROYERUNIT,
        CREATIVEUNIT

    }
    [SerializeField]
    private EntityType entityType;

    void Start()
    {
        this.mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        this.gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
    }

    MapTile lastMapTile;
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        //Debug.Log(mousePosition);
        MapTile mt = this.mapManager.getMapTileAtWorldCoordinate(mousePosition.x, mousePosition.y);
        if (mt == null || lastMapTile == mt) return;
        if (lastMapTile != null)
        {
            lastMapTile.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            lastMapTile = null;
        }
        lastMapTile = mt;
        Vector2 gridIndex = this.mapManager.getGridIndexAtWorldCoordinate(mt.position.x, mt.position.y);
        //Building highlight
        if (entityType is EntityType.BARRACK || entityType is EntityType.MINE || entityType is EntityType.SIMPLETOWER
                || entityType is EntityType.DEADLYFIRETOWER || entityType is EntityType.LONGRANGETOWER)
        {
            List<Vector2> buildingLocations = this.mapManager.getBuildibleGridIndexesForPlayer(this.playerManager.currentPlayer);
            if (buildingLocations.Exists((gridIndexVector) => (int)gridIndexVector.x == (int)gridIndex.x && (int)gridIndexVector.y == (int)gridIndex.y))
            {
                mt.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.6F, 0.6F, 1, 1);
            }
            else
            {
                mt.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 0.27F, 0.27F, 1);
            }
        }
        //Unit highlight
        else
        {
            if (this.mapManager.isUnitPlaceableAtGridIndex(gridIndex, this.playerManager.currentPlayer))
            {
                mt.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.6F, 0.6F, 1, 1);
            }
            else
            {
                mt.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 0.27F, 0.27F, 1);
            }
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (lastMapTile != null)
        {
            lastMapTile.gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            lastMapTile = null;
        }
        //Debug.Log("OnEndDrag");
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        //Debug.Log(mousePosition);
        MapTile mt = this.mapManager.getMapTileAtWorldCoordinate(mousePosition.x, mousePosition.y);
        if (mt == null) return;
        Vector2 gridIndex = this.mapManager.getGridIndexAtWorldCoordinate(mt.position.x, mt.position.y);
        List<Vector2> buildingLocations = this.mapManager.getBuildibleGridIndexesForPlayer(this.playerManager.currentPlayer);
        
        //Place building
        if (buildingLocations.Exists((gridIndexVector) => (int)gridIndexVector.x == (int)gridIndex.x && (int)gridIndexVector.y == (int)gridIndex.y))
        {           
            //megnézni hogy mit rakunk le goldmine, tower, barrack
            switch (entityType)
            {
                case EntityType.BARRACK:
                    //Check gold amount, Goldot levonni, Buildinget lerakni
                    if (playerManager.currentPlayer.goldAmount >= 15)
                    {
                        Barrack barrack = new Barrack(mt.position, playerManager.currentPlayer);
                        if (gameManager.placeBuilding(barrack))
                        {
                            playerManager.takeGold(playerManager.currentPlayer, 15);
                        }
                        else
                        {
                            //Popup nem lehet oda rakni
                            gameManager.popUpSystem.openPopUpBoxBuild();
                            Debug.Log("cannot place here (path)");
                        }                        
                        this.playerManager.showCurrentPlayerData();
                    }
                    else
                    {
                        gameManager.popUpSystem.openPopUpBoxGold();
                        Debug.Log("Not enough gold!");
                    }
                    break;
                case EntityType.MINE:
                    //Check gold amount, Goldot levonni, Buildinget lerakni
                    if (playerManager.currentPlayer.goldAmount >= 25)
                    {
                        GoldMine mine = new GoldMine(mt.position, playerManager.currentPlayer);
                        if (gameManager.placeBuilding(mine))
                        {
                            playerManager.takeGold(playerManager.currentPlayer, 25);
                        }
                        else
                        {
                            //Popup nem lehet oda rakni
                            gameManager.popUpSystem.openPopUpBoxBuild();
                            Debug.Log("cannot place here (path)");
                        }
                        this.playerManager.showCurrentPlayerData();
                    }
                    else
                    {
                        //Popup nincs elég gold
                        gameManager.popUpSystem.openPopUpBoxGold();
                        Debug.Log("Not enough gold!");
                    }
                    break;
                case EntityType.DEADLYFIRETOWER:
                    //Check gold amount, Goldot levonni, Buildinget lerakni
                    if (playerManager.currentPlayer.goldAmount >= 20)
                    {
                        DeadlyFireTower DeadlyFireTower = new DeadlyFireTower(mt.position, playerManager.currentPlayer);
                        if (gameManager.placeBuilding(DeadlyFireTower))
                        {
                            playerManager.takeGold(playerManager.currentPlayer, 20);
                        }
                        else
                        {
                            //Popup nem lehet oda rakni
                            gameManager.popUpSystem.openPopUpBoxBuild();
                            Debug.Log("cannot place here (path)");
                        }
                        this.playerManager.showCurrentPlayerData();
                    }
                    else
                    {
                        //Popup nincs elég gold
                        gameManager.popUpSystem.openPopUpBoxGold();
                        Debug.Log("Not enough gold!");
                    }
                    break;
                case EntityType.SIMPLETOWER:
                    //Check gold amount, Goldot levonni, Buildinget lerakni
                    if (playerManager.currentPlayer.goldAmount >= 15)
                    {
                        SimpleTower simpleTower = new SimpleTower(mt.position, this.playerManager.currentPlayer);
                        if (gameManager.placeBuilding(simpleTower))
                        {
                            playerManager.takeGold(playerManager.currentPlayer, 15);
                        }
                        else
                        {
                            //Popup nem lehet oda rakni
                            gameManager.popUpSystem.openPopUpBoxBuild();
                            Debug.Log("cannot place here (path)");
                        }
                        this.playerManager.showCurrentPlayerData();
                    }
                    else
                    {
                        //Popup nincs elég gold
                        gameManager.popUpSystem.openPopUpBoxGold();
                        Debug.Log("Not enough gold!");
                    }
                    break;
                case EntityType.LONGRANGETOWER:
                    //Check gold amount, Goldot levonni, Buildinget lerakni
                    if (playerManager.currentPlayer.goldAmount >= 15)
                    {
                        LongRangeTower longRangeTower = new LongRangeTower(mt.position, playerManager.currentPlayer);
                        if (gameManager.placeBuilding(longRangeTower))
                        {
                            playerManager.takeGold(playerManager.currentPlayer, 15);
                        }
                        else
                        {
                            //Popup nem lehet oda rakni
                            gameManager.popUpSystem.openPopUpBoxBuild();
                            Debug.Log("cannot place here (path)");
                        }
                        this.playerManager.showCurrentPlayerData();
                    }
                    else
                    {
                        //Popup nincs elég gold
                        gameManager.popUpSystem.openPopUpBoxGold();
                        Debug.Log("Not enough gold!");
                    }
                    break;
                default:
                    break;
            }
        } 
        else if (entityType is EntityType.BARRACK || entityType is EntityType.MINE || entityType is EntityType.SIMPLETOWER
                || entityType is EntityType.DEADLYFIRETOWER || entityType is EntityType.LONGRANGETOWER)
        {
            gameManager.popUpSystem.openPopUpBoxBuild();
        }

        //Place unit
        if (this.mapManager.isUnitPlaceableAtGridIndex(gridIndex, this.playerManager.currentPlayer)) {
            switch (this.entityType)
            {
                case EntityType.FIGHTERUNIT:
                    if (playerManager.currentPlayer.goldAmount >= 10)
                    {
                        FighterUnit newUnit = new FighterUnit(mt.position, this.playerManager.currentPlayer, this.playerManager.otherPlayer.castle);
                        this.gameManager.placeUnit(newUnit);
                        playerManager.takeGold(playerManager.currentPlayer, 10);
                        this.playerManager.showCurrentPlayerData();
                    }
                    else
                    {
                        gameManager.popUpSystem.openPopUpBoxGold();
                    }
                    break;
                case EntityType.HEAVYDAMAGEUNIT:
                    if (playerManager.currentPlayer.goldAmount >= 10)
                    {
                        HeavyDamageUnit newUnit = new HeavyDamageUnit(mt.position, this.playerManager.currentPlayer, this.playerManager.otherPlayer.castle);
                        this.gameManager.placeUnit(newUnit);
                        playerManager.takeGold(playerManager.currentPlayer, 10);
                        this.playerManager.showCurrentPlayerData();
                    }
                    else
                    {
                        gameManager.popUpSystem.openPopUpBoxGold();
                    }
                    break;
                case EntityType.HEAVYARMORUNIT:
                    if (playerManager.currentPlayer.goldAmount >= 15)
                    {
                        HeavyArmorUnit newUnit = new HeavyArmorUnit(mt.position, this.playerManager.currentPlayer, this.playerManager.otherPlayer.castle);
                        this.gameManager.placeUnit(newUnit);
                        playerManager.takeGold(playerManager.currentPlayer, 15);
                        this.playerManager.showCurrentPlayerData();
                    }
                    else
                    {
                        gameManager.popUpSystem.openPopUpBoxGold();
                    }
                    break;
                case EntityType.BUILDINGDESTROYERUNIT:
                    if (playerManager.currentPlayer.goldAmount >= 15)
                    {
                        List<Building> possibleBuildings = this.playerManager.otherPlayer.buildings.FindAll((b) => { return b.GetType() != typeof(Castle); });
                        BuildingDestroyerUnit newUnit;
                        if (possibleBuildings.Count == 0)
                        {
                             newUnit = new BuildingDestroyerUnit(mt.position, this.playerManager.currentPlayer, this.playerManager.otherPlayer.castle);
                        }
                        else
                        {
                            newUnit = new BuildingDestroyerUnit(mt.position, this.playerManager.currentPlayer, possibleBuildings[Random.Range(0, possibleBuildings.Count - 1)]);
                        }
                        this.gameManager.placeUnit(newUnit);
                        playerManager.takeGold(playerManager.currentPlayer, 15);
                        this.playerManager.showCurrentPlayerData();
                    }
                    break;
               case EntityType.CREATIVEUNIT:
                    if (playerManager.currentPlayer.goldAmount >= 15)
                    {
                        CreativeUnit newUnit = new CreativeUnit(mt.position, this.playerManager.currentPlayer, this.playerManager.otherPlayer.castle);
                        this.gameManager.placeUnit(newUnit);
                        playerManager.takeGold(playerManager.currentPlayer, 15);
                        this.playerManager.showCurrentPlayerData();
                    }
                    else
                    {
                        gameManager.popUpSystem.openPopUpBoxGold();
                    }
                    break;
            }
        }
        else if(entityType is EntityType.FIGHTERUNIT || entityType is EntityType.CREATIVEUNIT || entityType is EntityType.HEAVYARMORUNIT
                || entityType is EntityType.HEAVYDAMAGEUNIT || entityType is EntityType.BUILDINGDESTROYERUNIT)
        {
            gameManager.popUpSystem.openPopUpBoxUnit();
        }

        //Debug.Log(mt.position + " || " + mt.isWalkable);

        //FighterUnit newUnit = new FighterUnit(mt.gameObject.transform.position, this.playerManager.currentPlayer, this.playerManager.players.Find((player) => player != this.playerManager.currentPlayer).castle);
        //this.gameManager.placeUnit(newUnit);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
    }
}
