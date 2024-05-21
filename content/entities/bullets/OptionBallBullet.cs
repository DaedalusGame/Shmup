using Godot;
using Shmup.Code;
using System;

public partial class OptionBallBullet : Bullet
{
    [Export]
    public CpuParticles2D[] SprayParticles;

    float sprayTime;

    public override void _Process(double delta)
    {
        base._Process(delta);

        var deltaReal = TimeController.GetDeltaTime(delta);

        if (sprayTime > 0)
        {
            sprayTime -= deltaReal;
        }
        else
        {
            sprayTime = 0;
        }

        foreach(var particles in SprayParticles)
        {
            particles.Emitting = sprayTime > 0;
        }
    }

    public override bool Hit(IHurter hurter)
    {
        var result = base.Hit(hurter);
        if (result)
        {
            sprayTime = 0.1f;
            TimeController.SlowTime = Math.Max(TimeController.SlowTime, 0.2f);
            if(hurter is Enemy enemy)
            {
                enemy.TimeController.SlowTime = Math.Max(enemy.TimeController.SlowTime, 0.2f);
            }
        }

        return result;
    }
}
