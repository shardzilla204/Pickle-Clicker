using Godot;
using System;

public partial class Accessory : TextureRect
{
	[Export] public AccessoryType setAccessoryType { get; set; }

	public override void _Ready()
	{
		TextureRect accessoryIcon = GetNode<TextureRect>("../../AccessoryIconPanel/MarginContainer/AccessoryIcon");
		Image image = new Image();
		switch(setAccessoryType)
		{
			case AccessoryType.Topper:
			{
				image.Load("res://Assets/Images/Icons/CosmeticTypeIcons/TopperTypeIcon.png");
				break;
			}
			case AccessoryType.Body:
			{
				image.Load("res://Assets/Images/Icons/CosmeticTypeIcons/BodyTypeIcon.png");
				break;
			}
			case AccessoryType.Tool:
			{
				image.Load("res://Assets/Images/Icons/CosmeticTypeIcons/ToolTypeIcon.png");
				break;
			}
			case AccessoryType.Skin:
			{
				image.Load("res://Assets/Images/Icons/CosmeticTypeIcons/SkinTypeIcon.png");
				break;
			}
		}
		ImageTexture texture = ImageTexture.CreateFromImage(image);
		accessoryIcon.Texture = texture;
	}

	public override Variant _GetDragData(Vector2 atPosition)
	{
		var previewTexture = new TextureRect();
		previewTexture.Texture = Texture;
		previewTexture.ExpandMode = ExpandModeEnum.KeepSize;
		previewTexture.Size = new Vector2(15,15);
		
		var preview = new Control();
		preview.AddChild(previewTexture);

		SetDragPreview(preview);
		Texture = null;
		return previewTexture.Texture;
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		AccessoryType getAccessoryType = data.As<AccessoryType>();
		GD.Print($"Accessory Type: {getAccessoryType}");
		GD.Print($"Dictionary: {getAccessoryType == setAccessoryType}");
		return getAccessoryType == setAccessoryType;
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
	 	Texture = (Texture2D) data;
	}
}
