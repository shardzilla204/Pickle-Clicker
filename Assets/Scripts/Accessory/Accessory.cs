using Godot;

public partial class Accessory : Control
{
	public AccessoryData accessoryData;

    public AccessoryData GetResource(string resourceName)
	{
		AccessoryData resource = GD.Load<AccessoryData>($"res://Assets/Resources/Accessories/{resourceName}.tres");
		return resource;
	}

	public AccessoryItem GetAccessoryContainer()
	{
		PackedScene scene = GD.Load<PackedScene>($"res://Assets/Scenes/Accessory.tscn");
		Node node = scene.Instantiate();
		return (AccessoryItem) node;
	}

	public Texture2D GetTypeIcon(AccessoryType accessoryType)
	{
		string imageFilename;
		switch(accessoryType)
		{
			case AccessoryType.TOPPER:
			{
				imageFilename = "Topper";
				break;
			}
			case AccessoryType.BODY:
			{
				imageFilename = "Body";
				break;
			}
			case AccessoryType.TOOL:
			{
				imageFilename = "Tool";
				break;
			}
			case AccessoryType.SKIN:
			{
				imageFilename = "Skin";
				break;
			}
			default:
			{
				imageFilename = "";
				break;
			}
		}
		Image image = new Image();
		image.Load($"res://Assets/Images/Icons/CosmeticTypes/{imageFilename}.png");
		ImageTexture imageTexture = ImageTexture.CreateFromImage(image);
		return imageTexture;
	}

	public string GetTypeName(AccessoryType accessoryType)
	{
		switch(accessoryType)
		{
			case AccessoryType.TOPPER:
			{
				return "Topper";
			}
			case AccessoryType.BODY:
			{
				return "Body";
			}
			case AccessoryType.TOOL:
			{
				return "Tool";
			}
			case AccessoryType.SKIN:
			{
				return "Skin";
			}
			default:
			{
				return "";
			}
		}
	}
}
