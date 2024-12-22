using Godot;
using System;

public partial class CameraScript : Node2D
{
	CharacterBody2D target;
	Camera2D camera;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		target = GetNode<CharacterBody2D>("CharacterBody2D");
		camera = target.GetNode<Camera2D>("Camera2D");
		if (target == null) GD.PrintErr("Target node not found!");
		else GD.PrintRich("Target node found!");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (camera != null)
			if (target != null)
			{
				// Follow the target but only on the X-axis
				camera.GlobalPosition = new Vector2(target.GlobalPosition.X, 0);
				GD.Print("Camera: " + camera.GlobalPosition + "\nTarget" + target.Position);
			}

	}
}
