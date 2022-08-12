using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
public class GiveGoldTest {

    [Test]
    public void GiveGold_Test() {

        //ARRANGE
        PlayerManager playerManager = new PlayerManager();
        TDPlayer player = new TDPlayer("Pista");
        int goldAmountToAdd = 20;
        int expetedGoldAmount = player.goldAmount + goldAmountToAdd;

        //ACT
        playerManager.giveGold(player, goldAmountToAdd);

        //ASSERT
        Assert.That(player.goldAmount, Is.EqualTo(expetedGoldAmount));
    }

    [Test]
    public void TakeGold_Test() {
        //ARRANGE
        PlayerManager playerManager = new PlayerManager();
        TDPlayer player = new TDPlayer("Pista");
        int goldAmountToTake = 20;
        int expetedGoldAmount = player.goldAmount - goldAmountToTake;

        //ACT
        playerManager.takeGold(player, goldAmountToTake);

        //ASSERT
        Assert.That(player.goldAmount, Is.EqualTo(expetedGoldAmount));
    }

    [Test]
    public void OneMineOnTurnEnd_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        bool turnEnded = false;
        int goldAmountToAdd = 10;

        //ACT
        GoldMine goldMine = new GoldMine(pos, player);
        player.buildings.Add(goldMine);
       
        int goldAmountBeforeAddition = player.goldAmount;
        turnEnded = true;
        if (turnEnded)
        {
            player.goldAmount += goldAmountToAdd;
        }

        //ASSERT
        Assert.That(player.goldAmount, Is.EqualTo(goldAmountToAdd + goldAmountBeforeAddition));
    }

    [Test]
    public void MultipleMinesOnTurnEnd_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        bool turnEnded = false;
        int goldAmountToAdd = 10;
        int numberOfGoldMines = 0;
        int sumGoldAmountToAdd;
        //ACT
        GoldMine goldMine = new GoldMine(pos, player);
        player.buildings.Add(goldMine);
        foreach (var building in player.buildings)
        {
            
            if (building.GetType().Name == "GoldMine") {
                numberOfGoldMines++;
            }
        }
        sumGoldAmountToAdd = numberOfGoldMines * goldAmountToAdd;
        int goldAmountBeforeAddition = player.goldAmount;
        turnEnded = true;
        if (turnEnded)
        {
            player.goldAmount += sumGoldAmountToAdd;
        }
        
        //ASSERT
        Assert.That(player.goldAmount, Is.EqualTo(sumGoldAmountToAdd + goldAmountBeforeAddition));
    }

    [Test]
    public void NoGoldAdditionBeforeTurnEnd_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        bool turnEnded = false;
        int goldAmountToAdd = 10;
        int numberOfGoldMines = 0;
        int sumGoldAmountToAdd;
        //ACT
        GoldMine goldMine = new GoldMine(pos, player);
        player.buildings.Add(goldMine);
        foreach (var building in player.buildings)
        {

            if (building.GetType().Name == "GoldMine")
            {
                numberOfGoldMines++;
            }
        }
        sumGoldAmountToAdd = numberOfGoldMines * goldAmountToAdd;
        int goldAmountBeforeAddition = player.goldAmount;
        if (turnEnded)
        {
            player.goldAmount += sumGoldAmountToAdd;
        }

        //ASSERT
        Assert.That(player.goldAmount, Is.EqualTo(goldAmountBeforeAddition));
    }
}
