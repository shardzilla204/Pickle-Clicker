using Godot;

public partial class NormalPoglin : CharacterBody2D
{
<<<<<<< Updated upstream
	[Export] public float minSpeed = 10;
	[Export] public float maxSpeed = 20;
	[Export] public float minSteal = 1.5f;
	[Export] public float maxSteal = 2.5f;
	public bool stolePickles;
	public PickleButton pickleButton;
=======
	public RandomNumberGenerator randomNumber = new RandomNumberGenerator();
	public Pickle pickle;
	public TextureProgressBar healthBar;
	public Vector2 spawnPosition;
	public AnimatedSprite2D sprite;
	public AudioStreamPlayer audioStreamPlayer;
	public AnimationTree animationTree;

	public int picklesStole;
	public int picklesToAdd;

	[Export] public float minSpeed = 1f;
	[Export] public float maxSpeed = 2.5f;
	[Export] public float minSteal = 1.5f;
	[Export] public float maxSteal = 2.5f;
	[Export] public float maxHealth = 3f;
	[Export] public int requiredLevel = 0;
	[Export] public float spawnRarity = 100f;
	[Export] public float collectionRate = 0.25f;

	public bool hasStolePickles = false;
	public bool isHovering = false;
	public bool isDead = false;
>>>>>>> Stashed changes

	public float knockbackAmount = 0.075f;

    public override void _Ready()
    {
<<<<<<< Updated upstream
        pickleButton = GetNode<PickleButton>("root/MainCanvas");
=======
		spawnPosition = Position;

		sprite = GetNode<AnimatedSprite2D>("./AnimatedSprite2D");
		sprite.Play();

        pickle = GetNode<Pickle>("/root/MainCanvas/Pickle");

		healthBar = GetNode<TextureProgressBar>("./HealthBar");
		AssignAttributes();
>>>>>>> Stashed changes
    }

