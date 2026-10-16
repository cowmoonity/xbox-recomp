using Microsoft.Xna.Framework.GamerServices;
using SuperCow.Properties;

namespace SuperCow;

public class Settings
{
	public int numlevelstages;

	public int[] ar_numlevels_in_stage = new int[110];

	public int curlevel;

	public int curlevelstage;

	public void LoadSettings()
	{
		PlayerProfile.b_fresh = GameSettings.Default.FreshAccount;
		PlayerProfile.curlevel = GameSettings.Default.CurLevel;
		PlayerProfile.curlevelstage = GameSettings.Default.CurLevelStage;
		PlayerProfile.maxlevel = GameSettings.Default.MaxLevel;
		PlayerProfile.maxlevelstage = GameSettings.Default.MaxLevelStage;
		PlayerProfile.score = GameSettings.Default.Score;
		PlayerProfile.numlives = GameSettings.Default.NumLives;
		PlayerProfile.completedPercents = GameSettings.Default.CompletedPercents;
		Sound.SetMusicVolume(GameSettings.Default.MusicVolume);
		Sound.SetSoundVolume(GameSettings.Default.SoundVolume);
		for (int i = 0; i < 13; i++)
		{
			PlayerProfile.ar_trophies[i].active = GameSettings.Default.TrophiesActive[i];
			PlayerProfile.ar_trophies[i].state = GameSettings.Default.TrophiesState[i];
		}
	}

	public void SaveSettings()
	{
		if (!Guide.IsTrialMode)
		{
			GameSettings.Default.FreshAccount = PlayerProfile.b_fresh;
			GameSettings.Default.CurLevel = PlayerProfile.curlevel;
			GameSettings.Default.CurLevelStage = PlayerProfile.curlevelstage;
			GameSettings.Default.MaxLevel = PlayerProfile.maxlevel;
			GameSettings.Default.MaxLevelStage = PlayerProfile.maxlevelstage;
			GameSettings.Default.Score = PlayerProfile.score;
			GameSettings.Default.NumLives = PlayerProfile.numlives;
			GameSettings.Default.CompletedPercents = PlayerProfile.completedPercents;
			for (int i = 0; i < 13; i++)
			{
				GameSettings.Default.TrophiesState[i] = PlayerProfile.ar_trophies[i].state;
				GameSettings.Default.TrophiesActive[i] = PlayerProfile.ar_trophies[i].active;
			}
			GameSettings.Save();
		}
	}

	public void LoadRecords()
	{
	}

	public void LoadProfiles()
	{
	}

	public void RemovePlayer(int num)
	{
	}

	public bool AddPlayer(string name)
	{
		return true;
	}

	public bool AddPlayer(uint theID, string name)
	{
		return true;
	}

	public bool RenamePlayer(int num, string name)
	{
		return true;
	}

	public void SelectPlayer(int num)
	{
	}

	public void PlayerCompleteLevel(ref bool b_stagepassed, ref bool b_gamepassed)
	{
		b_stagepassed = false;
		b_gamepassed = false;
		PlayerProfile.curlevel++;
		if (PlayerProfile.curlevel >= ar_numlevels_in_stage[PlayerProfile.curlevelstage])
		{
			PlayerProfile.curlevelstage++;
			PlayerProfile.curlevel = 0;
			b_stagepassed = true;
			if (PlayerProfile.curlevelstage == 10)
			{
				b_gamepassed = true;
			}
		}
		if (PlayerProfile.maxlevelstage < PlayerProfile.curlevelstage)
		{
			PlayerProfile.maxlevelstage = PlayerProfile.curlevelstage;
			PlayerProfile.maxlevel = PlayerProfile.curlevel;
		}
		else if (PlayerProfile.maxlevelstage == PlayerProfile.curlevelstage && PlayerProfile.maxlevel < PlayerProfile.curlevel)
		{
			PlayerProfile.maxlevel = PlayerProfile.curlevel;
		}
		curlevel = PlayerProfile.curlevel;
		curlevelstage = PlayerProfile.curlevelstage;
	}

