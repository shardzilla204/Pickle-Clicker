using Godot;
using System;

public partial class PickleButton : TextureButton
{
	public double pickles = 0;
	const string USER_INTERFACE_PATH = "/root/Canvases/MainCanvas/UserInterface";

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	private void ClickPickle()
	{
		TextureProgressBar pickleProgressBar = GetNode<TextureProgressBar>($"{USER_INTERFACE_PATH}/PickleProgressBar");
		double pickleLevel = (double) pickleProgressBar.Get("pickleLevel");
		pickles += 1 + pickleLevel / 10;
		string picklesFormatted = BigNumberHandler(pickles);
		Label picklesPicked = GetNode<Label>($"{USER_INTERFACE_PATH}/PicklesPickedPanel/PicklesPicked");
		picklesPicked.Text = $"{picklesFormatted}";

		pickleProgressBar.Call("AddProgress");
	}

	private string BigNumberHandler(double pickles)
	{
		string[] abbreviations = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No"};
		int mag = (int)(Math.Floor(Math.Log10(pickles))/3); // Truncates to 6, divides to 2
		double divisor = Math.Pow(10, mag * 3);
		double shortNumber = pickles / divisor;
		return $"{shortNumber:N2} {abbreviations[mag]}";
	}
}
