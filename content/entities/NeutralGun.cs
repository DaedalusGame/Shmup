using Godot;
using Shmup.Code;
using System;

public partial class NeutralGun : Weapon
{
    [Export]
    PackedScene BulletPrefab;
    [Export]
    PackedScene BlastPrefab;
    [Export]
    float ShootStartDelay;
    [Export]
    float ShootDelay;
    [Export]
    float BlastDelay;
    [Export]
    Vector2 MuzzlePosition;

    float shootTime;
    float blastTime;

    public override void _Ready()
    {
        base._Ready();

        blastTime = BlastDelay;
    }

    public override void _PhysicsProcess(double delta)
    {
        var position = Player.Position;

        if (Input.IsKeyPressed(Key.Space))
        {
            if(blastTime >= BlastDelay)
            {
                ShootBlast();
            }

            blastTime = 0;
            shootTime += (float)delta;
            if (shootTime >= ShootDelay + ShootStartDelay)
            {
                ShootBullet();
                shootTime -= ShootDelay;
            }
        }
        else
        {
            shootTime = 0;
            blastTime += (float)delta;
        }
    }

    private void ShootBullet()
    {
        var position = Player.GlobalPosition + MuzzlePosition;

        /*Node node = BulletPrefab.Instantiate();
        if (node is Bullet bullet)
        {
            bullet.Position = position;
            bullet.Shooter = this;
            bullet.Speed = new Vector2(0, -400);
        }
        Player.GetParent().AddChild(node);*/

        Player.ShootBullet(BulletPrefab, position, new Vector2(0, -600));
    }

    private void ShootBlast()
    {
        var position = MuzzlePosition;

        Player.ShootBullet(BlastPrefab, position, Vector2.Zero, Player);

        /*Node node = BlastPrefab.Instantiate();
        if (node is Bullet bullet)
        {
            bullet.Position = position;
            bullet.Shooter = this;
        }
        Player.AddChild(node);*/
    }

}