	public void PlayerGameOver()
	{
		PlayerProfile.score = 0u;
		PlayerProfile.numlives = 3;
		PlayerProfile.curlevel = 0;
		if (PlayerProfile.curlevelstage == PlayerProfile.maxlevelstage)
		{
			PlayerProfile.maxlevel = 0;
		}
	}

	public void PlayerLoseLife()
	{
		PlayerProfile.numlives--;
	}

	public void PlayerAddLife()
	{
		PlayerProfile.numlives++;
	}

	public void AddPlayerScore(uint addscore)
	{
		PlayerProfile.score += addscore;
	}

	public uint GetPlayerScore()
	{
		return PlayerProfile.score;
	}

	public int GetPlayerCurLevel()
	{
		return PlayerProfile.curlevel;
	}

	public int GetPlayerMaxLevel()
	{
		return GetAbsCurLevel(PlayerProfile.maxlevelstage, PlayerProfile.maxlevel);
	}

	public int GetPlayerGlobalMaxLevel()
	{
		return 0;
	}

	public string GetPlayerName()
	{
		return PlayerProfile.PlayerName;
	}

	public void AddTrophy(int index)
	{
		PlayerProfile.AddTrophy(index);
	}

	public bool IsTrophyActive(int index)
	{
		return PlayerProfile.IsTrophyActive(index);
	}

	public trophies_t GetTrophy(int index)
	{
		return PlayerProfile.ar_trophies[index];
	}

	public trophies_t[] GetTrophies()
	{
		return PlayerProfile.ar_trophies;
	}

	public int GetNumPlayerLives()
	{
		return PlayerProfile.numlives;
	}

	public void SetPlayerCurLevel(int numlev)
	{
		curlevel = 0;
		curlevelstage = 0;
		for (int i = 0; i < numlevelstages; i++)
		{
			if (curlevel + ar_numlevels_in_stage[i] <= numlev)
			{
				curlevelstage++;
				curlevel += ar_numlevels_in_stage[i];
				continue;
			}
			curlevel = numlev - curlevel;
			break;
		}
		PlayerProfile.curlevel = curlevel;
		PlayerProfile.curlevelstage = curlevelstage;
	}

	public void SetPlayerScore(uint score)
	{
		PlayerProfile.score = score;
	}

	public void SetPlayerGameMode(int num)
	{
		PlayerProfile.gamemode = num;
	}

	public int GetPlayerMaxStage()
	{
		return PlayerProfile.maxlevelstage;
	}

	public void ClearPlayerTrophies()
	{
		PlayerProfile.InitTrophies();
	}

	public int GetPlayerCurStage()
	{
		return PlayerProfile.curlevelstage;
	}

	public int GetStageLevel(int n)
	{
		int num = 0;
		for (int i = 0; i < GetStage(n); i++)
		{
			num += ar_numlevels_in_stage[i];
		}
		return n - num;
	}

	public int GetStage(int numlevel)
	{
		int num = 0;
		int i;
		for (i = 0; i < numlevelstages; i++)
		{
			num += ar_numlevels_in_stage[i];
			if (numlevel < num)
			{
				break;
			}
		}
		return i;
	}

	public int GetLevelsBeforeStage(int stage)
	{
		int num = 0;
		for (int i = 0; i < stage; i++)
		{
			num += ar_numlevels_in_stage[i];
		}
		return num;
	}

	public int GetAbsCurLevel(int stage, int level_in_stage)
	{
		int num = 0;
		for (int i = 0; i < stage; i++)
		{
			num += ar_numlevels_in_stage[i];
		}
		return num + level_in_stage;
	}

	public void LoadGameSettings()
	{
		Store.SetCurrentDirectory("levels", theAdd: true);
		int len = 0;
		byte[] array = Store.storeLoadFileArray("stages.txt", ref len, b_errorexit: false);
		array[len - 1] = 0;
		string script = SuperCowHelper.StringFromByteArray(array);
		numlevelstages = SuperCowHelper.get_script_int(script, "numstages=", 0);
		for (int i = 0; i < numlevelstages; i++)
		{
			string text = "";
			text = $"stage{i:D}=";
			ar_numlevels_in_stage[i] = SuperCowHelper.get_script_int(script, text, 0);
		}
	}
}
