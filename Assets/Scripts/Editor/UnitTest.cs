using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
public class UnitTest
{
    [Test]
    public void UnitTakeDamage_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        TDPlayer player2 = new TDPlayer("Béla");
        Castle castle = new Castle(pos, player2);
        int amount = 5;
        int expectedHP = 10 - 5;

        //ACT
        BuildingDestroyerUnit buildingDestroyerUnit = new BuildingDestroyerUnit(pos, player, castle);
        buildingDestroyerUnit.takeDamage(amount);

        //ASSERT
        Assert.That(buildingDestroyerUnit.health, Is.EqualTo(expectedHP));
    }

    [Test]
    public void UnitChangedPos_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        Vector2 towerPos = new Vector2(10, 12);
        TDPlayer player = new TDPlayer("Pista");
        SimpleTower enemySimpleTower = new SimpleTower(towerPos, player);
        HeavyArmorUnit heavyArmorUnit = new HeavyArmorUnit(pos, player, enemySimpleTower);
        int unitMovementSpeed = 1;
        //ACT
        Vector2 actPos = heavyArmorUnit.position;
        Vector2 newPos = new Vector2(actPos.x + unitMovementSpeed, actPos.y);
        heavyArmorUnit.position = newPos;
        //ASSERT
        Assert.That(actPos != newPos, Is.EqualTo(true));
    }

    [Test]
    public void UnitAttacksTarget_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        Vector2 enemyTowerPos = new Vector2(10, 12);
        TDPlayer player = new TDPlayer("Pista");
        SimpleTower enemySimpleTower = new SimpleTower(enemyTowerPos, player);
        HeavyArmorUnit heavyArmorUnit = new HeavyArmorUnit(pos, player, enemySimpleTower);
        //ACT
        int beforeAttackHp = enemySimpleTower.health;
        heavyArmorUnit.attack(enemySimpleTower);
        
        //ASSERT
        Assert.That(beforeAttackHp > enemySimpleTower.health, Is.EqualTo(true));
    }

    [Test]
    public void UnitDiesAfterTakingTowerDamage_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        Vector2 enemyTowerPos = new Vector2(10, 12);
        TDPlayer player = new TDPlayer("Pista");
        SimpleTower enemySimpleTower = new SimpleTower(enemyTowerPos, player);
        HeavyArmorUnit heavyArmorUnit = new HeavyArmorUnit(pos, player, enemySimpleTower);

        //ACT
        int beforeHp = heavyArmorUnit.health;
        while (heavyArmorUnit.health > 0)
        {
            enemySimpleTower.attack(heavyArmorUnit);
        }

        

        //ASSERT
        Assert.That(beforeHp > 0 && heavyArmorUnit.health <= 0, Is.EqualTo(true));
    }

    //[Test]
    //public void UnitDiesAfterAttackOnBuilding_Test()
    //{
    //    //ARRANGE
    //    Vector2 pos = new Vector2(0, 0);
    //    Vector2 enemyTowerPos = new Vector2(10, 12);
    //    TDPlayer player = new TDPlayer("Pista");
    //    SimpleTower enemySimpleTower = new SimpleTower(enemyTowerPos, player);
    //    HeavyArmorUnit heavyArmorUnit = new HeavyArmorUnit(pos, player, enemySimpleTower);

    //    //ACT
    //    heavyArmorUnit.attack(enemySimpleTower);

    //    //ASSERT
    //    Assert.That(beforeAttackHp > enemySimpleTower.health, Is.EqualTo(true));
    //}
}
