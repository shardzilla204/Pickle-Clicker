using Godot;

public enum Canvas 
{
	MAIN,
	SHOP,
	SETTINGS,
	APPAREL,
	CONFIRM
}

public partial class CanvasManager : VBoxContainer
{
	private PackedScene shopCanvasScene;
	private PackedScene settingsCanvasScene;
	private PackedScene confirmCanvasScene;

	private Node root;
	private Node currentCanvas;

	public override void _Ready()
	{
		shopCanvasScene = GD.Load<PackedScene>("res://Assets/Scenes/Canvases/ShopCanvas.tscn");
		settingsCanvasScene = GD.Load<PackedScene>("res://Assets/Scenes/Canvases/SettingsCanvas.tscn");
		confirmCanvasScene = GD.Load<PackedScene>("res://Assets/Scenes/Canvases/ConfirmCanvas.tscn");
		root = GetNode<Node>("/root");
	}

	public void OpenCanvas(Canvas canvasType)
	{
		switch (canvasType)
		{
			case Canvas.SHOP:
			{
				currentCanvas = shopCanvasScene.Instantiate();
				break;
			}
			case Canvas.APPAREL:
			{
				currentCanvas = shopCanvasScene.Instantiate();
				break;
			}

			case Canvas.SETTINGS:
			{
				currentCanvas = settingsCanvasScene.Instantiate();
				break;
			}

			case Canvas.CONFIRM:
			{
				currentCanvas = confirmCanvasScene.Instantiate();
				break;
			}
		}
		root.AddChild(currentCanvas);
	}

	public void OpenCanvas(string warning, string mode, CanvasLayer canvas)
	{
		currentCanvas = confirmCanvasScene.Instantiate();
		root.AddChild(currentCanvas);

		ConfirmCanvas confirmCanvas = GetNode<ConfirmCanvas>("/root/ConfirmCanvas");
		confirmCanvas.OpenConfirm(warning, mode, canvas);
	}
}
