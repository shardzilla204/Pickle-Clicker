using Godot;
using System;
using System.Collections.Generic;

public partial class Settings : CanvasLayer
{
	Vector2I initialScreenPosition;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		DisplayServer.WindowSetMinSize(new Vector2I(720, 480));
		DisplayServer.WindowSetSize(new Vector2I(1280, 720));
		SpinBox framerateOption = GetNode<SpinBox>("./VBoxContainer/FramerateOptions");
		framerateOption.GetLineEdit().ContextMenuEnabled = false;
		initialScreenPosition = DisplayServer.WindowGetPosition();
		Console.WriteLine(initialScreenPosition);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2I currentScreenSize = DisplayServer.WindowGetSize();
		List<Vector2I> resolutions = new List<Vector2I> { new Vector2I(720, 480), new Vector2I(960, 540), new Vector2I(1280, 720), new Vector2I(1920, 1080) };
		bool found = false;
		foreach(Vector2I resolution in resolutions)
		{
			if (currentScreenSize[0] == resolution[0] && currentScreenSize[1] == resolution[1])
			{
				found = true;
				break;
			}
		}

		if (found) return;

		SetCustomResolutionOption();
	}

	public void SetWindowOption(int index)
	{
		CheckButton borderlessOption = GetNode<CheckButton>("./VBoxContainer/BorderlessOption");
		OptionButton resolutionOptions = GetNode<OptionButton>("./VBoxContainer/ResolutionOptions");
		resolutionOptions.Disabled = false;
		borderlessOption.Disabled = false;
		DisplayServer.MouseSetMode(DisplayServer.MouseMode.Visible);
		switch(index)
		{
			case 0: 
			{
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
				CheckForBorderless();
				if (DisplayServer.WindowGetSize()[0] <= 720)
				{
					DisplayServer.WindowSetSize(new Vector2I(1280, 720));
					DisplayServer.WindowSetPosition(initialScreenPosition);
				}
				break;
			}
			case 1: 
			{
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
				ToggleFullscreen();
				break;
			}
			case 2: 
			{
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
				ToggleFullscreen();
				break;
			}
		}
		SetCustomResolutionOption();
	}

	private void ToggleFullscreen()
	{
		CheckButton borderlessOption = GetNode<CheckButton>("./VBoxContainer/BorderlessOption");
		OptionButton resolutionOptions = GetNode<OptionButton>("./VBoxContainer/ResolutionOptions");
		DisplayServer.MouseSetMode(DisplayServer.MouseMode.Confined);
		resolutionOptions.Disabled = true;
		ToggleBorderlessOption(false);
		borderlessOption.ButtonPressed = false;
		borderlessOption.Disabled = true;
	}

	private void CheckForBorderless()
	{
		CheckButton borderlessOption = GetNode<CheckButton>("./VBoxContainer/BorderlessOption");
		Vector2I currentWindowSize = DisplayServer.WindowGetSize();
		Vector2I currentScreenSize = DisplayServer.ScreenGetSize();
		bool match = currentWindowSize[0] >= currentScreenSize[0];
		if (borderlessOption.ButtonPressed && match)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Maximized);
		}
	}

	public void SetCustomResolutionOption()
	{
		OptionButton resolutionOptions = GetNode<OptionButton>("./VBoxContainer/ResolutionOptions");
		
		Vector2I currentScreenSize = DisplayServer.WindowGetSize();
		if (resolutionOptions.ItemCount >= 5)
		{
			resolutionOptions.SetItemText(4, $"Custom ({currentScreenSize[0]}x{currentScreenSize[1]})");
		}
		else 
		{
			resolutionOptions.AddItem($"Custom ({currentScreenSize[0]}x{currentScreenSize[1]})");
			resolutionOptions.Select(4);
		}
	}

	public void SetResolutionOption(int index)
	{
		OptionButton resolutionOptions = GetNode<OptionButton>("./VBoxContainer/ResolutionOptions");
		if (resolutionOptions.ItemCount >= 5)
		{
			resolutionOptions.RemoveItem(4);
		}
		switch(index)
		{
			case 0: 
			{
				DisplayServer.WindowSetSize(new Vector2I(720, 480));
				break;
			}
			case 1: 
			{
				DisplayServer.WindowSetSize(new Vector2I(960, 540));
				break;
			}
			case 2:
			{
				DisplayServer.WindowSetSize(new Vector2I(1280, 720));
				break;
			}
			case 3:
			{
				DisplayServer.WindowSetSize(new Vector2I(1920, 1080));
				break;
			}
		}
	}

	public void CloseSettings()
	{
		QueueFree();
	}

	public void ExitGame()
	{
		GetTree().Quit();
	}

	public void ToggleBorderlessOption(bool toggled)
	{
		DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, toggled);
		CheckForBorderless();
		SetCustomResolutionOption();
	}

	public void ToggleVSyncOption(bool toggled)
	{
		SpinBox framerateOption = GetNode<SpinBox>("./VBoxContainer/FramerateOptions");
		switch(toggled)
		{
			case true:
			{
				DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Adaptive);
				break;
			}

			case false:
			{
				DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Disabled);
				break;
			}
		}
		framerateOption.Editable = !toggled;
		framerateOption.SelectAllOnFocus = !toggled;
		RemoveFramerateFocus();
	}

	public void SetFramerate(string framerate)
	{
		Engine.MaxFps = framerate.ToInt();
		RemoveFramerateFocus();
	}

	public void RemoveFramerateFocus()
	{
		SpinBox framerateOption = GetNode<SpinBox>("./VBoxContainer/FramerateOptions");
		framerateOption.GetLineEdit().ReleaseFocus();
	}
}