using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class LivingEntity
{
    public Vector2 position
    {
        get
        {

            return this.gameObject.transform.position;
        }
        set
        {
            this.gameObject.transform.position = new Vector3(value.x, value.y, this.gameObject.transform.position.z);
        }
    }
    public GameObject gameObject = new GameObject();
    public TDPlayer owner;
    public int health;
    public int cost;
    public int damage = 0;

    public UnityAction entityDeadEvent;

    public LivingEntity()
    {

    }

    /// <summary>
    /// Creates a LivingEntity
    /// </summary>
    /// <param name="_position">Position of LivingEntity in WorldCoordinates</param>
    /// <param name="_owner">Player owner of the LivingEntity</param>
    public LivingEntity(Vector2 _position, TDPlayer _owner)
    {
        this.gameObject.name = $"LIVINGENTITY [{_owner.name}] ({_position.x},{_position.y})";
        this.position = _position;
        this.owner = _owner;
    }

    /// <summary>
    /// Takes damage, subtracts amount from health
    /// </summary>
    /// <param name="amount">Amount of damage to take</param>
    public void takeDamage(int amount)
    {
        this.health -= amount;
        if (this.health <= 0)
        {
            this.entityDeadEvent?.Invoke();
        }
    }
    
}
