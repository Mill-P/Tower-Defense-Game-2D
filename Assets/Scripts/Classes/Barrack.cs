using UnityEngine;

public class Barrack : Building
{
    public Barrack(Vector2 _position, TDPlayer _owner) : base(_position, _owner)
    {
        this.health = 50;
        this.gameObject.name = this.gameObject.name.Replace("BUILDING", "BARRACK");
    }


    public override Transform getTransform()
    {
        return Resources.Load<Transform>("Prefabs/Barrack");
    }
}
