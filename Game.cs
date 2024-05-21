using Godot;
using Shmup.Code;
using System;

public partial class Game : Node2D
{
	public static Game Instance { get; private set; }

	[Export]
	public Player Player { get; set; }
	[Export]
	public LevelGenerator Level;

	public DeltaController TimeController = new DeltaController();

	public override void _Ready()
	{
        Instance = this;
		//GetTree().DebugCollisionsHint = true;
    }

	public override void _Process(double delta)
	{
		TimeController.Update(delta);
	}
}
