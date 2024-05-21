using Godot;
using Shmup.Code;
using System;
using static Godot.HttpRequest;
using static System.Net.Mime.MediaTypeNames;

public partial class Orb : Enemy, IHitter
{
    [Export]
    public Vector2 Velocity;
    [Export]
    public Vector2 WaveAmplitude;
    [Export]
    public Vector2 WaveParams;

    Vector2? origin;
    float lifeTime;
	
	public override void _Ready()
	{
        base._Ready();
		
	}

    public override void _PhysicsProcess(double delta)
    {
        CheckDeath();

        var deltaTrue = TimeController.Update(delta);

        if (origin == null)
        {
            origin = Position;
        }

        lifeTime += deltaTrue;
        var waveParams = WaveParams * lifeTime;

        Vector2 velocity = Velocity;
        Vector2 waveAmplitude = WaveAmplitude;

        velocity.X *= MirrorMod;
        waveAmplitude.X *= MirrorMod;

        Position = origin.Value + velocity * lifeTime + new Vector2(Mathf.Sin(waveParams.X * Mathf.Tau), Mathf.Cos(waveParams.Y * Mathf.Tau)) * waveAmplitude;
    }

    public bool Hit(IHurter hurter)
    {
        return hurter.TakeDamage(10);
    }
}
