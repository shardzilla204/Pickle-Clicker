using Godot;
using Steamworks;
using System;

public partial class SteamManager : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		try
		{
			SteamClient.Init(1919500, true);
		}
		catch (Exception e)
		{
			GD.Print(e);
		}
		
	}

	public void StopSteam()
	{
		SteamClient.Shutdown();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
