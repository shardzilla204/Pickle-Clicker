using Godot;
using System;

public enum Option
{
	SAVE, 
	LOAD, 
	DELETE,
	EXIT
}

public partial class ConfirmCanvas : CanvasLayer
{
	Option currentMode;
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
		currentMode = Enum.Parse<Option>(mode, true);

		Label warningText = GetNode<Label>($"/root/ConfirmCanvas/MarginContainer/VBoxContainer/WarningText");
		warningText.Text = warning;
	}

	public void Accept()
	{
		switch(currentMode)
		{
			case Option.SAVE:
			{
				break;
			}

			case Option.LOAD:
			{
				break;
			}

			case Option.DELETE:
			{
				break;
			}

			case Option.EXIT:
			{
				SettingsCanvas settingsCanvas = GetNode<SettingsCanvas>($"/root/SettingsCanvas");
				settingsCanvas.ExitGame();

				SteamManager steamManager = GetNode<SteamManager>($"/root/SteamworksManager");
				steamManager.ExitSteam();
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
