using UnityEngine;

public class Castle : Building {
    public Castle(Vector2 _position, TDPlayer _owner) : base(_position, _owner) {
        this.health = 100;
        this.gameObject.name = this.gameObject.name.Replace("BUILDING", "CASTLE");
    }

    public override Transform getTransform()
    {
        return Resources.Load<Transform>("Prefabs/Castle");
    }
}


