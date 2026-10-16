#define TRACE
using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;
using SuperCow.Helpers;

namespace SuperCow.Properties;

[Serializable]
public class GameSettings
{
	private const string SettingsFilename = "SuperCow.xml";

	private static GameSettings defaultInstance = new GameSettings();

	private static bool needSave = false;

	public float musicvolume = 0.5f;

	public float soundvolume = 0.5f;

	public bool vibration = true;

	public bool freshAccount = true;

	public int curlevel;

	public int curlevelstage;

	public int maxlevel;

	public int maxlevelstage;

	public uint score;

	public int numlives = 3;

	public int[] completedPercents = new int[50];

	public bool[] trophiesState = new bool[13];

	public bool[] trophiesActive = new bool[13];

	public static GameSettings Default => defaultInstance;

	public float MusicVolume
	{
		get
		{
			return musicvolume;
		}
		set
		{
			if (musicvolume != value)
			{
				needSave = true;
			}
			musicvolume = value;
		}
	}

	public float SoundVolume
	{
		get
		{
			return soundvolume;
		}
		set
		{
			if (soundvolume != value)
			{
				needSave = true;
			}
			soundvolume = value;
		}
	}

	public bool Vibration
	{
		get
		{
			return vibration;
		}
		set
		{
			if (vibration != value)
			{
				needSave = true;
			}
			vibration = value;
		}
	}

	public bool FreshAccount
	{
		get
		{
			return freshAccount;
		}
		set
		{
			if (freshAccount != value)
			{
				needSave = true;
			}
			freshAccount = value;
		}
	}

	public int CurLevel
	{
		get
		{
			return curlevel;
		}
		set
		{
			if (curlevel != value)
			{
				needSave = true;
			}
			curlevel = value;
		}
	}

	public int CurLevelStage
	{
		get
		{
			return curlevelstage;
		}
		set
		{
			if (curlevelstage != value)
			{
				needSave = true;
			}
			curlevelstage = value;
		}
	}

	public int MaxLevel
	{
		get
		{
			return maxlevel;
		}
		set
		{
			if (maxlevel != value)
			{
				needSave = true;
			}
			maxlevel = value;
		}
	}

	public int MaxLevelStage
	{
		get
		{
			return maxlevelstage;
		}
		set
		{
			if (maxlevelstage != value)
			{
				needSave = true;
			}
			maxlevelstage = value;
		}
	}

	public uint Score
	{
		get
		{
			return score;
		}
		set
		{
			if (score != value)
			{
				needSave = true;
			}
			score = value;
		}
	}

	public int NumLives
	{
		get
		{
			return numlives;
		}
		set
		{
			if (numlives != value)
			{
				needSave = true;
			}
			numlives = value;
		}
	}

	public int[] CompletedPercents
	{
		get
		{
			return completedPercents;
		}
		set
		{
			completedPercents = value;
		}
	}

	public bool[] TrophiesState
	{
		get
		{
			return trophiesState;
		}
		set
		{
			trophiesState = value;
		}
	}

	public bool[] TrophiesActive
	{
		get
		{
			return trophiesActive;
		}
		set
		{
			trophiesActive = value;
		}
	}

	private GameSettings()
	{
	}

	public static void Initialize()
	{
		if (!Guide.IsTrialMode)
		{
			Load();
		}
	}

	public static void Load()
	{
		bool flag = false;
		needSave = false;
		FileHelper.StorageContainerMRE.WaitOne();
		FileHelper.StorageContainerMRE.Reset();
		try
		{
			StorageDevice xnaUserDevice = FileHelper.XnaUserDevice;
			if (xnaUserDevice != null && xnaUserDevice.IsConnected)
			{
				StorageContainer val = xnaUserDevice.OpenContainer("SuperCow");
				try
				{
					string path = Path.Combine(val.Path, "SuperCow.xml");
					if (File.Exists(path))
					{
						using FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
						if (fileStream.Length > 0)
						{
							GameSettings gameSettings = (GameSettings)new XmlSerializer(typeof(GameSettings)).Deserialize(fileStream);
							if (gameSettings != null)
							{
								defaultInstance = gameSettings;
							}
						}
						else
						{
							needSave = true;
							flag = true;
						}
					}
					else
					{
						needSave = true;
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else if (xnaUserDevice != null && !xnaUserDevice.IsConnected && !SCGlobals.bMUErrorVisible && !SCGlobals.bMUError)
			{
				SCGlobals.bMUError = true;
			}
		}
		catch (Exception ex)
		{
			Trace.WriteLine("Settings Load Failure: " + ex.ToString());
		}
		FileHelper.StorageContainerMRE.Set();
		if (flag)
		{
			Save();
		}
	}

	public static void Save()
	{
		if (!needSave)
		{
			return;
		}
		needSave = false;
		FileHelper.StorageContainerMRE.WaitOne();
		FileHelper.StorageContainerMRE.Reset();
		try
		{
			StorageDevice xnaUserDevice = FileHelper.XnaUserDevice;
			if (xnaUserDevice != null && xnaUserDevice.IsConnected)
			{
				StorageContainer val = xnaUserDevice.OpenContainer("SuperCow");
				try
				{
					string path = Path.Combine(val.Path, "SuperCow.xml");
					using FileStream stream = File.Open(path, FileMode.Create, FileAccess.Write);
					new XmlSerializer(typeof(GameSettings)).Serialize(stream, defaultInstance);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else if (xnaUserDevice != null && !xnaUserDevice.IsConnected && !SCGlobals.bMUErrorVisible && !SCGlobals.bMUError)
			{
				SCGlobals.bMUError = true;
			}
		}
		catch (Exception ex)
		{
			Trace.WriteLine("Settings Load Failure: " + ex.ToString());
		}
		FileHelper.StorageContainerMRE.Set();
	}
}
