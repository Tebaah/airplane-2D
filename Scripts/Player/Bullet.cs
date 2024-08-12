using Godot;
using System;

public partial class Bullet : Area2D
{
    // variables

    private float _speed = 7.5f; //12.5f;

    // metodos
    public override void _PhysicsProcess(double delta)
    {
        Position -= new Vector2(0, _speed);

        if(Position.Y < -2)
        {
            QueueFree();
        }
    }
}
