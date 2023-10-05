using Godot;

public partial class AccessorySlot : Accessory
{
    [Export] public AccessoryType accessoryType;
    public TextureRect typeIcon;
    public TextureRect icon;
    public TextureRect picklePreview;

    public override void _Ready()
    {
        picklePreview = GetNode<TextureRect>("../../PreviewPanel/CenterContainer/PicklePreview");
        icon = GetNode<TextureRect>("./MarginContainer/Icon");
        typeIcon = GetNode<TextureRect>("./TypeContainer/MarginContainer/Icon");
        typeIcon.Texture = GetTypeIcon(accessoryType);
        typeIcon.Texture.ResourceName = GetTypeName(accessoryType);
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

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        accessoryData = data.As<AccessoryData>();
        accessoryData.toAccessorySlot = true;
        if (accessoryData.fromAccessorySlot)
        {
            icon.Texture = null;
        }
        return accessoryData.type == accessoryType;
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        if (accessoryData.toAccessorySlot && !accessoryData.fromAccessorySlot)
        {
            accessoryData.accessoryItem.QueueFree();
            accessoryData.accessoryItem = GetNode<Control>(".");
            accessoryData.fromAccessorySlot = true;
        }
        MergeTextures();
    }

    public void MergeTextures()
    {
        GetTree().CallGroup("AccessorySlots", "MergeTexture");
    }

    public void MergeTexture()
    {
        icon.Texture = accessoryData.icon;
        Image pickleImage = picklePreview.Texture.GetImage();
        Image accessoryImage = accessoryData.accessory.GetImage();
        pickleImage.BlendRect(accessoryImage, new Rect2I(0, 0, 1000, 1000), Vector2I.Zero);
        picklePreview.Texture = ImageTexture.CreateFromImage(pickleImage);
    }
}
