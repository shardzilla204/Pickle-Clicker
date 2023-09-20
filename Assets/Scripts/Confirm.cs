using Godot;
using System;

enum Mode
{
	Save, 
	Load, 
	Delete,
	Exit
}

public partial class Confirm : CanvasLayer
{
	Mode currentMode;
	const string CANVASES_PATH = "/root/Canvases";
	
	public void ToggleConfirm(bool toggled, string warning, string mode)
	{
		CanvasLayer confirmCanvas = GetNode<CanvasLayer>($"{CANVASES_PATH}/ConfirmCanvas");
		confirmCanvas.Visible = toggled;

		CanvasLayer settingsCanvas = GetNode<CanvasLayer>($"{CANVASES_PATH}/SettingsCanvas");
		settingsCanvas.Visible = false;

		if (mode == "") return;

		currentMode = Enum.Parse<Mode>(mode, true);

		if (warning == "") return;

		Label warningText = GetNode<Label>($"{CANVASES_PATH}/ConfirmCanvas/UserInterface/WarningText");
		warningText.Text = warning;
	}

	public void Accept()
	{
		switch(currentMode)
		{
			case Mode.Save:
			{
				break;
			}

			case Mode.Load:
			{
				break;
			}

			case Mode.Delete:
			{
				break;
			}

			case Mode.Exit:
			{
				CanvasLayer settingsCanvas = GetNode<CanvasLayer>($"{CANVASES_PATH}/SettingsCanvas");
				settingsCanvas.Call("ExitGame");

				Node steamManager = GetNode<Node>($"{CANVASES_PATH}/Steamworks");
				steamManager.Call("ExitSteam");
				break;
			}
		}
	}

	public void Decline()
	{
		ToggleConfirm(false, "", "");
	}
}
