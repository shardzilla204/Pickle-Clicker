using Godot;

public class Poglin
{
	public Poglin (string value)
	{
		poglinCount++;
		poglinName = value;
		levelRequired = getLevelRequired(value);
	}

	public string poglinName { get; private set; } 
	public int levelRequired { get; private set; } 
	public static int poglinCount = 0;

	public static Poglin Normal = new Poglin("Normal");
	public static Poglin Pink = new Poglin("Pink");
	public static Poglin Gold = new Poglin("Gold");
	public static Poglin Fire = new Poglin("Fire");
	public static Poglin Earth = new Poglin("Earth");
	public static Poglin Water = new Poglin("Water");

	public int getLevelRequired(string value)
	{
		Variant poglinVariant = GD.Load<PackedScene>($"res://Assets/Scenes/Poglins/{value}Poglin.tscn").Instantiate();
		NormalPoglin poglin = poglinVariant.As<NormalPoglin>();
		return poglin.requiredLevel;
	}
}

public partial class PoglinManager : Node
{
	public RandomNumberGenerator randomNumber = new RandomNumberGenerator();
	public Pickle pickle;
	public PickleProgressBar pickleProgressBar;

	[Export] public int picklesRequiredToSpawn = 100;
	[Export] public int maxPoglins = 5;
	[Export] public int waveTimer = 5;
	[Export] public int spawnTimer = 1;

	public int poglinCount = Poglin.poglinCount;

    public override void _Ready()
    {
		pickle = GetNode<Pickle>("/root/MainCanvas/Pickle");
		pickleProgressBar = GetNode<PickleProgressBar>("/root/MainCanvas/MarginContainer/UserInterface/PickleProgressBar");
		WaveInterval();
    }

	public async void WaveInterval()
	{
		while (true) 
		{
			await ToSignal(GetTree().CreateTimer(waveTimer), SceneTreeTimer.SignalName.Timeout);

			if (pickle.pickles < picklesRequiredToSpawn) return;

			AudioStreamPlayer audioStreamPlayer = new AudioStreamPlayer();
			AddChild(audioStreamPlayer);

			audioStreamPlayer.Stream = GD.Load<AudioStreamMP3>("res://Assets/Audio/Poglin/PoglinRaid.mp3");
			audioStreamPlayer.VolumeDb = -17.5f;
			audioStreamPlayer.Bus = "SoundEffects";
			audioStreamPlayer.Play();

			InitiateWave();
		}
	}

	public async void InitiateWave()
	{
		for (int spawned = 0; maxPoglins > spawned; spawned++)
		{
			SpawnPoglin();
			await ToSignal(GetTree().CreateTimer(spawnTimer), SceneTreeTimer.SignalName.Timeout);
		}
	}

    public void SpawnPoglin()
	{
		Variant variant = GD.Load<PackedScene>($"res://Assets/Scenes/Poglins/{RandomPoglin().poglinName}Poglin.tscn").Instantiate();
		NormalPoglin poglin = SetPoglinPosition(variant.As<NormalPoglin>());

		Node mainCanvas = GetNode<Node>("../MainCanvas");
		mainCanvas.AddChild(poglin);
	}

	public Poglin RandomPoglin()
	{
		int poglinID = randomNumber.RandiRange(1, poglinCount);
		switch(poglinID)
		{
			case 1:
			{
				return Poglin.Normal;
			}
			case 2:
			{
				return Poglin.Pink;
			}
			case 3:
			{
				return Poglin.Gold;
			}
			case 4:
			{
				return SpawnNormalPoglin(Poglin.Fire);
			}
			case 5:
			{
				return SpawnNormalPoglin(Poglin.Earth);
			}
			case 6:
			{
				return SpawnNormalPoglin(Poglin.Water);
			}
			default:
			{
				return Poglin.Normal;
			}
		}
	}

	public NormalPoglin SetPoglinPosition(NormalPoglin poglin)
	{
		Window window = GetWindow();
		Vector2 distance = window.Size * 2;

		float angle = randomNumber.RandfRange(-Mathf.Pi, Mathf.Pi);

		poglin.Position = new Vector2(Mathf.Cos(angle) * distance.X, Mathf.Sin(angle) * distance.Y);
		AnimatedSprite2D sprite = poglin.GetNode<AnimatedSprite2D>("./AnimatedSprite2D");

		if (poglin.Position > window.Size) sprite.FlipH = true;

		return poglin;
	}

	public Poglin SpawnNormalPoglin(Poglin poglinType)
	{
		if (poglinType.levelRequired > pickleProgressBar.pickleLevel) return Poglin.Normal;

		if (poglinType.Equals(Poglin.Normal)) return Poglin.Normal;

		return poglinType;
	}
}
