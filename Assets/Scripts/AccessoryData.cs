using Godot;

public enum AccessoryType
{
	Topper,
	Body, 
	Tool,
	Skin
}

public partial class AccessoryData : Resource
{
	[Export] public AccessoryType accessoryType { get; set; }
}
