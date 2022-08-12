using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgrade : MonoBehaviour
{
    public GameObject TowerUpgradePanel;

    private MapManager mapManager;
    private PlayerManager playerManager;
    private GameManager gameManager;

    public TMP_Text towerHealth;
    public TMP_Text towerDamage;
    public TMP_Text towerRange;
    public TMP_Text towerLevel;
    public TMP_Text towerUpgradeCost;

    public Button towerUpgradeButton;

    // Start is called before the first frame update
    void Start()
    {
        this.mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        this.gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        closeTowerUpgradePanel();
    }

    // Update is called once per frame
    Tower tower;
    bool isPanelOpen = false;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isPanelOpen)
        {
            Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();            
            MapTile mt = this.mapManager.getMapTileAtWorldCoordinate(mousePosition.x, mousePosition.y);
            try
            {
                tower = (Tower)playerManager.currentPlayer.towers.Find(x => x.position == mt.position);
            }
            catch { }            
            if (tower != null)
            {
                openTowerUpgradePanel();                
                RefreshTexts();
                isPanelOpen = true;
            }            
        }
    }

    /// <summary>
    /// Upgrades the tower
    /// </summary>
    public void Upgrade()
    {
        if(tower != null && playerManager.currentPlayer.goldAmount >= tower.upgradeCost)
        {
            tower.level++;
            tower.health += 5;
            tower.damage += 2;
            tower.range += 1;
            playerManager.takeGold(playerManager.currentPlayer, tower.upgradeCost);
            tower.upgradeCost = tower.level * tower.upgradeCost;
            playerManager.showCurrentPlayerData();
            RefreshTexts();
        }
        else
        {
            gameManager.popUpSystem.openPopUpBoxGold();
        }
    }


    /// <summary>
    /// Refreshes text
    /// </summary>
    public void RefreshTexts()
    {
        towerHealth.text = tower.health.ToString();
        towerDamage.text = tower.damage.ToString();
        towerRange.text = tower.range.ToString();
        towerUpgradeCost.text = tower.upgradeCost.ToString();
        towerLevel.text = tower.level.ToString();
    }

    /// <summary>
    /// Opens the panel on the right side
    /// </summary>
    public void openTowerUpgradePanel()
    {
        TowerUpgradePanel.SetActive(true);
    }

    /// <summary>
    /// Closes the panel on the right side
    /// </summary>
    public void closeTowerUpgradePanel()
    {
        tower = null;
        isPanelOpen = false;
        TowerUpgradePanel.SetActive(false);        
    }
}
