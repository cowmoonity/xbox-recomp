using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public class SuperCowGame : ThreadInterface
{
	private enum game_icons_t
	{
		MAIN_ICON,
		LIFE_ICON,
		SCORE_ICON,
		POWERUP1_ICON,
		POWERUP2_ICON,
		POWERUP3_ICON,
		MONSTER1_ICON,
		MONSTER2_ICON,
		MONSTER3_ICON,
		MONSTER4_ICON,
		TOTAL_ICONS
	}

	private vec2_t p1scr = default(vec2_t);

	private vec2_t p2scr = default(vec2_t);

	private vec2_t p3scr = default(vec2_t);

	private vec2_t p4scr = default(vec2_t);

	public static Texture2D mainTex = null;

	private string lives = "x3";

	private string stageLevel = "1-1";

	public vec2_t vnull = default(vec2_t);

	public vec2_t ray = default(vec2_t);

	public vec2_t v = default(vec2_t);

	private Cue mMusic;

	public vec2_t camerabossposition0;

	public frect_t camerabossrect;

	public bool b_bossfight;

	public bool b_bossdead;

	public float bossfighttimer;

	public float firststarttimer;

	public int scoreatlevel;

	public int numlives = 3;

	public static bool bFirstDraw = true;

	public int totalscore;

	public static ManagedThread calcThread;

	public static vec3_t pl_p1 = default(vec3_t);

	public static vec3_t pl_p2 = default(vec3_t);

	public static vec3_t pl_p3 = default(vec3_t);

	public static vec3_t pl_p4 = default(vec3_t);

	public static vec2_t pl_t1 = default(vec2_t);

	public static vec2_t pl_t2 = default(vec2_t);

	public static vec2_t pl_t3 = default(vec2_t);

	public static vec2_t pl_t4 = default(vec2_t);

	public int START_LEVEL_TIME = 1000;

	private static float ftime = 0f;

	private static float ftime2 = 0f;

	public float level_bottom;

	public float level_top;

	public float level_left;

	public float level_right;

	public float startleveltime;

	public float endleveltime;

	public float camera_height;

	public float cameradown;

	public float le_scale = 1f;

	private float le_plane_scale = 1f;

	public float CAMERAEFFECTTIME = 1000f;

	public float cameraeffecttimer;

	public int cameraeffectindex;

	public float life0 = 1f;

	public int liveparcount;

	public float totalscore0;

	public int scoreparcount;

	public vec2_t arrow_taks_pos = new vec2_t(0f, 0f);

	public CMonster p_bossmonster;

	public bool b_startlevel;

	public bool b_showedTask;

	public bool b_maindeath;

	public bool b_jumpkey;

	public bool b_switchexit;

	public float prevhero_y;

	public int END_LEVEL_TIME = 6900;

	public int TROPHY_APP_TIME = 3000;

	public static float fgageffecttime = 0f;

	public static float fgagtime = 0f;

	public static float fgagstep = 0f;

	public static texture_t p_gag = default(texture_t);

	public CSprite lifesprite = new CSprite();

	private int CalcFrames;

	private int CalcPhysics;

	private int CalcMonsterPhys;

	private frect_t draw_frect = default(frect_t);

	private int bonusleveltimer;

	public bool ToggleGround = true;

	public string str_trophyname;

	public bool b_bonuslevel;

	private int NUM_LEVEL_TASKS = 6;

	public texture_t[] ar_taskicon = new texture_t[6];

	public texture_t[] ar_texicons = new texture_t[10];

	public texture_t p_levelbartex;

	public texture_t p_taskbartex;

	public texture_t p_heroarrow;

	public texture_t p_compass;

	public texture_t p_livebar_tex;

	public SuperCowGame()
	{
		p1scr.assign(-400f, 300f);
		p2scr.assign(400f, 300f);
		p3scr.assign(400f, -300f);
		p4scr.assign(-400f, -300f);
	}

	public void GameStartup()
	{
		SetGameLight();
		Ground.LoadGround();
		Bonus.LoadBonuses();
		Store.SetCurrentDirectory("ui", theAdd: false);
		ref texture_t reference = ref ar_texicons[3];
		reference = SCGlobals.texturehost.Load("jump.tga");
		ref texture_t reference2 = ref ar_texicons[4];
		reference2 = SCGlobals.texturehost.Load("horseshoe.tga");
		ref texture_t reference3 = ref ar_texicons[5];
		reference3 = SCGlobals.texturehost.Load("firmness.tga");
		ref texture_t reference4 = ref ar_texicons[0];
		reference4 = SCGlobals.texturehost.Load("lifebar.tga");
		ref texture_t reference5 = ref ar_texicons[2];
		reference5 = SCGlobals.texturehost.Load("scorebar.tga");
		ref texture_t reference6 = ref ar_taskicon[0];
		reference6 = SCGlobals.texturehost.Load("taskicon1.tga");
		ref texture_t reference7 = ref ar_taskicon[1];
		reference7 = SCGlobals.texturehost.Load("taskicon2.tga");
		ref texture_t reference8 = ref ar_taskicon[2];
		reference8 = SCGlobals.texturehost.Load("taskicon3.tga");
		ref texture_t reference9 = ref ar_taskicon[3];
		reference9 = SCGlobals.texturehost.Load("taskicon4.tga");
		ref texture_t reference10 = ref ar_taskicon[4];
		reference10 = SCGlobals.texturehost.Load("taskicon2.tga");
		SCGlobals.p_mainshadow = SCGlobals.texturehost.Load("mainshadow.tga");
		p_levelbartex = SCGlobals.texturehost.Load("levelbar.tga");
		p_taskbartex = SCGlobals.texturehost.Load("taskbar.tga");
		p_heroarrow = SCGlobals.texturehost.Load("arrow.tga");
		p_compass = SCGlobals.texturehost.Load("compass.tga");
		p_livebar_tex = SCGlobals.texturehost.Load("lifebar2.tga");
		Store.SetCurrentDirectory("", theAdd: false);
		lifesprite.Load("life.tga", 32, 8);
		mainTex = SCGlobals.texturehost.GetByName("_a_aid.jpg").mImage;
	}

	public void InitLevel(int numlevel)
	{
		scoreatlevel = 0;
		totalscore = (int)SuperCow.Settings.GetPlayerScore();
		numlives = PlayerProfile.numlives;
		lives = "x" + (numlives + 1);
		SuperCow.Settings.curlevelstage = SuperCow.Settings.GetPlayerCurStage();
		SuperCow.Settings.curlevel = SuperCow.Settings.GetPlayerCurLevel();
		stageLevel = SuperCow.Settings.curlevelstage + 1 + "-" + (SuperCow.Settings.curlevel + 1);
		InitGame();
	}

	public void InitLevelThread()
	{
		SuperCow.Settings.curlevelstage = SuperCow.Settings.GetPlayerCurStage();
		SuperCow.Settings.curlevel = SuperCow.Settings.GetPlayerCurLevel();
		if (SuperCow.Settings.curlevel == 0 && SuperCow.Settings.curlevelstage == 0)
		{
			firststarttimer = 20000f;
		}
		else
		{
			firststarttimer = 0f;
		}
		scoreatlevel = 0;
		totalscore = (int)SuperCow.Settings.GetPlayerScore();
		numlives = PlayerProfile.numlives;
		lives = "x" + (numlives + 1);
		SCGlobals.level.Load(SuperCow.Settings.curlevelstage, SuperCow.Settings.curlevel);
	}

	public void SetGameLight()
	{
		SCGlobals.matcommon.Diffuse.assign(1f, 1f, 1f, 1f);
		SCGlobals.matcommon.Ambient.assign(0.4f, 0.4f, 0.4f, 1f);
		SCGlobals.matselected.Diffuse.assign(1f, 1f, 1f, 1f);
		SCGlobals.matselected.Ambient.assign(0.4f, 0.4f, 0.4f, 1f);
		SCGlobals.color_selected.assign(byte.MaxValue, 0, 0, byte.MaxValue);
		SCGlobals.light.Ambient.assign(0.7f, 0.7f, 0.7f, 1f);
		SCGlobals.light.Diffuse.assign(1f, 1f, 1f, 0f);
		SCGlobals.light.Specular.assign(1f, 1f, 1f, 1f);
		SCGlobals.light.Direction.assign(-1f, 1f, -1f);
		SCGlobals.light.Type = LightingType.LightDirectional;
		SCGlobals.light.Range = 500f;
		SCGlobals.light.Attenuation0 = 1f;
		CowRender.SetLighting(theEnable: false);
		CowRender.SetLight(0u, SCGlobals.light);
		CowRender.EnableLight(0u);
		CowRender.SetMaterial(SCGlobals.matcommon);
		SCGlobals.b_needsetlight = false;
	}

	public void DrawGame()
	{
		if (SCGlobals.b_needsetlight)
		{
			SetGameLight();
		}
		CalculateGameGirdSize();
		if (SCGlobals.setpausetimer > 0f && SCGlobals.b_gamemenu)
		{
			SCGlobals.setpausetimer += SCGlobals.ftickperframe;
			if (SCGlobals.setpausetimer > 500f)
			{
				SCGlobals.setpausetimer = 500f;
			}
			float num = 1f - 0.3f * SCGlobals.setpausetimer / 500f;
			CowRender.SetGlobal2DColor(new color_t(num, num, num, 1f));
		}
		if (SCGlobals.setpausetimer > 0f && !SCGlobals.b_gamemenu)
		{
			SCGlobals.setpausetimer -= SCGlobals.ftickperframe;
			if (SCGlobals.setpausetimer < 0f)
			{
				SCGlobals.setpausetimer = 0f;
			}
			float num2 = 1f - 0.3f * SCGlobals.setpausetimer / 500f;
			CowRender.SetGlobal2DColor(new color_t(num2, num2, num2, 1f));
		}
		Particles.SetParticleLayer(0);
		CowRender.SetLighting(theEnable: false);
		CalculateCameraEffect();
		DrawBackground2();
		CowRender.EnableRS(2u);
		DrawLevelObjects2();
		Weapons.DrawBullets();
		CoolDraw.DrawLightning();
		CowRender.SetGlobal2DColor(color_t.ColorWhite);
		CowRender.EnableRS(7u);
		Particles.CalculateParticles();
		Particles.DrawParticles();
		GlowFXHost.gfxhCalculate();
		GlowFXHost.gfxhDraw();
		Particles.SetParticleLayer(1);
		GameDrawInfoPannel();
		Particles.CalculateParticles();
		Particles.DrawParticles();
		Particles.SetParticleLayer(0);
	}

	public void DrawBackground2()
	{
		ftime2 += SCGlobals.ftickperframe;
		SCGlobals.cam.load_identity();
		SCGlobals.cam.translate(0f, 0f, 50f);
		CowRender.LoadCamera(SCGlobals.cam);
		CowRender.SetOrtho(800f, 600f, 1f, 500f);
		CowRender.DisableRS(5u);
		CowRender.SetBlend(0);
		frect_t vc = default(frect_t);
		levelback_t levelback_t2 = SCGlobals.ar_levelbacks[SCGlobals.curlevelback];
		float z = 10f;
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		// REMARK: it was not mirror after all, huh?
		graphicsDevice.SamplerStates[0] = AnisotropicHorisontalWrap;
		CowRender.EnableRS(7u);
		for (int i = 0; i < levelback_t2.numplanes; i++)
		{
			levelbackplane_t levelbackplane_t2 = levelback_t2.ar_planes[i];
			float num = ((levelbackplane_t2.width == 0f) ? ((float)levelbackplane_t2.p_tex.width) : levelbackplane_t2.width);
			float num2 = ((levelbackplane_t2.height == 0f) ? ((float)levelbackplane_t2.p_tex.height) : levelbackplane_t2.height);
			float num3 = (0f - SCGlobals.camerapos.x) * 32f / num * levelbackplane_t2.speed + ftime2 * levelbackplane_t2.wind * 1E-05f;
			float y = levelbackplane_t2.vertical + levelbackplane_t2.vertdelta * (-0.5f + camera_height);
			if (levelbackplane_t2.b_noblend)
			{
				CowRender.DisableRS(4u);
				CowRender.DisableRS(5u);
			}
			else
			{
				CowRender.EnableRS(5u);
				// TODO: alpha test not implemented - 1f
				CowRender.EnableRS(4u);
			}
			CowRender.BindTexture(levelbackplane_t2.p_tex);
			if (levelbackplane_t2.b_single)
			{
				float num4 = (float)Math.IEEERemainder((0f - num3) * num, levelbackplane_t2.distance);
				int num5 = (int)(1067f / levelbackplane_t2.distance);
				for (int j = -num5 - 1; j <= num5 + 1; j++)
				{
					vc.assign(new vec2_t(levelbackplane_t2.distance * (float)j + num4, y), num, num2);
					CowRender.DrawColorQuad2DDynBuffer(vc, SCGlobals.tc_normal, levelbackplane_t2.color.dcolor(), z);
				}
			}
			else
			{
				float num6 = 1067f;
				vc.assign(new vec2_t(0f, y), num6, num2);
				draw_frect.assign(num3, 0f, num6 / num + num3, 1f);
				CowRender.DrawColorQuad2DDynBuffer(vc, draw_frect, levelbackplane_t2.color.dcolor(), z);
				if (levelbackplane_t2.zastup_down > 0f)
				{
					vc.y0 = vc.y1 + 2f;
					vc.y1 -= levelbackplane_t2.zastup_down;
					CowRender.DrawColorQuad2DDynBuffer(vc, new frect_t(num3, 1f - 2f / num2, num6 / num + num3, 1f), levelbackplane_t2.color.dcolor(), z);
				}
				vc.assign(new vec2_t(0f, y), num6, num2);
				if (levelbackplane_t2.zastup_up > 0f)
				{
					vc.y1 = vc.y0 - 2f;
					vc.y0 += levelbackplane_t2.zastup_up;
					CowRender.DrawColorQuad2DDynBuffer(vc, new frect_t(num3, 0f, num6 / num + num3, 0f + 2f / num2), levelbackplane_t2.color.dcolor(), z);
				}
			}
			CowRender.DrawDynBuffer();
		}
		CowRender.DrawDynBuffer();
		graphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
		CowRender.DisableRS(7u);
		CowRender.SetGlobal2DColor(color_t.ColorWhite);
	}
	
	// TODO: defining shit in the middle of game class not good
	public static SamplerState AnisotropicMirror = new()
	{
		Filter = TextureFilter.Anisotropic,
		AddressU = TextureAddressMode.Mirror,
		AddressV = TextureAddressMode.Mirror
	};

	private static SamplerState AnisotropicHorisontalWrap = new()
	{
		Filter = TextureFilter.Anisotropic,
		AddressU = TextureAddressMode.Wrap,
		AddressV = TextureAddressMode.Clamp
	};

	public void DrawBackground()
	{
		ftime2 += SCGlobals.ftickperframe;
		SCGlobals.cam.load_identity();
		SCGlobals.cam.translate(0f, 0f, 50f);
		CowRender.LoadCamera(SCGlobals.cam);
		CowRender.SetOrtho(800f, 600f, 1f, 500f);
		CowRender.DisableRS(5u);
		CowRender.SetBlend(0);
		frect_t vc = default(frect_t);
		levelback_t levelback_t2 = SCGlobals.ar_levelbacks[SCGlobals.curlevelback];
		float z = 10f;
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		// REMARK: not mirror, again
		graphicsDevice.SamplerStates[0] = AnisotropicHorisontalWrap;
		CowRender.EnableRS(7u);
		for (int i = 0; i < levelback_t2.numplanes; i++)
		{
			levelbackplane_t levelbackplane_t2 = levelback_t2.ar_planes[i];
			float num = ((levelbackplane_t2.width == 0f) ? ((float)levelbackplane_t2.p_tex.width) : levelbackplane_t2.width);
			float num2 = ((levelbackplane_t2.height == 0f) ? ((float)levelbackplane_t2.p_tex.height) : levelbackplane_t2.height);
			float num3 = (0f - SCGlobals.camerapos.x) * 32f / num * levelbackplane_t2.speed + ftime2 * levelbackplane_t2.wind * 1E-05f;
			float y = levelbackplane_t2.vertical + levelbackplane_t2.vertdelta * (-0.5f + camera_height);
			if (levelbackplane_t2.b_noblend)
			{
				CowRender.DisableRS(4u);
				CowRender.DisableRS(5u);
			}
			else
			{
				CowRender.EnableRS(5u);
				// TODO: alpha test not implemented - 1f
				CowRender.EnableRS(4u);
			}
			CowRender.BindTexture(levelbackplane_t2.p_tex);
			if (levelbackplane_t2.b_single)
			{
				float num4 = (float)Math.IEEERemainder((0f - num3) * num, levelbackplane_t2.distance);
				int num5 = (int)(1067f / levelbackplane_t2.distance);
				for (int j = -num5 - 1; j <= num5 + 1; j++)
				{
					vc.assign(new vec2_t(levelbackplane_t2.distance * (float)j + num4, y), num, num2);
					CowRender.DrawColorQuad2DDynBuffer(vc, SCGlobals.tc_normal, levelbackplane_t2.color.dcolor(), z);
				}
				continue;
			}
			float num6 = 1067f;
			vc.assign(new vec2_t(0f, y), num6, num2);
			draw_frect.assign(num3, 0f, num6 / num + num3, 1f);
			CowRender.DrawColorQuad2DDynBuffer(vc, draw_frect, levelbackplane_t2.color.dcolor(), z);
			if (levelbackplane_t2.zastup_down > 0f)
			{
				vc.y0 = vc.y1 + 2f;
				vc.y1 -= levelbackplane_t2.zastup_down;
				CowRender.DrawColorQuad2DDynBuffer(vc, new frect_t(num3, 1f - 2f / num2, num6 / num + num3, 1f), levelbackplane_t2.color.dcolor(), z);
			}
			vc.assign(new vec2_t(0f, y), num6, num2);
			if (levelbackplane_t2.zastup_up > 0f)
			{
				vc.y1 = vc.y0 - 2f;
				vc.y0 += levelbackplane_t2.zastup_up;
				CowRender.DrawColorQuad2DDynBuffer(vc, new frect_t(num3, 0f, num6 / num + num3, 0f + 2f / num2), levelbackplane_t2.color.dcolor(), z);
			}
		}
		CowRender.DrawDynBuffer();
		graphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
		CowRender.DisableRS(7u);
		CowRender.SetGlobal2DColor(color_t.ColorWhite);
	}

	public void GameVictory()
	{
		if (SCGlobals.levelstat.coins > SCGlobals.levelstat.numcoins)
		{
			SCGlobals.levelstat.numcoins = SCGlobals.levelstat.coins;
		}
		if (SCGlobals.levelstat.garbage > SCGlobals.levelstat.numgarbage)
		{
			SCGlobals.levelstat.numgarbage = SCGlobals.levelstat.garbage;
		}
		if (SCGlobals.levelstat.gems > SCGlobals.levelstat.numgems)
		{
			SCGlobals.levelstat.numgems = SCGlobals.levelstat.gems;
		}
		if (SCGlobals.levelstat.keys > SCGlobals.levelstat.numkeys)
		{
			SCGlobals.levelstat.numkeys = SCGlobals.levelstat.keys;
		}
		if (SCGlobals.levelstat.monsters > SCGlobals.levelstat.nummonsters)
		{
			SCGlobals.levelstat.nummonsters = SCGlobals.levelstat.monsters;
		}
		if (SCGlobals.levelstat.secrets > SCGlobals.levelstat.numsecrets)
		{
			SCGlobals.levelstat.numsecrets = SCGlobals.levelstat.secrets;
		}
		int num = SCGlobals.levelstat.numcoins + SCGlobals.levelstat.numgems + SCGlobals.levelstat.nummonsters + SCGlobals.levelstat.numsecrets;
		int num2 = SCGlobals.levelstat.coins + SCGlobals.levelstat.gems + SCGlobals.levelstat.monsters + SCGlobals.levelstat.secrets;
		if (num > 0)
		{
			SCGlobals.levelcompleteness = (int)(100f * (float)num2 / (float)num);
		}
		else
		{
			SCGlobals.levelcompleteness = 0;
		}
		if (!Guide.IsTrialMode && PlayerProfile.completedPercents[SuperCow.Settings.GetAbsCurLevel(PlayerProfile.curlevelstage, PlayerProfile.curlevel)] < SCGlobals.levelcompleteness)
		{
			PlayerProfile.completedPercents[SuperCow.Settings.GetAbsCurLevel(PlayerProfile.curlevelstage, PlayerProfile.curlevel)] = SCGlobals.levelcompleteness;
		}
		bool b_stagepassed = false;
		SuperCow.Settings.PlayerCompleteLevel(ref b_stagepassed, ref SCGlobals.b_completegame);
		SuperCow.RemoveScreen(11, clear: true);
		SCGlobals.menusetk = 0f;
		Sound.Stop(mMusic);
		StatsScreen statsScreen = (StatsScreen)SuperCow.GetScreenInTrash(12);
		if (statsScreen != null)
		{
			SuperCow.AddScreen(statsScreen);
		}
		else
		{
			SuperCow.AddScreen(12, new StatsScreen());
		}
		SuperCow.Settings.AddPlayerScore((uint)scoreatlevel);
		PlayerProfile.numlives = numlives;
		lives = "x" + (numlives + 1);
		SuperCow.Settings.SaveSettings();
	}

	public obstacle_t AddObstacle(ref CLevelObj p_lo)
	{
		if (p_lo.draw_plane > 2)
		{
			return default(obstacle_t);
		}
		if (SCGlobals.numobstacles == 2047)
		{
			return default(obstacle_t);
		}
		SCGlobals.ar_obstacles[SCGlobals.numobstacles] = default(obstacle_t);
		obstacle_t obstacle_t2 = SCGlobals.ar_obstacles[SCGlobals.numobstacles++];
		obstacle_t2.pos = p_lo.position;
		obstacle_t2.rect = p_lo.rect;
		obstacle_t2.sizex = p_lo.sizex;
		obstacle_t2.sizey = p_lo.sizey;
		obstacle_t2.p_levelobj = p_lo;
		p_lo.p_obstacle_idx = SCGlobals.numobstacles - 1;
		obstacle_t2.b_triangle = p_lo.objID == 19;
		if (obstacle_t2.b_triangle)
		{
			float x = p_lo.endpos.x;
			float y = p_lo.endpos.y;
			float x2 = obstacle_t2.rect.p1.x;
			if (x2 == x)
			{
				x2 = obstacle_t2.rect.p0.x;
			}
			float y2 = obstacle_t2.rect.p1.y;
			if (y2 == y)
			{
				y2 = obstacle_t2.rect.p0.y;
			}
			obstacle_t2.rect.assign(x, y, x2, y2);
		}
		obstacle_t2.obsflag = obstacle_flags_t.SEMITRANS_OBF;
		if (p_lo.name == "fire" || p_lo.name == "picket" || p_lo.name == "picket2")
		{
			obstacle_t2.obsflag = obstacle_flags_t.TRANS_OBF;
		}
		else if (p_lo.objID == 4)
		{
			obstacle_t2.obsflag = obstacle_flags_t.RIGID_OBF;
		}
		else if (p_lo.objID == 18)
		{
			obstacle_t2.obsflag = obstacle_flags_t.RIGID_OBF;
		}
		else if (p_lo.objID == 8)
		{
			obstacle_t2.obsflag = obstacle_flags_t.RIGID_OBF;
		}
		else if (p_lo.objID == 16)
		{
			obstacle_t2.obsflag = obstacle_flags_t.RIGID_OBF;
		}
		else if (p_lo.objID == 10 || p_lo.objID == 13 || p_lo.objID == 26 || p_lo.objID == 15)
		{
			obstacle_t2.obsflag = obstacle_flags_t.TRANS_OBF;
		}
		SCGlobals.ar_obstacles[SCGlobals.numobstacles - 1] = obstacle_t2;
		return obstacle_t2;
	}

	public static obstacle_t GetAnyObstacleByPoint(vec2_t p, bool b_nontriangle)
	{
		obstacle_t result = default(obstacle_t);
		vec2_t p2 = new vec2_t(p.x, p.y);
		float num = 10000f;
		for (int i = 0; i < SCGlobals.numobstacles; i++)
		{
			obstacle_t obstacle_t2 = SCGlobals.ar_obstacles[i];
			if ((!b_nontriangle || !obstacle_t2.b_triangle) && (obstacle_t2.obsflag == obstacle_flags_t.RIGID_OBF || obstacle_t2.obsflag == obstacle_flags_t.SEMITRANS_OBF) && !(p.x < obstacle_t2.rect.x0) && !(p.x > obstacle_t2.rect.x1) && !(p.y < obstacle_t2.rect.y1))
			{
				if (obstacle_t2.rect.isPointInRect(p2))
				{
					return obstacle_t2;
				}
				float num2 = Math.Abs(p.y - obstacle_t2.rect.y0);
				if (num > num2)
				{
					result = obstacle_t2;
					num = num2;
				}
			}
		}
		return result;
	}

	public static obstacle_t GetObstacleByPoint(vec2_t p, float maxdist)
	{
		return GetObstacleByPoint(new vec3_t(p.x, p.y, 0f), maxdist);
	}

	public static obstacle_t GetObstacleByPoint(vec3_t p, float maxdist)
	{
		obstacle_t result = default(obstacle_t);
		vec2_t p2 = new vec2_t(p.x, p.y);
		float num = 100000f;
		if (maxdist < 0f)
		{
			maxdist = 100000f;
		}
		for (int i = 0; i < SCGlobals.numobstacles; i++)
		{
			obstacle_t obstacle_t2 = SCGlobals.ar_obstacles[i];
			if (obstacle_t2.obsflag == obstacle_flags_t.SEMITRANS_OBF && !(p.x < obstacle_t2.rect.x0) && !(p.x > obstacle_t2.rect.x1) && !(p.y < obstacle_t2.rect.y1 - 5f))
			{
				if (obstacle_t2.rect.isPointInRect(p2))
				{
					return obstacle_t2;
				}
				float num2 = Math.Abs(p.y - obstacle_t2.rect.y0);
				if (num > num2 && num2 < maxdist)
				{
					result = obstacle_t2;
					num = num2;
				}
			}
		}
		return result;
	}

	public void SetupMonster(CMonster pm)
	{
		obstacle_t obstacle_t2 = GetObstacleByPoint(pm.position, -1f);
		if (pm.monsterID == 4 || pm.monsterID == 5)
		{
			obstacle_t2 = default(obstacle_t);
		}
		if (obstacle_t2.p_levelobj != null)
		{
			switch (pm.monsterID)
			{
			case 6:
				pm.startpos.x = obstacle_t2.rect.x0 + 0.5f;
				pm.endpos.x = obstacle_t2.rect.x1 - 0.5f;
				pm.position.y = obstacle_t2.rect.y0;
				break;
			case 7:
			case 9:
			case 15:
				camerabossrect.assign(new vec2_t(pm.position), SCGlobals.GRID_SIZE_X - 2f, SCGlobals.GRID_SIZE_Y);
				p_bossmonster = pm;
				pm.startpos.x = obstacle_t2.rect.x0 + 1f;
				pm.endpos.x = obstacle_t2.rect.x1 - 1f;
				pm.position.y = obstacle_t2.rect.y0;
				if (pm.monsterID == 7)
				{
					pm.startpos.x += 1f;
					pm.endpos.x -= 1f;
				}
				break;
			case 0:
			case 2:
			case 12:
			case 14:
				pm.startpos.x = obstacle_t2.rect.x0 + 1f;
				pm.endpos.x = obstacle_t2.rect.x1 - 1f;
				pm.position.y = obstacle_t2.rect.y0;
				break;
			case 8:
				pm.position.y = obstacle_t2.rect.y0 - 0.3f;
				break;
			case 1:
				pm.startpos.x = obstacle_t2.rect.x0 + 2f;
				pm.endpos.x = obstacle_t2.rect.x1 - 2f;
				pm.coord_y = obstacle_t2.rect.y0 - 0.2f;
				pm.position.y = pm.coord_y - pm.monster_height;
				break;
			}
		}
		if (obstacle_t2.p_levelobj != null && (obstacle_t2.p_levelobj.type & 2) != 0)
		{
			obstacle_t2.p_levelobj.AddLinkedPers(pm);
		}
		pm.Setup();
		pm.bound_rect.assign(new vec2_t(pm.position) + new vec2_t(0f, pm.monster_height / 2f), pm.monster_width, pm.monster_height);
	}

	public void StopMusic()
	{
		Sound.Stop(mMusic);
	}

	public void StartMusic()
	{
		int num = SCGlobals.curlevelmusic;
		if (num > 4)
		{
			num = 5;
		}
		mMusic = Sound.Play((Sounds)(2 + num));
	}

	public void FreeGame()
	{
		SCGlobals.pershost.FreePers();
		SCGlobals.nummonsters = 0;
		SCGlobals.numobstacles = 0;
		for (int i = 0; i < SCGlobals.level.numlevelobjs; i++)
		{
			SCGlobals.level.ar_levelobjs[i].Init();
		}
	}

	public void ThreadFinished()
	{
		bFirstDraw = true;
	}

	public void ThreadFinished(int taskId)
	{
		bFirstDraw = true;
	}

	public void ThreadProc()
	{
		while (true)
		{
			SCGlobals.hero.CalcMath();
		}
	}

	public void InitGame()
	{
		FreeGame();
		life0 = 1f;
		CRand.frandomize(0);
		SCGlobals.setpausetimer = 0f;
		SCGlobals.levelstat.numcoins = 0;
		SCGlobals.levelstat.numgems = 0;
		SCGlobals.levelstat.nummonsters = 0;
		SCGlobals.levelstat.numsecrets = 0;
		SCGlobals.levelstat.numkeys = 0;
		SCGlobals.levelstat.numgarbage = 0;
		SCGlobals.levelstat.coins = 0;
		SCGlobals.levelstat.gems = 0;
		SCGlobals.levelstat.monsters = 0;
		SCGlobals.levelstat.secrets = 0;
		SCGlobals.levelstat.keys = 0;
		SCGlobals.levelstat.garbage = 0;
		int num = SCGlobals.curlevelmusic;
		if (num > 4)
		{
			num = 5;
		}
		mMusic = Sound.Play((Sounds)(2 + num));
		SCGlobals.b_gamemenu = false;
		SCGlobals.b_pause = false;
		b_bossfight = false;
		b_bossdead = false;
		SCGlobals.b_trophy4was_not_punished = true;
		b_switchexit = false;
		p_bossmonster = null;
		SetGameLight();
		CalculateGameGirdSize();
		SCGlobals.numobstacles = 0;
		CRand.frandomize(0);
		SCGlobals.hero.Refresh();
		obstacle_t obstacle_t2;
		for (int i = 0; i < SCGlobals.level.numlevelobjs; i++)
		{
			CLevelObj p_lo = SCGlobals.level.ar_levelobjs[i];
			p_lo.Init();
			gameobject_t gameobject_t2 = SCGlobals.ar_gameobjs[p_lo.game_object_index];
			_ = SCGlobals.group_names[gameobject_t2.group].name;
			if (p_lo.draw_plane > 2)
			{
				continue;
			}
			if (p_lo.name == "secret")
			{
				SCGlobals.levelstat.numsecrets++;
				continue;
			}
			if (p_lo.name == "key")
			{
				SCGlobals.levelstat.numkeys++;
				continue;
			}
			switch (p_lo.objID)
			{
			case 29:
				SCGlobals.levelstat.numcoins++;
				break;
			case 31:
			case 32:
				SCGlobals.levelstat.numgems++;
				break;
			case 0:
				if (p_lo.rotation != 0f)
				{
					break;
				}
				goto case 1;
			case 1:
			case 2:
			case 17:
			case 25:
				obstacle_t2 = AddObstacle(ref p_lo);
				obstacle_t2.rect.y0 = obstacle_t2.rect.p0.y * 0.9f + obstacle_t2.rect.p1.y * 0.1f;
				obstacle_t2.sizex = p_lo.sizex;
				obstacle_t2.sizey = p_lo.sizey * 0.9f;
				if (p_lo.objID == 17)
				{
					obstacle_t2.rect.y0 -= 0.9f;
					obstacle_t2.rect.y1 -= 0.5f;
					obstacle_t2.pos.y -= 2.9f;
				}
				if (p_lo.objID == 2)
				{
					obstacle_t2.rect.x0 -= 1f;
					obstacle_t2.rect.x1 += 1f;
				}
				SCGlobals.ar_obstacles[SCGlobals.numobstacles - 1] = obstacle_t2;
				break;
			case 24:
				obstacle_t2 = AddObstacle(ref p_lo);
				obstacle_t2.rect.translate(0f, -1.2f);
				SCGlobals.ar_obstacles[SCGlobals.numobstacles - 1] = obstacle_t2;
				break;
			case 18:
			case 19:
			case 20:
			case 26:
				obstacle_t2 = AddObstacle(ref p_lo);
				break;
			case 10:
				obstacle_t2 = AddObstacle(ref p_lo);
				obstacle_t2.sizex *= 0.2f;
				obstacle_t2.rect.assign(obstacle_t2.pos - new vec2_t(0f, 1f), obstacle_t2.sizex, obstacle_t2.sizey);
				SCGlobals.ar_obstacles[SCGlobals.numobstacles - 1] = obstacle_t2;
				break;
			case 8:
				obstacle_t2 = AddObstacle(ref p_lo);
				obstacle_t2.rect.y0 -= 1f;
				obstacle_t2.rect.x0 += 0.3f;
				obstacle_t2.rect.x1 -= 0.3f;
				SCGlobals.levelstat.numgarbage++;
				SCGlobals.ar_obstacles[SCGlobals.numobstacles - 1] = obstacle_t2;
				break;
			case 4:
				if (p_lo.draw_plane == 1)
				{
					AddObstacle(ref p_lo);
				}
				break;
			case 14:
				SCGlobals.hero.position.assign(p_lo.position.x, p_lo.position.y, 0f);
				break;
			case 13:
				obstacle_t2 = AddObstacle(ref p_lo);
				obstacle_t2.rect.assign(p_lo.position, 2f, 1f);
				SCGlobals.ar_obstacles[SCGlobals.numobstacles - 1] = obstacle_t2;
				break;
			case 15:
				obstacle_t2 = AddObstacle(ref p_lo);
				obstacle_t2.rect.assign(obstacle_t2.rect.pos(), obstacle_t2.rect.sizex() * 2f, obstacle_t2.rect.sizey());
				obstacle_t2.rect.y1 -= 1f;
				SCGlobals.ar_obstacles[SCGlobals.numobstacles - 1] = obstacle_t2;
				break;
			case 16:
				if (p_lo.b_invertobj)
				{
					obstacle_t2 = AddObstacle(ref p_lo);
					obstacle_t2.rect.y1 = obstacle_t2.rect.y0 - 2f;
					obstacle_t2.rect.y0 -= 1f;
					obstacle_t2.rect.x1 -= 2.3f;
					obstacle_t2.rect.x0 = obstacle_t2.rect.x0 + 2.3f;
					obstacle_t2 = AddObstacle(ref p_lo);
					obstacle_t2.rect.x1 -= 4f;
					obstacle_t2.rect.x0 += 1.3f;
					obstacle_t2.rect.y1 = obstacle_t2.rect.y0 - 1.7f;
					obstacle_t2.rect.y0 -= 0.5f;
					obstacle_t2.obsflag = obstacle_flags_t.SEMITRANS_OBF;
					obstacle_t2 = AddObstacle(ref p_lo);
					obstacle_t2.rect.y0 -= 1.5f;
					obstacle_t2.rect.x0 = obstacle_t2.rect.x1 - 2.3f;
					obstacle_t2.rect.x1 -= 1f;
					obstacle_t2 = AddObstacle(ref p_lo);
					obstacle_t2.rect.y0 -= 2.5f;
					obstacle_t2.rect.x0 += 4f;
					obstacle_t2.rect.x1 -= 2.5f;
					obstacle_t2.obsflag = obstacle_flags_t.TRANS_OBF;
				}
				else
				{
					obstacle_t2 = AddObstacle(ref p_lo);
					obstacle_t2.rect.y1 = obstacle_t2.rect.y0 - 2f;
					obstacle_t2.rect.y0 -= 1f;
					obstacle_t2.rect.x0 += 2.3f;
					obstacle_t2.rect.x1 = obstacle_t2.rect.x1 - 2.3f;
					obstacle_t2 = AddObstacle(ref p_lo);
					obstacle_t2.rect.x0 += 4f;
					obstacle_t2.rect.x1 -= 1.3f;
					obstacle_t2.rect.y1 = obstacle_t2.rect.y0 - 1.7f;
					obstacle_t2.rect.y0 -= 0.5f;
					obstacle_t2.obsflag = obstacle_flags_t.SEMITRANS_OBF;
					obstacle_t2 = AddObstacle(ref p_lo);
					obstacle_t2.rect.y0 -= 1.5f;
					obstacle_t2.rect.x1 = obstacle_t2.rect.x0 + 2.3f;
					obstacle_t2.rect.x0 += 1f;
					obstacle_t2 = AddObstacle(ref p_lo);
					obstacle_t2.rect.y0 -= 2.5f;
					obstacle_t2.rect.x1 -= 4f;
					obstacle_t2.rect.x0 += 2.5f;
					obstacle_t2.obsflag = obstacle_flags_t.TRANS_OBF;
				}
				SCGlobals.ar_obstacles[SCGlobals.numobstacles - 1] = obstacle_t2;
				break;
			case 6:
			{
				CMonster cMonster = SCGlobals.pershost.AddMonster(gameobject_t2.name);
				if (cMonster == null)
				{
					break;
				}
				cMonster.startpos.assign(p_lo.startpos.x, p_lo.startpos.y, 0f);
				cMonster.endpos.assign(p_lo.endpos.x, p_lo.endpos.y, 0f);
				cMonster.position.assign(p_lo.position.x, p_lo.position.y, 0f);
				if (p_lo.extratextparam.Length > 0)
				{
					cMonster.ilevelobjscale = int.Parse(p_lo.extratextparam, CultureInfo.InvariantCulture);
					if (cMonster.ilevelobjscale != 2 && cMonster.ilevelobjscale != 3)
					{
						cMonster.ilevelobjscale = 1;
					}
				}
				else
				{
					cMonster.ilevelobjscale = 1;
				}
				break;
			}
			}
		}
		SCGlobals.level.SortDrawPlanes(b_game: true);
		for (int i = 0; i < SCGlobals.level.numplaneobjs[1]; i++)
		{
			CLevelObj p_lo = SCGlobals.level.ar_drawplanes[1, i];
			if (p_lo.draw_plane <= 2)
			{
				obstacle_t obstacleByPoint = GetObstacleByPoint(p_lo.position, 3f);
				if (obstacleByPoint.p_levelobj != null && obstacleByPoint.p_levelobj != null && obstacleByPoint.p_levelobj != p_lo && (obstacleByPoint.p_levelobj.type & 2) != 0)
				{
					obstacleByPoint.p_levelobj.AddLinkedObjs(p_lo);
				}
			}
		}
		obstacle_t2 = GetObstacleByPoint(SCGlobals.hero.position, -1f);
		if (obstacle_t2.p_levelobj != null)
		{
			SCGlobals.hero.position.y = obstacle_t2.rect.y0;
		}
		SCGlobals.hero.bound_rect.assign(new vec2_t(SCGlobals.hero.position.x, SCGlobals.hero.position.y + SCGlobals.hero.monster_height / 2f), SCGlobals.hero.monster_width, SCGlobals.hero.monster_height);
		SCGlobals.hero.safeposition = SCGlobals.hero.position;
		SCGlobals.hero.curpos.assign(SCGlobals.hero.position.x, SCGlobals.hero.position.y, 0f);
		SCGlobals.camerapos = -new vec2_t(SCGlobals.hero.position);
		SCGlobals.levelstat.nummonsters = SCGlobals.nummonsters;
		for (int j = 0; j < SCGlobals.nummonsters; j++)
		{
			SetupMonster(SCGlobals.ar_monsters[j]);
		}
		level_top = -100f;
		level_bottom = 100f;
		level_left = 10000f;
		level_right = -10000f;
		for (int k = 0; k < SCGlobals.numobstacles; k++)
		{
			obstacle_t2 = SCGlobals.ar_obstacles[k];
			if (level_left > obstacle_t2.rect.x0)
			{
				level_left = obstacle_t2.rect.x0;
			}
			if (level_right < obstacle_t2.rect.x1)
			{
				level_right = obstacle_t2.rect.x1;
			}
			if (level_top < obstacle_t2.rect.y0)
			{
				level_top = obstacle_t2.rect.y0;
			}
			if (level_bottom > obstacle_t2.rect.y0)
			{
				level_bottom = obstacle_t2.rect.y0;
			}
		}
		if (level_top - level_bottom < 20f)
		{
			level_top = level_bottom + 20f;
		}
		b_maindeath = false;
		SCGlobals.b_victory = false;
		b_startlevel = true;
		b_showedTask = false;
		startleveltime = START_LEVEL_TIME;
		Balloon.str_balloon = null;
		endleveltime = 0f;
		le_scale = 1f;
		SCGlobals.setpausetimer = 0f;
		SCGlobals.camerapos.x = 0f - SCGlobals.hero.position.x;
		float num2 = 0f - SCGlobals.camerapos.x;
		if (num2 - SCGlobals.GRID_SIZE_X * 0.5f < level_left)
		{
			num2 = level_left + SCGlobals.GRID_SIZE_X * 0.5f;
		}
		if (num2 + SCGlobals.GRID_SIZE_X * 0.5f > level_right)
		{
			num2 = level_right - SCGlobals.GRID_SIZE_X * 0.5f;
		}
		SCGlobals.camerapos.x = 0f - num2;
		SCGlobals.camerapos.y = 0f - SCGlobals.hero.position.y;
		bFirstDraw = true;
	}

	public void FillStaticBuffers()
	{
		Array.Clear(CowRender.m_Layers[0], 0, CowRender.m_Layers[0].Length);
		Array.Clear(CowRender.m_Layers[1], 0, CowRender.m_Layers[1].Length);
		Array.Clear(CowRender.m_Layers[2], 0, CowRender.m_Layers[2].Length);
		Array.Clear(CowRender.m_Layers[3], 0, CowRender.m_Layers[3].Length);
		Array.Clear(CowRender.m_Layers[4], 0, CowRender.m_Layers[4].Length);
		Array.Clear(CowRender.m_Layers[5], 0, CowRender.m_Layers[5].Length);
		Array.Clear(CowRender.layerPolyCount, 0, 6);
		int num = 0;
		CLevelObj cLevelObj = null;
		dcolor_t colorWhite = dcolor_t.ColorWhite;
		int num2 = 0;
		bool flag = false;
		if (SuperCow.Settings.curlevelstage == 9 && SuperCow.Settings.curlevel == 1)
		{
			flag = true;
		}
		CowRender.SetVertexPixelShadersData(322uL);
		for (num = 5; num != -1; num--)
		{
			SCGlobals.currentLayer = num;
			SetGameCameraByPlane(SCGlobals.camerapos, num);
			Ground.DrawGround(num, new frect_t(level_left, level_top, level_right + 20f, level_bottom - 10f));
			for (int i = 0; i < SCGlobals.level.numplaneobjs[num]; i++)
			{
				cLevelObj = SCGlobals.level.ar_drawplanes[num, i];
				if (num < num2)
				{
					colorWhite.a = (byte)(255f - (float)((num2 - num) * 255) / 7f);
				}
				if (!flag || !cLevelObj.name.Contains("rope"))
				{
					cLevelObj.DrawStatic(colorWhite);
					colorWhite.a = byte.MaxValue;
				}
			}
			SetGameCamera(SCGlobals.camerapos);
		}
	}

	public void CalculateViewportRect()
	{
		SCGlobals.viewport_rect.assign(0f - SCGlobals.camerapos.x - SCGlobals.GRID_SIZE_X * 0.6f, 0f - SCGlobals.camerapos.y + SCGlobals.GRID_SIZE_Y * 0.6f, 0f - SCGlobals.camerapos.x + SCGlobals.GRID_SIZE_X * 0.6f, 0f - SCGlobals.camerapos.y - SCGlobals.GRID_SIZE_Y * 0.6f);
	}

	public void SetGameCamera(vec2_t pos)
	{
		SCGlobals.cam.load_identity();
		SCGlobals.cam.translate(pos.x, pos.y, 50f);
		CowRender.LoadCamera(SCGlobals.cam);
		SCGlobals.frustum.Calculate();
		SCGlobals.viewport_rect.assign(0f - pos.x - SCGlobals.GRID_SIZE_X * 0.6f, 0f - pos.y + SCGlobals.GRID_SIZE_Y * 0.6f, 0f - pos.x + SCGlobals.GRID_SIZE_X * 0.6f, 0f - pos.y - SCGlobals.GRID_SIZE_Y * 0.6f);
	}

	public static void SetGameCameraByPlane(vec2_t pos, int nplane)
	{
		vec2_t vec2_t2 = pos;
		vec2_t2 *= GetGamePlaneScale(nplane);
		SCGlobals.cam.load_identity();
		SCGlobals.cam.translate(vec2_t2.x, vec2_t2.y, 50f);
		CowRender.LoadCamera(SCGlobals.cam);
		SCGlobals.frustum.Calculate();
		SCGlobals.viewport_rect.assign(0f - vec2_t2.x - SCGlobals.GRID_SIZE_X * 0.6f, 0f - vec2_t2.y + SCGlobals.GRID_SIZE_Y * 0.6f, 0f - vec2_t2.x + SCGlobals.GRID_SIZE_X * 0.6f, 0f - vec2_t2.y - SCGlobals.GRID_SIZE_Y * 0.6f);
	}

	public static float GetGamePlaneScale(int nplane)
	{
		float result = 1f;
		switch (nplane)
		{
		case 0:
		case 1:
		case 2:
			result = 1f;
			break;
		case 3:
			result = 0.8f;
			break;
		case 4:
			result = 0.6f;
			break;
		case 5:
			result = 0.4f;
			break;
		}
		return result;
	}

	public void PhysicGame()
	{
		if (SCGlobals.b_pause || b_startlevel)
		{
			return;
		}
		Particles.SetParticleLayer(0);
		Bonus.PhysicsBonuses();
		if (SCGlobals.numobstacles > 0)
		{
			for (int i = 0; i < SCGlobals.level.numlevelobjs; i++)
			{
				SCGlobals.level.ar_levelobjs[i].Physics();
			}
		}
		for (int j = 0; j < SCGlobals.nummonsters; j++)
		{
			if (SCGlobals.hero.behavior != behaviors_t.BT_WOW || (SCGlobals.hero.behavior == behaviors_t.BT_WOW && SCGlobals.ar_monsters[j].behavior == behaviors_t.BT_DEATH))
			{
				SCGlobals.ar_monsters[j].Physics();
			}
		}
		SCGlobals.hero.Physics();
		if (SCGlobals.hero.behavior != behaviors_t.BT_SITDOWN)
		{
			if (cameradown > 0.01f)
			{
				cameradown *= 0.9f;
			}
			else
			{
				cameradown = 0f;
			}
		}
		Weapons.PhysicsBullets();
		if (p_bossmonster != null)
		{
			if (!p_bossmonster.b_alive && b_bossfight)
			{
				StopMusic();
				StartMusic();
				b_bossfight = false;
				if (p_bossmonster.monsterID == 7)
				{
					AddGameTrophy(4);
				}
				else if (p_bossmonster.monsterID == 9)
				{
					AddGameTrophy(5);
				}
				else if (p_bossmonster.monsterID == 15)
				{
					AddGameTrophy(12);
				}
				if (SCGlobals.curleveltask == 5)
				{
					b_switchexit = true;
				}
			}
			camerabossrect.assign(new vec2_t(p_bossmonster.position), SCGlobals.GRID_SIZE_X + 2f, SCGlobals.GRID_SIZE_Y);
			if (camerabossrect.isPointInRect(new vec2_t(SCGlobals.hero.position)) && !b_bossfight && p_bossmonster.b_alive)
			{
				camerabossposition0 = SCGlobals.camerapos;
				b_bossfight = true;
				bossfighttimer = 0f;
				StopMusic();
				mMusic = Sound.Play("boss");
				SCGlobals.b_stopphysics = true;
				SCGlobals.hero.SetBehavior(behaviors_t.BT_STAND);
				p_bossmonster.SetBehavior(behaviors_t.BT_STAND);
				Text.AddEventText(wstrstuff.GetStr("bossfight"), new vec2_t(0f, 0f), 35f, 2500f);
			}
		}
		SCGlobals.pershost.PhysicPers();
		bonusleveltimer = 0;
		if (b_bonuslevel)
		{
			bonusleveltimer += (int)SCGlobals.engine_frequency;
			vec2_t pos = default(vec2_t);
			if (bonusleveltimer > 700)
			{
				bonusleveltimer -= 700;
				pos.assign(SCGlobals.viewport_rect.x0 + SCGlobals.viewport_rect.sizex() * (float)CRand.phys_rand(100) / 100f, SCGlobals.viewport_rect.y0);
				Bonus.AddCoin(pos, 64f, SCGlobals.viewport_rect.y1, 1 + CRand.phys_rand(5));
				Bonus.AddBonusLevelCoin(pos, 2 + CRand.phys_rand(3));
			}
		}
		switch (SCGlobals.curleveltask)
		{
		case 0:
			b_switchexit = true;
			break;
		case 1:
			if (SCGlobals.levelstat.keys >= SCGlobals.levelstat.numkeys)
			{
				b_switchexit = true;
			}
			break;
		case 2:
			if (SCGlobals.levelstat.monsters >= SCGlobals.levelstat.nummonsters)
			{
				b_switchexit = true;
			}
			break;
		case 3:
			if (SCGlobals.levelstat.gems >= SCGlobals.levelstat.numgems)
			{
				b_switchexit = true;
			}
			break;
		case 4:
			if (SCGlobals.levelstat.garbage >= SCGlobals.levelstat.numgarbage)
			{
				b_switchexit = true;
			}
			break;
		}
		if (b_switchexit)
		{
			for (int k = 0; k < SCGlobals.level.numlevelobjs; k++)
			{
				if (SCGlobals.level.ar_levelobjs[k].objID == 10 && SCGlobals.level.ar_levelobjs[k].b_switch == 0)
				{
					if (SCGlobals.curleveltask != 0)
					{
						Text.AddEventText(wstrstuff.GetStrDelta("donetask1", SCGlobals.curleveltask), new vec2_t(0f, 40f), 25f, 3500f);
						Text.AddEventText(wstrstuff.GetStr("openexit"), new vec2_t(0f, -40f), 25f, 3500f);
						Sound.Addsound("taskcompleted");
					}
					SCGlobals.level.ar_levelobjs[k].b_switch = 1;
				}
			}
		}
		if (SuperCow.Settings.curlevelstage >= 1)
		{
			bool flag = true;
			if (SCGlobals.levelstat.coins >= 30 && SCGlobals.levelstat.coins >= SCGlobals.levelstat.numcoins)
			{
				AddGameTrophy(1);
			}
			else
			{
				flag = false;
			}
			if (SCGlobals.levelstat.gems >= 10 && SCGlobals.levelstat.gems >= SCGlobals.levelstat.numgems)
			{
				AddGameTrophy(11);
			}
			else
			{
				flag = false;
			}
			if (SCGlobals.levelstat.monsters >= 10 && SCGlobals.levelstat.monsters >= SCGlobals.levelstat.nummonsters)
			{
				AddGameTrophy(9);
			}
			else
			{
				flag = false;
			}
			if (flag)
			{
				AddGameTrophy(7);
			}
			if (SCGlobals.levelstat.secrets >= 5)
			{
				AddGameTrophy(10);
			}
		}
	}

	private void CalculateGameGirdSize()
	{
		SCGlobals.GRID_SIZE_X0 = le_scale / 32f * (float)SuperCow.getWidth;
		SCGlobals.GRID_SIZE_Y0 = le_scale / 32f * (float)SuperCow.getHeight;
		SCGlobals.GRID_SIZE_X = SCGlobals.GRID_SIZE_X0 * le_plane_scale;
		SCGlobals.GRID_SIZE_Y = SCGlobals.GRID_SIZE_Y0 * le_plane_scale;
		SCGlobals.GRID_STEP_X = le_plane_scale;
		SCGlobals.GRID_STEP_Y = le_plane_scale;
	}

	public void DrawObj(dcolor_t dc, bool toBuffer, bool StopCalc, int ojbID, int drawCount)
	{
	}

	public void DrawLevelObjects2()
	{
		CalculateViewportRect();
		CowRender.SetOrtho(SCGlobals.GRID_SIZE_X, SCGlobals.GRID_SIZE_Y, 1f, 500f);
		SetGameCamera(SCGlobals.camerapos);
		var num = 0;
		if (!SCGlobals.b_le_active)
		{
			num = 0;
		}
		CowRender.EnableRS(4u);
		CowRender.SetBlend(0);
		CowRender.EnableRS(5u);
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		// TODO: alpha test not implemented - 3f
		int num2 = 0;
		CLevelObj cLevelObj = null;
		dcolor_t colorWhite = dcolor_t.ColorWhite;
		CowRender.SetVertexPixelShadersData(322uL);
		graphicsDevice.Textures[0] = (Texture)(object)mainTex;
		if (bFirstDraw)
		{
			FillStaticBuffers();
			bFirstDraw = false;
			return;
		}
		for (num2 = 5; num2 != 0; num2--)
		{
			SCGlobals.currentLayer = num2;
			SetGameCameraByPlane(SCGlobals.camerapos, num2);
			CowRender.DrawStaticBuffer();
			colorWhite.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			if (num2 < num)
			{
				colorWhite.a = (byte)(255f - (float)((num - num2) * 255) / 7f);
			}
			for (int i = 0; i < SCGlobals.level.numplaneobjs[num2]; i++)
			{
				cLevelObj = SCGlobals.level.ar_drawplanes[num2, i];
				cLevelObj.Draw(colorWhite);
			}
			colorWhite.a = byte.MaxValue;
			CowRender.DrawDynBuffer();
			SetGameCamera(SCGlobals.camerapos);
		}
		CowRender.SetGlobal2DColor(color_t.ColorWhite);
		DrawCharacters();
		graphicsDevice.Textures[0] = (Texture)(object)mainTex;
		Bonus.DrawBonuses();
		CowRender.EnableRS(4u);
		CowRender.SetBlend(0);
		CowRender.SetVertexPixelShadersData(322uL);
		graphicsDevice.Textures[0] = mainTex;
		SCGlobals.currentLayer = 0;
		CowRender.DrawStaticBuffer();
		colorWhite.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		if (num2 < num)
		{
			colorWhite.a = (byte)(255f - (float)((num - num2) * 255) / 7f);
		}
		for (int j = 0; j < SCGlobals.level.numplaneobjs[num2]; j++)
		{
			cLevelObj = SCGlobals.level.ar_drawplanes[num2, j];
			cLevelObj.Draw(colorWhite);
		}
		colorWhite.a = byte.MaxValue;
		CowRender.DrawDynBuffer();
	}

	public void DrawLevelObjects()
	{
		CalculateViewportRect();
		CowRender.SetOrtho(SCGlobals.GRID_SIZE_X, SCGlobals.GRID_SIZE_Y, 1f, 500f);
		SetGameCamera(SCGlobals.camerapos);
		int num = 0;
		CowRender.EnableRS(4u);
		CowRender.SetBlend(0);
		CowRender.EnableRS(5u);
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		// TODO: alpha test not implemented - 3f
		CLevelObj cLevelObj = null;
		dcolor_t colorWhite = dcolor_t.ColorWhite;
		for (int num2 = 5; num2 != 0; num2--)
		{
			SCGlobals.currentLayer = num2;
			SetGameCameraByPlane(SCGlobals.camerapos, num2);
			Ground.DrawGroundOld(num2);
			colorWhite.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			if (num2 < num)
			{
				colorWhite.a = (byte)(255f - (float)((num - num2) * 255) / 7f);
			}
			for (int i = 0; i < SCGlobals.level.numplaneobjs[num2]; i++)
			{
				cLevelObj = SCGlobals.level.ar_drawplanes[num2, i];
				cLevelObj.Draw(colorWhite);
			}
			colorWhite.a = byte.MaxValue;
			SetGameCamera(SCGlobals.camerapos);
		}
		CowRender.SetGlobal2DColor(color_t.ColorWhite);
	}

	public void DrawCharacters()
	{
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		graphicsDevice.RasterizerState = RasterizerState.CullNone;
		CowRender.SetOrtho(SCGlobals.GRID_SIZE_X, SCGlobals.GRID_SIZE_Y, 1f, 300f);
		SCGlobals.cam.load_identity();
		SCGlobals.cam.translate(SCGlobals.camerapos.x, SCGlobals.camerapos.y, 50f);
		CowRender.LoadCamera(SCGlobals.cam);
		CowRender.EnableRS(4u);
		CowRender.DisableRS(4u);
		CowRender.EnableRS(1u);
		CowRender.EnableRS(0u);
		CowRender.EnableRS(7u);
		_ = SCGlobals.ftickperframe;
		_ = SCGlobals.frameinterp;
		_ = SCGlobals.tickperframe;
		CowRender.EnableRS(7u);
		if (!b_startlevel || !SCGlobals.b_pause)
		{
			SCGlobals.hero.Draw();
		}
		CowRender.DisableRS(7u);
		for (int i = 0; i < SCGlobals.nummonsters; i++)
		{
			SCGlobals.ar_monsters[i].Draw();
		}
		graphicsDevice.RasterizerState = RasterizerState.CullNone;
		CowRender.LoadMatrixNull();
		CowRender.DisableRS(0u);
		CowRender.DisableRS(1u);
	}

	public void DrawObstacles()
	{
		CowRender.BindTexture(SCGlobals.p_white);
		CowRender.EnableRS(4u);
		float thick = 0.0625f;
		dcolor_t c = default(dcolor_t);
		for (int i = 0; i < SCGlobals.numobstacles; i++)
		{
			if (SCGlobals.ar_obstacles[i].b_triangle)
			{
				c.assign(55, byte.MaxValue, 55, byte.MaxValue);
				continue;
			}
			if (SCGlobals.ar_obstacles[i].obsflag == obstacle_flags_t.TRANS_OBF)
			{
				c.assign(55, 55, byte.MaxValue, byte.MaxValue);
			}
			else if (SCGlobals.ar_obstacles[i].obsflag == obstacle_flags_t.SEMITRANS_OBF)
			{
				c.assign(55, byte.MaxValue, 55, byte.MaxValue);
			}
			else if (SCGlobals.ar_obstacles[i].obsflag == obstacle_flags_t.RIGID_OBF)
			{
				c.assign(byte.MaxValue, 55, 55, byte.MaxValue);
			}
			CowRender.DrawEmptyRect(SCGlobals.ar_obstacles[i].rect, thick, c);
		}
		CowRender.DrawEmptyRect(SCGlobals.hero.bound_rect, thick, new dcolor_t(byte.MaxValue, byte.MaxValue, 55, byte.MaxValue));
		for (int j = 0; j < SCGlobals.nummonsters; j++)
		{
			if (SCGlobals.ar_monsters[j].b_alive)
			{
				CowRender.DrawEmptyRect(SCGlobals.ar_monsters[j].bound_rect, thick, new dcolor_t(55, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			}
		}
		CowRender.DisableRS(4u);
	}

	public void CalculateGame()
	{
	}

	public void CalculateVisibility()
	{
		CowRender.SetOrtho(SCGlobals.GRID_SIZE_X, SCGlobals.GRID_SIZE_Y, 1f, 500f);
		SetGameCamera(SCGlobals.camerapos);
		int num = 0;
		CLevelObj cLevelObj = null;
		CLevelObj cLevelObj2 = null;
		for (num = 5; num != 0; num--)
		{
			SetGameCameraByPlane(SCGlobals.camerapos, num);
			for (int i = 0; i < SCGlobals.level.numplaneobjs[num]; i++)
			{
				cLevelObj = SCGlobals.level.ar_drawplanes[num, i];
				cLevelObj.Calculate();
			}
			SetGameCamera(SCGlobals.camerapos);
		}
		for (int j = 0; j < SCGlobals.level.numplaneobjs[num]; j++)
		{
			cLevelObj2 = SCGlobals.level.ar_drawplanes[num, j];
			cLevelObj2.Calculate();
		}
	}

	public void CalcPers()
	{
		SCGlobals.hero.Calculate();
		for (int i = 0; i < SCGlobals.nummonsters; i++)
		{
			SCGlobals.ar_monsters[i].Calculate();
		}
	}

	public void CalculateGame2()
	{
		bool flag = true;
		CalcPhysics++;
		if (CalcPhysics >= 10)
		{
			flag = false;
			CalcPhysics = 0;
		}
		_ = SCGlobals.ftickperframe;
		_ = SCGlobals.frameinterp;
		_ = SCGlobals.tickperframe;
		CalculateGameGirdSize();
		GameCalculateCamera();
		CalculateViewportRect();
		SetGameCamera(SCGlobals.camerapos);
		CowRender.SetOrtho(SCGlobals.GRID_SIZE_X, SCGlobals.GRID_SIZE_Y, 1f, 300f);
		SCGlobals.frustum.Calculate();
		SCGlobals.hero.Calculate();
		if (!flag)
		{
			CalculateVisibility();
		}
		SetGameCamera(SCGlobals.camerapos);
		for (int i = 0; i < SCGlobals.nummonsters; i++)
		{
			if (!SCGlobals.b_pause)
			{
				SCGlobals.ar_monsters[i].Calculate();
			}
		}
		if (b_startlevel && !b_showedTask)
		{
			SuperCow.AddScreen(18, activate: true);
			b_showedTask = true;
		}
		if (b_startlevel && !SCGlobals.b_pause)
		{
			startleveltime -= SCGlobals.ftickperframe;
			if (startleveltime + SCGlobals.ftickperframe > 0f && startleveltime <= 0f)
			{
				SCGlobals.hero.SetBehavior(behaviors_t.BT_STAND);
				SCGlobals.b_stopphysics = false;
				b_startlevel = false;
				startleveltime = 0f;
			}
		}
		if (b_maindeath)
		{
			numlives--;
			lives = "x" + (numlives + 1);
			if (numlives < 0)
			{
				GameOver();
			}
			else
			{
				RestartAfterLoseLife();
			}
		}
		if (SCGlobals.b_victory)
		{
			GameVictory();
		}
		if (SCGlobals.trophytimer > 0f && !SCGlobals.b_pause)
		{
			SCGlobals.trophytimer += SCGlobals.ftickperframe;
			if (SCGlobals.trophytimer >= TROPHY_APP_TIME)
			{
				SCGlobals.b_stopphysics = true;
				SCGlobals.b_pause = true;
				TrophyRoom.InitTrophyRoom(0, SuperCow.Settings.GetPlayerName(), ingame: true);
				SuperCow.AddScreen(10, activate: true);
				SCGlobals.trophytimer = 0f;
			}
		}
		if (SCGlobals.hero.behavior == behaviors_t.BT_VICTORY && endleveltime == 0f)
		{
			endleveltime = 1f;
		}
		if (endleveltime > 0f)
		{
			if (endleveltime < (END_LEVEL_TIME - 1000) && endleveltime + SCGlobals.ftickperframe >= (END_LEVEL_TIME - 1000))
			{
				SetGagDisapp(1000f);
			}
			if (endleveltime < END_LEVEL_TIME && endleveltime + SCGlobals.ftickperframe >= END_LEVEL_TIME)
			{
				// REMARK: without these lines pausing the game in end gates will be very surprising for ScreenManager, goddamn I hate that guy
				SCGlobals.b_pause = false;
				SCGlobals.b_gamemenu = false;
				SuperCow.GetScreen(17)?.Deactivate();
				
				SCGlobals.b_victory = true;
			}
			endleveltime += SCGlobals.ftickperframe;
		}
		SCGlobals.supercowgame.PhysicGame();
	}

	public void SetCameraEffect(int neffect)
	{
		cameraeffectindex = neffect;
		cameraeffecttimer = 1f;
		switch (neffect)
		{
		case 0:
			CAMERAEFFECTTIME = 3000f;
			break;
		case 1:
			CAMERAEFFECTTIME = 4000f;
			break;
		case 2:
			CAMERAEFFECTTIME = 200f;
			break;
		case 3:
			CAMERAEFFECTTIME = 3000f;
			break;
		}
	}

	public void CalculateCameraEffect()
	{
		if (!(cameraeffecttimer <= 0f))
		{
			float num = cameraeffecttimer / CAMERAEFFECTTIME;
			switch (cameraeffectindex)
			{
			case 0:
				le_scale = 1f - 0.3f * (float)Math.Sin(num * (float)Math.PI);
				break;
			case 1:
				le_scale = 1f - 0.45f * num;
				break;
			case 2:
				SCGlobals.camerapos.y += 0.15f * (float)Math.Sin((float)Math.PI * 5f * num) * (1f - num);
				break;
			case 3:
				le_scale = 1f - 0.6f * num;
				SCGlobals.camerapos = SCGlobals.camerapos.interp(-(new vec2_t(p_bossmonster.curpos.x, p_bossmonster.curpos.z) + new vec2_t(0f, p_bossmonster.monster_height / 2f)), num);
				break;
			}
			cameraeffecttimer += SCGlobals.ftickperframe;
			if (cameraeffecttimer >= CAMERAEFFECTTIME)
			{
				cameraeffecttimer = 0f;
			}
		}
	}

	public void GameCalculateCamera()
	{
		if (SCGlobals.hero.behavior == behaviors_t.BT_DEATHFALL)
		{
			return;
		}
		float num = 0.004f;
		if (SCGlobals.hero.behavior == behaviors_t.BT_JUMP || SCGlobals.hero.behavior == behaviors_t.BT_FALL || SCGlobals.hero.behavior == behaviors_t.BT_DEATH)
		{
			num = 0f;
		}
		float num2 = SCGlobals.hero.position.y;
		if (SCGlobals.hero.herobridgey > -1000f)
		{
			num2 = SCGlobals.hero.herobridgey;
		}
		camera_height = (num2 - level_bottom) / (level_top - level_bottom);
		float num3 = 0f - num2;
		float num4 = ((!(endleveltime > 0f)) ? 1f : (1f - endleveltime / 5500f));
		num3 += (13f * (camera_height * 0.5f - 0.5f) + cameradown) * num4 - 2.5f * (1f - num4);
		if (0f - num3 + SCGlobals.camerapos.y <= 0f && SCGlobals.hero.behavior == behaviors_t.BT_FALL)
		{
			SCGlobals.camerapos.y = num3;
		}
		if (num2 + SCGlobals.camerapos.y >= 4f && SCGlobals.hero.behavior == behaviors_t.BT_JUMP)
		{
			SCGlobals.camerapos.y -= num2 - prevhero_y - 0.001f;
		}
		float num5 = num3 - SCGlobals.camerapos.y;
		if (num5 != 0f)
		{
			if ((int)Math.Abs(num5) < 1)
			{
				SCGlobals.camerapos.y += ((num5 > 0f) ? 1f : (-1f)) * SCGlobals.ftickperframe * num;
			}
			else
			{
				SCGlobals.camerapos.y += num5 * SCGlobals.ftickperframe * num * 0.5f;
			}
			float num6 = num3 - SCGlobals.camerapos.y;
			if (num5 * num6 < 0f)
			{
				SCGlobals.camerapos.y = num3;
			}
		}
		SCGlobals.camerapos.x = 0f - SCGlobals.hero.position.x;
		if (b_bossfight && bossfighttimer < 3000f)
		{
			num4 = bossfighttimer / 3000f;
			num4 = (float)Math.Sqrt((float)Math.Sin(num4 * (float)Math.PI));
			le_scale = 1f - 0.6f * num4;
			SCGlobals.camerapos = camerabossposition0.interp(-(new vec2_t(p_bossmonster.curpos.x, p_bossmonster.curpos.z) + new vec2_t(0f, p_bossmonster.monster_height / 2f)), num4);
			bossfighttimer += SCGlobals.ftickperframe;
			if (bossfighttimer >= 3000f)
			{
				SCGlobals.b_stopphysics = false;
			}
		}
		float num7 = 0f - SCGlobals.camerapos.y;
		SCGlobals.camerapos.y = 0f - num7;
		num7 = 0f - SCGlobals.camerapos.x;
		if (num7 - SCGlobals.GRID_SIZE_X * 0.5f < level_left)
		{
			num7 = level_left + SCGlobals.GRID_SIZE_X * 0.5f;
		}
		if (num7 + SCGlobals.GRID_SIZE_X * 0.5f > level_right)
		{
			num7 = level_right - SCGlobals.GRID_SIZE_X * 0.5f;
		}
		SCGlobals.camerapos.x = 0f - num7;
		camera_height = (0f - SCGlobals.camerapos.y - (level_bottom + 6.5f)) / (level_top - level_bottom - 13f);
		if (camera_height < 0f)
		{
			camera_height = 0f;
		}
		SCGlobals.camerapos.x += SCGlobals.cameraposModifier.x;
		SCGlobals.camerapos.y += SCGlobals.cameraposModifier.y;
		prevhero_y = num2;
	}

	public void GameDrawInfoPannel()
	{
		CowRender.LoadMatrixNull();
		CowRender.LoadCameraNull();
		CowRender.SetOrtho(800f, 600f, 1f, 500f);
		CowRender.SetTextMode();
		CowRender.EnableRS(7u);
		SCGlobals.cam.load_identity();
		SCGlobals.cam.translate(0f, 0f, 50f);
		CowRender.LoadCamera(SCGlobals.cam);
		SCGlobals.frustum.Calculate();
		dcolor_t dcolor_t2 = new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, 225);
		float num = 64f;
		int num2 = 0;
		dcolor_t2.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		Text.SetTextColor(dcolor_t2);
		num = 64f;
		float num3 = -270f;
		float num4 = -330f;
		float num5 = SCGlobals.hero.curlife / SCGlobals.hero.maxlife;
		life0 += (num5 - life0) * SCGlobals.ftickperframe * 0.005f;
		frect_t vc = default(frect_t);
		float num6 = 40f;
		float num7 = 40f;
		if (SuperCow.IsHDTV)
		{
			num6 = -82f;
			num7 = 32f;
		}
		float num8 = num6;
		float num9 = num7;
		CowRender.BindTexture(ar_texicons[0]);
		CowRender.DrawColorQuad2D(new frect_t(-400f + num8, -172f + num9, -144f + num8, -300f + num9), SCGlobals.tc_normal, dcolor_t.ColorWhite, 0f);
		lifesprite.SetParams(32f, 32f, 30);
		lifesprite.Draw(new vec2_t(-293f + num8, num3 + 20f + num9), -1, dcolor_t.ColorWhite, 0f, 0f);
		CowRender.DrawDynBuffer();
		CowRender.EnableRS(7u);
		float num10 = 150f;
		vc.assign(new vec2_t(-258f + num8, -288f + num9), 256f, 32f);
		vc.x1 = vc.x0 + life0 * num10;
		dcolor_t2.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		liveparcount = 0;
		if (Math.Abs(life0 - num5) > 0.01f)
		{
			dcolor_t2.b = (byte)(55f + 200f * SuperCowHelper.psin(8f));
			dcolor_t2.r = (byte)(55f + 200f * SuperCowHelper.psin(8f));
			liveparcount += (int)SCGlobals.tickperframe;
			while (liveparcount > 40)
			{
				liveparcount -= 40;
				Particles.AddDynamicParticle(new vec2_t(vc.x1, vc.y1 + CRand.prand(32f)), 30, 1, -1);
			}
		}
		CowRender.BindTexture(p_livebar_tex);
		CowRender.DrawColorQuad2D(vc, new frect_t(0f, 0f, life0 * num10 / 256f, 1f), dcolor_t2, 0f);
		string str = "x" + (numlives + 1);
		Text.OutTextXY(num4 + 62f + num8, num3 + 22f + num9, (numlives >= 9) ? 14f : 18f, str, b_center: false);
		num = 45f;
		num2++;
		if (SuperCow.IsHDTV)
		{
			num6 = -36f;
			num7 = 42f;
		}
		else
		{
			num6 = 85f;
		}
		num8 = 0f - num6;
		num9 = num7;
		float num11 = (float)totalscore - totalscore0;
		num4 = 370f + num8;
		num3 = -284f + num9;
		Text.OutBitmap(num4 - 50f, num3, 128f, 32f, ar_texicons[2]);
		str = ((int)totalscore0).ToString();
		Text.OutTextXY(num4 - Text.GetStringWidth(str, 18f) + 60f, num3, 18f, str, b_center: false);
		if (SuperCow.IsHDTV)
		{
			num6 = -56f;
			num7 = 34f;
		}
		else
		{
			num6 = 65f;
		}
		num8 = 0f - num6;
		num9 = 0f - num7;
		num4 = 350f + num8;
		num3 = 280f + num9;
		CowRender.BindTexture(p_levelbartex);
		CowRender.DrawColorQuad2D(num4, num3, 256f, 64f, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		Text.OutTextCenterXY(num4 + 10f, num3, 15f, stageLevel);
		num6 = ((!SuperCow.IsHDTV) ? (-35f) : (-16f));
		num8 = 0f - num6;
		num9 = num7 - 30f;
		num4 = 210f + num8;
		num3 = -230f + num9;
		if (SCGlobals.hero.megajumptimer > 0f)
		{
			num = 32f + 2f * (float)Math.Sin(SCGlobals.global_timer * 0.01f);
			CowRender.BindTexture(ar_texicons[3]);
			CowRender.DrawColorQuad2DDynBuffer(new vec2_t(num4, num3), num, num, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
			CowRender.DrawDynBuffer();
			str = ((int)(SCGlobals.hero.megajumptimer / 1000f)).ToString();
			Text.OutTextCenterXY(num4, num3 - 30f, 19f, str);
			num4 -= 100f;
		}
		if (SCGlobals.hero.hypershoetimer > 0f)
		{
			num = 32f + 2f * (float)Math.Sin(SCGlobals.global_timer * 0.01f);
			CowRender.BindTexture(ar_texicons[4]);
			CowRender.DrawColorQuad2DDynBuffer(new vec2_t(num4, num3), num, num, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
			CowRender.DrawDynBuffer();
			str = ((int)(SCGlobals.hero.hypershoetimer / 1000f)).ToString();
			Text.OutTextCenterXY(num4, num3 - 30f, 19f, str);
			num4 -= 100f;
		}
		if (SCGlobals.hero.firmnesstimer > 0f)
		{
			num = 32f + 2f * (float)Math.Sin(SCGlobals.global_timer * 0.01f);
			CowRender.BindTexture(ar_texicons[5]);
			CowRender.DrawColorQuad2DDynBuffer(new vec2_t(num4, num3), num, num, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
			CowRender.DrawDynBuffer();
			str = ((int)(SCGlobals.hero.firmnesstimer / 1000f)).ToString();
			Text.OutTextCenterXY(num4, num3 - 30f, 19f, str);
		}
		num8 = 0f - num6;
		num9 = num7;
		num4 = 380f + num8;
		if (num11 <= 1f)
		{
			totalscore0 = totalscore;
		}
		else
		{
			scoreparcount += (int)SCGlobals.tickperframe;
			while (scoreparcount > 40)
			{
				scoreparcount -= 40;
				Particles.AddDynamicParticle(new vec2_t(num4 - CRand.prand(80f) + 50f, -270f + CRand.frand(15f) + 30f), 29, 1, -1);
			}
			totalscore0 += num11 * SCGlobals.ftickperframe / 200f;
		}
		CowRender.LoadMatrixNull();
		Text.DrawEventTexts();
		Text.DrawLInfoTexts();
		num6 = ((!SuperCow.IsHDTV) ? 46f : (-66f));
		num8 = num6;
		num9 = 0f - num7;
		int num12 = 0;
		int num13 = 0;
		switch (SCGlobals.curleveltask)
		{
		case 1:
			num12 = SCGlobals.levelstat.keys;
			num13 = SCGlobals.levelstat.numkeys;
			break;
		case 2:
			num12 = SCGlobals.levelstat.monsters;
			num13 = SCGlobals.levelstat.nummonsters;
			break;
		case 3:
			num12 = SCGlobals.levelstat.gems;
			num13 = SCGlobals.levelstat.numgems;
			break;
		case 4:
			num12 = SCGlobals.levelstat.garbage;
			num13 = SCGlobals.levelstat.numgarbage;
			break;
		case 5:
			num12 = ((!p_bossmonster.b_alive) ? 1 : 0);
			num13 = 1;
			break;
		}
		num3 = 281f;
		if (SCGlobals.curleveltask > 0)
		{
			CowRender.BindTexture(p_taskbartex);
			CowRender.DrawColorQuad2D(-339f + num8, num3 + num9, 256f, 64f, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
			CowRender.BindTexture(ar_taskicon[SCGlobals.curleveltask - 1]);
			CowRender.DrawColorQuad2D(-293f + num8, num3 + num9, 64f, 64f, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
			str = num12 + "/" + num13;
			Text.OutTextCenterXY(-362f + num8, num3 + num9, 17f, str);
		}
		if (!b_switchexit && SCGlobals.curleveltask > 0)
		{
			float dist_to_obj = 0f;
			dcolor_t2.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			vec2_t vec2_t2 = new vec2_t(0f, 2f) + new vec2_t(SCGlobals.hero.position);
			vec2_t nearestTaskPoint = GetNearestTaskPoint(SCGlobals.curleveltask, ref dist_to_obj);
			dist_to_obj *= 32f;
			ray = nearestTaskPoint - vec2_t2;
			ray.normalize();
			vnull = (vec2_t2 + SCGlobals.camerapos) * 32f;
			float num14;
			if ((num14 = Collision.intersetRayLine(ref v, p1scr, p2scr, vnull, ray)) < 0f && (num14 = Collision.intersetRayLine(ref v, p2scr, p3scr, vnull, ray)) < 0f && (num14 = Collision.intersetRayLine(ref v, p3scr, p4scr, vnull, ray)) < 0f)
			{
				num14 = Collision.intersetRayLine(ref v, p4scr, p1scr, vnull, ray);
			}
			CowRender.BindTexture(p_compass);
			float num15 = (dist_to_obj - num14) / 128f;
			if (num15 > 1f)
			{
				num15 = 1f;
			}
			else if (num15 < 0f)
			{
				num15 = 0f;
			}
			dcolor_t2.a = (byte)(num15 * (float)(int)dcolor_t2.a);
			dcolor_t2.g = (byte)(num15 * (float)(int)dcolor_t2.g);
			dcolor_t2.b = (byte)(num15 * (float)(int)dcolor_t2.b);
			num15 = (dist_to_obj - num14) / 1280f;
			if (num15 > 1f)
			{
				num15 = 1f;
			}
			else if (num15 < 0f)
			{
				num15 = 0f;
			}
			dcolor_t2.r = (byte)((1f - num15) * (float)(int)dcolor_t2.r);
			num15 = ((num15 == 0f) ? 0f : ((num15 <= 0.2f) ? 0.2f : ((num15 <= 0.4f) ? 0.4f : ((num15 <= 0.6f) ? 0.6f : ((!(num15 <= 0.8f)) ? 1f : 0.8f)))));
			num15 = 1f - num15;
			float num16 = num14 - 32f - (0.5f + 5f * num15) * 32f * SuperCowHelper.psin(1f + 2.5f * num15);
			v = vnull + ray * num16;
			arrow_taks_pos = arrow_taks_pos.interp(v, 0.003f * SCGlobals.ftickperframe);
			CowRender.DrawRotatedColorQuad2D(arrow_taks_pos, arrow_taks_pos, ray.angle() + 180f, 64f, 64f, new frect_t(0f, 0f, 1f, 1f), dcolor_t2, 0f);
		}
		CowRender.DisableRS(7u);
	}

	public vec2_t GetNearestTaskPoint(int ntask, ref float dist_to_obj)
	{
		vec2_t vec2_t2 = default(vec2_t);
		vec2_t vec2_t3 = default(vec2_t);
		float num = 100000f;
		vec2_t vec2_t4 = new vec2_t(0f, 2f) + new vec2_t(SCGlobals.hero.position);
		switch (ntask)
		{
		case 1:
		{
			for (int i = 0; i < SCGlobals.level.numlevelobjs; i++)
			{
				CLevelObj cLevelObj = SCGlobals.level.ar_levelobjs[i];
				if (cLevelObj.b_active != 0 && (cLevelObj.type & 8) != 0 && !(cLevelObj.name != "key"))
				{
					float num2 = (cLevelObj.position - vec2_t4).len();
					if (num > num2)
					{
						num = num2;
						vec2_t3 = cLevelObj.position;
					}
				}
			}
			break;
		}
		case 2:
		{
			for (int i = 0; i < SCGlobals.nummonsters; i++)
			{
				CMonster cMonster = SCGlobals.ar_monsters[i];
				if (cMonster.behavior != behaviors_t.BT_DEATH && cMonster.b_alive)
				{
					float num2 = (new vec2_t(cMonster.position) - vec2_t4).len();
					if (num > num2)
					{
						num = num2;
						vec2_t3 = new vec2_t(cMonster.position);
					}
				}
			}
			break;
		}
		case 3:
		{
			for (int i = 0; i < SCGlobals.level.numlevelobjs; i++)
			{
				CLevelObj cLevelObj = SCGlobals.level.ar_levelobjs[i];
				if (cLevelObj.b_active != 0 && (cLevelObj.type & 8) != 0 && (cLevelObj.objID == 32 || cLevelObj.objID == 31))
				{
					float num2 = (cLevelObj.position - vec2_t4).len();
					if (num > num2)
					{
						num = num2;
						vec2_t3 = cLevelObj.position;
					}
				}
			}
			break;
		}
		case 4:
		{
			for (int i = 0; i < SCGlobals.level.numlevelobjs; i++)
			{
				CLevelObj cLevelObj = SCGlobals.level.ar_levelobjs[i];
				if (cLevelObj.b_active != 0 && cLevelObj.objID == 8 && cLevelObj.b_switch <= 0)
				{
					float num2 = (cLevelObj.position - vec2_t4).len();
					if (num > num2)
					{
						num = num2;
						vec2_t3 = cLevelObj.position;
					}
				}
			}
			break;
		}
		case 5:
			if (SCGlobals.supercowgame.p_bossmonster != null)
			{
				vec2_t3 = SCGlobals.supercowgame.p_bossmonster.position.v2t();
				num = (vec2_t3 - vec2_t4).len();
			}
			break;
		}
		dist_to_obj = num;
		return vec2_t3;
	}

	public static void DrawDynPlatform(frect_t rect0, texture_t p_tex, dcolor_t dc, int force_tile)
	{
		CowRender.BindTexture(p_tex);
		vec3_t vec3_t2 = rect0.p0.v3t();
		int num = (int)p_tex.height;
		if (force_tile != 0)
		{
			num = force_tile;
		}
		int num2 = (int)Math.Round((rect0.x1 - rect0.x0) * 32f / (float)num);
		int num3 = (int)p_tex.width / num;
		float num4 = (float)num / 32f;
		float num5 = rect0.sizey();
		frect_t frect_t2 = default(frect_t);
		for (int i = 0; i < num2; i++)
		{
			int num6 = ((i != num2 - 1) ? ((i <= 2 || num2 <= 2) ? i : (1 + i % (num3 - 2))) : (num3 - 1));
			frect_t2.assign(vec3_t2.x + num4 * (float)i, vec3_t2.y, vec3_t2.x + num4 * (float)(i + 1), vec3_t2.y - num5);
			float x = 1f / (float)num3 * (float)num6;
			float x2 = 1f / (float)num3 * (float)(num6 + 1);
			float y = 0f;
			float y2 = 1f;
			pl_p1.assign(frect_t2.x1, frect_t2.y1, 0f);
			pl_p2.assign(frect_t2.x1, frect_t2.y0, 0f);
			pl_p3.assign(frect_t2.x0, frect_t2.y0, 0f);
			pl_p4.assign(frect_t2.x0, frect_t2.y1, 0f);
			pl_t1.assign(x2, y2);
			pl_t2.assign(x2, y);
			pl_t3.assign(x, y);
			pl_t4.assign(x, y2);
			CowRender.DrawColorQuadDynBuffer(pl_p1, pl_p2, pl_p3, pl_p4, pl_t1, pl_t2, pl_t3, pl_t4, dc);
		}
	}

	public static void DrawPlatform2(frect_t rect0, texture_t p_tex, dcolor_t dc, int force_tile, float rotation)
	{
		CowRender.BindTexture(p_tex);
		vec3_t vec3_t2 = rect0.p0.v3t();
		int num = (int)p_tex.height;
		if (force_tile != 0)
		{
			num = force_tile;
		}
		int num2 = (int)Math.Round((rect0.x1 - rect0.x0) * 32f / (float)num);
		int num3 = (int)p_tex.width / num;
		float num4 = (float)num / 32f;
		float num5 = rect0.sizey();
		frect_t frect_t2 = default(frect_t);
		for (int i = 0; i < num2; i++)
		{
			int num6 = ((i != num2 - 1) ? ((i <= 2 || num2 <= 2) ? i : (1 + i % (num3 - 2))) : (num3 - 1));
			frect_t2.assign(vec3_t2.x + num4 * (float)i, vec3_t2.y, vec3_t2.x + num4 * (float)(i + 1), vec3_t2.y - num5);
			float num7 = 1f / (float)num3 * (float)num6;
			float num8 = 1f / (float)num3 * (float)(num6 + 1);
			float num9 = 0f;
			float num10 = 1f;
			pl_p1.assign(frect_t2.x1, frect_t2.y1, 0f);
			pl_p2.assign(frect_t2.x1, frect_t2.y0, 0f);
			pl_p3.assign(frect_t2.x0, frect_t2.y0, 0f);
			pl_p4.assign(frect_t2.x0, frect_t2.y1, 0f);
			pl_t1.assign(num8, num10);
			pl_t2.assign(num8, num9);
			pl_t3.assign(num7, num9);
			pl_t4.assign(num7, num10);
			CowRender.DrawRotatedColorQuad2DStaticBuffer(new vec2_t(rect0.x0, rect0.y0), new vec2_t(frect_t2.x0, frect_t2.y0), rotation, num4, num5, new frect_t(num7, num9, num8, num10), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		}
	}

	public static void DrawPlatform(frect_t rect0, texture_t p_tex, dcolor_t dc, int force_tile, float rotation, vec2_t position)
	{
		CowRender.BindTexture(p_tex);
		vec3_t vec3_t2 = rect0.p0.v3t();
		if (rotation != 0f)
		{
			vec3_t2.y += 1f;
			vec3_t2.x -= 1f;
		}
		int num = (int)p_tex.height;
		if (force_tile != 0)
		{
			num = force_tile;
		}
		int num2 = (int)Math.Round((rect0.x1 - rect0.x0) * 32f / (float)num);
		int num3 = (int)p_tex.width / num;
		float num4 = (float)num / 32f;
		float num5 = rect0.sizey();
		frect_t frect_t2 = default(frect_t);
		for (int i = 0; i < num2; i++)
		{
			int num6 = ((i == num2 - 1) ? (num3 - 1) : ((i <= 2 || num2 <= 2) ? i : (1 + i % (num3 - 2))));
			frect_t2.assign(vec3_t2.x + num4 * (float)i, vec3_t2.y, vec3_t2.x + num4 * (float)(i + 1), vec3_t2.y - num5);
			float num7 = 1f / (float)num3 * (float)num6;
			float num8 = 1f / (float)num3 * (float)(num6 + 1);
			float num9 = 0f;
			float num10 = 1f;
			pl_p1.assign(frect_t2.x1, frect_t2.y1, 0f);
			pl_p2.assign(frect_t2.x1, frect_t2.y0, 0f);
			pl_p3.assign(frect_t2.x0, frect_t2.y0, 0f);
			pl_p4.assign(frect_t2.x0, frect_t2.y1, 0f);
			pl_t1.assign(num8, num10);
			pl_t2.assign(num8, num9);
			pl_t3.assign(num7, num9);
			pl_t4.assign(num7, num10);
			if (rotation != 0f)
			{
				CowRender.DrawRotatedColorQuad2DStaticBuffer(pl_p1.v2t(), position, rotation, num4, num5, new frect_t(num7, num9, num8, num10), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
			}
			else
			{
				CowRender.DrawColorQuadStaticBuffer(pl_p1, pl_p2, pl_p3, pl_p4, pl_t1, pl_t2, pl_t3, pl_t4, dc);
			}
		}
	}

	public static void SetGagDisapp(float time)
	{
	}

	public static bool GagActive()
	{
		return fgagtime != 0f;
	}

	public static void DrawGag()
	{
		if (fgagtime == 0f)
		{
			return;
		}
		float num = fgagtime / fgageffecttime;
		if (fgagstep < 0f)
		{
			num = 1f - num;
		}
		DrawGagFrame(num, p_gag);
		fgagtime += SCGlobals.ftickperframe;
		if (fgagtime > fgageffecttime)
		{
			if (fgagstep < 0f)
			{
				fgagtime = 1f;
				fgagstep = 1f;
			}
			else
			{
				fgagtime = 0f;
			}
		}
	}

	public static void DrawGagFrame(float k, texture_t p_gagtex)
	{
		if (k > 1f)
		{
			k = 1f;
		}
		if (!(k < 0f) && !(k > 1f))
		{
			float num = SuperCow.getWidth * 1.5f;
			float num2 = SuperCow.getWidth;
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			graphicsDevice.SamplerStates[0] = SamplerState.AnisotropicClamp;
			vec2_t vec2_t2 = new vec2_t(0f, 0f);
			vec2_t2 *= 1f - k;
			matrix_t m = default(matrix_t);
			m.load_identity();
			m.translate(-vec2_t2);
			m.zrot(50f - 360f * k);
			CowRender.LoadMatrix(m);
			if (p_gagtex.p_texD3D == null)
			{
				CowRender.BindTexture(p_gag);
			}
			else
			{
				CowRender.BindTexture(p_gagtex);
			}
			float num3 = 10000f * k * k * k;
			frect_t vc = default(frect_t);
			frect_t tc = default(frect_t);
			tc.assign(0f, 0f, 1f, 1f);
			byte aa = (byte)(255f * (1f - k * 0.7f));
			dcolor_t c = new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, aa);
			CowRender.DrawColorQuad2D(new vec2_t(0f, 0f), num3, num3, tc, c, 0f);
			CowRender.BindTexture(default(texture_t));
			c.assign(0, 0, 0, aa);
			vc.assign(0f - num, num3 / 2f, (0f - num3) / 2f, (0f - num3) / 2f);
			CowRender.DrawColorQuad2D(vc, tc, c, 0f);
			vc.assign(num3 / 2f, num3 / 2f, num, (0f - num3) / 2f);
			CowRender.DrawColorQuad2D(vc, tc, c, 0f);
			vc.assign(0f - num, num2, num, num3 / 2f - 0.1f);
			CowRender.DrawColorQuad2D(vc, tc, c, 0f);
			vc.assign(0f - num, (0f - num3) / 2f + 0.1f, num, 0f - num2);
			CowRender.DrawColorQuad2D(vc, tc, c, 0f);
			CowRender.LoadMatrixNull();
		}
	}

	public void AddScore(vec2_t p, int addscore)
	{
		AddScore(new vec3_t(p.x, p.y, 0f), addscore, 0);
	}

	public void AddScore(vec3_t p, int addscore)
	{
		AddScore(p, addscore, 0);
	}

	public void AddScore(vec2_t p, int addscore, int type)
	{
		AddScore(new vec3_t(p.x, p.y, 0f), addscore, type);
	}

	public void AddScore(vec3_t p, int addscore, int type)
	{
		if (totalscore < 100000 && totalscore + addscore >= 100000)
		{
			AddGameTrophy(2);
		}
		if (totalscore < 500000 && totalscore + addscore >= 500000)
		{
			AddGameTrophy(6);
		}
		totalscore += addscore;
		scoreatlevel += addscore;
		string str = "+" + addscore;
		Particles.AddDynamicText(p, str, type);
	}

	public int AddHeroHealth(float health)
	{
		return SCGlobals.hero.AddHealth(health);
	}

	public void AddHeroLife()
	{
		numlives++;
	}

	public void GameOver()
	{
		SuperCow.Settings.SetPlayerScore((uint)totalscore);
		SuperCow.Settings.PlayerGameOver();
		SuperCow.Settings.SaveSettings();
		b_maindeath = false;
		SCGlobals.b_pause = true;
		SCGlobals.b_gamemenu = true;
		SCGlobals.b_stopphysics = true;
		SCGlobals.menusetk = 1f;
		SuperCow.AddScreen(23, activate: true);
	}

	public void EnableGameplayMenu(string name)
	{
		SCGlobals.b_pause = true;
		SCGlobals.b_gamemenu = true;
		SCGlobals.b_stopphysics = true;
	}

	public void RestartAfterLoseLife()
	{
		SCGlobals.hero.Refresh();
		b_maindeath = false;
		SCGlobals.hero.position = SCGlobals.hero.safeposition;
		SCGlobals.camerapos = -SCGlobals.hero.position.v2t();
		SCGlobals.hero.SetBehavior(behaviors_t.BT_START);
		if (numlives == 0)
		{
			Text.AddFunnyText(wstrstuff.GetStr("lastlife"), new vec2_t(0f, -30f), 0, 25f, 4000f);
		}
		else
		{
			Text.AddFunnyText(wstrstuff.GetStr("onemorelife"), new vec2_t(0f, -30f), 0, 25f, 4000f);
		}
		Weapons.InitBullets();
		b_startlevel = true;
		startleveltime = START_LEVEL_TIME;
		SCGlobals.b_stopphysics = true;
		for (int i = 0; i < SCGlobals.nummonsters; i++)
		{
			SCGlobals.ar_monsters[i].SetBehavior(behaviors_t.BT_STAND);
		}
		SCGlobals.hero.startharmlesstimer = 3000f;
	}

	public void AddGameTrophy(int num)
	{
		if (!Guide.IsTrialMode && !(SCGlobals.trophytimer > 0f) && TrophyRoom.AddTrophy(num, SuperCow.Settings.GetPlayerName()))
		{
			SetCameraEffect(0);
			SCGlobals.trophytimer = 1f;
			Text.RemovEventText();
			SCGlobals.hero.SetBehavior(behaviors_t.BT_WOW);
			Text.AddEventText(wstrstuff.GetStr("newtrophy"), new vec2_t(0f, 0f), 30f, TROPHY_APP_TIME);
		}
	}

	public void FromDemoToFull()
	{
		Sound.Stop(mMusic);
		SuperCow.RemoveScreen(11, clear: true);
		SuperCow.AddScreen(4, activate: true);
		SuperCow.AddScreen(16, activate: true);
		SuperCow.AddScreen(6, activate: true);
	}
}
