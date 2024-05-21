using Godot;
using Shmup.Code;
using System;

public partial class EnemyHex : Enemy
{
	[Export]
	public Node2D Eye;
	[Export]
	public CurseMarker Marker;
    [Export]
    public PackedScene SparklePrefab;
    [Export]
	public float HomingDuration;
	[Export]
	public float ChargeDuration;
    [Export]
    public float RestDuration;

    public float attackTime;
	bool sparkled;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		var player = Game.Instance?.Player;
		bool isOnScreen = MainCamera.Instance?.IsInView(GlobalPosition) ?? false;

        float homingSlide = attackTime / HomingDuration;
        float chargeSlide = attackTime / ChargeDuration;
        float restSlide = (attackTime - ChargeDuration) / RestDuration;

        if (player.IsValid() && (isOnScreen || attackTime > 0))
		{
			var deltaVec = player.GlobalPosition - GlobalPosition;
            attackTime += (float)delta;
			
			if (homingSlide <= 1)
			{
				if (Eye.IsValid())
				{
					Eye.Position = deltaVec.Normalized() * 4;
				}
				sparkled = false;
            }
			else if(!sparkled)
			{
				var sparkle = SparklePrefab.Instantiate<Node2D>();
				Eye.AddChild(sparkle);
				sparkled = true;
            }
        }

		if(Marker.IsValid())
		{
			if (homingSlide <= 1)
			{
				Marker.GlobalPosition = Marker.GlobalPosition.Lerp(player.GlobalPosition, 0.15f);
			}

            if (chargeSlide >= 1)
            {
				Marker.Emit();
            }

            if (restSlide > 1)
            {
                attackTime = 0;
                Marker.Reset();
            }

            Marker.SetCharge(homingSlide, chargeSlide);
        }
	}
}