    public override void _Process(double delta)
	{
		Position = Position.MoveToward(pickleButton.Position, maxSpeed);

		if (!stolePickles) return;

		Position = Position.MoveToward(pickleButton.Position, minSpeed);
	}

<<<<<<< Updated upstream
	public void OnPickleContact(Node2D area)
	{
		if (area is PickleButton)
		{
			stolePickles = true;
		}
=======
	public void AssignAttributes()
	{
		float minSize = 0.125f;
		float maxSize = 0.4f;

		float size = (float) randomNumber.RandfRange(minSize, maxSize);
		AssignHealth(size);

		GlobalScale = new Vector2(size, size);
	}

	public void AssignHealth(float size)
	{
		if (size > 0.325f) 
		{
			healthBar.MaxValue = Math.Floor(maxHealth * (size + 2));
			AssignSpeed(2.5f);
		}
		else if (size <= 0.325f && size > 0.275f) 
		{
			healthBar.MaxValue = Math.Floor(maxHealth * (size + 1));
			AssignSpeed(1.5f);
		}
		else if (size <= 0.275f && size > 0.2f)
		{
			healthBar.MaxValue = maxHealth;
		}
		else if (size <= 0.2f)
		{
			healthBar.MaxValue = Math.Floor(maxHealth - size);
			AssignSpeed(0.5f);
		}

		healthBar.MaxValue += healthBar.MaxValue * pickle.pickleProgressBar.pickleLevel/2;
		healthBar.Value = healthBar.MaxValue;
	}

	public void AssignSpeed(float multiplier)
	{
		minSpeed /= multiplier;
		maxSpeed /= multiplier;
	}

	public void CurrentMovement()
	{
		if (isDead) return;

		if (!hasStolePickles) Position = Position.MoveToward(pickle.Position, maxSpeed);

		if (hasStolePickles) Position = Position.MoveToward(spawnPosition, minSpeed);
	}

	public void IsClicked()
	{
		if (!isHovering) return;

		if (!Input.IsActionJustPressed("damagePoglin")) return;

		if (isDead) return;

		DamagePoglin();
		EmitParticles();
		KnockbackPoglin();
	}

	public void IsOutOfBounds()
	{
		if (!hasStolePickles) return;

		if (Position > GetViewportRect().Size || Position < new Vector2(0, 0)) QueueFree();
	}

	public void IsOutOfHealth()
	{
		if (healthBar.Value > 0) return;

		if (isDead) return;

		PlayDeath();
		AutoCollect();
	}

	public async void AutoCollect()
	{
		picklesToAdd = (int) Math.Floor(collectionRate * picklesStole);
		pickle.pickles += picklesToAdd;

		SetUpLabel();

		await ToSignal(GetTree().CreateTimer(1), SceneTreeTimer.SignalName.Timeout);
		QueueFree();

		pickle.picklesPicked.Text = $"{pickle.pickles:N0}";
	}

	public void PlayDeath()
	{
		isDead = true;
		PlayAnimation("Death");
		
		AudioStreamMP3 deathAudio = GD.Load<AudioStreamMP3>("res://Assets/Audio/Poglin/PoglinDeath.mp3");
		PlayAudio(deathAudio);

		sprite.Stop();
	}

	public void PlayAudio(AudioStreamMP3 audio)
	{	

		AudioStreamPlayer audioStreamPlayer = GetNode<AudioStreamPlayer>("./AudioStreamPlayer");
		audioStreamPlayer.Stream = audio;
		audioStreamPlayer.Play();
	}

	public void PlayAnimation(string animation)
	{
		AnimationTree animationTree = GetNode<AnimationTree>("./AnimationTree");
		switch (animation)
		{
			case "Death":
				animationTree.Set("parameters/conditions/isDead", true);
			break;
			case "Stole":
				animationTree.Set("parameters/conditions/isAlive", true);
			break;
		}
	}

	public void OnHover(bool value)
	{
		isHovering = value;
	}

	public void DamagePoglin()
	{
		healthBar.Value -= pickle.damage;
	}

	public void KnockbackPoglin()
	{ 
		if (hasStolePickles) return;

		Vector2 direction = Position - pickle.Position;
		Vector2 newPosition = Position + direction * knockbackAmount;

		if (knockbackAmount > 0) knockbackAmount -= 0.015f;

		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "position", newPosition, 0.1f);
	}

	public async void EmitParticles()
	{
		Node attackParticlesNode = GD.Load<PackedScene>("res://Assets/Scenes/Particles/AttackParticles.tscn").Instantiate();
		GpuParticles2D attackParticles = (GpuParticles2D) attackParticlesNode;
		attackParticles.Position = GetLocalMousePosition();
		AddChild(attackParticles);
		await ToSignal(GetTree().CreateTimer(1), SceneTreeTimer.SignalName.Timeout);
		attackParticles.QueueFree();
	}

	public void OnPickleContact(Area2D area)
	{
		if (hasStolePickles) return;

		if (!(area.GetParent() is Pickle)) return;

		sprite.FlipH = !sprite.FlipH;
		picklesStole = (int) Math.Floor(pickle.pickles / randomNumber.RandfRange(minSteal, maxSteal));
		pickle.pickles -= picklesStole;
		pickle.picklesPicked.Text = $"{pickle.pickles:N0}";
		hasStolePickles = true;
		
		AudioStreamMP3 stealAudio = GD.Load<AudioStreamMP3>("res://Assets/Audio/Poglin/PoglinSteal.mp3");
		PlayAudio(stealAudio);

		SetUpLabel();
	}

	public void SetUpLabel()
	{
		Label picklesLabel = GetNode<Label>("./PicklesLabel");
		if (!isDead)
		{
			picklesLabel.Text = $"-{picklesStole}";
		}
		else 
		{
			if (picklesToAdd <= 0) return;

			picklesLabel.Text = $"+{picklesToAdd}";
		}
		
		PlayAnimation("Stole");
>>>>>>> Stashed changes
	}
}
