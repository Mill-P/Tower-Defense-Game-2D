using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : LivingEntity
{
    public Building(Vector2 _position, TDPlayer _owner) : base(_position, _owner)
    {
        this.gameObject.name = this.gameObject.name.Replace("LIVINGENTITY", "BUILDING");
    }

    public abstract Transform getTransform();
}