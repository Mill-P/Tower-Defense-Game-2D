using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMineTest 
{
    [Test]
    public void GoldMineTakeDamage_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int amount = 10;
        int expectedHP = 50 - 10;

        //ACT
        GoldMine goldMine = new GoldMine(pos, player);
        goldMine.takeDamage(amount);

        //ASSERT
        Assert.That(goldMine.health, Is.EqualTo(expectedHP));
    }
    [Test]
    public void GoldMineBaseHealth_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int expectedHP = 50;

        //ACT
        GoldMine goldMine = new GoldMine(pos, player);

        //ASSERT
        Assert.That(goldMine.health, Is.EqualTo(expectedHP));
    }
    [Test]
    public void PlayerCreatedAGoldMine_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");

        //ACT
        GoldMine goldMine = new GoldMine(pos, player);
        player.buildings.Add(goldMine);

        //ASSERT
        Assert.That(player.buildings.Contains(goldMine), Is.EqualTo(true));
    }
    [Test]
    public void DestroyGoldMine_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int amount = 55;

        //ACT
        GoldMine goldMine = new GoldMine(pos, player);
        player.buildings.Add(goldMine);
        goldMine.takeDamage(amount);
        if (goldMine.health <= 0)
        {
            player.buildings.Remove(goldMine);
        }
        //ASSERT
        Assert.That(player.buildings.Contains(goldMine), Is.EqualTo(false));
    }

    
}
