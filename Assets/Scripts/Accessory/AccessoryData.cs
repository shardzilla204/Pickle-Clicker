using Godot;

public enum AccessoryType
{
	TOPPER,
	BODY, 
	TOOL,
	SKIN
}

public partial class AccessoryData : Resource
{
	[Export] 
	public AccessoryType type { get; set; }

	[Export] 
	public Texture2D accessory { get; set; }

	[Export] 
	public Texture2D icon { get; set; }

	[Export] 
	public string description { get; set; }

	public Control accessoryItem { get; set; }

	public Control accessoryInventory { get; set; }

	public bool toAccessorySlot { get; set; }

	public bool fromAccessorySlot { get; set; }
}
