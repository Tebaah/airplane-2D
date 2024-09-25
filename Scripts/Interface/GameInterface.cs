using Godot;
using System;

public partial class GameInterface : GraphicalInterface
{
    // variables

    // global
    private Global _global;

    // variables de componentes de interfaz
    private Label _score;
    private Label _gameOver;
    private Label _lastScore;
    private ColorRect _background;
    private Button _restartButton;
    private Button _saveScore;
    private LineEdit _namePlayer;
    
    // metodos
    public override void _Ready()
    {
        // inicializacion de varaibles y nodos
        _score = GetNode<Label>("Container/TopContainer/InformationContainer/Score");
        _gameOver = GetNode<Label>("Container/MiddleContainer/GameOver");
        _background = GetNode<ColorRect>("Container/BackgroundColor");
        _restartButton = GetNode<Button>("Container/BottomContainer/ButtonsContainer/RestartButton");
        _saveScore = GetNode<Button>("Container/BottomContainer/ButtonsContainer/SaveScore");
        _namePlayer = GetNode<LineEdit>("Container/MiddleContainerName/TextField");
        _lastScore = GetNode<Label>("Container/TopContainer/InformationContainer/Ranking");

        // inicializacion de variables globales y externas 
        _global = GetNode<Global>("/root/Global");
        _lastScore.Text = $"Last Score: {_global.rankingPlayer} {_global.rankingScore}";
    }

    public override void _Process(double delta)
    {
        // actualizar el puntaje y mostrarlo en pantalla
        int value = _global.score;
        _score.Text = $"Score: {value}";

        // si la vida del jugador es menor o igual a 0, se muestra el mensaje de game over
        if(_global.life <= 0)
        {
            _background.Visible = true;
            _gameOver.Visible = true;
            _restartButton.Visible = true;
            _saveScore.Visible = true;
            _namePlayer.Visible = true;
        }
    }

    public void OnRestartPressed()
    {
        // al presionar el boton de reiniciar, se reinicia el juego y sus variables
        ChangeGlobalValues();
        GetTree().ChangeSceneToFile("res://Scenes/Levels/menu_level.tscn");
    }

    public void OnSaveScorePressed()
    {
        _global.playerName = _namePlayer.Text;
        _global.SaveScore();
        _global.LoadScore();
        ChangeGlobalValues();
        GetTree().ChangeSceneToFile("res://Scenes/Levels/menu_level.tscn");
    }

    public void ChangeGlobalValues()
    {
        _global.score = 0;
        _global.life = 1;
        _global.isPlaying = false;
    }
}
