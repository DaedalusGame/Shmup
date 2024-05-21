using Godot;
using Shmup.Code;
using System;

public partial class Flash : Node2D
{
    [Export]
    public Node2D Star;
    [Export]
    public Node2D Wave;
    [Export]
    public float Duration = 0.33f;

    float time;
    ShaderMaterial shaderMaterial;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		time += (float)delta;

		float slide = time / Duration;

        Star.Rotation = LerpHelper.QuadraticOut(0, Mathf.Tau * 0.5f, slide);
        Star.Scale = Mathf.Sin(slide * Mathf.Pi) * Vector2.One;

        shaderMaterial ??= Wave.Material as ShaderMaterial;

        shaderMaterial.SetShaderParameter("minRadius", LerpHelper.QuadraticOut(0, 1, slide));
        shaderMaterial.SetShaderParameter("maxRadius", Mathf.Min(LerpHelper.QuadraticOut(0, 1, slide) + 0.25f, 1f));

        if(slide > 1)
        {
            QueueFree();
        }
    }
}
