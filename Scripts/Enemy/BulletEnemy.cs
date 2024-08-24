using Godot;
using System;

public partial class BulletEnemy : Area2D
{
    // variables

    // movimiento
    private float _speed = 250;

    public override void _PhysicsProcess(double delta)
    {
        Position += new Vector2(0, _speed * (float)delta);
        if(Position.Y > 730)
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
