using Godot;
using System;
using System.Linq;
using System.Reflection;

public partial class EnemyController : Area2D
{
    #region variables

    // comportamiento 
    public int level;
    private int _life;
    private int _score;
    public enum EnemyType { basic, special, boss }

    // movimiento 
    private float _speed;
    private bool _changeDirection = false;
    private Vector2 _direction;
    private CollisionShape2D _collisionShape;
    private int _initialPosition = 20;
    private int _finalPosition = 460;

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

    public void SetAttributes(EnemyType type)
    {
        Random randomAttributes = new();
        
        // establece las caracteristicas del enemigo segun el nivel
        if(type == EnemyType.basic)
        {
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
        else if(type == EnemyType.special)
        {
            switch(level)
            {
                case 1:
                    _life = randomAttributes.Next(5, 10);
                    _speed = randomAttributes.Next(150, 225);
                    _score = randomAttributes.Next(11, 15);
                    _animatedSprite.Animation = "move";
                    break;
                case 2:
                    _life = randomAttributes.Next(10, 15);
                    _speed = randomAttributes.Next(150, 225);
                    _score = randomAttributes.Next(15, 20);
                    _animatedSprite.Animation = "move";
                    break;
                case 3:
                    _life = randomAttributes.Next(15, 20);
                    _speed = randomAttributes.Next(150, 225);
                    _score = randomAttributes.Next(20,25);
                    _animatedSprite.Animation = "move";
                    break;
                default:
                    _life = 1;
                    break;
            }
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

    public void Movement(double delta, int limit)
    {
        // valida si se encuentra en el rango que puede cambiar de direccion y avanza hasta el limite
        if(Position.Y < limit)
        {
            Position += new Vector2(0, _speed * (float)delta);
        }
        // valida si se encuentra en el rango que puede cambiar de direccion y se mueve de un lado a otro
        else if(Position.Y >= limit)
        {
            if(Position.X > _initialPosition && !_changeDirection)
            {
                Position -= new Vector2(_speed * (float)delta, 0);
                if(Position.X <= _initialPosition)
                {
                    _changeDirection = true;
                }
            }
            else if(Position.X < _finalPosition && _changeDirection)
            {
                Position += new Vector2(_speed * (float)delta, 0);
                if(Position.X >= _finalPosition)
                {
                    _changeDirection = false;
                }
            }

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

