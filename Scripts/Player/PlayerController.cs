using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
    // variables

    // movimiento
    [Export] public float speed;
    private float _backlink = 5f;

    // ataque
    [Export] public PackedScene[] bullet;
    private Marker2D _spawnBullets;
    private bool _canShoot = true;
    private int _levelAttack = 1;

    // sonidos y efectos
    private AudioStreamPlayer _audio;
    // globales
    public Global global;

    // metodos
    public override void _Ready()
    {
        // TODO: inicializar variables de manera mas efectva y limpia
        _spawnBullets = GetNode<Marker2D>("Weapons/Marker2D");

        global = GetNode<Global>("/root/Global");
        _audio = GetNode<AudioStreamPlayer>("AudioStreamPlayer");

    }

    public override void _Process(double delta)
    {
        Attack();
    }

    public override void _PhysicsProcess(double delta)
    {
        Movement(delta);
        MoveAndSlide();
    }

    private void Movement(double delta)
    {
        // obtenemos la direccion del input
        Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
        
        // almacenamos en velocity la direccion normalizada por la velocidad
        Velocity = inputDirection.Normalized() * speed * (float)delta;

        LimitOfMovement();
    }
    
    private void LimitOfMovement()
    {
        if(Position.X < 2)
        {
            Position = new Vector2(Position.X + _backlink, Position.Y);
        }
        else if(Position.X > 478)
        {
            Position = new Vector2(Position.X - _backlink, Position.Y);
        }
        else if(Position.Y < 2)
        {
            Position = new Vector2(Position.X, Position.Y + _backlink);
        }
        else if(Position.Y > 718)
        {
            Position = new Vector2(Position.X, Position.Y - _backlink);
        }
        
    }

    private async void Attack()
    {
        // 
        if (Input.IsActionJustPressed("attack") && _canShoot) 
        {
            _canShoot = false;
            Shoot();

            await ToSignal(GetTree().CreateTimer(0.20f), "timeout");
            _canShoot = true;
        }
    }

    private void Shoot()
    {
        switch (_levelAttack)
        {
            // instanciamos la bala dependiendo del nivel de ataque
            case 1:
                Bullet newBullet1 = bullet[0].Instantiate() as Bullet;
                newBullet1.Position = _spawnBullets.GlobalPosition;
                GetParent().AddChild(newBullet1);
                _audio.Play();
                break;
            case 2:
                Bullet newBullet2 = bullet[1].Instantiate() as Bullet;
                newBullet2.Position = _spawnBullets.GlobalPosition;
                GetParent().AddChild(newBullet2);
                _audio.Play();
                break;
            case 3:
                Bullet newBullet3 = bullet[2].Instantiate() as Bullet;
                newBullet3.Position = _spawnBullets.GlobalPosition;
                GetParent().AddChild(newBullet3);
                _audio.Play();
                break;
        }
    }

    public void LevelUpAttack()
    {
        // modificamos de nivel el ataque
        _levelAttack++;
        if(_levelAttack > 3)
        {
            _levelAttack = 3;
        }
        if(_levelAttack == 3)
        {
            LevelDownAttack();
        }
    }

    public async void LevelDownAttack()
    {
        // bajamos de nivel el ataque despues de 30 segundos
        await ToSignal(GetTree().CreateTimer(30), "timeout");
        _levelAttack = 2;
    }

    public void DetectPowerUp(Area2D body)
    {
        // al detectar un power up, subimos de nivel el ataque
        if(body.IsInGroup("PowerUp"))
        {
            LevelUpAttack();
            body.QueueFree();
        }
    }

    public void DetectDamage(Area2D body)
    {
        // al detectar un enemigo o una bala, bajamos la vida
        if(body.IsInGroup("Enemy") || body.IsInGroup("BulletEnemy"))
        {
            global.life--;
            if(global.life <= 0)
            {
                QueueFree();
            }
        }
    }
}
