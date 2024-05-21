using Godot;
using Shmup.Code;
using System;

public partial class Petal : Bullet
{
    static Random random = new Random();

    [Export]
    public Node2D Hitbox;

    float rotate;
    float rotateSelf;

    public override void _Ready()
    {
        base._Ready();
        Rotation = random.NextFloat() * Mathf.Tau;
        Hitbox.Rotation = random.NextFloat() * Mathf.Tau;
        Hitbox.Scale = Vector2.One * random.NextFloat(0.5f, 1.0f);
        Hitbox.Position = new Vector2(random.NextFloat(8f, 24f), 0);
        rotate = random.NextFloat(0.15f, 0.30f) * (random.NextFloat() > 0.5f ? 1 : -1);
        rotateSelf = random.NextFloat(0.05f, 0.20f) * (random.NextFloat() > 0.5f ? 1 : -1);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Rotation += (float)delta * Mathf.Tau * rotate;
        Hitbox.Rotation += (float)delta * Mathf.Tau * rotateSelf;
    }
    public override bool Hit(IHurter hurter)
    {
        return base.Hit(hurter);
    }
}
