using Godot;
using Shmup.Code;
using System;

public partial class Doll : Enemy
{
    [Export]
    AnimatedSprite2D Body;
    [Export]
    AnimatedSprite2D Swipe;

	[Export]
	public PackedScene BulletEmitterPrefab;
    [Export]
    public PackedScene SparklePrefab;
    [Export]
    public Vector2 SparkleMuzzle;
    [Export]
    public Vector2 AppearOffset;
    [Export]
    public float AppearDuration;
    [Export]
    public Vector2 Velocity;
    [Export]
    public bool CompensateScroll;
    [Export]
    public Vector2 DisappearOffset;
    [Export]
    public float DisappearDuration;
    [Export]
    public float AttackDelay;
    [Export]
    public float ChargeDelay;
    [Export]
    public float RestDelay;
    [Export]
    public int AttackCount = 1;

    float lifeTime;
    float attackTime;
    float disappearTime;
    Vector2? origin;
    bool fired;
    bool charged;
    int attacks;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
        Visible = false;
	}

	public void UpdatePosition(float delta)
    {
        var oldPosition = Position;

        origin ??= Position;

        Vector2 appearOffset = AppearOffset;
        Vector2 disappearOffset = DisappearOffset;
        Vector2 velocity = Velocity;

        if(CompensateScroll)
        {
           velocity -= new Vector2(0, Game.Instance.Level.ScrollingSpeed);
        }

        velocity.X *= MirrorMod;
        appearOffset.X *= MirrorMod;
        disappearOffset.X *= MirrorMod;

        if (lifeTime < AppearDuration)
        {
            
            Position = origin.Value + appearOffset.Lerp(Vector2.Zero, LerpHelper.Quadratic(0, 1, lifeTime / AppearDuration));
        }
        else if(disappearTime > 0 && disappearTime < DisappearDuration)
        {
            
            Position = origin.Value + Vector2.Zero.Lerp(disappearOffset, LerpHelper.Quadratic(0, 1, disappearTime / DisappearDuration));
        }
        else
        {
            if (detach)
            {
                origin = GetParent<Node2D>().ToGlobal(origin.Value);
                this.Reparent(Game.Instance);
                detach = false;
            }
            Position = origin.Value;
        }

        origin = origin + velocity * delta * LerpHelper.QuadraticIn(0, 1, Mathf.Clamp(lifeTime / AppearDuration, 0, 1));

        var deltaMove = Position - oldPosition;
        UpdateAnimation(deltaMove);
    }

    private void UpdateAnimation(Vector2 deltaMove)
    {
        if (attackTime > 0)
        {
            if (attackTime < ChargeDelay)
            {
                Body.Animation = "charge";
            }
            else
            {
                Body.Animation = "fire";
            }
        }
        else if (deltaMove.X > 0.01)
        {
            Body.Animation = "right";
        }
        else if (deltaMove.X < -0.01)
        {
            Body.Animation = "left";
        }
        else
        {
            Body.Animation = "default";
        }
    }

    bool detach = false;

    public override void _PhysicsProcess(double delta)
    {
        CheckDeath();
        CheckActive(origin);

        var deltaTrue = TimeController.Update(delta);

        Visible = true;

        UpdatePosition(deltaTrue);

        if (Active)
        {
            lifeTime += deltaTrue;

            if (lifeTime > AppearDuration)
            {
                if ((AttackCount <= 0 || attacks < AttackCount))
                {
                    attackTime += deltaTrue;

                    if(attackTime > 0 && !charged)
                    {
                        charged = true;
                        var sparkle = SparklePrefab.Instantiate<Node2D>();
                        sparkle.Position = SparkleMuzzle;
                        AddChild(sparkle);
                    }
                    if (attackTime > ChargeDelay && !fired)
                    {
                        fired = true;
                        Swipe.Visible = true;
                        Swipe.Frame = 0;
                        Swipe.Play();
                        MakeEmitter();
                    }
                    if (attackTime > ChargeDelay + RestDelay)
                    {
                        fired = false;
                        charged = false;
                        attackTime = 0;
                        attacks++;
                    }
                }
                else
                {
                    disappearTime += deltaTrue;

                    if(disappearTime >= DisappearDuration)
                    {
                        QueueFree();
                    }
                }
            }
        }
    }

    private void MakeEmitter()
    {
        var bulletEmitter = BulletEmitterPrefab.Instantiate<Node2D>();
        AddChild(bulletEmitter);
        bulletEmitter.GlobalPosition = GlobalPosition;
    }
}
