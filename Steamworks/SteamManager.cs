using Godot;
using Steamworks;
using Steamworks.Data;
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

	public void ExitSteam()
	{
		SteamClient.Shutdown();
	}

	public void ClearAchievements()
	{
		SteamUserStats.ResetAll(true);
		SteamUserStats.StoreStats();
		SteamUserStats.RequestCurrentStats();
	}
	
	public void GetAchievement()
	{
		Achievement achievement = new Achievement("ACH_KILL_250");
		achievement.Trigger();
		SteamUserStats.StoreStats();
		SteamUserStats.RequestCurrentStats();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
