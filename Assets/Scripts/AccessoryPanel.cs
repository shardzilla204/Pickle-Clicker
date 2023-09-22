using Godot;
using System;

public partial class AccessoryPanel : TextureRect
{
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
		return Texture;
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		return data.VariantType == Variant.Type.Dictionary && dict.AsGodotDictionary().ContainsKey("color");
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		Texture2D texture = (Texture2D) data;
	}
}
