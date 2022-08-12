using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class PlayerTests
{
    [Test]
    public void PlayerHasACastle_Test()
    {
        //arrange
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("pista");
        bool found = false;
        //act
        Castle castle = new Castle(pos, player);
        player.buildings.Add(castle);
        foreach (var building in player.buildings)
        {
            Debug.Log(building.GetType().Name);
            if (building.GetType().Name == "Castle") {
                found = true;
            }
        }
        //assert
        Assert.That(found, Is.EqualTo(true));
    }
}
