using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
    #region  variables

    // comportamiento del jugador
    [Export] public float speed;
    private float _backlink = 5f;

    // ataque del jugador
    [Export] public PackedScene[] bullet;
    private Marker2D _spawnBullets;
    private bool _canShoot = true;
    private int _levelAttack = 1;

    // sonidos y efectos
    private AudioStreamPlayer _audio;

    // globales
    public Global global;

    #endregion

    
    #region metodos godot

    public override void _Ready()
    {
        // inicializa las variables
        _spawnBullets = GetNode<Marker2D>("Weapons/Marker2D");
        global = GetNode<Global>("/root/Global");
        _audio = GetNode<AudioStreamPlayer>("AudioStreamPlayer");

    }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("attack") && _canShoot)
        {
            Attack();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Movement(delta);
        MoveAndSlide();
    }

    #endregion


    #region metodos

    private void Movement(double delta)
    {
        // obtiene la direccion del input
        Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");

        // almacena la velocidad del jugador
        Velocity = inputDirection.Normalized() * speed * (float)delta;

        LimitOfMovement();
    }
    
    private void LimitOfMovement()
    {
        // limita el movimiento del jugador
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
        // modifica el estado para no disparar seguido
        // posterior realiza el disparo
        _canShoot = false;
        Shoot();

        // espera 0.20 segundos para cambiar el estado de disparo
        await ToSignal(GetTree().CreateTimer(0.20f), "timeout");
        _canShoot = true;
    }

    private void Shoot()
    {
        switch (_levelAttack)
        {
            // instancia el disparo segun el nivel de ataque
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

    public void AttackLevelManager()
    {
        // limita el nivel de ataque
        if(_levelAttack > 3)
        {
            _levelAttack = 3;
        }

        // activa la disminucion del nivel de ataque
        if(_levelAttack == 3)
        {
            DecreaseAttackLevel();
        }
    }

    public void IncreaseAttackLevel()
    {
        _levelAttack++;
    }

    public async void DecreaseAttackLevel()
    {
        // disminuye el nivel del ataque despues de 30 segundos
        await ToSignal(GetTree().CreateTimer(30), "timeout");
        _levelAttack = 2;
    }

    public void DetectPowerUp(Area2D body)
    {
        // detecta el power up y aumenta el nivel de ataque
        if(body.IsInGroup("PowerUp"))
        {
            IncreaseAttackLevel();
            AttackLevelManager();
            body.QueueFree();
        }
    }

    public void DetectDamage(Area2D body)
    {
        // detecta el daño y disminuye la vida
        if(body.IsInGroup("Enemy") || body.IsInGroup("BulletEnemy"))
        {
            global.life--;
            if(global.life <= 0)
            {
                QueueFree();
            }
        }
    }

    #endregion
}
