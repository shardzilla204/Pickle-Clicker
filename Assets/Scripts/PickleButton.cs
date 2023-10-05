using Godot;
using System;

public partial class PickleButton : Sprite2D
{
	public bool hovering = false;

	public double pickles = 0;

	public override void _Process(double delta)
	{
		if (!Input.IsActionJustPressed("clickPickle")) return;

		if (!hovering) return;

		ClickPickle();
		EmitParticles();
	}

	public void OnHover(bool isHovering)
	{
		hovering = isHovering;
	}

	private async void EmitParticles()
	{
		PackedScene pickleParticlesScene = GD.Load<PackedScene>("res://Assets/Scenes/PickleParticles.tscn");
		Node pickleParticlesNode = pickleParticlesScene.Instantiate();
		GpuParticles2D pickleParticles = (GpuParticles2D) pickleParticlesNode;
		pickleParticles.Position = GetLocalMousePosition();
		AddChild(pickleParticles);
		await ToSignal(GetTree().CreateTimer(1), SceneTreeTimer.SignalName.Timeout);
		pickleParticles.QueueFree();
	}

	private void ClickPickle()
	{
		TextureRect pickleProgressBar = GetNode<TextureRect>($"../PickleProgressBar");
		double pickleLevel = (double) pickleProgressBar.Get("pickleLevel");
		pickles += 1 + (Math.Pow(pickleLevel, 2) / 10);
		string picklesFormatted = BigNumberHandler(pickles);
		Label picklesPicked = GetNode<Label>($"../Counters/PicklesPicked/HBoxContainer/Counter");
		picklesPicked.Text = $"{picklesFormatted}";

		pickleProgressBar.Call("AddProgress");
	}

	private string BigNumberHandler(double pickles)
	{
		string[] abbreviations = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No"};
		int mag = (int)(Math.Floor(Math.Log10(pickles))/3); // Truncates to 6, divides to 2
		double divisor = Math.Pow(10, mag * 3);
		double shortNumber = pickles / divisor;
		return $"{shortNumber:N2} {abbreviations[mag]}";
	}
}
