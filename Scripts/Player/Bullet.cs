using Godot;
using System;

public partial class Bullet : Area2D
{
    // variables

    private float _speed = 350f; //12.5f;

    // metodos
    public override void _PhysicsProcess(double delta)
    {
        Position -= new Vector2(0, _speed * (float)delta);

        if(Position.Y < -2)
        {
            QueueFree();
        }
    }

    public void OnEnteredEnemy(Area2D area)
    {
        if(area.IsInGroup("Enemy"))
        {
            QueueFree();
        }
    }
}
