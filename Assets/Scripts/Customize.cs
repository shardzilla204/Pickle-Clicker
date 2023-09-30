using Godot;
using System;

public partial class Customize : CanvasLayer
{
	public void ToggleCustomizeCanvas(bool toggled)
	{
		CanvasLayer customizeCanvas = GetNode<CanvasLayer>("/root/Canvases/CustomizeCanvas");
		BoxContainer canvasButtons = GetNode<BoxContainer>("/root/Canvases/MainCanvas/UserInterface/CanvasButtons");
		customizeCanvas.Visible = toggled;
		canvasButtons.Visible = !toggled;
	}
}
