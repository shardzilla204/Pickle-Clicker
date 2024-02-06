using Godot;
using System;
using System.Linq.Expressions;

public partial class Shopkeeper : TextureRect
{
	public int maxWidth = 4323;
	public int maxHeight = 432;
	public int currentWidth = 0;
	public int minWidth = 393;
	public bool reverse = false;

	public AtlasTexture atlas;

    public override void _Ready()
    {
        atlas = (AtlasTexture) Texture;
		LoopAnimation(0.025f);
    }

	public async void LoopAnimation(float delay)
	{
		while (true)
		{
			if (currentWidth >= maxWidth - minWidth)
			{
				currentWidth -= minWidth;
				reverse = true;
			}
			else if (currentWidth <= 0)
			{
				reverse = false;
			}

			if (reverse) currentWidth -= minWidth;
			if (!reverse) currentWidth += minWidth;

			atlas.Region = new Rect2(currentWidth, 0, 393, 432);
			Texture = atlas;

			await ToSignal(GetTree().CreateTimer(delay), SceneTreeTimer.SignalName.Timeout);
		}
	}
}
