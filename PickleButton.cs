using Godot;
using System;

public partial class PickleButton : TextureButton
{
	public double pickles = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// AnimationPlayer animation_player = GetNode<AnimationPlayer>("AnimationPlayer");
		// animation_player.Play("PickleButtonMovement");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		DisplayServer.WindowSetMinSize(new Vector2I(960, 540));
	}

	public void SetFullscreen()
	{
		DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
	}

	public void SetWindowed()
	{
		DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
	}

	public void OpenSettings()
	{
		CanvasLayer settingsCanvas = GetNode<CanvasLayer>("/root/Canvases/SettingsCanvas");
		settingsCanvas.Visible = true;
	}

	public void CloseSettings()
	{
		CanvasLayer settingsCanvas = GetNode<CanvasLayer>("/root/Canvases/SettingsCanvas");
		settingsCanvas.Visible = false;
	}

	public void ExitGame()
	{
		GetTree().Quit();
	}

	private int index = 0;
	private void click_pickle()
	{
		pickles += 1000000;
		string pickles_formatted = big_number_handler(pickles);
		Label pickles_picked = GetNode<Label>("/root/Canvases/MainCanvas/UserInterface/PicklesPickedPanel/PicklesPicked");
		pickles_picked.Text = $"{pickles_formatted}";
	}

	private string big_number_handler(double pickles)
	{
		string[] abbreviations = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No"};
		int mag = (int)(Math.Floor(Math.Log10(pickles))/3); // Truncates to 6, divides to 2
		double divisor = Math.Pow(10, mag*3);
		double shortNumber = pickles / divisor;
		return $"{shortNumber:N2} {abbreviations[mag]}";
	}
}
