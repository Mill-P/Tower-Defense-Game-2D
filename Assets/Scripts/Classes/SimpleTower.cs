using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTower : Tower
{
    public SimpleTower(Vector2 _position, TDPlayer _owner) : base(_position, _owner)
    {
        this.upgradeCost = 5;
    }

    public override Transform getTransform()
    {
        return Resources.Load<Transform>("Prefabs/SimpleTower");
    }
}
