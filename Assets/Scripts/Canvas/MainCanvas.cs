using Godot;
using System;

public partial class MainCanvas : CanvasLayer
{
	CanvasManager canvasManager;
	public override void _Ready()
	{
	   	canvasManager = GetNode<CanvasManager>("/root/CanvasManager");
	}

	public void OpenCanvas(string canvas)
	{
		Canvas currentCanvas = Enum.Parse<Canvas>(canvas, true);
		canvasManager.OpenCanvas(currentCanvas);
	}
}
