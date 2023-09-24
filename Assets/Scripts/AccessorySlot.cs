using Godot;
using System;

public partial class AccessorySlot : TextureRect
{
	[Export] public AccessoryType setAccessoryType { get; set; }
	// public override void _Ready()
	// {
	// 	Image image = new Image();
	// 	switch(setAccessoryType)
	// 	{
	// 		case AccessoryType.Topper:
	// 		{
	// 			image.Load("res://Assets/Images/Icons/CosmeticTypeIcons/TopperTypeIcon.png");
	// 			break;
	// 		}
	// 		case AccessoryType.Body:
	// 		{
	// 			image.Load("res://Assets/Images/Icons/CosmeticTypeIcons/BodyTypeIcon.png");
	// 			break;
	// 		}
	// 		case AccessoryType.Tool:
	// 		{
	// 			image.Load("res://Assets/Images/Icons/CosmeticTypeIcons/ToolTypeIcon.png");
	// 			break;
	// 		}
	// 		case AccessoryType.Skin:
	// 		{
	// 			image.Load("res://Assets/Images/Icons/CosmeticTypeIcons/SkinTypeIcon.png");
	// 			break;
	// 		}
	// 	}
	// 	ImageTexture texture = ImageTexture.CreateFromImage(image);
	// 	Texture = texture;
	// }
}
