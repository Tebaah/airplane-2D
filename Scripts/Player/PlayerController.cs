using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
    // variables

    // movimiento
    [Export] public float speed;

    // ataque
    [Export] public PackedScene bullet;
    private Marker2D _spawnBullets;
    private Marker2D _spawnBullets2;
    private Marker2D _spawnBullets3;
    private int _levelAttack = 3;

    // metodos
    public override void _Ready()
    {
        // TODO inicializar variables de maenra mas efectva y limpia
        _spawnBullets = GetNode<Marker2D>("Weapons/Marker2D");
        _spawnBullets2 = GetNode<Marker2D>("Weapons/Marker2D2");
        _spawnBullets3 = GetNode<Marker2D>("Weapons/Marker2D3");

    }

    public override void _Process(double delta)
    {
        Attack();
    }

    public override void _PhysicsProcess(double delta)
    {
        Movement();
        MoveAndSlide();
    }

    private void Movement()
    {
        // obtenemos la direccion del input
        Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
        
        // almacenamos en velocity la direccion normalizada por la velocidad
        Velocity = inputDirection.Normalized() * speed;

        LimitOfMovement();
    }
    
    private void LimitOfMovement()
    {
        if(Position.X < 2)
        {
            Position = new Vector2(Position.X + 30, Position.Y);
        }
        else if(Position.X > 478)
        {
            Position = new Vector2(Position.X - 30, Position.Y);
        }
        else if(Position.Y < 2)
        {
            Position = new Vector2(Position.X, Position.Y + 30);
        }
        else if(Position.Y > 718)
        {
            Position = new Vector2(Position.X, Position.Y - 30);
        }
        
    }

    private void Attack()
    {
        // 
        if (Input.IsActionJustPressed("attack"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if(_levelAttack == 1)
        {
            Area2D newBullet = (Area2D)bullet.Instantiate();
            newBullet.GlobalPosition = _spawnBullets.GlobalPosition;
            GetParent().AddChild(newBullet);
        }
        if(_levelAttack == 2)
        {
            Area2D newBullet2 = (Area2D)bullet.Instantiate();
            newBullet2.GlobalPosition = _spawnBullets2.GlobalPosition;
            GetParent().AddChild(newBullet2);

            Area2D newBullet3 = (Area2D)bullet.Instantiate();
            newBullet2.GlobalPosition = _spawnBullets3.GlobalPosition;
            GetParent().AddChild(newBullet3);
        }
        if(_levelAttack == 3)
        {
            Area2D newBullet = (Area2D)bullet.Instantiate();
            newBullet.GlobalPosition = _spawnBullets.GlobalPosition;
            GetParent().AddChild(newBullet);

            Area2D newBullet2 = (Area2D)bullet.Instantiate();
            newBullet2.GlobalPosition = _spawnBullets2.GlobalPosition;
            GetParent().AddChild(newBullet2);

            Area2D newBullet3 = (Area2D)bullet.Instantiate();
            newBullet3.GlobalPosition = _spawnBullets3.GlobalPosition;
            GetParent().AddChild(newBullet3);
        }
    }
}
