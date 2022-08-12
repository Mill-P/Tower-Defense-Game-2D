using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
public class EndGameTest
{
    [Test]
    public void IsGameEnd_Test()
    {
        //ARRANGE
        Vector2 pos2 = new Vector2(10, 10);
        TDPlayer player2 = new TDPlayer("Béla");
        Castle castle2 = new Castle(pos2, player2);
        int lethalDamageAmount = 10000;
        //ACT
        castle2.takeDamage(lethalDamageAmount);
        //ASSERT
        Assert.That(castle2.health <= 0, Is.EqualTo(true));
    }
}
