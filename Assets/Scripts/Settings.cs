using Godot;
using System;
using System.Collections.Generic;

public partial class SettingsCanvas : CanvasLayer
{
	Vector2I initialScreenPosition;

	[Export] public OptionButton windowOptions;
	[Export] public OptionButton resolutionOptions;
	[Export] public CheckButton borderlessOption;
	[Export] public CheckButton vSyncOption;
	[Export] public OptionButton framerateOptions;

	private CanvasManager canvasManager;

	public override void _Ready()
	{
		DisplayServer.WindowSetMinSize(new Vector2I(720, 480));
		initialScreenPosition = DisplayServer.WindowGetPosition();
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
				resolutionOptions.Disabled = false;
				borderlessOption.Disabled = false;
				DisplayServer.MouseSetMode(DisplayServer.MouseMode.Visible);

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
		windowOptions.Selected = index;
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
		resolutionOptions.Selected = index;
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
		framerateOptions.Disabled = !toggled;
	}

	public void SetFramerate(int index)
	{
		switch (index)
		{
			case 0:
				Engine.MaxFps = 20;
			break;
			case 1:
				Engine.MaxFps = 30;
			break;
			case 2:
				Engine.MaxFps = 45;
			break;
			case 3:
				Engine.MaxFps = 60;
			break;
			case 4:
				Engine.MaxFps = 90;
			break;
			case 5:
				Engine.MaxFps = 144;
			break;
			case 6:
				Engine.MaxFps = 200;
			break;
			case 7:
				Engine.MaxFps = 0;
			break;
		}
		framerateOptions.Selected = index;
	}

	public void SetVolume(float value, string busName)
	{
		int busIndex = AudioServer.GetBusIndex($"{busName}");
		AudioServer.SetBusVolumeDb(busIndex, value);
	}
}