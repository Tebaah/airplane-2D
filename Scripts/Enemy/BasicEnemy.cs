using Godot;
using System;

public partial class BasicEnemy : EnemyController
{
    #region metodos godot
    public override void _Ready()
    {
        base._Ready();
        SetLevel();
        SetAttributes(EnemyType.basic);
    }

    public override void _PhysicsProcess(double delta)
    {
        Movement(delta);
        Shoot();
    }
    
    #endregion
}
