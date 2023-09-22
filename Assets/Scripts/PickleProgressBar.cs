using Godot;
using System;

public partial class PickleProgressBar : TextureProgressBar
{
	TextureProgressBar pickleProgressBar;
	int pickleLevel = 0;
	const string USER_INTERFACE_PATH = "/root/Canvases/MainCanvas/UserInterface";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pickleProgressBar = GetNode<TextureProgressBar>($"{USER_INTERFACE_PATH}/PickleProgressBar");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void AddProgress()
	{
		pickleProgressBar.Value += 1;
	}
	
	public void RemoveProgress()
	{
		pickleProgressBar.Value -= pickleProgressBar.MaxValue;
	}

	public void LevelUp()
	{
		RemoveProgress();
		
		pickleLevel += 1;
		Label pickleLevelText = GetNode<Label>($"{USER_INTERFACE_PATH}/PickleProgressBar/PickleLevel");
		pickleLevelText.Text = $"{pickleLevel}";
		pickleProgressBar.MaxValue += 2 ^ pickleLevel;
	}

	public void UpdateProgress(float currentValue)
	{
		double maxValue = pickleProgressBar.MaxValue;
		if (maxValue <= currentValue)
		{
			LevelUp();
		}
	}

	public void CheckProgress(bool toggled)
	{
		Label pickleLevelText = GetNode<Label>($"{USER_INTERFACE_PATH}/PickleProgressBar/PickleLevel");
		pickleLevelText.Text = $"{pickleLevel}";
		pickleLevelText.AddThemeFontSizeOverride("font_size", 40);

		if (!toggled) return;

		double percentage = pickleProgressBar.Value / pickleProgressBar.MaxValue * 100;
		pickleLevelText.Text = $"{Math.Floor(percentage)}%";
		pickleLevelText.AddThemeFontSizeOverride("font_size", 30);
	}
}
