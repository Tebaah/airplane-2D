using Godot;
using System;

public partial class HomeInterface : GraphicalInterface
{
    // metodos
    public override void _Ready()
    {
        ChangeScene();
    }

    public async void ChangeScene()
    {
        await ToSignal(GetTree().CreateTimer(3), "timeout");
        GetTree().ChangeSceneToFile("res://Scenes/Levels/menu_level.tscn");
    }
}
