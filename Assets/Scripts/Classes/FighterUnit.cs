using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterUnit : Unit
{
    public FighterUnit(Vector2 _position, TDPlayer _owner, LivingEntity _target) : base(_position, _owner, _target)
    {
        this.speed = 2;
        this.health = 2;
        this.damage = 5;
    }

    public override Transform getTransform()
    {
        return Resources.Load<Transform>("Prefabs/FighterUnit");
    }
}
