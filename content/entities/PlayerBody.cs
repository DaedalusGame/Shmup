using Godot;
using Shmup.Code;
using System;

public partial class PlayerBody : AnimatedSprite2D
{
	[Export]
	AnimatedSprite2D Hair;
    [Export]
    Sprite2D Wings;
    [Export]
    float WingFlapDuration = 0.5f;

	float wingFlapSpeed;
	float wingFlapTime;

	public override void _Ready()
	{
	}

	public void SetDirection(Vector2 dir)
	{
		if(dir.Y < 0 || (dir.Y >= 0 && !Mathf.IsZeroApprox(dir.X)))
		{
			wingFlapSpeed = 1.0f; 
		}
		else if(dir.Y > 0)
		{
            wingFlapSpeed = 0;
        }
		else
		{
            wingFlapSpeed = 0.75f;
        }

        if (dir.X > 0)
		{
			setPose("right");
        }
		else if(dir.X < 0)
		{
            setPose("left");
        }
		else
		{
            setPose("default");
        }
	}

	private void setPose(string name)
	{
		Animation = name;
		Hair.Animation = name;
    }

	public override void _Process(double delta)
	{
        wingFlapTime += (float)delta * wingFlapSpeed;
		if(wingFlapTime >= WingFlapDuration)
		{
			wingFlapTime -= WingFlapDuration;
		}

        float wingScale = LerpHelper.QuadraticOut(0.1f, 1f, wingFlapTime / WingFlapDuration);

		if(wingFlapSpeed <= 0)
		{
			wingScale = 1;
		}

        Wings.Scale = new Vector2(wingScale, 1);
	}
}
