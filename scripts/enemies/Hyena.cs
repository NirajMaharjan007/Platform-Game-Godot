using Godot;


public partial class Hyena : Enemy
{
	private EnemyState state;
	public override void _Ready()
	{
		base._Ready();
		speed = 350.0f;
	}
	public override void _Process(double delta)
	{
		base._Process(delta);
		switch (state)
		{
			case EnemyState.Idle:
				// Idle state logic
				break;
			case EnemyState.Hit:
				// Hit state logic
				break;
			case EnemyState.Running:
				// Attacking state logic
				break;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		MoveIt();
	}

	private void MoveIt()
	{
		var velocity = Velocity;
		if (IsOnFloor() && velocity == Vector2.Zero)
		{
			velocity.X = speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, speed);
		}
		Velocity = velocity;
	}

}
