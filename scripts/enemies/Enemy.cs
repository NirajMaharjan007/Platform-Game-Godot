using Godot;

public partial class Enemy : CharacterBody2D
{
	protected enum EnemyState { Running, Idle, Jump, Hit };
	protected float speed = 0.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		Update(delta);
		MoveAndSlide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	protected void Update(double delta)
	{
		Vector2 velocity = Velocity;
		if (!IsOnFloor()) velocity += GetGravity() * (float)delta;
		Velocity = velocity;
	}
}
