using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleTest 
{
    [Test]
    public void CastleBaseHealth_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int expectedHP = 100;

        //ACT
        Castle castle = new Castle(pos, player);

        //ASSERT
        Assert.That(castle.health, Is.EqualTo(expectedHP));
    }
    [Test]
    public void CastleTakeDamage_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int amount = 10;
        int expectedHP = 100 - 10;

        //ACT
        Castle castle = new Castle(pos, player);
        castle.takeDamage(amount);

        //ASSERT
        Assert.That(castle.health, Is.EqualTo(expectedHP));
    }
}
