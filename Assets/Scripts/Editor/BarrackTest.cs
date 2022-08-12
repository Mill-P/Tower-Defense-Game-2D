using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrackTest 
{
    [Test]
    public void BarrackBaseHealth_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int expectedHP = 50;

        //ACT
        Barrack barrack = new Barrack(pos, player);

        //ASSERT
        Assert.That(barrack.health, Is.EqualTo(expectedHP));
    }
    [Test]
    public void BarrackTakeDamage_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int amount = 10;
        int expectedHP = 50 - 10;

        //ACT
        Barrack barrack = new Barrack(pos, player);
        barrack.takeDamage(amount);

        //ASSERT
        Assert.That(barrack.health, Is.EqualTo(expectedHP));
    }
    [Test]
    public void PlayerCreatedABarrack_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");

        //ACT
        Barrack barrack = new Barrack(pos, player);
        player.buildings.Add(barrack);

        //ASSERT
        Assert.That(player.buildings.Contains(barrack), Is.EqualTo(true));
    }
    [Test]
    public void DestroyBarrack_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        int amount = 55;

        //ACT
        Barrack barrack = new Barrack(pos, player);
        player.buildings.Add(barrack);
        barrack.takeDamage(amount);
        if (barrack.health <= 0) {
            player.buildings.Remove(barrack);
        }
        //ASSERT
        Assert.That(player.buildings.Contains(barrack), Is.EqualTo(false));
    }
   
}
