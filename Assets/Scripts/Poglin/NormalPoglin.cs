using Godot;

public partial class NormalPoglin : CharacterBody2D
{
	[Export] public float minSpeed = 10;
	[Export] public float maxSpeed = 20;
	[Export] public float minSteal = 1.5f;
	[Export] public float maxSteal = 2.5f;
	public bool stolePickles;
	public PickleButton pickleButton;

    public override void _Ready()
    {
        pickleButton = GetNode<PickleButton>("root/MainCanvas");
    }

    public override void _Process(double delta)
	{
		Position = Position.MoveToward(pickleButton.Position, maxSpeed);

		if (!stolePickles) return;

		Position = Position.MoveToward(pickleButton.Position, minSpeed);
	}

	public void OnPickleContact(Node2D area)
	{
		if (area is PickleButton)
		{
			stolePickles = true;
		}
	}
}
