using Godot;
using System;

public partial class EnemyController : Area2D
{
    #region variables

    // comportamiento 
    public int level;
    private int _life;
    private int _score;

    // movimiento 
    private float _speed;
    private bool _changeDirection = false;
    private Vector2 _direction;
    private CollisionShape2D _collisionShape;

    // ataque 
    [Export] public PackedScene bulletEnemy;
    private Marker2D _marker;
    private bool _canShoot = true;
    private CharacterBody2D _target;

    // animaciones y sonidos
    private AudioStreamPlayer _audio;
    private AnimatedSprite2D _animatedSprite;

    // global
    private Global _global;

    #endregion

    #region metodos godot
    public override void _Ready()
    {
        // inicializa las variables y nodos
        _marker = GetNode<Marker2D>("Marker2D");
        _audio = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        
        // inicializa las variables globales y externas
        _global = GetNode<Global>("/root/Global");
        _target = GetParent().GetNode<CharacterBody2D>("/root/NormalLevel/Player");

    }

    #endregion

    #region metodos

    public void SetLevel()
    {
        Random randomLevel = new();
        level = randomLevel.Next(1, 4);
    }

    public void SetAttributes()
    {
        Random randomAttributes = new();

        // establecera las caracteristicas del enemigo segun el nivel
        switch(level)
        {
            case 1:
                _life = randomAttributes.Next(1, 2);
                _speed = randomAttributes.Next(75, 100);
                _score = randomAttributes.Next(1, 2);
                _animatedSprite.Animation = "move";
                break;
            case 2:
                _life = randomAttributes.Next(2, 3);
                _speed = randomAttributes.Next(100, 125);
                _score = randomAttributes.Next(3, 5);
                _animatedSprite.Animation = "move";
                break;
            case 3:
                _life = randomAttributes.Next(3, 4);
                _speed = randomAttributes.Next(125, 150);
                _score = randomAttributes.Next(5, 10);
                _animatedSprite.Animation = "move";
                break;
            default:
                _life = 1;
                break;
        }
    }

    public void Movement(double delta)
    {
        // valida si se encuentra en el rango que puede cambiar de direccion 
        // de lo contrario seguira su camino
        if(Position.Y > 300 && _changeDirection == false)
        {
            if(Position.X < 80 || Position.X > 400)
            {
                // almacena la direccion hacia el jugador
                LookAt(_target.Position);
                _direction = Position.DirectionTo(_target.Position);
                _changeDirection = true;
            }
        }
        // cambia direccion hacia el jugador 
        // de lo contrario seguira su camino
        if(_changeDirection == true)
        {
            Position += _direction * _speed * 2.5f * (float)delta;
        }
        else
        {
            Position += new Vector2(0, _speed * (float)delta);
        }

        CheckPosition();

    }

    public void CheckCollisions(Area2D area)
    {
        if(area.IsInGroup("Bullet") && _life > 0)
        {
            Bullet bullet = area as Bullet;
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        _life -= damage;

        if(_life <= 0)
        {
            ChangeScore();
            ActivateEffects();
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

    // TODO: revisar funcionamiento de las siguientes funciones
    public void CheckPosition()
    {
        if(Position.Y > GetViewportRect().Size.Y || _global.life <= 0)
        {
            Destroy();
        }
    }

    public async void ActivateEffects()
    {
        _audio.Play();
        _animatedSprite.Animation = "destroy";

        await _audio.ToSignal(_audio, "finished");
        Destroy();
    }

    public void ChangeScore()
    {
        _global.score += _score;
    }

    public void Destroy() 
    {
        QueueFree();
    }

    #endregion
}

