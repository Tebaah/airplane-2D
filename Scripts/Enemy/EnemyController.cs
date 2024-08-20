using Godot;
using System;

public partial class EnemyController : Area2D
{
    // variables

    // nivel que establece las caracteristicas del enemigo
    [Export]public int level;

    // comportamiento del enemigo
    private float _speed;
    private int _life;

    // ataque del enemigo
    [Export] public PackedScene bulletEnemy;
    private Marker2D _marker;
    private bool _canShoot = true;

    // metodos
    public override void _Ready()
    {
        SetLevel(level);

        // inicializar variables
        _marker = GetNode<Marker2D>("Marker2D");
    }

    public override void _Process(double delta)
    {
        Shoot();
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += new Vector2(0, _speed * (float)delta);
        if(Position.Y > 730)
        {
            QueueFree();
        }
    }

    public void SetLevel(int level)
    {
        this.level = level;
        Random random = new Random();

        // establecera las caracteristicas del enemigo segun el nivel
        switch(level)
        {
            case 1:
                _life = random.Next(1, 2);
                _speed = random.Next(75, 100);
                break;
            case 2:
                _life = random.Next(3, 5);
                _speed = random.Next(100, 125);
                break;
            case 3:
                _life = random.Next(5, 10);
                _speed = random.Next(125, 150);
                break;
            default:
                _life = 1;
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
        _life -= damage;

        if(_life <= 0)
        {
            QueueFree();
        }
    }

    public async void Shoot()
    {
        if(_canShoot == true)
        {
            BulletEnemy bullet = (BulletEnemy)bulletEnemy.Instantiate();
            bullet.GlobalPosition = _marker.GlobalPosition;
            GetParent().AddChild(bullet);
            _canShoot = false;

            await ToSignal(GetTree().CreateTimer(2), "timeout");
            _canShoot = true;
        }
    }
}
