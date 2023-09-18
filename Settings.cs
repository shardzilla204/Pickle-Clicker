using Godot;
using System;
using System.Collections.Generic;

public partial class Settings : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		DisplayServer.WindowSetMinSize(new Vector2I(720, 480));
		DisplayServer.WindowSetSize()
		SpinBox framerateOption = GetNode<SpinBox>("/root/Canvases/SettingsCanvas/UserInterface/VBoxContainer/FramerateOptions");
		framerateOption.GetLineEdit().ContextMenuEnabled = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2I currentScreenSize = DisplayServer.ScreenGetSize();
		List<Vector2I> resolutions = new List<Vector2I> { new Vector2I(720, 480), new Vector2I(960, 540), new Vector2I(1280, 720), new Vector2I(1920, 1080) };
		foreach(Vector2I resolution in resolutions)
		{
			Console.WriteLine(currentScreenSize[0] == resolution[0]);
			Console.WriteLine(currentScreenSize[0]);
			Console.WriteLine(currentScreenSize[1] == resolution[1]);
			if (currentScreenSize[0] == resolution[0] && currentScreenSize[1] == resolution[1]) return;

			SetCustomResolutionOption();
		}
	}

	public void SetWindowOption(int index)
	{
		OptionButton windowOptions = GetNode<OptionButton>("/root/Canvases/SettingsCanvas/UserInterface/VBoxContainer/WindowOptions");
		CheckButton borderlessOption = GetNode<CheckButton>("/root/Canvases/SettingsCanvas/UserInterface/VBoxContainer/BorderlessOption");
		OptionButton resolutionOptions = GetNode<OptionButton>("/root/Canvases/SettingsCanvas/UserInterface/VBoxContainer/ResolutionOptions");
		resolutionOptions.Disabled = false;
		borderlessOption.Disabled = false;
		switch(windowOptions.GetSelectedId())
		{
			case 0: 
			{
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
				break;
			}
			case 1: 
			{
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Maximized);
				break;
			}
			case 2: 
			{
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
				resolutionOptions.Disabled = true;
				borderlessOption.Disabled = true;
				break;
			}
			case 3: 
			{
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
				resolutionOptions.Disabled = true;
				borderlessOption.Disabled = true;
				break;
			}
		}
	}

	public void SetCustomResolutionOption()
	{
		OptionButton resolutionOptions = GetNode<OptionButton>("/root/Canvases/SettingsCanvas/UserInterface/VBoxContainer/ResolutionOptions");
		
		if (resolutionOptions.ItemCount <= 5) return;

		resolutionOptions.AddItem("Custom");
		resolutionOptions.Select(4);

		Console.WriteLine("Added Custom");
	}

	public void SetResolutionOption(int index)
	{
		OptionButton resolutionOptions = GetNode<OptionButton>("/root/Canvases/SettingsCanvas/UserInterface/VBoxContainer/ResolutionOptions");
		if (resolutionOptions.ItemCount <= 5)
		{
			resolutionOptions.RemoveItem(4);
			Console.WriteLine("Removed Custom");
		}
		switch(resolutionOptions.GetSelectedId())
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

	public void OpenSettings()
	{
		CanvasLayer settingsCanvas = GetNode<CanvasLayer>("/root/Canvases/SettingsCanvas");
		settingsCanvas.Visible = true;
	}

	public void CloseSettings()
	{
		CanvasLayer settingsCanvas = GetNode<CanvasLayer>("/root/Canvases/SettingsCanvas");
		settingsCanvas.Visible = false;
	}

	public void ExitGame()
	{
		GetTree().Quit();
	}

	public void ToggleBorderlessOption(bool toggled)
	{
		DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, toggled);
	}

	public void ToggleVSyncOption(bool toggled)
	{
		SpinBox framerateOption = GetNode<SpinBox>("/root/Canvases/SettingsCanvas/UserInterface/VBoxContainer/FramerateOptions");
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
		SpinBox framerateOption = GetNode<SpinBox>("/root/Canvases/SettingsCanvas/UserInterface/VBoxContainer/FramerateOptions");
		framerateOption.GetLineEdit().ReleaseFocus();
	}
}
