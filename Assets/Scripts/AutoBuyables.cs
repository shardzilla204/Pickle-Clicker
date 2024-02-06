using Godot;
using System;
using System.Collections.Generic;

public class Auto
{
	public Auto(string value)
	{
		alias = value;
		amount = 0;
		level = 1;
	}

	public string alias { get; private set; }
	public int cost { get; set; }

	public int amount { get; set; }
	public const int MAX_AMOUNT = 100;
	
	public int upgradeCost { get; set; }
	
	public int level { get; set; }
	public const int MAX_LEVEL = 5;

	public static List<Auto> autos = new List<Auto>();
}

public partial class AutoBuyables : Node
{
    public override void _Ready()
    {
		Auto.autos.Add(new Auto("Hand"));
		Auto.autos.Add(new Auto("Jar"));
		Auto.autos.Add(new Auto("Book"));
		Auto.autos.Add(new Auto("Tavern"));
		Auto.autos.Add(new Auto("Forest"));
		Auto.autos.Add(new Auto("Beast"));
		Auto.autos.Add(new Auto("Cave"));
		Auto.autos.Add(new Auto("Dungeon"));
		Auto.autos.Add(new Auto("Town"));
		Auto.autos.Add(new Auto("Kingdom"));

		PackedScene autoBuyableScene = GD.Load<PackedScene>("res://Assets/Scenes/AutoBuyable.tscn");
		for (int index = 0; Auto.autos.Count > index; index++)
		{
			Node autoBuyable = autoBuyableScene.Instantiate(); 
			Label autoBuyableName = autoBuyable.GetNode<Label>("./Label");
			autoBuyableName.Text = $"Pickle {Auto.autos[index].alias}";
			AddChild(autoBuyable);

			Auto.autos[index].cost = 100 * (int) Mathf.Pow(2, index) + 100 * 3 * index;
			Auto.autos[index].upgradeCost = 250 * (int) Mathf.Pow(2, index) + 250 * 3 * index;
		}
    }
}