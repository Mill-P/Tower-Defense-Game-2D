using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreativeUnit : Unit
{
    public CreativeUnit(Vector2 _position, TDPlayer _owner, LivingEntity _target) : base(_position, _owner, _target) 
    {
        this.speed = 1;
        this.health = 10;
        this.damage = 5;
    }
    public override Transform getTransform()
    {
        return Resources.Load<Transform>("Prefabs/CreativeUnit");
    }
}
