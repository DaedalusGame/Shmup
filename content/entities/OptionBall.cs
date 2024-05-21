using Godot;
using Shmup.Code;
using System;

public partial class OptionBall : Bullet
{
    [Export]
    public PackedScene BulletPrefab;
	Hurtbox Hurtbox;

    [Export]
    public float ImmuneTime = 0.25f;

    public float time;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		Hurtbox = GetNode<Hurtbox>("Hurtbox");
        Hurtbox.OnHurt += Hurtbox_OnHurt;
    }

    private void Hurtbox_OnHurt(Hitbox other)
    {
        if (ImmuneTime < time)
        {
            Vector2 delta = (GlobalPosition - other.GlobalPosition).Normalized();

            ShootBullet(delta);

            this.QueueFree();
        }
    }

    private void ShootBullet(Vector2 direction)
    {
        var position = GlobalPosition;

        direction = (direction - new Vector2(0, 1.1f)).Normalized();
        direction.X *= 0.6f;

        Node node = BulletPrefab.Instantiate();
        if (node is Bullet bullet)
        {
            bullet.GlobalPosition = position;
            bullet.Shooter = Shooter;
            bullet.Speed = direction * 300;
            bullet.TopLevel = true;
        }
        GetParent().AddChild(node);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        time += (float)delta;
	}
}
