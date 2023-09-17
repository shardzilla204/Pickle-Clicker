using Godot;
using System;

public partial class Settings : Window
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		DisplayServer.WindowSetMinSize(new Vector2I(960, 540));
	}

	public void SetFullscreen()
	{
		DisplayServer.WindowSetMinSize(new Vector2I(960, 540));
	}
}
