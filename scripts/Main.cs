using Godot;

public partial class Main : Node2D
{
	CollisionShape2D collider;
	CharacterBody2D target, enemy;
	Camera2D camera;

	Node2D spawn;

	Color color;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var area2D = GetNode<Area2D>("Area2D");
		spawn = area2D.GetNode<Node2D>("SpawnPoint");
		collider = area2D.GetNode<CollisionShape2D>("CollisionShape2D");
		color = collider.DebugColor;

		target = GetNode<CharacterBody2D>("CharacterBody2D");
		camera = target.GetNode<Camera2D>("Camera2D");
		if (target == null) GD.PrintErr("Target node not found!");
		else GD.PrintRich("Target node found!");

		enemy = GetNode<CharacterBody2D>("Hyena");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (camera != null)
			if (target != null) camera.GlobalPosition = new Vector2(target.GlobalPosition.X, 0);


		if (collider.GlobalPosition.Y < target.GlobalPosition.Y)
		{
			var velocity = target.Velocity;
			target.GlobalPosition = new Vector2(spawn.GlobalPosition.X, spawn.GlobalPosition.Y);
			velocity.Y = Mathf.MoveToward(target.Velocity.Y, 0, 800.0f);
			target.Velocity = velocity;
			collider.DebugColor = new Color(0.855f, 0.212f, 0.741f, 0.420f);
		}
		else if (target.IsOnFloor()) collider.DebugColor = color;
	}
}
