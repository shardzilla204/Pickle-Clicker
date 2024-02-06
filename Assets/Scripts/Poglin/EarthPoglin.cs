using Godot;
using System;

public partial class EarthPoglin : NormalPoglin
{
	public int maxShield;
	public double shieldMultiplier = 1.5;

	public Color missingHealth = new Color(0.808f, 0.392f, 0.31f);
	public Color currentHealth = new Color(0.463f, 0.651f, 0.318f);
	public Color shield = new Color(0.749f, 0.537f, 0.314f);
	public bool hasShield = true;

	public override void _Ready()
	{
		maxShield = (int) Math.Floor(maxHealth * shieldMultiplier);

		spawnPosition = Position;
		sprite = GetNode<AnimatedSprite2D>("./AnimatedSprite2D");
		healthBar = GetNode<TextureProgressBar>("./HealthBar");
		healthBar.MaxValue = maxShield;
		healthBar.Value = maxShield;
        pickle = GetNode<Pickle>("/root/MainCanvas/Pickle");
		audioStreamPlayer = GetNode<AudioStreamPlayer>("./AudioStreamPlayer");
		sprite.Play();

		healthBar.TintUnder = currentHealth;
		healthBar.TintProgress = shield;
	}

	public override void _Process(double delta)
	{
		CurrentMovement();
		IsClicked();
		IsOutOfBounds();
		IsOutOfHealth();
	}

	public new void IsOutOfHealth()
	{
		if (healthBar.Value > 0) return;
		if (hasShield)
		{
			hasShield = false;
			healthBar.MaxValue = maxHealth;
			healthBar.Value = healthBar.MaxValue;
			healthBar.TintUnder = missingHealth;
			healthBar.TintProgress = currentHealth;
			return;
		}
		
		if (isDead) return;

		PlayDeath();
		AutoCollect();
	}

	public new void IsClicked()
	{
		if (!isHovering) return;

		if (!Input.IsActionJustPressed("damagePoglin")) return;

		if (isDead) return;

		DamagePoglin();
		EmitParticles();

		if (hasShield) return;

		KnockbackPoglin();
	}
}
