using Godot;

public partial class AccessoryInventory : Accessory
{
    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        accessoryData = data.As<AccessoryData>();
        return accessoryData is AccessoryData;
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        if (accessoryData.fromAccessorySlot) 
        {
            TextureRect icon = accessoryData.accessoryItem.GetNode<TextureRect>("./MarginContainer/Icon");
            icon.Texture = null;
            string resourceName = data.As<AccessoryData>().ResourceName;
            AccessoryItem accessoryItem = GetAccessoryContainer();
            accessoryItem.Name = resourceName;
            accessoryData.accessoryInventory.AddChild(accessoryItem);
        }
        accessoryData.fromAccessorySlot = false;
    }
}
