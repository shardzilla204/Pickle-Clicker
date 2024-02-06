using Godot;
using System;

public partial class Pickle : Sprite2D
{
	[Export] public PickleProgressBar pickleProgressBar;
	[Export] public Label picklesPicked;

	[Export] public double pickles = 0;
	public float damage = 1f;

	public bool isHovering = false;

	public Vector2 firstPosition;
	public Vector2 secondPosition;

    public override void _Ready()
    {
		CenterPickle();
		AnimatePickle();

        picklesPicked.Text = $"{pickles:N0}";
	}

    public override void _Process(double delta)
	{
		CenterPickle();
		IsClicked();
	}

	public void AnimatePickle()
	{
		Vector2 firstPosition = Position;
		Vector2 secondPosition = new Vector2(firstPosition.X, firstPosition.Y - 25);

		Tween tween = GetTree().CreateTween().SetLoops();
		tween.TweenProperty(this, "position", secondPosition, 1f);
		tween.TweenProperty(this, "position", firstPosition, 1f);
	}

	public void IsClicked() 
	{
		if (!isHovering) return;

		if (!Input.IsActionJustPressed("click")) return;

		PlayAudio();
		GetPickles();
		EmitParticles();
	}

	private void GetPickles()
	{
		pickles += 1;
		picklesPicked.Text = $"{pickles:N0}";
        pickleProgressBar.AddProgress(1);
	}

	public async void PlayAudio()
	{
		RandomNumberGenerator randomNumber = new RandomNumberGenerator();
		int value = randomNumber.RandiRange(1, 5);

		AudioStreamPlayer audioStreamPlayer = new AudioStreamPlayer();
		AddChild(audioStreamPlayer);

		AudioStreamMP3 audio = GD.Load<AudioStreamMP3>($"res://Assets/Audio/Pickle/Pickle{value}.mp3");
		audioStreamPlayer.Stream = audio;
		audioStreamPlayer.VolumeDb = -17.5f;
		audioStreamPlayer.Bus = "SoundEffects";
		audioStreamPlayer.Play();

		await ToSignal(GetTree().CreateTimer(1), SceneTreeTimer.SignalName.Timeout);

		audioStreamPlayer.QueueFree();
	}

	public void OnHover(bool value)
	{
		isHovering = value;
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
		double pickleLevel = (double) pickleProgressBar.Get("pickleLevel");
		Label picklesPicked = GetNode<Label>($"../Counters/PicklesPicked/HBoxContainer/Counter");
		pickles += 1 + (int) (Math.Pow(pickleLevel, 2) / 10);
		picklesPicked.Text = $"{pickles:N0}";
		pickleProgressBar.AddProgress(1);
	}

	public void CenterPickle()
	{
		Window window = GetWindow();
		Position = window.Size / 2;
	}
}
