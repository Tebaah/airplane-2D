using Godot;
using System;

public partial class MenuInterface : GraphicalInterface
{
    // variables
    private Label _message;
    private AudioStreamPlayer _audio;
    private Global _global;

    // medotos
    public override void _Ready()
    {
        _message = GetNode<Label>("Container/MessageContainer/Mesagge");
        _audio = GetNode<AudioStreamPlayer>("Container/ButtonContainer/StartAudio");
    }
    public override void _Process(double delta)
    {
        MeessageOnTheScreen();
    }
    public async void OnStartButtonPressed()
    {   
        _audio.Play();
        await ToSignal(_audio, "finished");
        GetTree().ChangeSceneToFile("res://Scenes/Levels/normal_level.tscn");
    }

    public async void MeessageOnTheScreen()
    {
        await ToSignal(GetTree().CreateTimer(0.75), "timeout");
        _message.VisibleRatio += 0.05f;
    }
}
