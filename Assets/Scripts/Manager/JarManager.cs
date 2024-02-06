using Godot;

public partial class JarManager : Node
{
	public RandomNumberGenerator randomNumber = new RandomNumberGenerator();

	[Export] public int spawnTimer = 60;

	public override void _Ready()
    {
		SpawnInterval();
    }
	
	public async void SpawnInterval()
	{
		while (true) 
		{
			await ToSignal(GetTree().CreateTimer(spawnTimer), SceneTreeTimer.SignalName.Timeout);

			SpawnJar();
		}
	}

	public void SpawnJar()
	{
		Variant variant = GD.Load<PackedScene>("res://Assets/Scenes/PickleJar.tscn").Instantiate();
		PickleJar jar = SetJarPosition(variant.As<PickleJar>());

		Node mainCanvas = GetNode<Node>("../MainCanvas");
		mainCanvas.AddChild(jar);
	}

	public PickleJar SetJarPosition(PickleJar jar)
	{
		Window window = GetWindow();
		Vector2 distance = window.Size/2;

		float angle = randomNumber.RandfRange(0, Mathf.Pi)/2;

		jar.Position = new Vector2(angle * distance.X, angle * distance.Y);
		GD.Print(jar.Position);
		return jar;
	}
}
