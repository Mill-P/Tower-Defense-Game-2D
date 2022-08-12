using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyArmorUnit : Unit
{
    public HeavyArmorUnit(Vector2 _position, TDPlayer _owner, LivingEntity _target) : base(_position, _owner, _target)
    {
        this.health = 25;
        this.damage = 3;
    }

    public override Transform getTransform()
    {
        return Resources.Load<Transform>("Prefabs/HeavyArmorUnit");
    }
}
