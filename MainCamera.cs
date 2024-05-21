using Godot;
using System;

public partial class MainCamera : Camera2D
{
	public static MainCamera Instance { get; private set; } 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
    }

	public bool IsInView(Vector2 pos, float grow = 0)
	{
		var viewportPos = GetViewportTransform() * pos;
		return GetViewportRect().Grow(grow).HasPoint(viewportPos);
    }
}
