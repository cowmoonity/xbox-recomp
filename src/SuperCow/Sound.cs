using System;
using Microsoft.Xna.Framework.Audio;

namespace SuperCow;

public static class Sound
{
	private static AudioEngine engine;

	private static WaveBank wavebank;

	private static SoundBank soundbank;

	private static string[] cueNames = new string[376]
	{
		"boss", "credits", "game1", "game2", "game3", "game4", "game5", "map", "menu_music", "addthunder",
		"addtrophy", "appgag", "applause", "bat", "batvoice", "birdeggcrash", "bonuscoin1", "bonuscoin2", "bonuscoin3", "bonusgem",
		"bonustreasure", "bridge1", "bridge2", "brook", "bug", "cabbage", "cabbagecrash", "camera", "checkpont", "checkpont2",
		"dog", "dogvoice", "ExpSalute1", "ExpSalute2", "flycatcher", "flycatcherbite", "frog", "frogcrash", "frogspitjump", "fruitdown",
		"fruiteat", "garbage1", "garbage2", "goat", "guillo1", "guillo2", "hedgehog", "herojump", "herojump1", "herojump2",
		"herojump3", "hurt1", "hurt2", "hurt3", "hurt4", "hurt5", "hurt6", "hurt7", "hyperstephero1", "hyperstephero2",
		"hyperstephero3", "key", "killmonster", "kiss", "landing", "landinghard", "levelcomplete", "liana", "lightning", "loadcomplete",
		"magic", "mapopenlevel", "matrac", "menu", "menu2", "menu3", "moledig", "molestone", "molestonecrash", "monsterdeath",
		"openexit", "pan", "pigstep1", "pigstep2", "pigstep3", "pigvoice", "platform1", "platform1stop", "platform2", "platform3",
		"platform3stop", "powerupfirmness", "poweruphealth", "powerupkefir", "poweruplife", "powerupmegajump", "powerupshoe", "praise1", "praise2", "praise3",
		"profretort", "profretortflight", "rake", "raven", "ravenvoice", "secretfound", "spider", "spring", "stats0", "stats1",
		"stats2", "stats3", "stats4", "stats5", "stephero1", "stephero2", "stephero3", "taskcompleted", "tip", "trampo",
		"wasp", "waterfall", "whistle", "write1", "write2", "write3", "write4", "write5", "write6", "write7",
		"yahoo", "1_h1", "1_h2", "1_h3", "1_h4", "1_h5", "1_h6", "1_h7", "1_h8", "1_v1",
		"1_v10", "1_v11", "1_v2", "1_v3", "1_v4", "1_v5", "1_v6", "1_v7", "1_v8", "1_v9",
		"10_h1", "10_h2", "10_h3", "10_h4", "10_h5", "10_v1", "10_v10", "10_v11", "10_v12", "10_v13",
		"10_v14", "10_v15", "10_v16", "10_v17", "10_v18", "10_v2", "10_v3", "10_v4", "10_v5", "10_v6",
		"10_v7", "10_v8", "10_v9", "2_h1", "2_h2", "2_h3", "2_h4", "2_h5", "2_h6", "2_h7",
		"2_h8", "2_v1", "2_v10", "2_v11", "2_v12", "2_v13", "2_v14", "2_v15", "2_v16", "2_v17",
		"2_v18", "2_v2", "2_v3", "2_v4", "2_v5", "2_v6", "2_v7", "2_v8", "2_v9", "3_h1",
		"3_h2", "3_h3", "3_h4", "3_h5", "3_h6", "3_h7", "3_h8", "3_h9", "3_v1", "3_v10",
		"3_v11", "3_v12", "3_v13", "3_v14", "3_v15", "3_v16", "3_v2", "3_v3", "3_v4", "3_v5",
		"3_v6", "3_v7", "3_v8", "3_v9", "4_h1", "4_h10", "4_h11", "4_h2", "4_h3", "4_h4",
		"4_h5", "4_h6", "4_h7", "4_h8", "4_h9", "4_v1", "4_v10", "4_v11", "4_v12", "4_v2",
		"4_v3", "4_v4", "4_v5", "4_v6", "4_v7", "4_v8", "4_v9", "5_h1", "5_h10", "5_h11",
		"5_h12", "5_h2", "5_h3", "5_h4", "5_h5", "5_h6", "5_h7", "5_h8", "5_h9", "5_v1",
		"5_v10", "5_v11", "5_v12", "5_v13", "5_v14", "5_v2", "5_v3", "5_v4", "5_v5", "5_v6",
		"5_v7", "5_v8", "5_v9", "6_h1", "6_h2", "6_h3", "6_h4", "6_h5", "6_h6", "6_h7",
		"6_h8", "6_v1", "6_v10", "6_v2", "6_v3", "6_v4", "6_v5", "6_v6", "6_v7", "6_v8",
		"6_v9", "7_h1", "7_h2", "7_h3", "7_h4", "7_h5", "7_h6", "7_h7", "7_h8", "7_h9",
		"7_v1", "7_v10", "7_v11", "7_v12", "7_v13", "7_v14", "7_v2", "7_v3", "7_v4", "7_v5",
		"7_v6", "7_v7", "7_v8", "7_v9", "8_h1", "8_h10", "8_h11", "8_h12", "8_h2", "8_h3",
		"8_h4", "8_h5", "8_h6", "8_h7", "8_h8", "8_h9", "8_v1", "8_v10", "8_v11", "8_v12",
		"8_v13", "8_v2", "8_v3", "8_v4", "8_v5", "8_v6", "8_v7", "8_v8", "8_v9", "9_h1",
		"9_h2", "9_h3", "9_h4", "9_h5", "9_h6", "9_h7", "9_h8", "9_v1", "9_v10", "9_v11",
		"9_v12", "9_v13", "9_v14", "9_v15", "9_v16", "9_v2", "9_v3", "9_v4", "9_v5", "9_v6",
		"9_v7", "9_v8", "9_v9", "backdanger", "backfarm", "death1", "death2", "death3", "death4", "death5",
		"death6", "intro1", "intro2", "intro3", "intro4", "intro5"
	};

