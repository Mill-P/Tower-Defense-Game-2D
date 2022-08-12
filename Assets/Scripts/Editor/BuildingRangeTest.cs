using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
public class BuildingRangeTest
{
    [Test]
    public void CanBuildInRadius_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        Castle castle = new Castle(pos, player);
        Vector2 towerPos = new Vector2(1, 0);
        SimpleTower simpleTower = new SimpleTower(towerPos, player);
        //ACT
        //ASSERT
        Assert.That(Mathf.Abs(simpleTower.position.x - 2 - castle.position.x) <= 2 &&
           Mathf.Abs(simpleTower.position.x - 2 - castle.position.x) >= 0 &&
           Mathf.Abs(simpleTower.position.y - 2 - castle.position.x) <= 2 &&
           Mathf.Abs(simpleTower.position.y - 2 - castle.position.x) >= 0, Is.EqualTo(true));
    }

    [Test]
    public void CannotBuildInRadius_Test()
    {
        //ARRANGE
        Vector2 pos = new Vector2(0, 0);
        TDPlayer player = new TDPlayer("Pista");
        Castle castle = new Castle(pos, player);
        Vector2 towerPos = new Vector2(5, 0);
        SimpleTower simpleTower = new SimpleTower(towerPos, player);
        //ACT
        //ASSERT
        Assert.That(Mathf.Abs(simpleTower.position.x - 2 - castle.position.x) <= 2 &&
           Mathf.Abs(simpleTower.position.x - 2 - castle.position.x) >= 0 &&
           Mathf.Abs(simpleTower.position.y - 2 - castle.position.x) <= 2 &&
           Mathf.Abs(simpleTower.position.y - 2 - castle.position.x) >= 0, Is.EqualTo(false));
    }
}
