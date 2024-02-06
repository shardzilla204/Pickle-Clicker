using Godot;
using System;

public partial class PickleProgressBar : TextureRect
{
	private TextureProgressBar initialProgressBar;
	private TextureProgressBar finalProgressBar;
	private Label pickleLevelLabel;

	public int pickleLevel = 0;
	private const int MAX_PICKLE_LEVEL = 250;

	private bool removeProgress = false;

	public override void _Ready()
	{
		initialProgressBar = GetNode<TextureProgressBar>($"./InitialProgressBar");
		finalProgressBar = GetNode<TextureProgressBar>($"./FinalProgressBar");
	}

	public void AddProgress(int gain)
	{
		initialProgressBar.Value += gain;

		if (removeProgress) return;
		
		SetFinalProgressBar();
	}


	public void SetFinalProgressBar()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(finalProgressBar, "value", initialProgressBar.Value, 1f);
	}

	public void AddProgress()
	{
		initialProgressBar.Value += 1;
		if (removeProgress) return;
		SetFinalProgressBar();
	}

	
	public async void RemoveProgress()
	{
		removeProgress = true;
		Tween tween = GetTree().CreateTween().SetParallel(true);
		tween.SetEase(Tween.EaseType.Out);
		tween.TweenProperty(finalProgressBar, "value", 0, 1f);
		tween.TweenProperty(initialProgressBar, "value", 0, 1f);
		await ToSignal(tween, "finished");
		removeProgress = false;

	}

	public void LevelUp()
	{
		RemoveProgress();
		pickleLevel += 1;
		Label pickleLevelText = GetNode<Label>($"./PickleLevel");
		pickleLevelText.Text = $"{pickleLevel}";
		initialProgressBar.MaxValue += 2 ^ pickleLevel;
		finalProgressBar.MaxValue += 2 ^ pickleLevel;
	}

	public void UpdateProgress(float currentValue)
	{
		double maxValue = finalProgressBar.MaxValue;
		if (maxValue <= currentValue)
		{
			LevelUp();
		}
	}

	public void CheckProgress(bool toggled)
	{
		Label pickleLevelText = GetNode<Label>($"./PickleLevel");
		pickleLevelText.Text = $"{pickleLevel}";
		pickleLevelText.AddThemeFontSizeOverride("font_size", 40);

		if (!toggled) return;

		double percentage = finalProgressBar.Value / finalProgressBar.MaxValue * 100;
		pickleLevelText.Text = $"{Math.Floor(percentage)}%";
		pickleLevelText.AddThemeFontSizeOverride("font_size", 30);
	}
}