	public static Cue Play(string sound)
	{
		Cue cue = soundbank.GetCue(sound);
		cue.Play();
		return cue;
	}

	public static Cue Play(Sounds sound)
	{
		Cue cue = soundbank.GetCue(cueNames[(int)sound]);
		cue.Play();
		return cue;
	}

	public static void PlayCue(Sounds sound)
	{
		soundbank.PlayCue(cueNames[(int)sound]);
	}

	public static void SetSoundParam(string sound, float value)
	{
		soundbank.GetCue(sound).SetVariable("Volume", value);
	}

	public static void Addsound(string sound)
	{
		soundbank.PlayCue(sound);
	}

	public static void Update()
	{
		engine.Update();
	}

	public static void Stop(Cue cue)
	{
		if (cue != null)
		{
			cue.Stop((AudioStopOptions)1);
		}
	}

	public static void Initialize()
	{
		engine = new AudioEngine(@"content\audio\supercow.xgs");
		wavebank = new WaveBank(engine, @"content\audio\supercow.xwb");
		soundbank = new SoundBank(engine, @"content\audio\supercow.xsb");
		SetMusicVolume(0.5f);
		SetSoundVolume(0.5f);
	}

	public static void Shutdown()
	{
		soundbank.Dispose();
		wavebank.Dispose();
		engine.Dispose();
	}

	public static void SetSoundVolume(float fVolume)
	{
		AudioCategory category = engine.GetCategory("Sounds");
		category.SetVolume(fVolume);
		AudioCategory category2 = engine.GetCategory("Voices");
		category2.SetVolume(fVolume);
	}

	public static void SetMusicVolume(float fVolume)
	{
		AudioCategory category = engine.GetCategory("Music");
		category.SetVolume(fVolume);
	}
}
