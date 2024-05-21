using Godot;
using Shmup.Code;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

public partial class Player : Node2D, IWeaponHaver, IHurter, IFactionMember
{
	[Export]
	PackedScene BulletPrefab;
    [Export]
    PackedScene AfterBurnerPrefab;

	[Export]
	PlayerBody PlayerBody;

    [Export]
	Vector2 Velocity;
	[Export]
	float ShootDelay;

	[Export] 
	float AfterBurnerStart;
    [Export]
    float AfterBurnerDelay;

    float shootTime;
	float afterBurnerTime;
    int flash = 0;
    float hurtTime = 0;

    public float IdleTime;
	public LerpSlide<Vector2> Knockback = new LerpSlide<Vector2>(Vector2.Zero, 0);
    public LerpSlide<float> KnockbackDI = new LerpSlide<float>(1, 0);

    List<Weapon> weapons = new List<Weapon>();

    public Faction Faction => Faction.Player;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        flash++;

        hurtTime -= (float)delta;

        if (hurtTime > 0)
        {
            if (flash % 2 == 0)
            {
                Modulate = new Color(3, 3, 3, 1);
            }
            else
            {
                Modulate = new Color(0.5f, 0.5f, 0.5f, 1);
            }
        }
        else
        {
            Modulate = new Color(1, 1, 1, 1);
        }
    }

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey eventKey)
		{
			/*if(eventKey.Keycode == Key.W)
			{
				int bulletCount = 10;
				for (int i = 0; i < bulletCount; i++)
				{
					float angle = 2 * Mathf.Pi * (float)i / bulletCount;
					
					Node node = BulletPrefab.Instantiate();
					if (node is Bullet bullet)
					{
						GetParent().AddChild(bullet);
						bullet.Position = Position;
						bullet.Speed = Vector2.FromAngle(angle) * 100;
					}
				}
            }*/
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		var move = new Vector2();

		bool up = Input.IsKeyPressed(Key.W);
		bool left = Input.IsKeyPressed(Key.A);
		bool down = Input.IsKeyPressed(Key.S);
		bool right = Input.IsKeyPressed(Key.D);

		var knockback = !Knockback.Done;
		var knockbackDelta = Vector2.Zero;
		float di = 1;
		if (knockback)
		{
			KnockbackDI.Update((float)delta);
			knockbackDelta -= Knockback.Get();
			Knockback.Update((float)delta);
			knockbackDelta += Knockback.Get();
			di = KnockbackDI.Get();
		}

		if (up)
		{
			move.Y -= Velocity.Y * (float)delta;
		}
		if (left)
		{
			move.X -= Velocity.X * (float)delta;
		}
		if (down)
		{
			move.Y += Velocity.X * (float)delta;
		}
		if (right)
		{
			move.X += Velocity.Y * (float)delta;
		}

		if(up && move.Y < 0)
		{
            afterBurnerTime += (float)delta;
        }
		else
		{
            afterBurnerTime = 0;
        }

		PlayerBody.SetDirection(move);

		if (!up && !left && !down && !right)
		{
			IdleTime += (float)delta;
		}
		else
		{
			IdleTime = 0;
		}

		if (afterBurnerTime >= AfterBurnerStart)
		{
			if (afterBurnerTime >= AfterBurnerDelay + AfterBurnerStart)
			{
				ShootAfterburner();
				afterBurnerTime -= AfterBurnerDelay;
			}
		}

		/*if (Input.IsKeyPressed(Key.Space))
		{
			shootTime += (float)delta;
			if(shootTime >= ShootDelay)
            {
                ShootBullet();
                shootTime -= ShootDelay;
            }
        }
		else
		{
			shootTime = 0;
		}*/

		//if (Input.IsKeyPressed())

		var position = Position;

		if (!knockback)
		{
            position += move;
		}
		else
		{
            position += knockbackDelta + move * di;
		}

		var rect = MainCamera.Instance.GetViewportRect();

        position.X = Mathf.Clamp(position.X, -LevelSegment.LEVEL_WIDTH / 2 + 4, +LevelSegment.LEVEL_WIDTH / 2 - 4);
		position.Y = Mathf.Clamp(position.Y, -rect.Size.Y / 4 + 8, rect.Size.Y / 4 - 8);
        Position = position;
	}

    private void ShootBullet()
    {
        Node node = BulletPrefab.Instantiate();
        if (node is Bullet bullet)
        {
            bullet.Position = Position;
			bullet.Shooter = this;
            bullet.Speed = new Vector2(0, -200);
        }
        GetParent().AddChild(node);
    }

    private void ShootAfterburner()
    {
        Node node = AfterBurnerPrefab.Instantiate();
        if (node is Afterburner afterburner)
        {
            afterburner.Position = Position;
			afterburner.Shooter = this;
        }
        GetParent().AddChild(node);
    }

    public void AddWeapon(Weapon weapon)
    {
		weapons.Add(weapon);
    }

	static Random random = new Random();

    public bool TakeDamage(float damage, bool contact = false)
    {
		if(hurtTime > 0)
		{
			return false;
		}
		var distance = 40;
		var randomOffset = Vector2.FromAngle(random.NextFloat() * Mathf.Tau) * distance;
		Knockback = new LerpSlide<Vector2>(slide => Vector2.Zero.Lerp(randomOffset, LerpHelper.CubicOut(0,1,slide)), 0.8f);
		KnockbackDI = new LerpSlide<float>(slide => LerpHelper.CubicIn(0.2f, 1, slide), 0.8f);
		hurtTime = 1.4f;


        return true;
    }
}
