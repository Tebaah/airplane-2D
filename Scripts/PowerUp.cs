using Godot;
using System;

public partial class PowerUp : Area2D
{
    // variables
    // global variables
    private Global _global;

    // metodos
    public override void _Ready()
    {
        _global = GetNode<Global>("/root/Global");
    }
    public override void _PhysicsProcess(double delta)
    {
        Position += new Vector2(0, 1.5f);

        if(Position.Y > 730 || _global.life <= 0)
        {
            QueueFree();
        }
    }
}
