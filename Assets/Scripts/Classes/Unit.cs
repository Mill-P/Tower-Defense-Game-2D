using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Unit : LivingEntity
{
    protected enum TargetType {
        CASTLE,
        BUILDING
    };
    protected int speed = 1;
    protected List<Vector2> possiblePath;
    protected int pathPosition = 0;

    public LivingEntity target;
    public Unit(Vector2 _position, TDPlayer _owner, LivingEntity _target) : base(_position, _owner) {
        this.target = _target;
        this.gameObject.name = this.gameObject.name.Replace("LIVINGENTITY", "UNIT");
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 3);
    }

    /// <summary>
    /// Attacks the given target
    /// </summary>
    /// <param name="le">Target to attack</param>
    public void attack(LivingEntity le)
    {
        le.takeDamage(this.damage);
    }
    /// <summary>
    /// Moves the unit forward in its own calculated route towards its target
    /// </summary>
    public void moveForward(MapManager mapManager)
    {
        if (this.possiblePath == null) throw new System.Exception("Ennek az egységnek nincs kiszámolt útja!");
        this.pathPosition = Mathf.Min(this.pathPosition + this.speed, this.possiblePath.Count - 1);
        Vector2 newGridIndex = this.possiblePath[this.pathPosition];
        this.position = mapManager.getWorldCoordinateAtGridIndex((int)newGridIndex.x, (int)newGridIndex.y);
    }

    /// <summary>
    /// Calculates the path for the unit
    /// </summary>
    /// <param name="mapManager">mapManager that contains data about the map</param>
    /// <returns>True if any path was found</returns>
    public bool calculatePath(MapManager mapManager) {
        List<Vector2> newPath = mapManager.findPath(mapManager.getGridIndexAtWorldCoordinate(this.position.x, this.position.y), mapManager.getGridIndexAtWorldCoordinate(this.target.position.x, this.target.position.y), this);
        Debug.Log("Út: " + newPath.Count);
        if (newPath == null) return false;
        this.possiblePath = newPath;
        this.pathPosition = 0;
        return true;
    }

    /// <summary>
    /// Refreshes Health text under unit
    /// </summary>
    public void refreshHealthText()
    {
        TextMeshPro healthText = this.gameObject.GetComponentInChildren<TextMeshPro>();
        if (healthText != null)
        {
            healthText.text = this.health.ToString();
        }
    }

    public abstract Transform getTransform();
}
