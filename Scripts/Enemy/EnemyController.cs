using Godot;
using System;

public partial class EnemyController : Area2D
{
    // variables

    // nivel que establece las caracteristicas del enemigo
    public int level;

    // comportamiento del enemigo
    private float _speed;
    private int _life;
    private int _score;
    private bool _changeDirection = false;
    private Vector2 _direction;

    // ataque del enemigo
    [Export] public PackedScene bulletEnemy;
    private Marker2D _marker;
    private bool _canShoot = true;
    private CharacterBody2D _target;

    // global variables
    private Global _global;

    // metodos
    public override void _Ready()
    {
        // establecer el nivel del enemigo de manera aleatoria
        Random random = new Random();
        level = random.Next(1, 4);
        SetLevel(level);

        // inicializar variables
        _marker = GetNode<Marker2D>("Marker2D");
        _target = GetParent().GetNode<CharacterBody2D>("/root/NormalLevel/Player");
        _global = GetNode<Global>("/root/Global");
    }

    public override void _Process(double delta)
    {
        // llamamos al metodo de disparo
        Shoot();
    }

    public override void _PhysicsProcess(double delta)
    {
        Movement(delta);

        // si el enemigo sale de la pantalla, se elimina
        if(Position.Y > 730 || _global.life <= 0)
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
                _score = random.Next(1, 2);
                break;
            case 2:
                _life = random.Next(2, 3);
                _speed = random.Next(100, 125);
                _score = random.Next(3, 5);
                break;
            case 3:
                _life = random.Next(3, 4);
                _speed = random.Next(125, 150);
                _score = random.Next(5, 10);
                break;
            default:
                _life = 1;
                break;
        }
    }

    private void Movement(double delta)
    {
        // en el eje Y en el pixel 300 cambiara de direccion
        if(Position.Y > 300 && _changeDirection == false)
        {
            if(Position.X < 80 || Position.X > 400)
            {
                // tomara la direccion del jugador
                LookAt(_target.Position);
                _direction = Position.DirectionTo(_target.Position);
                _changeDirection = true;

            }

        }
        // si puede cambiar de direccion, se movera en la direccion del jugador, 
        // de lo contrario seguira su camino
        if(_changeDirection == true)
        {
            Position += _direction * _speed * 2.5f * (float)delta;
        }
        else
        {
            Position += new Vector2(0, _speed * (float)delta);
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
            _global.score += _score;
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
