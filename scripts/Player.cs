using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private enum PlayerState { Idle, Running, Attack }
	private PlayerState state = PlayerState.Idle;
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	private AnimatedSprite2D sprite;

	public override void _Ready()
	{
		// Get the Sprite2D node
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		Move();
		MoveAndSlide();
	}

	public override void _Process(double delta)
	{
		switch (state)
		{
			case PlayerState.Idle:
				sprite.Play("idle");
				break;
			case PlayerState.Running:
				sprite.Play("run");
				break;
		}

		// Flip sprite based on player movement direction
		if (Input.IsActionPressed("ui_right"))
		{
			sprite.FlipH = false; // Facing right
		}
		else if (Input.IsActionPressed("ui_left"))
		{
			sprite.FlipH = true; // Facing left
		}
	}

	private void Move()
	{
		// Handle Jump.
		Vector2 velocity = Velocity;

		// Add the gravity.
		// if (!IsOnFloor())
		// {
		// 	velocity += GetGravity() * (float)delta;
		// }

		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (direction != Vector2.Zero) state = PlayerState.Running;
		else state = PlayerState.Idle;

		GD.Print(direction);



		if (state.Equals(PlayerState.Running))
		{
			if (direction != Vector2.Zero)
				velocity.X = direction.X * Speed;
			else
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}
		else
		{
			velocity.X = 0;
			velocity.Y = 0;
		}
		Velocity = velocity;
	}
}
