using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private enum PlayerState { Idle, Running, Attack, Jump }
	private PlayerState state = PlayerState.Idle;
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	private AnimatedSprite2D sprite;
	private Label label;
	private CollisionShape2D hitbox, bottom, playerBox;

	private bool attack = false, jump = false;


	public override void _Ready()
	{
		var area2D = GetNode<Area2D>("Area2D");
		hitbox = area2D.GetNode<CollisionShape2D>("hitbox");

		bottom = area2D.GetNode<CollisionShape2D>("bottom");

		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		label = GetNode<Label>("Label");
		playerBox = GetNode<CollisionShape2D>("CollisionShape2D");

	}

	public override void _PhysicsProcess(double delta)
	{
		Update(delta);
		MoveAndSlide();
	}

	public override void _Process(double delta)
	{
		var radius = 8.00f;
		var x = 0.00f;

		switch (state)
		{
			case PlayerState.Idle:
				sprite.Play("idle");
				break;
			case PlayerState.Running:
				sprite.Play("run");
				radius = 14.25f;
				if (sprite.FlipH) x = -6.00f;
				else x = 6.00f;
				break;
			case PlayerState.Attack:
				sprite.Play("attack");
				break;
			case PlayerState.Jump:
				radius = 12.00f;
				if (sprite.FlipH) x = 8.00f;
				else x = -8.00f;
				sprite.Play("jump");
				break;
			default:
				radius = 8.00f;
				x = 0.00f;
				break;
		}

		if (playerBox?.Shape is CapsuleShape2D rectangle)
		{
			rectangle.Radius = radius;
			playerBox.Position = new Vector2(x, 0);
		}

		label.Text = state.ToString() + "\nFLOOR:" + IsOnFloor() + "\nFlip:" + sprite.FlipH;


		if (Input.IsActionJustPressed("player_attack")) attack = true;


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



	private void Update(double delta)
	{
		Vector2 velocity = Velocity;
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		// Add the gravity.
		if (!IsOnFloor()) velocity += GetGravity() * (float)delta;

		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
			jump = true;
		}

		// BEGIN STATES
		if (direction == Vector2.Left || direction == Vector2.Right)
		{
			if (IsOnFloor()) state = PlayerState.Running;
			else state = PlayerState.Jump;
			attack = false;
		}
		else if (attack)
		{
			state = PlayerState.Attack;
			if (sprite.Frame == sprite.SpriteFrames.GetFrameCount("attack") - 1) attack = false;
		}
		else if (jump)
		{
			state = PlayerState.Jump;
			if (sprite.Frame == sprite.SpriteFrames.GetFrameCount("jump") - 1 || IsOnFloor())
				jump = false;
		}
		else if (velocity == Vector2.Zero) state = PlayerState.Idle;
		// END STATES


		if (state.Equals(PlayerState.Running) || state.Equals(PlayerState.Jump)) velocity.X = direction.X * Speed;
		else velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);

		Velocity = velocity;
	}
}
