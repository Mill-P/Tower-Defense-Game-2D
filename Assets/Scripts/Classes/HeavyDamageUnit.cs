using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyDamageUnit : Unit
{    
    public HeavyDamageUnit(Vector2 _position, TDPlayer _owner, LivingEntity _target) : base(_position, _owner, _target)
    {
        this.health = 10;
        this.damage = 10;
    }
    public override Transform getTransform()
    {
        return Resources.Load<Transform>("Prefabs/HeavyDamageUnit");
    }
}
