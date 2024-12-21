using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private enum PlayerState { Idle, Running, Attack }
	private PlayerState state = PlayerState.Idle;
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	private AnimatedSprite2D sprite;
	private Label label;
	private CollisionShape2D hitbox;

	private bool attack = false;

	public override void _Ready()
	{
		var area2D = GetNode<Area2D>("Area2D");
		hitbox = area2D.GetNode<CollisionShape2D>("hitbox");

		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		label = GetNode<Label>("Label");
	}

	public override void _PhysicsProcess(double delta)
	{
		Update();
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
			case PlayerState.Attack:
				sprite.Play("attack");
				break;
		}

		label.Text = state.ToString();


		if (Input.IsActionJustPressed("player_attack"))
		{
			attack = true;
		}

		/*  */

		// Flip sprite based on player movement direction
		if (Input.IsActionPressed("ui_right"))
		{
			sprite.FlipH = false; // Facing right
			hitbox.Position = new Vector2(24, hitbox.Position.Y);
		}
		else if (Input.IsActionPressed("ui_left"))
		{
			sprite.FlipH = true; // Facing left
			hitbox.Position = new Vector2(-24, hitbox.Position.Y);
		}

	}



	private void Update()
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

		//STATES
		if (direction == Vector2.Left || direction == Vector2.Right)
		{
			state = PlayerState.Running;
			attack = false;
		}

		else if (attack)
		{
			state = PlayerState.Attack;
			if (sprite.Frame == sprite.SpriteFrames.GetFrameCount("attack") - 1) attack = false;
		}
		else state = PlayerState.Idle;


		if (state.Equals(PlayerState.Running))
		{
			if (direction != Vector2.Zero)
				velocity.X = direction.X * Speed;
			else
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}
		else if (state.Equals(PlayerState.Attack))
		{
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
