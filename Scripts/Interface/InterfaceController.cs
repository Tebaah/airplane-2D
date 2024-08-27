using Godot;
using System;

public partial class InterfaceController : CanvasLayer
{
    // variables

    // global
    private Global _global;

    // informacion del juego
    private Label _score;
    private Label _gameOver;
    private ColorRect _background;

    // metodos
    public override void _Ready()
    {
        // inicializar variables
        _global = GetNode<Global>("/root/Global");
        _score = GetNode<Label>("Control/VBoxContainer/HBoxContainer/Score");
        _gameOver = GetNode<Label>("Control/VBoxContainer2/GameOver");
        _background = GetNode<ColorRect>("Control/ColorRect");
    }

    public override void _Process(double delta)
    {
        // actualizar el puntaje y mostrarlo en pantalla
        int value = _global.score;
        _score.Text = $"Score: {value}";

        // si la vida del jugador es menor o igual a 0, se muestra el mensaje de game over
        if(_global.life <= 0)
        {
            _gameOver.Visible = true;
            _background.Visible = true;
        }
    }
}
