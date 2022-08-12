using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{

    public int playerCount = 2;

    [SerializeField]
    private int currentPlayerIndex = 0;

    public List<TDPlayer> players;

    private TMP_Text currentPlayerName;
    private TMP_Text currentPlayerGold;
    private TMP_Text currentPlayerUnits;
    private TMP_Text currentPlayerMines;
    private TMP_Text currentPlayerCastle;

    public UnityAction playerManagerReady;
    public TDPlayer currentPlayer
    {
        get
        {
            return this.players[this.currentPlayerIndex];
        }
    }

    public TDPlayer otherPlayer
    {
        get
        {
            return currentPlayerIndex == 0 ? players[1] : players[0];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.players = new List<TDPlayer>();
        for (int i = 0; i < this.playerCount; i++)
        {
            this.players.Add(new TDPlayer($"Player [{i}]"));
        }

        this.currentPlayerName = GameObject.Find("TurnPlayerName").GetComponent<TMP_Text>();
        this.currentPlayerGold = GameObject.Find("PlayerGoldAmount").GetComponent<TMP_Text>();
        this.currentPlayerUnits = GameObject.Find("PlayerUnitsAmount").GetComponent<TMP_Text>();
        this.currentPlayerMines = GameObject.Find("PlayerMinesAmount").GetComponent<TMP_Text>();
        this.currentPlayerCastle = GameObject.Find("PlayerHealthAmount").GetComponent<TMP_Text>();

        this.playerManagerReady?.Invoke();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Gives current round to the next player and refreshes UI with their data
    /// </summary>
    public void nextPlayer()
    {
        this.currentPlayerIndex = this.currentPlayerIndex + 1 == this.playerCount ? 0 : this.currentPlayerIndex + 1;
        this.showCurrentPlayerData();
    }

    /// <summary>
    /// Refreshes UI
    /// </summary>
    public void showCurrentPlayerData()
    {
        this.currentPlayerName.text = this.currentPlayer.name.ToString();
        this.currentPlayerGold.text = this.currentPlayer.goldAmount.ToString();
        this.currentPlayerUnits.text = this.currentPlayer.units.Count.ToString();
        this.currentPlayerMines.text = this.currentPlayer.goldmineCount.ToString();
        this.currentPlayerCastle.text = this.currentPlayer.castle?.health.ToString();
    }

    /// <summary>
    /// Gives gold to player
    /// </summary>
    /// <param name="player">Player to give gold to</param>
    /// <param name="amount">Gold amount to give</param>
    public void giveGold(TDPlayer player, int amount)
    {
        player.goldAmount += amount;
    }

    /// <summary>
    /// Takes gold
    /// </summary>
    /// <param name="player">Player to take gold from</param>
    /// <param name="amount">Amount to take</param>
    public void takeGold(TDPlayer player, int amount)
    {
        player.goldAmount -= amount;
    }



}

public class TDPlayer
{
    public int goldAmount = 50;
    public string name;
    public Castle castle {
        get {
            return (Castle)this.buildings.Find((b) => { return b.GetType() == typeof(Castle); });
        } 
        set {
            int castleIndex = this.buildings.FindIndex((b) => { return b.GetType() == typeof(Castle); });
            if (castleIndex > -1)
            {
                this.buildings[castleIndex] = value;
            }
            else
            {
                this.buildings.Add(value);
            }
        }
    }
    public List<Building> buildings = new List<Building>();
    public List<Unit> units = new List<Unit>();
    public List<Building> towers 
    {
        get 
        {
            return this.buildings.Where((building) => { return building.GetType().IsSubclassOf(typeof(Tower)); }).ToList();
        }    
    }
    public int goldmineCount
    {
        get
        {
            return this.buildings.Where((building) => { return building.GetType() == typeof(GoldMine); }).Count();
        }
    }

    public TDPlayer(string _name)
    {
        this.name = _name;
    }
}


