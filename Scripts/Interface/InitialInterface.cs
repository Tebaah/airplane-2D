using Godot;
using System;

public partial class InitialInterface : CanvasLayer
{
    // variables
    private Label _message;
    private AudioStreamPlayer _audio;

    // medotos
    public override void _Ready()
    {
        _message = GetNode<Label>("Control/MessageContainer/Message");
        _audio = GetNode<AudioStreamPlayer>("Control/ButtonContainer/AudioButton");
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
        await ToSignal(GetTree().CreateTimer(2), "timeout");
        _message.VisibleRatio += 0.05f;
    }
}
