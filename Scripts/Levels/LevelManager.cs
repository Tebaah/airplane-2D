using Godot;
using System;

public partial class LevelManager : Node2D
{
    // variables

    // spawn de enemigos
    [Export] public PackedScene[] enemy;
    private Marker2D _enemyMarker;
    private float _time = 0.75f;
    private bool _canSpawn = true;
    private int _enemyIndex;
    
    // spawn de powerups
    [Export] public PackedScene powerup;
    private Marker2D _powerupMarker;
    private float _timePowerup = 10f;
    private bool _canSpawnPowerup = true;
    // global variables
    private Global _global;

    // metodos
    public override void _Ready()
    {
        // inicializamos el marcador de spawn de enemigos
        _enemyMarker = GetNode<Marker2D>("EnemySpawn/Marker2D");
        _global = GetNode<Global>("/root/Global");
        _powerupMarker = GetNode<Marker2D>("PowerUpSpawn/Marker2D");

    }

    public override void _Process(double delta)
    {
        // creamos un valor random para la posicion de los marcadores en x
        Random random = new Random();
        int positionX = random.Next(2, 478);

        // asignamos la posicion de los marcadores
        _enemyMarker.Position = new Vector2(positionX, -5);
        _powerupMarker.Position = new Vector2(positionX, -5);

        // llamamos al metodo de spawn de enemigos
        if(_global.life == 1)
        {
            SpawnEnemy();
            SpawnPowerup();
        }
    }

    private async void SpawnEnemy()
    {
        Random random = new();
        _enemyIndex = random.Next(0, enemy.Length);
        
        // si _canSpawn es true, iniciamos el spawn de enemigos
        if(_canSpawn == true)
        {
            // instanciamos un nuevo enemigo
            Area2D newEnemy = (Area2D)enemy[_enemyIndex].Instantiate();
            newEnemy.GlobalPosition = _enemyMarker.GlobalPosition;
            GetParent().AddChild(newEnemy);
            _canSpawn = false;

            // esperamos un tiempo para volver a instanciar un nuevo enemigo
            await ToSignal(GetTree().CreateTimer(_time), "timeout");
            _canSpawn = true;
        }
    }

    private async void SpawnPowerup()
    {
        // si _canSpawnPowerup es true, iniciamos el spawn de powerups
        if(_canSpawnPowerup == true)
        {
            // instanciamos un nuevo powerup
            Area2D newPowerUp = (Area2D)powerup.Instantiate();
            newPowerUp.GlobalPosition = _powerupMarker.GlobalPosition;
            GetParent().AddChild(newPowerUp);
            _canSpawnPowerup = false;

            // esperamos un tiempo para volver a instanciar un nuevo powerup
            await ToSignal(GetTree().CreateTimer(_timePowerup), "timeout");
            _canSpawnPowerup = true;
        }
    }

}
