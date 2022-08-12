using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTest 
{
    public const int TOWER_HEATH = 20;
    public const int TOWER_DAMAGE = 3;
    [Test]
    public void DeadlyFireTowerBaseHealth_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int expectedHP = 20;

        //ACT
        DeadlyFireTower deadlyFireTower = new DeadlyFireTower(pos, player);

        //ASSERT
        Assert.That(deadlyFireTower.health, Is.EqualTo(expectedHP));
    }
    [Test]
    public void DeadlyFireTowerTakeDamage_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int amount = 10;
        int expectedHP = 20 - 10;

        //ACT
        DeadlyFireTower deadlyFireTower = new DeadlyFireTower(pos, player);
        deadlyFireTower.takeDamage(amount);

        //ASSERT
        Assert.That(deadlyFireTower.health, Is.EqualTo(expectedHP));
    }
    [Test]
    public void PlayerCreatedADeadlyFireTower_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");

        //ACT
        DeadlyFireTower deadlyFireTower = new DeadlyFireTower(pos, player);
        player.buildings.Add(deadlyFireTower);

        //ASSERT
        Assert.That(player.buildings.Contains(deadlyFireTower), Is.EqualTo(true));
    }
    [Test]
    public void DestroyDeadlyFireTower_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int amount = 55;

        //ACT
        DeadlyFireTower deadlyFireTower = new DeadlyFireTower(pos, player);
        player.buildings.Add(deadlyFireTower);
        deadlyFireTower.takeDamage(amount);
        if (deadlyFireTower.health <= 0)
        {
            player.buildings.Remove(deadlyFireTower);
        }
        //ASSERT
        Assert.That(player.buildings.Contains(deadlyFireTower), Is.EqualTo(false));
    }
    [Test]
    public void LongRangeTowerBaseHealth_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int expectedHP = 20;

        //ACT
        LongRangeTower longRangeTower = new LongRangeTower(pos, player);

        //ASSERT
        Assert.That(longRangeTower.health, Is.EqualTo(expectedHP));
    }
    [Test]
    public void LongRangeTowerTakeDamage_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int amount = 10;
        int expectedHP = 20 - 10;

        //ACT
        LongRangeTower longRangeTower = new LongRangeTower(pos, player);
        longRangeTower.takeDamage(amount);

        //ASSERT
        Assert.That(longRangeTower.health, Is.EqualTo(expectedHP));
    }
    [Test]
    public void PlayerCreatedALongRangeTower_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");

        //ACT
        LongRangeTower longRangeTower = new LongRangeTower(pos, player);
        player.buildings.Add(longRangeTower);

        //ASSERT
        Assert.That(player.buildings.Contains(longRangeTower), Is.EqualTo(true));
    }
    [Test]
    public void DestroyLongRangeTower_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int amount = 55;

        //ACT
        LongRangeTower longRangeTower = new LongRangeTower(pos, player);
        player.buildings.Add(longRangeTower);
        longRangeTower.takeDamage(amount);
        if (longRangeTower.health <= 0)
        {
            player.buildings.Remove(longRangeTower);
        }
        //ASSERT
        Assert.That(player.buildings.Contains(longRangeTower), Is.EqualTo(false));
    }
    [Test]
    public void SimpleTowerBaseHealth_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int expectedHP = 20;

        //ACT
        SimpleTower simpleTower = new SimpleTower(pos, player);

        //ASSERT
        Assert.That(simpleTower.health, Is.EqualTo(expectedHP));
    }
    [Test]
    public void SimpleTowerTakeDamage_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int amount = 10;
        int expectedHP = 20 - 10;

        //ACT
        SimpleTower simpleTower = new SimpleTower(pos, player);
        simpleTower.takeDamage(amount);

        //ASSERT
        Assert.That(simpleTower.health, Is.EqualTo(expectedHP));
    }
    [Test]
    public void PlayerCreatedASimpleTower_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");

        //ACT
        SimpleTower simpleTower = new SimpleTower(pos, player);
        player.buildings.Add(simpleTower);

        //ASSERT
        Assert.That(player.buildings.Contains(simpleTower), Is.EqualTo(true));
    }
    [Test]
    public void DestroySimpleTower_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int amount = 55;

        //ACT
        SimpleTower simpleTower = new SimpleTower(pos, player);
        player.buildings.Add(simpleTower);
        simpleTower.takeDamage(amount);
        if (simpleTower.health <= 0)
        {
            player.buildings.Remove(simpleTower);
        }
        //ASSERT
        Assert.That(player.buildings.Contains(simpleTower), Is.EqualTo(false));
    }

    [Test]
    public void UpgradeSimpleTower_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int upgradeDamageAmount = 2;
        int upgradeHealthAmount = 10;
        //int upgradeRangeAmount = 1;
        int expectedDamageAmountAfterUpgrade = 5;
        int expectedHealthAmountAfterUpgrade = 30;

        //ACT
        SimpleTower simpleTower = new SimpleTower(pos, player);
        
        simpleTower.damage += upgradeDamageAmount;
        simpleTower.health += upgradeHealthAmount;
        //simpleTower.range += upgradeRangeAmount;


        //ASSERT
        Assert.That(simpleTower.damage, Is.EqualTo(expectedDamageAmountAfterUpgrade));
        Assert.That(simpleTower.health, Is.EqualTo(expectedHealthAmountAfterUpgrade));
    }
    [Test]
    public void UpgradeLongRangeTower_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int upgradeDamageAmount = 2;
        int upgradeHealthAmount = 10;
        //int upgradeRangeAmount = 1;
        int expectedDamageAmountAfterUpgrade = 5;
        int expectedHealthAmountAfterUpgrade = 30;

        //ACT
        LongRangeTower longRangeTower = new LongRangeTower(pos, player);

        longRangeTower.damage += upgradeDamageAmount;
        longRangeTower.health += upgradeHealthAmount;
        //simpleTower.range += upgradeRangeAmount;


        //ASSERT
        Assert.That(longRangeTower.damage, Is.EqualTo(expectedDamageAmountAfterUpgrade));
        Assert.That(longRangeTower.health, Is.EqualTo(expectedHealthAmountAfterUpgrade));
    }

    [Test]
    public void UpgradeDeadlyFireTower_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int upgradeDamageAmount = 2;
        int upgradeHealthAmount = 10;
        //int upgradeRangeAmount = 1;
        int expectedDamageAmountAfterUpgrade = 7;
        int expectedHealthAmountAfterUpgrade = 30;

        //ACT
        DeadlyFireTower deadlyFireTower = new DeadlyFireTower(pos, player);

        deadlyFireTower.damage += upgradeDamageAmount;
        deadlyFireTower.health += upgradeHealthAmount;
        //simpleTower.range += upgradeRangeAmount;


        //ASSERT
        Assert.That(deadlyFireTower.damage, Is.EqualTo(expectedDamageAmountAfterUpgrade));
        Assert.That(deadlyFireTower.health, Is.EqualTo(expectedHealthAmountAfterUpgrade));
    }

}
