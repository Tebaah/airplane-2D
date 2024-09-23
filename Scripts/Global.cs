using Godot;
using Godot.Collections;
using System;

public partial class Global : Node2D
{
    // variables

    // variables gloables del jugador
    public int life = 1;
    public int score = 0;
    public string playerName;
    public string rankingPlayer;
    public int rankingScore;

    // variables de estado del juego
    public bool isPlaying = false;

    // variables para el guardado de puntajes
    public string savePath = "user://scores.dat";
    public Dictionary data;

    // metodos
    public override void _Ready()
    {
        // inicializacion de variables
        LoadScore();
        GD.Print(rankingPlayer, rankingScore);
    }
    public void SaveScore()
    {
        // guardar el nombre y puntaje del jugador
        if(playerName != null)
        {
            var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Write);
            data = new Dictionary{{ "name", playerName },{ "score", score }};
            file.StoreVar(data);
            file.Close();
        }
    }

    public void LoadScore()
    {
        // cargar el nombre y puntaje del jugador
        if(FileAccess.FileExists(savePath))
        {
            var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Read);
            data = (Dictionary)file.GetVar();
            rankingPlayer = (string)data["name"];
            rankingScore = (int)data["score"];
            file.Close();
        }
    }
}
