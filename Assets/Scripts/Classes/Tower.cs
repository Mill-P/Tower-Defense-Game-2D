using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : Building
{
    public int range { get; set; }
    public int level { get; set; }
    public int upgradeCost { get; set; }

    /// <summary>
    /// Creates a tower
    /// </summary>
    /// <param name="_position">Position of tower</param>
    /// <param name="_owner">Owner of the tower</param>
    public Tower(Vector2 _position, TDPlayer _owner) : base(_position, _owner)
    {
        this.gameObject.name = this.gameObject.name.Replace("BUILDING", "TOWER");
        this.health = 20;
        this.damage = 3;
        this.range = 2;
        this.level = 1;
    }

    /// <summary>
    /// Attacks LivingEntity, mostly Units
    /// </summary>
    /// <param name="le">Entity to attack</param>
    public void attack(LivingEntity le) {
        le.takeDamage(this.damage);
    }

    //public abstract Transform getTransform();
}
