using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMine : Building
{
    public GoldMine(Vector2 _position, TDPlayer _owner) : base(_position, _owner)
    {
        this.health = 50;
        this.gameObject.name = this.gameObject.name.Replace("BUILDING", "GOLDMINE");
    }


    public override Transform getTransform()
    {
        return Resources.Load<Transform>("Prefabs/Mine");
    }
}
