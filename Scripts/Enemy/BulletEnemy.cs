using Godot;
using System;

public partial class BulletEnemy : Area2D
{
    // variables

    // global variables
    private Global _global;
    // movimiento
    private float _speed = 250;

    public override void _Ready()
    {
        _global = GetNode<Global>("/root/Global");
    }
    public override void _PhysicsProcess(double delta)
    {
        Position += new Vector2(0, _speed * (float)delta);
        if(Position.Y > 730 || _global.life <= 0)
        {
            QueueFree();
        }
    }

    public void OnPlayerEntered(Node2D body)
    {
        if(body.Name == "Player")
        {
            QueueFree();
        }
    }
}
