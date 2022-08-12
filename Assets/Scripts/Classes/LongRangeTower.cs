using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeTower : Tower
{
    public LongRangeTower(Vector2 _position, TDPlayer _owner) : base(_position, _owner)
    {
        this.range = 3;
        this.upgradeCost = 5;
    }

    public override Transform getTransform()
    {
        return Resources.Load<Transform>("Prefabs/LongRangeTower");
    }
}
