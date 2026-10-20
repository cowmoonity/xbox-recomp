using System;

namespace SuperCow;

internal class PlayerProfile
{
	public static bool b_fresh = true;

	public static int curlevel = 0;

	public static int curlevelstage = 0;

	public static int maxlevel = 0;

	public static int maxlevelstage = 0;

	public static uint score = 0u;

	public static int numlives = 0;

	public static int[] completedPercents = new int[50];

	public static int gamemode = 0;

	public static uint totalgameplaytime = 0u;

	public static bool b_savegame = false;

	public static trophies_t[] ar_trophies = new trophies_t[13];

	public static string PlayerName;

	public static void AddTrophy(int index)
	{
		ar_trophies[index].active = true;
		ar_trophies[index].state = true;
	}

	public static bool IsTrophyActive(int index)
	{
		return ar_trophies[index].active;
	}

	public static void InitTrophies()
	{
		Array.Clear(ar_trophies, 0, 13);
	}
}
