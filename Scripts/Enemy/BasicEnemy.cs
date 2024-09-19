using Godot;
using System;

public partial class BasicEnemy : EnemyController
{
    public override void _Ready()
    {
        base._Ready();
        SetLevel();
    }

    public override void _PhysicsProcess(double delta)
    {
        Movement(delta);
        Shoot();
        Destroy();
    }
    

}
