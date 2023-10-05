using Godot;
using System;

public partial class CanvasManager : VBoxContainer
{
	[Export] public PackedScene settingsCanvasScene;

	public void OpenSettings()
	{
		Node settingsCanvas = settingsCanvasScene.Instantiate();
		Node root = GetNode<Node>("/root");
		root.AddChild(settingsCanvas);
	}
}
