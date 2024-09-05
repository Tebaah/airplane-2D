using Godot;
using System;

public partial class Bullet : Area2D
{
    // variables
    // movimiento
    private float _speed = 350f; //12.5f;

    // nivel
    [Export]public int level;

    // ataque
    public int damage;

    // metodos

    public override void _Ready()
    {
        DamagePerLevel();
    }
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
    
    public void DamagePerLevel()
    {
        switch(level)
        {
            case 1:
                damage = 1;
                break;
            case 2:
                damage = 2;
                break;
            case 3:
                damage = 3;
                break;
            default:
                GD.Print("Bullet level 1");
                break;
        }
    }
}
