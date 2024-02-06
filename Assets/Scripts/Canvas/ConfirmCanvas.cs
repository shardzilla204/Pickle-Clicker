using Godot;
using System;

public enum Mode
{
	SAVE, 
	LOAD, 
	DELETE,
	EXIT
}

public partial class ConfirmCanvas : CanvasLayer
{
	Mode currentMode;
	CanvasLayer previousCanvas;
	private CanvasManager canvasManager;

    public override void _Ready()
    {
        canvasManager = GetNode<CanvasManager>("/root/CanvasManager");
    }

    public void OpenConfirm(string warning, string mode, CanvasLayer canvas)
	{
		if (warning == "") return;

		previousCanvas = canvas;
		currentMode = Enum.Parse<Mode>(mode, true);

		Label warningText = GetNode<Label>($"/root/ConfirmCanvas/MarginContainer/VBoxContainer/WarningText");
		warningText.Text = warning;
	}

	public void Accept()
	{
		switch(currentMode)
		{
			case Mode.SAVE:
			{
				break;
			}

			case Mode.LOAD:
			{
				break;
			}

			case Mode.DELETE:
			{
				break;
			}

			case Mode.EXIT:
			{
				SettingsCanvas settingsCanvas = GetNode<SettingsCanvas>($"/root/SettingsCanvas");
				settingsCanvas.CloseGame();

				SteamworksManager steamworksManager = GetNode<SteamworksManager>($"/root/SteamworksManager");
				steamworksManager.CloseSteam();
				break;
			}
		}
	}

	public void Decline()
	{
		if (previousCanvas is SettingsCanvas) previousCanvas.Visible = true;
		
		QueueFree();
	}
}
