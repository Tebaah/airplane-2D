using Godot;
using System;

public partial class LevelManager : Node2D
{
    // variables

    // spawn de enemigos
    [Export] public PackedScene enemy;
    private Marker2D _horizontalMarker;
    private float _time = 0.75f;
    private bool _canSpawn = true;

    // global variables
    private Global _global;

    // metodos
    public override void _Ready()
    {
        // inicializamos el marcador de spawn de enemigos
        _horizontalMarker = GetNode<Marker2D>("EnemySpawn/Marker2D");
        _global = GetNode<Global>("/root/Global");
    }

    public override void _Process(double delta)
    {
        // creamos un valor random para la posicion de los marcadores en x
        Random random = new Random();
        int positionX = random.Next(2, 478);

        // asignamos la posicion de los marcadores
        _horizontalMarker.Position = new Vector2(positionX, -5);

        // llamamos al metodo de spawn de enemigos
        if(_global.life == 1)
        {
            HorizontalSpawnEnemy();
        }
    }

    private async void HorizontalSpawnEnemy()
    {
        // si _canSpawn es true, iniciamos el spawn de enemigos
        if(_canSpawn == true)
        {
            // instanciamos un nuevo enemigo
            Area2D newEnemy = (Area2D)enemy.Instantiate();
            newEnemy.GlobalPosition = _horizontalMarker.GlobalPosition;
            GetParent().AddChild(newEnemy);
            _canSpawn = false;

            // esperamos un tiempo para volver a instanciar un nuevo enemigo
            await ToSignal(GetTree().CreateTimer(_time), "timeout");
            _canSpawn = true;
        }
    }


}
