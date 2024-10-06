using Godot;
using System;

public partial class LevelManager : Node2D
{
    #region variables
    // spawn de enemigos
    [Export] public PackedScene[] enemy;
    [Export] public PackedScene[] specialEnemy;
    private int _enemyCounter = 0;
    private Marker2D _enemyMarker;
    private Marker2D _specialEnemyMarker;
    private float _timeSpawn = 0.75f;
    private bool _canSpawn = true;
    private bool _canSpawnSpecial = true;
    private int _enemyIndex;
    private int _specialEnemyIndex;
    
    // spawn de powerups
    [Export] public PackedScene powerup;
    private Marker2D _powerupMarker;
    private float _timePowerup = 10f;
    private bool _canSpawnPowerup = true;

    //  spawn de jugador
    [Export] public PackedScene player;
    private Marker2D _playerMarker;

    // global variables
    private Global _global;
    #endregion

    #region metodos godot
    public override void _Ready()
    {
        // inicializa nodos y variables
        _playerMarker = GetNode<Marker2D>("PlayerSpawn/Marker2D");
        _enemyMarker = GetNode<Marker2D>("EnemySpawn/Marker2D");
        _specialEnemyMarker = GetNode<Marker2D>("SpecialEnemySpawn/Marker2D");
        _powerupMarker = GetNode<Marker2D>("PowerUpSpawn/Marker2D");
        _global = GetNode<Global>("/root/Global");

        SpawnPlayer();
        GameStarter();
    }

    public override void _Process(double delta)
    {
        // crea un valor random para la posicion de los marcadores en x
        Random random = new Random();
        int positionX = random.Next(2, 478);
        int positionXSpecial = random.Next(25, 455);

        // asigna la posicion de los marcadores
        _enemyMarker.Position = new Vector2(positionX, -5);
        _specialEnemyMarker.Position = new Vector2(positionXSpecial, -5);
        _powerupMarker.Position = new Vector2(positionX, -5);

        if(_global.life == 1 && _global.isPlaying == true)
        {
            SpawnEnemy();
            SpawnSpecialEnemy();
            SpawnPowerup();
        }
    }
    #endregion

    #region metodos
    private async void SpawnEnemy()
    {
        Random random = new();
        _enemyIndex = random.Next(0, enemy.Length);
        
        // si _canSpawn es true, iniciamos el spawn de enemigos
        if(_canSpawn == true)
        {
            // instanciamos un nuevo enemigo 
            Area2D newEnemy = enemy[_enemyIndex].Instantiate() as Area2D;
            newEnemy.GlobalPosition = _enemyMarker.GlobalPosition;
            GetParent().AddChild(newEnemy);
            _canSpawn = false;
            EnemyCounter();

            // despues de unos segundos se instancia un enemigo
            await ToSignal(GetTree().CreateTimer(_timeSpawn), "timeout");
            _canSpawn = true;
        }
    }

    public async void SpawnSpecialEnemy()
    {
        Random random = new();
        _specialEnemyIndex = random.Next(0, specialEnemy.Length);
        
        // cumplida la condicion, se instancia un enemigo especial
        if(_canSpawnSpecial ==true && _enemyCounter == 30)
        {
            Area2D newEnemySpecial = specialEnemy[_specialEnemyIndex].Instantiate() as Area2D;
            newEnemySpecial.GlobalPosition = _enemyMarker.GlobalPosition;
            GetParent().AddChild(newEnemySpecial);
            _canSpawnSpecial = false;

            await ToSignal(GetTree().CreateTimer(_timeSpawn), "timeout");
            _canSpawnSpecial = true;
        }
    }

    private async void SpawnPowerup()
    {
        // si _canSpawnPowerup es true, iniciamos el spawn de powerups
        if(_canSpawnPowerup == true && _enemyCounter == 20)
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

    private void EnemyCounter()
    {
        _enemyCounter++;
        if(_enemyCounter > 30)
        {
            _enemyCounter = 0;
        }
    }

    private void SpawnPlayer()
    {
        // instanciamos al jugador
        CharacterBody2D newPlayer = (CharacterBody2D)player.Instantiate();
        newPlayer.GlobalPosition = _playerMarker.GlobalPosition;
        AddChild(newPlayer);
    }

    private async void GameStarter()
    {
        await ToSignal(GetTree().CreateTimer(2), "timeout");
        _global.isPlaying = true;
    }
    #endregion
}
