using Godot;
using System;
using System.Security.AccessControl;

public partial class EnvironmentController : Node2D
{
    // variables
    private Parallax2D _largeObsjects;
    private Parallax2D _smallObsjects;
    public int positionLargeObjects;
    public int positionSmallObjects;
    public bool canChangePosition = true;

    public override void _Ready()
    {
        _largeObsjects = GetNode<Parallax2D>("ParallaxLargeObjects");
        _smallObsjects = GetNode<Parallax2D>("ParallaxSmallObjects");
    }
    public override void _Process(double delta)
    {
        SetPositionX();

        _largeObsjects.ScrollOffset = new Vector2(positionLargeObjects, _largeObsjects.ScrollOffset.Y);
        _smallObsjects.ScrollOffset = new Vector2(positionSmallObjects, _smallObsjects.ScrollOffset.Y);
    }

    public async void SetPositionX()
    {
        if(canChangePosition == true)
        {
            Random random = new Random();
            positionLargeObjects = random.Next(0, 480);
            positionSmallObjects = random.Next(0, 480);
            canChangePosition = false;
        
            await ToSignal(GetTree().CreateTimer(2), "timeout");
            canChangePosition = true;
        }


    }

}
