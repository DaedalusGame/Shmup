using Godot;
using Shmup.Code;
using System;

public partial class CurseMarker : Node2D
{
    [Export]
    public PackedScene EmitterPrefab;
	[Export]
	public CpuParticles2D Particles;

	float homing;
    float charge;
    public bool Emitted;

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Particles.IsValid())
        {
            Particles.SelfModulate = homing >= 1 ? new Color(2, 2, 2, 1) : new Color(1, 1, 1, 1);
        }
    }

    public void SetCharge(float homingSlide, float chargeSlide)
	{
		homing = homingSlide;
        charge = chargeSlide;

        if (Particles.IsValid())
		{
			Particles.Emitting = homingSlide > 0 && !Emitted;
        }
	}

    public void Emit()
    {
        if(Emitted)
        {
            return;
        }

        var bulletEmitter = EmitterPrefab.Instantiate<Node2D>();
        AddChild(bulletEmitter);
        bulletEmitter.GlobalPosition = GlobalPosition;

        Emitted = true;
    }

    public void Reset()
    {
        GlobalPosition = GetParent<Node2D>().GlobalPosition;
        Emitted = false;
    }
}
