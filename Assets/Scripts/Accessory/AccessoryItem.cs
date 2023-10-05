using Godot;

public partial class AccessoryItem : Accessory
{
    public override void _Ready()
    {
        accessoryData = GetResource(Name);
        accessoryData.accessoryItem = GetNode<Control>(".");
        accessoryData.accessoryInventory = GetNode<Control>("..");
        TextureRect icon = GetNode<TextureRect>("./MarginContainer/Icon");
        icon.Texture = accessoryData.icon;
        TextureRect textureIcon = GetNode<TextureRect>("./TypeContainer/MarginContainer/Icon");
        textureIcon.Texture = GetTypeIcon(accessoryData.type);
    }

    public override Variant _GetDragData(Vector2 atPosition)
    {
        TextureRect previewTexture = new TextureRect()
        {
            Texture = accessoryData.icon,
            Size = new Vector2(15, 15)
        };

        Control preview = new Control();
        preview.AddChild(previewTexture);

        SetDragPreview(preview);

        return accessoryData;
    }
}
