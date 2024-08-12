using Godot;
using System;

public partial class EnemyController : Area2D
{
    // variables

    // movimiento
    private float _speed = 75f;

    // nivel que establece las caracteristicas del enemigo
    public int level = 3;

    // vida
    public int life;

    // metodos
    public override void _Ready()
    {
        SetLevel(level);
        GD.Print($"Enemy life: {life}");
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += new Vector2(0, _speed * (float)delta);
        if(Position.Y > 722)
        {
            QueueFree();
        }
    }

    public void SetLevel(int level)
    {
        this.level = level;
        Random random = new Random();

        switch(level)
        {
            case 1:
                life = random.Next(1, 2);
                break;
            case 2:
                life = random.Next(3, 5);
                break;
            case 3:
                life = random.Next(5, 10);
                break;
            default:
                life = 1;
                break;
        }
    }

    public void OnAreaEntered(Area2D area)
    {
        if(area.IsInGroup("Bullet"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        life -= damage;

        if(life <= 0)
        {
            QueueFree();
        }
    }
}
