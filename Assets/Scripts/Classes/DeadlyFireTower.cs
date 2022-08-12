using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyFireTower : Tower
{
    public DeadlyFireTower(Vector2 _position, TDPlayer _owner) : base(_position, _owner)
    {
        this.damage = 5;
        this.upgradeCost = 5;
    }

    public override Transform getTransform()
    {
        return Resources.Load<Transform>("Prefabs/DeadlyFireTower");
    }
}
