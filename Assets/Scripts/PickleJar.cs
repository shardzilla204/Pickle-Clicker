using Godot;

public partial class PickleJar : Node2D
{
    public Pickle pickle;
    public RandomNumberGenerator randomNumber = new RandomNumberGenerator();
	public Sprite2D liquid;
    public bool isHovering = false;
    public bool hasOpened = false;
    public int clicksToOpen;
    public Timer lifetime;

    public override void _Ready()
    {
        liquid = GetNode<Sprite2D>("./Mask/Liquid");
        clicksToOpen = randomNumber.RandiRange(3, 8);
        pickle = GetNode<Pickle>("../Pickle");
        SetLifetime();
    }
    public override void _Process(double delta)
    {
		liquid.RotationDegrees = -RotationDegrees;
        IsClicked();
        CanOpen();
        IsTimerDone();
	}

    public void IsClicked() 
	{
        if (hasOpened) return;

		if (!isHovering) return;

		if (!Input.IsActionJustPressed("click")) return;

        float jarRotationRadians = Mathf.DegToRad(randomNumber.RandiRange(-35, 35));

		PlayAudio();

        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(this, "rotation", jarRotationRadians, 0.5f);
        clicksToOpen--;
	}

    public async void CanOpen()
    {
        if (hasOpened) return;

        if (clicksToOpen > 0) return;

        GetPickles();

        hasOpened = !hasOpened;

        AnimationPlayer animationPlayer = GetNode<AnimationPlayer>("./AnimationPlayer");
        animationPlayer.CurrentAnimation = "Pour";
        animationPlayer.Play();

        lifetime.Stop();

        await ToSignal(GetTree().CreateTimer(3.5f), SceneTreeTimer.SignalName.Timeout);

        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(this, "modulate", new Color(1, 1, 1, 0), 1.0f);

        QueueFree();
    }

    public void OnHover(bool value)
	{
		isHovering = value;
	}

    private void GetPickles()
	{
		pickle.pickles += randomNumber.RandiRange(50, 250);
		pickle.picklesPicked.Text = $"{pickle.pickles:N0}";
        pickle.pickleProgressBar.AddProgress(randomNumber.RandiRange(15, 30));
	}

    private void PlayAudio()
    {

    }

    public Timer SetLifetime()
    {
        lifetime = new Timer()
        { 
            WaitTime = randomNumber.RandiRange(3, 8),
            OneShot = true,
            Autostart = true
        };
        return lifetime;
    }

    public void IsTimerDone()
    {
        if (lifetime.TimeLeft != 0) return;

        QueueFree();
    }
}
