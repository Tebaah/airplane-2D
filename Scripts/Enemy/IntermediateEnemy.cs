using Godot;
using System;

public partial class IntermediateEnemy : EnemyController
{
    #region variables

    #endregion

    #region metodos godot
    public override void _Ready()
    {
        base._Ready();
        SetLevel();
        SetAttributes(EnemyType.special);
    }

    public override void _PhysicsProcess(double delta)
    {
        Movement(delta, 250);
        Shoot();
    }
    #endregion

    #region metodos

    #endregion
}
