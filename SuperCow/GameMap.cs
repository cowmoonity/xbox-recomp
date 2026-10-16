using System;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public static class GameMap
{
	public const int NUM_STAGES = 11;

	public const int CONGRATULATION_DELAY = 500;

	public const float GM_SPEED_GLOW = 0.005f;

	public static uint nGameMapCommand;

	private static vec2_t[] splinePos = new vec2_t[160];

	private static vec2_t[,] stagePos = new vec2_t[11, 7];

	private static float part = 0f;

	private static float f_timerapp = 0f;

	private static float f_timerdisapp = 0f;

	private static vec2_t click_lpos;

	private static vec2_t delta_lpos;

	private static vec2_t click_rpos;

	private static vec2_t delta_rpos;

	private static float oppabb = 0f;

	private static float oppasb = 0f;

	public static vec2_t delta_stage_pos = new vec2_t(0f, 0f);

	public static vec2_t dot_spline_pos;

	public static frect_t gm_button_back = new frect_t(-396f, 296f, -332f, 232f);

	public static frect_t gm_button_start = new frect_t(130f, -215f, 386f, -294f);

	public static frect_t gamemap_window = default(frect_t);

	public static frect_t gamemap = default(frect_t);

	public static frect_t gm_button_ok = new frect_t(-64f, -112f, 64f, -176f);

	public static stage_config_t[] ar_stages_config = new stage_config_t[14];

	public static stage_config_t p_active_stage;

	public static stage_config_t p_selected_stage;

	public static level_config_t[] ar_levels_config = new level_config_t[10];

	public static vec2_t[,] ar_levelmarks_pos = new vec2_t[11, 10];

	public static float[,] ar_levelmarks_spline_pos = new float[11, 10];

	public static int[] ar_my_stages = new int[100];

	public static int mySelectedLevel = 0;

	public static int myPrevStage = 0;

	public static gm_state_t gm_state;

	public static CSprite[] level_tex = new CSprite[2];

	public static CSprite box_tex = new CSprite();

	public static bool bShowingBaloon = false;

	public static uint dwLastKey = 0u;

	public static texture_t p_gamemap;

	public static texture_t p_map_level_mark;

	public static texture_t p_map_stage_mark;

	public static texture_t p_map_stage_question;

	public static texture_t p_map_spline_mark;

	public static texture_t[] p_map_heads = new texture_t[11];

	public static texture_t[] p_stage_glow = new texture_t[3];

	public static texture_t[] p_level_glow = new texture_t[3];

	public static texture_t p_mapBack;

	public static texture_t p_mapFrameHDTV;

	public static texture_t p_mapFrame;

	public static texture_t p_mapHUD;

	public static texture_t p_background;

	public static texture_t p_backbutton;

	public static texture_t p_backbuttonlum;

	public static texture_t p_startbuttonlum;

	public static texture_t[] p_ropes = new texture_t[4];

	public static texture_t p_gm_mask_texture1;

	public static texture_t p_gm_mask_texture2;

	public static texture_t[] p_glow = new texture_t[3];

	public static bool b_gm_pause = false;

	public static bool b_gm_congratulation = false;

	public static bool b_gm_congratulation_skip = false;

	public static bool b_gm_editor_mode = false;

	public static bool b_gm_left_click = false;

	public static bool b_gm_right_click = false;

	public static bool b_gm_left_drag = false;

	public static bool b_gm_right_drag = false;

	public static bool b_gm_artefact_presentation = false;

	public static bool b_gm_escape = false;

	public static bool b_gm_start = false;

	public static int i_gm_congratulation = 4;

	public static int i_gm_numactive_stage = 0;

	public static int i_gm_numactive_level = -1;

	public static int i_gm_numselected_stage = 0;

	public static int i_gm_numselected_level = 0;

	public static int i_num_levels_in_cur_stage;

	public static float f_gm_randtimer = 0f;

	public static float f_gm_next_position;

	public static float f_gm_current_position;

	public static float f_gm_delta_position;

	public static float f_gm_level_open_timer = 0f;

	public static float f_levels_radius = 0f;

	public static float f_level_effects_timer = 0f;

	public static float f_gm_escape_timer = 0f;

	public static float f_gm_start_timer = 0f;

	public static float f_level_open_timer = 0f;

	public static float f_stage_open_timer = 0f;

	public static byte[] ar_gm_mask_state = new byte[4];

	public static void GameMapInit()
	{
		gmInit();
		gmLoad();
		gmLoadStagesConfig();
	}

	public static void gmSetClipPlaneWrapper(ref frect_t theRect)
	{
		matrix_t m = default(matrix_t);
		m.load_identity();
		matrix_t m2 = default(matrix_t);
		m2.load_identity();
		matrix_t m3 = default(matrix_t);
		m3.load_identity();
		CowRender.GetProjection(ref m);
		CowRender.GetMatrix(ref m2);
		CowRender.GetCamera(ref m3);
		vec3_t vec3_t2 = new vec3_t(theRect.x0, theRect.y0, 1f);
		vec3_t2 = m2 * vec3_t2;
		vec3_t2 = m3 * vec3_t2;
		vec3_t2 = m * vec3_t2;
		vec3_t vec3_t3 = new vec3_t(theRect.x1, theRect.y1, 1f);
		vec3_t3 = m2 * vec3_t3;
		vec3_t3 = m3 * vec3_t3;
		vec3_t3 = m * vec3_t3;
		theRect.assign(vec3_t2.x, vec3_t2.y, vec3_t3.x, vec3_t3.y);
	}

	public static void gmSetClipRectNull()
	{
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		ClipPlaneCollection clipPlanes = graphicsDevice.ClipPlanes;
		clipPlanes.DisableAll();
	}

	public static void gmSetClipRect(frect_t rc)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		frect_t frect_t2 = default(frect_t);
		ClipPlaneCollection clipPlanes = graphicsDevice.ClipPlanes;
		frect_t theRect = default(frect_t);
		theRect.assign(rc.x0, rc.y0, rc.x1, rc.y1);
		gmSetClipPlaneWrapper(ref theRect);
		frect_t2.assign(1f, 0f, 0f, 0f - theRect.x0);
		clipPlanes[0].Plane = frect_t2.GetPlane();
		frect_t2.assign(0f, -1f, 0f, theRect.y0);
		clipPlanes[1].Plane = frect_t2.GetPlane();
		frect_t2.assign(-1f, 0f, 0f, theRect.x1);
		clipPlanes[2].Plane = frect_t2.GetPlane();
		frect_t2.assign(0f, 1f, 0f, 0f - theRect.y1);
		clipPlanes[3].Plane = frect_t2.GetPlane();
		clipPlanes.EnableAll();
	}

	public static void gmDrawMap()
	{
		gmSetClipRect(gamemap_window);
		if (gm_state != gm_state_t.GMS_CHANGE_STAGE)
		{
			CowRender.DisableRS(7u);
		}
		CowRender.BindTexture(p_gamemap);
		CowRender.DrawColorQuad2D(gamemap.pos(), gamemap.sizex(), gamemap.sizey(), new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		CowRender.EnableRS(7u);
		float num = ((!(Math.Abs(f_gm_next_position - f_gm_current_position) <= 0.2f)) ? 1f : (Math.Abs(f_gm_next_position - f_gm_current_position) * 5f));
		CowRender.SetBlend(1);
		CowRender.BindTexture(SCGlobals.p_particlestex);
		Particles.DrawParticle(new vec3_t(gamemap_window.pos() - delta_stage_pos - dot_spline_pos), 0f, 128f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue) * num, 12, toBuffer: false);
		CowRender.DrawDynBuffer();
		CowRender.SetBlend(0);
		gmSetClipRectNull();
	}

	public static void gmDrawBackground()
	{
		CowRender.DisableRS(7u);
		CowRender.BindTexture(p_mapBack);
		CowRender.DrawColorQuad2D(p_mapBack.mImage, 0f, 0f, p_mapBack.mImage.Width, p_mapBack.mImage.Height, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		if (SuperCow.IsHDTV)
		{
			CowRender.BindTexture(p_mapFrameHDTV);
			CowRender.DrawColorQuad2D(p_mapFrameHDTV.mImage, -126f, 0f, 708.3333f, 541.6667f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		}
		else
		{
			CowRender.BindTexture(p_mapFrame);
			CowRender.DrawColorQuad2D(p_mapFrame.mImage, -127f, 0f, 469.16666f, 541.6667f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		}
		CowRender.BindTexture(p_mapHUD);
		if (SuperCow.IsHDTV)
		{
			CowRender.DrawColorQuad2D(p_mapHUD.mImage, 355f, 0f, 254.16667f, 541.6667f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		}
		else
		{
			CowRender.DrawColorQuad2D(p_mapHUD.mImage, 234f, 0f, 254.16667f, 541.6667f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		}
		CowRender.EnableRS(7u);
	}

	public static void gmDraw()
	{
		gmDrawBackground();
		gmDrawMap();
		gmDrawSpline();
		gmDrawLevelPreview();
		switch (gm_state)
		{
		case gm_state_t.GMS_STAND_BY:
			gmDrawStages();
			gmDrawLevels();
			gmDrawRopes();
			gmDrawButtons();
			MapArtefacts.maDraw();
			break;
		case gm_state_t.GMS_CHANGE_STAGE:
			gmDrawStages();
			gmDrawRopes();
			gmDrawButtons();
			MapArtefacts.maDraw();
			break;
		case gm_state_t.GMS_APPEARANCE_LEVELS:
			gmDrawStages();
			gmDrawLevels();
			gmDrawRopes();
			gmDrawButtons();
			MapArtefacts.maDraw();
			break;
		case gm_state_t.GMS_OPEN_MASK:
			gmDrawStages();
			gmDrawLevels();
			MapArtefacts.maDraw();
			MapArtefacts.maDrawOpeningMask();
			gmDrawRopes();
			gmDrawButtons();
			break;
		case gm_state_t.GMS_PRESENTATION_ARTEFACT:
			gmDrawStages();
			gmDrawLevels();
			gmDrawRopes();
			gmDrawButtons();
			MapArtefacts.maDraw();
			MapArtefacts.maDrawPresentation();
			break;
		}
		GlowFXHost.gfxhDraw();
		if (b_gm_editor_mode)
		{
			gmDrawEditorMode();
		}
		b_gm_left_click = false;
		b_gm_right_click = false;
		_ = SCGlobals.gLevelLoading;
	}

	public static void gmCalculate()
	{
		f_gm_randtimer += SCGlobals.ftickperframe * (CRand.frand(0.5f) + 0.5f);
		if ((double)f_gm_randtimer > 31415.9265 || (double)f_gm_randtimer < -31415.9265)
		{
			f_gm_randtimer = 0f;
		}
		if (b_gm_escape)
		{
			f_gm_escape_timer += SCGlobals.ftickperframe;
			if (f_gm_escape_timer > 10f)
			{
				b_gm_escape = false;
				f_gm_escape_timer = 0f;
				MapScreen mapScreen = (MapScreen)SuperCow.GetScreen(7);
				mapScreen.ButtonPressed(1);
			}
			return;
		}
		SuperCow.GetCursorPoint(SCGlobals.curspos.x, SCGlobals.curspos.y);
		gmProcessCommand();
		gmCalculateMap();
		gmCalculateStages();
		gmCalculateLevels();
		GlowFXHost.gfxhCalculate();
		MapArtefacts.maCalculate();
		if (b_gm_start)
		{
			if (!SCGlobals.gLevelLoading)
			{
				f_gm_start_timer += SCGlobals.ftickperframe;
			}
			if (f_gm_start_timer > 1000f)
			{
				b_gm_start = false;
				f_gm_start_timer = 0f;
			}
		}
	}

	public static void gmLoad()
	{
		Store.SetCurrentDirectory("", theAdd: false);
		Store.SetCurrentDirectory("gamemap", theAdd: true);
		p_mapBack = SCGlobals.texturehost.Load("hdtv_map_bg.jpg");
		p_mapFrameHDTV = SCGlobals.texturehost.Load("hdtv_frame.png");
		p_mapFrame = SCGlobals.texturehost.Load("sdtv_frame.png");
		p_mapHUD = SCGlobals.texturehost.Load("hud.jpg");
		p_gamemap = SCGlobals.texturehost.Load("megamap.jpg");
		p_map_spline_mark = SCGlobals.texturehost.Load("levelmark.tga");
		p_startbuttonlum = SCGlobals.texturehost.Load("mapstart.tga");
		Store.SetCurrentDirectory("", theAdd: false);
		level_tex[0] = new CSprite();
		level_tex[1] = new CSprite();
		level_tex[0].Load("diamond01.tga", 32, 8);
		level_tex[1].Load("coin2.tga", 32, 8);
		level_tex[0].SetParams(32f, 32f, 30);
		level_tex[1].SetParams(40f, 40f, 30);
		box_tex.Load("aid.tga", 32, 8);
		box_tex.SetParams(54f, 54f, 30);
		Store.SetCurrentDirectory("", theAdd: false);
		Store.SetCurrentDirectory("gamemap", theAdd: true);
		p_map_level_mark = SCGlobals.texturehost.Load("maplevelmark.tga");
		string text = "";
		for (int i = 0; i < 11; i++)
		{
			text = $"mapstagemark{i:D}.tga";
			ref texture_t reference = ref p_map_heads[i];
			reference = SCGlobals.texturehost.Load(text);
		}
		p_map_stage_mark = SCGlobals.texturehost.Load("mapstagemark.tga");
		p_map_stage_question = SCGlobals.texturehost.Load("mapstagequestion.tga");
		ref texture_t reference2 = ref p_level_glow[0];
		reference2 = SCGlobals.texturehost.Load("levelglow1.jpg");
		ref texture_t reference3 = ref p_level_glow[1];
		reference3 = SCGlobals.texturehost.Load("levelglow2.jpg");
		ref texture_t reference4 = ref p_level_glow[2];
		reference4 = SCGlobals.texturehost.Load("levelglow3.jpg");
		ref texture_t reference5 = ref p_stage_glow[0];
		reference5 = SCGlobals.texturehost.Load("boxglow1.jpg");
		ref texture_t reference6 = ref p_stage_glow[1];
		reference6 = SCGlobals.texturehost.Load("boxglow2.jpg");
		ref texture_t reference7 = ref p_stage_glow[2];
		reference7 = SCGlobals.texturehost.Load("boxglow3.jpg");
		Store.SetCurrentDirectory("", theAdd: false);
	}

	public static void gmInit()
	{
		MapArtefacts.maInit();
		Store.SetCurrentDirectory("ui", theAdd: true);
		p_backbutton = SCGlobals.texturehost.Load("button_back.tga");
		p_backbuttonlum = SCGlobals.texturehost.Load("button_back_lum.tga");
		Store.SetCurrentDirectory("", theAdd: false);
		if (SuperCow.IsHDTV)
		{
			gamemap_window.assign(-464f, 256f, 222f, -256f);
			gamemap.assign(-384f, 740f, 640f, -284f);
		}
		else
		{
			gamemap_window.assign(-344f, 254f, 102f, -254f);
			gamemap.assign(-384f, 740f, 640f, -284f);
		}
	}

	public static void gmLButtonDown()
	{
		if (b_gm_escape || b_gm_start)
		{
			return;
		}
		SuperCow.GetCursorPoint(SCGlobals.curspos.x, SCGlobals.curspos.y);
		b_gm_left_click = true;
		b_gm_left_drag = true;
		if (gm_button_ok.isPointInRect(SCGlobals.curspos) && i_gm_congratulation == 2)
		{
			b_gm_congratulation_skip = true;
		}
		if (gm_button_back.isPointInRect(SCGlobals.curspos))
		{
			gmEscape();
		}
		if (gm_button_start.isPointInRect(SCGlobals.curspos) && i_gm_numselected_stage < 10)
		{
			if (i_gm_numselected_level != SuperCow.Settings.GetAbsCurLevel(PlayerProfile.curlevelstage, PlayerProfile.curlevel) && PlayerProfile.score != 0)
			{
				SuperCow.Settings.curlevel = i_gm_numselected_level;
				b_gm_pause = true;
			}
			else if (!b_gm_start)
			{
				b_gm_start = true;
				gmStartGame();
			}
		}
	}

	public static void gmLButtonUp()
	{
		b_gm_left_drag = false;
		p_selected_stage = default(stage_config_t);
	}

	public static void gmRButtonDown()
	{
		b_gm_right_click = true;
		b_gm_right_drag = true;
	}

	public static void gmRButtonUp()
	{
		b_gm_right_drag = false;
	}

	public static void gmSound(int numsound)
	{
		switch (numsound)
		{
		case 1:
			Sound.Addsound("map1");
			break;
		case 2:
			Sound.Addsound("map2");
			break;
		case 3:
			Sound.Addsound("map3");
			break;
		case 4:
			Sound.Addsound("map4");
			break;
		case 5:
			Sound.Addsound("map5");
			break;
		case 6:
			Sound.Addsound("map6");
			break;
		case 7:
			Sound.Addsound("map7");
			break;
		case 8:
			Sound.Addsound("map8");
			break;
		case 9:
			Sound.Addsound("map9");
			break;
		}
	}

	public static void gmDrawGlow(texture_t[] tex, int numobject, vec2_t pos, vec2_t scale, float color, float speed_glow, bool rotated, vec2_t center, float angle)
	{
		dcolor_t dcolor_t2 = default(dcolor_t);
		float num = (float)Math.Sin(speed_glow * f_gm_randtimer + (float)numobject) * 0.5f + 0.5f;
		float num2 = (float)Math.Sin((double)(speed_glow * f_gm_randtimer) + Math.PI * 2.0 / 3.0 + (double)numobject) * 0.5f + 0.5f;
		float num3 = (float)Math.Sin((double)(speed_glow * f_gm_randtimer) + 4.1887902047863905 + (double)numobject) * 0.5f + 0.5f;
		vec2_t vec2_t2 = default(vec2_t);
		vec2_t2.assign((float)tex[0].width * scale.x, (float)tex[0].height * scale.y);
		_ = SuperCow.DeviceManager.GraphicsDevice;
		CowRender.SetBlend(1);
		if (rotated)
		{
			dcolor_t2.assign((byte)(255f * color), (byte)(255f * color), (byte)(255f * color), byte.MaxValue);
			CowRender.BindTexture(tex[0]);
			CowRender.DrawRotatedColorQuad2D(pos, center, f_gm_randtimer * 0.007f + angle, vec2_t2.x, vec2_t2.y, new frect_t(0f, 0f, 1f, 1f), dcolor_t2 * num, 0f);
			CowRender.BindTexture(tex[1]);
			CowRender.DrawRotatedColorQuad2D(pos, center, f_gm_randtimer * 0.003f + angle, vec2_t2.x, vec2_t2.y, new frect_t(0f, 0f, 1f, 1f), dcolor_t2 * num2, 0f);
			CowRender.BindTexture(tex[2]);
			CowRender.DrawRotatedColorQuad2D(pos, center, f_gm_randtimer * 0.005f + angle, vec2_t2.x, vec2_t2.y, new frect_t(0f, 0f, 1f, 1f), dcolor_t2 * num3, 0f);
		}
		else
		{
			dcolor_t2.assign((byte)(255f * color), (byte)(255f * color), (byte)(255f * color), byte.MaxValue);
			CowRender.BindTexture(tex[0]);
			CowRender.DrawColorQuad2D(pos, vec2_t2.x, vec2_t2.y, new frect_t(0f, 0f, 1f, 1f), dcolor_t2 * num, 0f);
			CowRender.BindTexture(tex[1]);
			CowRender.DrawColorQuad2D(pos, vec2_t2.x, vec2_t2.y, new frect_t(0f, 0f, 1f, 1f), dcolor_t2 * num2, 0f);
			CowRender.BindTexture(tex[2]);
			CowRender.DrawColorQuad2D(pos, vec2_t2.x, vec2_t2.y, new frect_t(0f, 0f, 1f, 1f), dcolor_t2 * num3, 0f);
		}
		CowRender.SetBlend(0);
	}

	public static void gmDrawCongratulation()
	{
		frect_t frect_t2 = default(frect_t);
		float num = 400f;
		float num2 = 400f;
		float num3 = 1f;
		frect_t2.assign((0f - num) / 2f, num2 / 2f, num, num2);
		matrix_t m = default(matrix_t);
		m.load_identity();
		switch (i_gm_congratulation)
		{
		case 1:
			f_timerapp += SCGlobals.ftickperframe;
			if (f_timerapp <= 500f)
			{
				num3 = 1f - f_timerapp / 500f;
				m.scale(1f - num3 * num3, 1f - num3 * num3, 1f);
				CowRender.LoadMatrix(m);
			}
			else
			{
				f_timerapp = 0f;
				i_gm_congratulation++;
			}
			break;
		case 2:
			if (b_gm_congratulation_skip)
			{
				b_gm_congratulation_skip = false;
				i_gm_congratulation++;
			}
			else
			{
				CowRender.LoadMatrixNull();
			}
			break;
		case 3:
			f_timerdisapp += SCGlobals.ftickperframe;
			if (f_timerdisapp <= 500f)
			{
				num3 = f_timerdisapp / 500f;
				m.scale(1f - num3 * num3, 1f - num3 * num3, 1f);
				CowRender.LoadMatrix(m);
			}
			else
			{
				f_timerdisapp = 0f;
				i_gm_congratulation++;
			}
			break;
		case 4:
			b_gm_congratulation = false;
			i_gm_congratulation = 4;
			break;
		}
	}

	public static void gmDrawLevelPreview()
	{
		matrix_t m = default(matrix_t);
		m.load_identity();
		float num = 0f;
		CowRender.EnableRS(7u);
		vec3_t v = new vec3_t(248f, -103f, 0f);
		if (SuperCow.IsHDTV)
		{
			v.x += 120f;
		}
		m.translate(v);
		m.scale(1f - num / 100f, 1f - num / 100f, 1f);
		CowRender.LoadMatrix(m);
		Text.SetTextColor(new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
		if (!bShowingBaloon)
		{
			string str = "Level " + (1 + SuperCow.Settings.GetStageLevel(i_gm_numselected_level)) + " (" + PlayerProfile.completedPercents[i_gm_numselected_level] + "%)";
			Text.OutTextXY(-20f, -73f, 11f, str, b_center: false);
			str = "Stage " + (1 + SuperCow.Settings.GetStage(i_gm_numselected_level));
			Text.OutTextXY(-20f, -56f, 11f, str, b_center: false);
		}
		Text.SetTextColor(new dcolor_t(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue));
		string str2 = wstrstuff.GetStr("score");
		Text.OutTextXY(-105f, -56f, 11f, str2, b_center: false);
		str2 = PlayerProfile.score.ToString();
		Text.OutTextXY(-105f, -73f, 11f, str2, b_center: false);
		CowRender.LoadMatrixNull();
	}

	public static void gmDrawButtons()
	{
		CowRender.DisableRS(7u);
		vec2_t pos = default(vec2_t);
		if (gm_button_start.isPointInRect(SCGlobals.curspos))
		{
			oppasb += SCGlobals.ftickperframe;
		}
		else
		{
			oppasb -= SCGlobals.ftickperframe;
		}
		if (oppasb < 0f)
		{
			oppasb = 0f;
		}
		if (oppasb > 255f)
		{
			oppasb = 255f;
		}
		float num = oppasb + 155f * SuperCowHelper.psin(1f, -1000001f);
		if (num < 0f)
		{
			num = 0f;
		}
		if (num > 255f)
		{
			num = 255f;
		}
		pos.assign(238f, -215f);
		if (SuperCow.IsHDTV)
		{
			pos.x += 120f;
		}
		CowRender.BindTexture(p_startbuttonlum);
		if (SuperCow.IsHDTV)
		{
			CowRender.DrawColorQuad2D(new vec2_t(pos.x - 6f, pos.y - 7f), 240f, 106f, SCGlobals.tc_normal, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)num), 0f);
		}
		else
		{
			CowRender.DrawColorQuad2D(new vec2_t(pos.x - 6f, pos.y - 7f), 240f, 106f, SCGlobals.tc_normal, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)num), 0f);
		}
		CowRender.EnableRS(7u);
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
		Text.OutTextXY(pos.x, pos.y - 12f, 28f, "START", b_center: true);
		CowRender.DisableRS(7u);
		pos.assign(-364f, -267f);
		if (SuperCow.IsHDTV)
		{
			pos.x += 120f;
		}
		gm_button_back.assign(pos, 64f, 64f);
		CowRender.BindTexture(p_backbutton);
		if (gm_button_back.isPointInRect(SCGlobals.curspos))
		{
			oppabb += SCGlobals.ftickperframe;
		}
		else
		{
			oppabb -= SCGlobals.ftickperframe;
		}
		if (oppabb < 0f)
		{
			oppabb = 0f;
		}
		if (oppabb > 255f)
		{
			oppabb = 255f;
		}
		CowRender.BindTexture(p_backbuttonlum);
		CowRender.EnableRS(7u);
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
	}

	public static void gmDrawRopes()
	{
	}

	public static void gmActivate()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < SuperCow.Settings.numlevelstages - 1; i++)
		{
			for (int j = 0; j < SuperCow.Settings.ar_numlevels_in_stage[i] + 1; j++)
			{
				ar_my_stages[num] = num2;
				if (j == SuperCow.Settings.ar_numlevels_in_stage[i])
				{
					ar_my_stages[num] = 100 + i + 1;
				}
				else
				{
					num2++;
				}
				num++;
			}
		}
		ar_my_stages[num - 1] = 47;
		ar_my_stages[num] = 70;
		bShowingBaloon = false;
		GlowFXHost.gfxhInitDischarges();
		b_gm_pause = false;
		b_gm_left_click = false;
		b_gm_left_drag = false;
		b_gm_right_click = false;
		b_gm_right_drag = false;
		p_selected_stage = default(stage_config_t);
		i_gm_numselected_level = SuperCow.Settings.GetAbsCurLevel(PlayerProfile.curlevelstage, PlayerProfile.curlevel);
		for (int k = 0; k < 100; k++)
		{
			if (ar_my_stages[k] == i_gm_numselected_level)
			{
				mySelectedLevel = k;
				break;
			}
		}
		if (!SCGlobals.b_victory)
		{
			gmSetState(1u);
			f_gm_current_position = PlayerProfile.curlevelstage;
			i_gm_numselected_stage = (int)f_gm_current_position;
			f_gm_next_position = f_gm_current_position;
			gamemap.setpos(ar_stages_config[i_gm_numselected_stage].abs_pos);
			if (i_gm_numselected_stage >= PlayerProfile.maxlevelstage)
			{
				MapArtefacts.maArtefactSetPer(i_gm_numselected_stage, (float)PlayerProfile.maxlevel / (float)SuperCow.Settings.ar_numlevels_in_stage[i_gm_numselected_stage], 0f, animate: false);
			}
			else
			{
				MapArtefacts.maArtefactSetPer(i_gm_numselected_stage, 1f, 0f, animate: false);
			}
		}
		else
		{
			MapArtefacts.maSetShowArtefact(PlayerProfile.curlevelstage, PlayerProfile.curlevel);
			if (PlayerProfile.curlevel == 0)
			{
				f_gm_current_position = PlayerProfile.curlevelstage - 1;
				i_gm_numselected_stage = (int)f_gm_current_position;
				f_gm_next_position = f_gm_current_position + 1f;
				b_gm_artefact_presentation = true;
				gmSetState(4u);
			}
			else
			{
				f_gm_current_position = PlayerProfile.curlevelstage;
				i_gm_numselected_stage = (int)f_gm_current_position;
				f_gm_next_position = f_gm_current_position;
				f_level_open_timer = 1f;
				if (i_gm_numselected_stage >= PlayerProfile.maxlevelstage)
				{
					gmSetState(1u);
					int num3 = PlayerProfile.curlevel - 1;
					if (num3 < 0)
					{
						num3 = SuperCow.Settings.ar_numlevels_in_stage[PlayerProfile.curlevelstage - 1] - 1;
					}
					vec2_t pos = ar_levels_config[num3].pos;
					Particles.AddDynamicParticle(pos, 10, 100, -1);
				}
				else
				{
					MapArtefacts.maArtefactSetPer(i_gm_numselected_stage, 1f, 0f, animate: false);
				}
			}
			gamemap.setpos(ar_stages_config[PlayerProfile.curlevelstage].abs_pos);
		}
		i_gm_numactive_stage = i_gm_numselected_stage;
		myPrevStage = i_gm_numactive_stage;
		SCGlobals.b_victory = false;
		precalcDotSpline();
		precalcStageSpline();
	}

	public static void gmDeactivate()
	{
		b_gm_left_drag = false;
		b_gm_left_click = false;
		b_gm_right_drag = false;
		b_gm_right_click = false;
	}

	public static void gmSetCommand(uint nCom)
	{
		nGameMapCommand = nCom;
	}

	public static void gmProcessCommand()
	{
		if (i_gm_numselected_level > 21 && i_gm_numselected_level < 37)
		{
			if (nGameMapCommand == 8)
			{
				nGameMapCommand = 9u;
			}
			else if (nGameMapCommand == 9)
			{
				nGameMapCommand = 8u;
			}
		}
		switch (nGameMapCommand)
		{
		case 2u:
			if (i_gm_numselected_stage < 10 && !b_gm_start && !bShowingBaloon)
			{
				b_gm_start = true;
				gmStartGame();
			}
			break;
		case 3u:
			if (!b_gm_start && !SCGlobals.gLevelLoading)
			{
				b_gm_editor_mode = false;
				b_gm_left_drag = false;
				gmEscape();
				gmSaveStageConfigs();
			}
			break;
		case 8u:
		{
			if (mySelectedLevel == 0)
			{
				return;
			}
			int num2 = ar_my_stages[mySelectedLevel - 1];
			mySelectedLevel--;
			bShowingBaloon = false;
			if (num2 > 100)
			{
				SCGlobals.curspos.x = ar_stages_config[num2 % 100].abs_pos.x;
				SCGlobals.curspos.y = ar_stages_config[num2 % 100].abs_pos.y;
				bShowingBaloon = true;
				i_gm_numactive_level = -1;
				break;
			}
			int stage3 = SuperCow.Settings.GetStage(num2);
			if (myPrevStage > stage3)
			{
				i_gm_numactive_stage--;
			}
			if (myPrevStage < stage3)
			{
				i_gm_numactive_stage++;
			}
			myPrevStage = stage3;
			i_gm_numactive_level = num2;
			gmLButtonDown();
			break;
		}
		case 9u:
		{
			if (mySelectedLevel >= 56)
			{
				return;
			}
			int num = ar_my_stages[mySelectedLevel + 1];
			int absCurLevel = SuperCow.Settings.GetAbsCurLevel(PlayerProfile.maxlevelstage, PlayerProfile.maxlevel);
			if (num % 100 > absCurLevel)
			{
				return;
			}
			if (num % 100 == 47)
			{
				bShowingBaloon = true;
				int stage = SuperCow.Settings.GetStage(num);
				if (myPrevStage < stage)
				{
					i_gm_numactive_stage++;
				}
				if (myPrevStage > stage)
				{
					i_gm_numactive_stage--;
				}
				myPrevStage = stage;
				mySelectedLevel++;
				i_gm_numactive_level = num;
				gmLButtonDown();
				return;
			}
			mySelectedLevel++;
			bShowingBaloon = false;
			if (num > 100)
			{
				SCGlobals.curspos.x = ar_stages_config[num % 100].abs_pos.x;
				SCGlobals.curspos.y = ar_stages_config[num % 100].abs_pos.y;
				bShowingBaloon = true;
				i_gm_numactive_level = -1;
				break;
			}
			int stage2 = SuperCow.Settings.GetStage(num);
			if (myPrevStage < stage2)
			{
				i_gm_numactive_stage++;
			}
			if (myPrevStage > stage2)
			{
				i_gm_numactive_stage--;
			}
			myPrevStage = stage2;
			i_gm_numactive_level = num;
			gmLButtonDown();
			break;
		}
		}
		nGameMapCommand = 0u;
	}

	public static void gmEscape()
	{
		b_gm_escape = true;
	}

	public static void gmInitStagesConfig()
	{
		for (int i = 0; i < 11; i++)
		{
			ar_stages_config[i].cursor_on_stage = false;
			ar_stages_config[i].size.assign(48f, 32f);
			ar_stages_config[i].pos.assign((float)(int)((float)i * 0.25f) * ar_stages_config[i].size.x - gamemap.sizex() * 0.5f, (float)(i % 4) * ar_stages_config[i].size.y - gamemap.sizey() * 0.5f);
			ar_stages_config[i].abs_pos = ar_stages_config[i].pos + gamemap.pos();
			ar_stages_config[i].scale = 1f;
			ar_stages_config[i].glow = 0f;
		}
		p_active_stage = default(stage_config_t);
		p_selected_stage = ar_stages_config[0];
		gmSaveStageConfigs();
	}

	public static void gmDrawStages()
	{
		gmSetClipRect(gamemap_window);
		for (int i = 0; i < 11; i++)
		{
			if (ar_stages_config[i].glow > 1f || ar_stages_config[i].glow < 0f)
			{
				return;
			}
			if (i <= PlayerProfile.maxlevelstage || SCGlobals.b_cheatzone)
			{
				if (gmGetState() == 2)
				{
					if (PlayerProfile.maxlevel == 0)
					{
						if (i == PlayerProfile.maxlevelstage - 1)
						{
							gmDrawAnimStage(i, 1, play: true);
						}
						else
						{
							gmDrawAnimStage(i, 1, play: false);
						}
					}
					else if (i == PlayerProfile.maxlevelstage)
					{
						gmDrawAnimStage(i, 1, play: true);
					}
					else
					{
						gmDrawAnimStage(i, 1, play: false);
					}
				}
				else if (i == i_gm_numactive_stage)
				{
					gmDrawAnimStage(i, 1, play: true);
				}
				else
				{
					gmDrawAnimStage(i, 1, play: false);
				}
			}
			else
			{
				gmDrawAnimStage(i, 0, play: false);
			}
		}
		gmSetClipRectNull();
		if (bShowingBaloon)
		{
			if (gm_state != gm_state_t.GMS_STAND_BY || !gamemap_window.isPointInRect(SCGlobals.curspos))
			{
				return;
			}
			for (int j = 0; j < 11; j++)
			{
				if (j <= PlayerProfile.maxlevelstage && (ar_stages_config[j].abs_pos - SCGlobals.curspos).len() < 32f)
				{
					string name = "stage" + j;
					Balloon.SetBalloon(ar_stages_config[j].abs_pos, wstrstuff.GetStr(name), 250f, b_arrow: true, -1000f, b_grow: false);
					break;
				}
			}
			if (i_gm_numselected_level == 47)
			{
				string name = "stage" + 10;
				Balloon.SetBalloon(ar_stages_config[10].abs_pos, wstrstuff.GetStr(name), 250f, b_arrow: true, -1000f, b_grow: false);
			}
		}
		else
		{
			Balloon.visible = false;
		}
	}

	public static void precalcDotSpline()
	{
		for (int i = 0; i < 160; i++)
		{
			int num = i / 16;
			float num2 = (float)i / 16f;
			vec2_t p = ((num - 1 >= 0) ? ar_stages_config[num - 1].pos : ar_stages_config[num].pos);
			vec2_t pos = ar_stages_config[num].pos;
			vec2_t pos2 = ar_stages_config[num + 1].pos;
			vec2_t p2 = ((num + 2 < 11) ? ar_stages_config[num + 2].pos : ar_stages_config[num + 1].pos);
			vec2_t vec2_t2 = gmSplineSmoothInterpolate(p, pos, pos2, p2, num2 - (float)num, 20);
			splinePos[i] = vec2_t2;
		}
	}

	public static void precalcStageSpline()
	{
		for (int i = 0; i < 10; i++)
		{
			int num = SuperCow.Settings.ar_numlevels_in_stage[i];
			for (int j = 0; j < num; j++)
			{
				int num2 = i;
				vec2_t p = ((num2 - 1 >= 0) ? ar_stages_config[num2 - 1].pos : ar_stages_config[num2].pos);
				vec2_t pos = ar_stages_config[num2].pos;
				vec2_t pos2 = ar_stages_config[num2 + 1].pos;
				vec2_t p2 = ((num2 + 2 < 11) ? ar_stages_config[num2 + 2].pos : ar_stages_config[num2 + 1].pos);
				float t = 1f / (float)(num + 1) + (float)j / (float)(num + 1);
				vec2_t vec2_t2 = gmSplineSmoothInterpolate(p, pos, pos2, p2, t, 20);
				stagePos[i, j] = vec2_t2;
			}
		}
	}

	public static void gmDrawSpline()
	{
		gmSetClipRect(gamemap_window);
		dcolor_t c = default(dcolor_t);
		CowRender.BindTexture(p_map_spline_mark);
		for (int i = 0; i < 160; i++)
		{
			CowRender.DrawColorQuad2DDynBuffer(splinePos[i] + gamemap.pos(), 4f, 4f, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		}
		CowRender.DrawDynBuffer();
		CowRender.BindTexture(p_map_level_mark);
		for (int j = 0; j < 10; j++)
		{
			int num = SuperCow.Settings.ar_numlevels_in_stage[j];
			for (int k = 0; k < num; k++)
			{
				float num2 = 1f / (float)(num + 1) + (float)k / (float)(num + 1);
				ref vec2_t reference = ref ar_levelmarks_pos[j, k];
				reference = stagePos[j, k];
				ar_levelmarks_spline_pos[j, k] = num2;
				int absCurLevel = SuperCow.Settings.GetAbsCurLevel(j, k);
				int absCurLevel2 = SuperCow.Settings.GetAbsCurLevel(PlayerProfile.maxlevelstage, PlayerProfile.maxlevel);
				if (absCurLevel <= absCurLevel2)
				{
					c.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				}
				else
				{
					c.assign(128, 128, 128, byte.MaxValue);
				}
				CowRender.DrawColorQuad2DDynBuffer(stagePos[j, k] + gamemap.pos(), 16f, 16f, new frect_t(0f, 0f, 1f, 1f), c, 0f);
			}
		}
		CowRender.DrawDynBuffer();
		gmSetClipRectNull();
	}

	public static void gmDrawLevels()
	{
		gmSetClipRect(gamemap_window);
		if (i_gm_numselected_level != 47)
		{
			for (int i = 0; i < i_num_levels_in_cur_stage; i++)
			{
				_ = ar_levels_config[i].pos;
				_ = ar_levels_config[i].size;
				int absCurLevel = SuperCow.Settings.GetAbsCurLevel((int)f_gm_current_position, i);
				int absCurLevel2 = SuperCow.Settings.GetAbsCurLevel(PlayerProfile.maxlevelstage, PlayerProfile.maxlevel);
				if (absCurLevel <= absCurLevel2 || SCGlobals.b_cheatzone)
				{
					if (i_gm_numselected_level == absCurLevel)
					{
						if (absCurLevel == absCurLevel2)
						{
							gmDrawAnimLevel(i, 2, play: true);
						}
						else if (!bShowingBaloon)
						{
							gmDrawAnimLevel(i, 1, play: true);
						}
						else
						{
							gmDrawAnimLevel(i, 1, play: false);
						}
					}
					else if (absCurLevel != absCurLevel2)
					{
						gmDrawAnimLevel(i, 1, play: false);
					}
					else
					{
						gmDrawAnimLevel(i, 2, play: false);
					}
				}
				else
				{
					gmDrawAnimLevel(i, 2, play: false);
				}
			}
		}
		if (f_level_open_timer > 0f)
		{
			_ = f_level_open_timer / 1000f;
			part += SCGlobals.ftickperframe;
			while (part > 0f)
			{
				Particles.AddDynamicParticle(ar_levels_config[PlayerProfile.curlevel - 1].pos, 32, 1, -1);
				part -= 30f;
			}
			f_level_open_timer += SCGlobals.ftickperframe;
			if (f_level_open_timer > 1000f)
			{
				Particles.AddDynamicParticle(ar_levels_config[PlayerProfile.curlevel - 1].pos, 33, 1, -1);
				f_level_open_timer = 0f;
			}
		}
		gmSetClipRectNull();
	}

	public static void gmCalculateLevels()
	{
		_ = gamemap_window.pos() - delta_stage_pos;
		i_num_levels_in_cur_stage = SuperCow.Settings.ar_numlevels_in_stage[(int)f_gm_current_position];
		if (gmGetState() == 5)
		{
			if (f_levels_radius > 0f)
			{
				f_levels_radius -= SCGlobals.ftickperframe * 0.5f;
			}
			else
			{
				gmSetState(1u);
				f_levels_radius = 0f;
			}
		}
		for (int i = 0; i < i_num_levels_in_cur_stage; i++)
		{
			int absCurLevel = SuperCow.Settings.GetAbsCurLevel((int)f_gm_next_position, i);
			int absCurLevel2 = SuperCow.Settings.GetAbsCurLevel(PlayerProfile.maxlevelstage, PlayerProfile.maxlevel);
			vec2_t vec2_t2 = default(vec2_t);
			vec2_t size = default(vec2_t);
			int num = (int)f_gm_current_position;
			vec2_t p = ((num - 1 >= 0) ? ar_stages_config[num - 1].abs_pos : ar_stages_config[num].abs_pos);
			vec2_t abs_pos = ar_stages_config[num].abs_pos;
			vec2_t abs_pos2 = ar_stages_config[num + 1].abs_pos;
			vec2_t p2 = ((num + 2 < 11) ? ar_stages_config[num + 2].abs_pos : ar_stages_config[num + 1].abs_pos);
			float num2 = (float)i / (float)(i_num_levels_in_cur_stage + 1);
			vec2_t2 = gmSplineSmoothInterpolate(p, abs_pos, abs_pos2, p2, 1f / (float)(i_num_levels_in_cur_stage + 1) + num2, 20);
			vec2_t2 += new vec2_t((float)Math.Sin((double)(f_levels_radius * 0.015f) + Math.PI * 4.0 * (double)num2), (float)Math.Cos((double)(f_levels_radius * 0.015f) + Math.PI * 4.0 * (double)num2)) * f_levels_radius;
			size.assign((float)p_level_glow[0].width + f_levels_radius * 0.3125f, (float)p_level_glow[0].height + f_levels_radius * (5f / 32f));
			ar_levels_config[i].pos = vec2_t2;
			ar_levels_config[i].size = size;
			if (gmCheckForCursOnLevel(i, vec2_t2, new vec2_t(36f, 36f)) && b_gm_left_click && (absCurLevel <= absCurLevel2 || SCGlobals.b_cheatzone) && i_gm_numselected_level != absCurLevel)
			{
				i_gm_numselected_level = absCurLevel;
			}
		}
	}

	public static void gmCalculateMap()
	{
		if (b_gm_editor_mode)
		{
			return;
		}
		vec2_t vec2_t2 = default(vec2_t);
		vec2_t vec2_t3 = default(vec2_t);
		vec2_t vec2_t4 = default(vec2_t);
		vec2_t vec2_t5 = default(vec2_t);
		_ = gamemap_window.pos() - delta_stage_pos;
		if (gmGetState() == 4)
		{
			if (f_gm_next_position > f_gm_current_position)
			{
				if (Math.Abs(f_gm_next_position - f_gm_current_position) >= 1f)
				{
					f_gm_current_position += SCGlobals.ftickperframe * 0.002f;
				}
				else
				{
					f_gm_current_position += SCGlobals.ftickperframe * 0.001f;
				}
			}
			else if (f_gm_next_position < f_gm_current_position - SCGlobals.ftickperframe * 0.001f)
			{
				if (Math.Abs(f_gm_next_position - f_gm_current_position) >= 1f)
				{
					f_gm_current_position -= SCGlobals.ftickperframe * 0.002f;
				}
				else
				{
					f_gm_current_position -= SCGlobals.ftickperframe * 0.001f;
				}
			}
			else
			{
				gmSetState(5u);
				f_gm_current_position = f_gm_next_position;
				i_gm_numselected_stage = (int)f_gm_current_position;
			}
		}
		int num = (int)(f_gm_current_position + 0.5f);
		vec2_t2 = ((num - 1 >= 0) ? new vec2_t(-136f - ar_stages_config[num - 1].pos.x, 0f - ar_stages_config[num - 1].pos.y) : new vec2_t(-136f - ar_stages_config[num].pos.x, 0f - ar_stages_config[num].pos.y));
		vec2_t3 = new vec2_t(-136f - ar_stages_config[num].pos.x, 0f - ar_stages_config[num].pos.y);
		vec2_t4 = new vec2_t(-136f - ar_stages_config[num + 1].pos.x, 0f - ar_stages_config[num + 1].pos.y);
		vec2_t5 = ((num + 2 < 11) ? new vec2_t(-136f - ar_stages_config[num + 2].pos.x, 0f - ar_stages_config[num + 2].pos.y) : new vec2_t(-136f - ar_stages_config[num + 1].pos.x, 0f - ar_stages_config[num + 1].pos.y));
		vec2_t vec2_t6 = gmSplineSmoothInterpolate(vec2_t2, vec2_t3, vec2_t4, vec2_t5, f_gm_current_position - (float)num + 0.5f, 20);
		num = (int)f_gm_current_position;
		vec2_t2 = ((num - 1 >= 0) ? new vec2_t(-136f - ar_stages_config[num - 1].pos.x, 0f - ar_stages_config[num - 1].pos.y) : new vec2_t(-136f - ar_stages_config[num].pos.x, 0f - ar_stages_config[num].pos.y));
		vec2_t3 = new vec2_t(-136f - ar_stages_config[num].pos.x, 0f - ar_stages_config[num].pos.y);
		vec2_t4 = new vec2_t(-136f - ar_stages_config[num + 1].pos.x, 0f - ar_stages_config[num + 1].pos.y);
		vec2_t5 = ((num + 2 < 11) ? new vec2_t(-136f - ar_stages_config[num + 2].pos.x, 0f - ar_stages_config[num + 2].pos.y) : new vec2_t(-136f - ar_stages_config[num + 1].pos.x, 0f - ar_stages_config[num + 1].pos.y));
		dot_spline_pos = gmSplineSmoothInterpolate(vec2_t2, vec2_t3, vec2_t4, vec2_t5, f_gm_current_position - (float)num, 20) - vec2_t6;
		delta_stage_pos = vec2_t6;
		vec2_t vec2_t7 = default(vec2_t);
		vec2_t vec2_t8 = default(vec2_t);
		if (SuperCow.IsHDTV)
		{
			vec2_t7.assign(gamemap_window.p0.x + 200f, gamemap_window.p0.y);
			vec2_t8.assign(gamemap_window.p1.x - 200f, gamemap_window.p1.y);
		}
		else
		{
			vec2_t7.assign(gamemap_window.p0.x, gamemap_window.p0.y);
			vec2_t8.assign(gamemap_window.p1.x, gamemap_window.p1.y);
		}
		if ((float)(int)vec2_t6.x < vec2_t7.x)
		{
			vec2_t6.x = vec2_t7.x;
		}
		if ((float)(int)vec2_t6.x > vec2_t8.x)
		{
			vec2_t6.x = vec2_t8.x;
		}
		if ((float)(int)vec2_t6.y < vec2_t7.y - gamemap.sizex() * 0.5f)
		{
			vec2_t6.y = vec2_t7.y - gamemap.sizex() * 0.5f;
		}
		if ((float)(int)vec2_t6.y > vec2_t8.y + gamemap.sizex() * 0.5f)
		{
			vec2_t6.y = vec2_t8.y + gamemap.sizex() * 0.5f;
		}
		delta_stage_pos -= vec2_t6;
		gamemap.setpos((int)vec2_t6.x, (int)vec2_t6.y);
	}

	public static void gmCalculateStages()
	{
		if (b_gm_right_click && b_gm_editor_mode)
		{
			click_rpos = SCGlobals.curspos;
			delta_rpos = gamemap.pos() - click_rpos;
			b_gm_right_click = false;
		}
		if (b_gm_right_drag && b_gm_editor_mode)
		{
			gamemap.setpos(SCGlobals.curspos + delta_rpos);
		}
		i_gm_numactive_stage = gmCheckForCursOnStage();
		if (gmGetState() == 1)
		{
			MapArtefacts.maSetShowArtefact(i_gm_numselected_stage, SuperCow.Settings.GetStageLevel(i_gm_numselected_level));
		}
		if (ar_stages_config[i_gm_numselected_stage].glow < 1f - SCGlobals.ftickperframe * 0.002f)
		{
			ar_stages_config[i_gm_numselected_stage].glow += SCGlobals.ftickperframe * 0.002f;
		}
		else
		{
			ar_stages_config[i_gm_numselected_stage].glow = 1f;
		}
		for (int i = 0; i < 11; i++)
		{
			ar_stages_config[i].abs_pos = ar_stages_config[i].pos + gamemap.pos();
			if (i_gm_numactive_stage == i)
			{
				if (i <= PlayerProfile.maxlevelstage || SCGlobals.b_cheatzone)
				{
					if (ar_stages_config[i].glow < 1f - SCGlobals.ftickperframe * 0.002f)
					{
						ar_stages_config[i].glow += SCGlobals.ftickperframe * 0.002f;
					}
					else
					{
						ar_stages_config[i].glow = 1f;
					}
				}
				if (b_gm_left_click)
				{
					if ((i <= PlayerProfile.maxlevelstage || SCGlobals.b_cheatzone) && i != i_gm_numselected_stage)
					{
						f_gm_next_position = i;
						gmSetState(4u);
						i_gm_numselected_stage = i;
						if (i == PlayerProfile.maxlevelstage)
						{
							MapArtefacts.maArtefactSetPer(i_gm_numselected_stage, (float)PlayerProfile.maxlevel / (float)SuperCow.Settings.ar_numlevels_in_stage[i_gm_numselected_stage], 0f, animate: false);
						}
						else
						{
							MapArtefacts.maArtefactSetPer(i_gm_numselected_stage, 1f, 0f, animate: false);
						}
					}
					if (f_gm_next_position != f_gm_current_position)
					{
						f_gm_delta_position = Math.Abs(f_gm_next_position - f_gm_current_position);
						if ((int)f_gm_next_position == PlayerProfile.maxlevelstage)
						{
							i_gm_numselected_level = SuperCow.Settings.GetAbsCurLevel(PlayerProfile.maxlevelstage, PlayerProfile.maxlevel);
						}
						else
						{
							i_gm_numselected_level = SuperCow.Settings.GetLevelsBeforeStage((int)f_gm_next_position);
						}
					}
					else
					{
						f_gm_delta_position = 1f;
					}
					click_lpos = SCGlobals.curspos;
					if (p_selected_stage.name != null)
					{
						delta_lpos = p_selected_stage.pos - click_lpos;
					}
				}
			}
			else if (ar_stages_config[i].glow > SCGlobals.ftickperframe * 0.002f)
			{
				ar_stages_config[i].glow -= SCGlobals.ftickperframe * 0.002f;
			}
			else
			{
				ar_stages_config[i].glow = 0f;
			}
			if (i_gm_numactive_stage != i || i_gm_numactive_stage != i)
			{
				if (ar_stages_config[i].glow > SCGlobals.ftickperframe * 0.002f)
				{
					ar_stages_config[i].glow -= SCGlobals.ftickperframe * 0.002f;
				}
				else
				{
					ar_stages_config[i].glow = 0f;
				}
			}
			if (b_gm_left_drag && b_gm_editor_mode && p_selected_stage.name != null)
			{
				p_selected_stage.pos = SCGlobals.curspos + delta_lpos;
			}
		}
	}

	public static int gmCheckForCursOnStage()
	{
		return i_gm_numactive_stage;
	}

	public static bool gmCheckForCursOnLevel(int num, vec2_t pos, vec2_t size)
	{
		if (i_gm_numactive_level == SuperCow.Settings.GetAbsCurLevel(i_gm_numselected_stage, num))
		{
			return true;
		}
		return false;
	}

	public static void gmDrawEditorMode()
	{
	}

	public static void gmLoadStagesConfig()
	{
		Store.SetCurrentDirectory("gamemap", theAdd: false);
		int len = 0;
		byte[] array = Store.storeLoadFileArray("stages.cfg", ref len, b_errorexit: false);
		if (array.Length == 0)
		{
			gmInitStagesConfig();
		}
		else
		{
			CBuffer cBuffer = new CBuffer();
			cBuffer.SetDataBuffer(array);
			for (int i = 0; i < 11; i++)
			{
				stage_config_t stage_config_t2 = ar_stages_config[i];
				stage_config_t2.pos.x = cBuffer.ReadFloat();
				stage_config_t2.pos.y = cBuffer.ReadFloat();
				stage_config_t2.abs_pos.x = cBuffer.ReadFloat();
				stage_config_t2.abs_pos.y = cBuffer.ReadFloat();
				stage_config_t2.size.x = cBuffer.ReadFloat();
				stage_config_t2.size.y = cBuffer.ReadFloat();
				stage_config_t2.scale = cBuffer.ReadFloat();
				stage_config_t2.glow = cBuffer.ReadFloat();
				stage_config_t2.name = cBuffer.ReadString(64);
				stage_config_t2.cursor_on_stage = cBuffer.ReadBool();
				cBuffer.SkipData(3);
				stage_config_t2.frame = (int)cBuffer.ReadLong();
				stage_config_t2.timer = cBuffer.ReadFloat();
				stage_config_t2.play = cBuffer.ReadBool();
				cBuffer.SkipData(3);
				ar_stages_config[i] = stage_config_t2;
			}
		}
		p_active_stage = default(stage_config_t);
		p_selected_stage = ar_stages_config[0];
		Store.SetCurrentDirectory("", theAdd: false);
	}

	public static void gmSaveStageConfigs()
	{
		Store.SetCurrentDirectory("", theAdd: false);
		Store.SetCurrentDirectory("gamemap", theAdd: true);
		Store.SetCurrentDirectory("", theAdd: false);
	}

	public static void gmCreateMaskTex(vec2_t pos, vec2_t size, texture_t tex)
	{
	}

	public static void gmGetMaskState()
	{
	}

	public static vec2_t gmSplineSmoothInterpolate(vec2_t p0, vec2_t p1, vec2_t p2, vec2_t p3, float t, int precision)
	{
		vec2_t[] array = new vec2_t[101];
		vec2_t result = default(vec2_t);
		float[] array2 = new float[101];
		Array.Clear(array2, 0, array2.Length);
		if (precision > 100)
		{
			precision = 100;
		}
		for (int i = 0; i <= precision; i++)
		{
			ref vec2_t reference = ref array[i];
			reference = gmSplineInterpolate(p0, p1, p2, p3, (float)i / (float)precision);
		}
		float num = 0f;
		for (int i = 1; i <= precision; i++)
		{
			array2[i] = (array[i] - array[i - 1]).len();
			num = (array2[i] = num + array2[i]);
		}
		float num2 = num * t;
		for (int i = 1; i <= precision; i++)
		{
			if (array2[i] >= num2)
			{
				result = array[i] - array[i - 1];
				result.normalize();
				result *= num2 - array2[i - 1];
				return array[i - 1] + result;
			}
		}
		return result;
	}

	public static vec2_t gmSplineInterpolate(vec2_t p0, vec2_t p1, vec2_t p2, vec2_t p3, float t)
	{
		vec2_t result = default(vec2_t);
		float num = p3.x - p2.x - (p0.x - p1.x);
		float num2 = p0.x - p1.x - num;
		float num3 = p2.x - p0.x;
		float x = p1.x;
		float num4 = p3.y - p2.y - (p0.y - p1.y);
		float num5 = p0.y - p1.y - num4;
		float num6 = p2.y - p0.y;
		float y = p1.y;
		result.x = num * t * t * t + num2 * t * t + num3 * t + x;
		result.y = num4 * t * t * t + num5 * t * t + num6 * t + y;
		return result;
	}

	public static void gmSetState(uint state)
	{
		switch (state)
		{
		case 1u:
			gm_state = gm_state_t.GMS_STAND_BY;
			break;
		case 2u:
			MapArtefacts.maInitOpeningMask();
			gm_state = gm_state_t.GMS_OPEN_MASK;
			break;
		case 3u:
			MapArtefacts.maInitArtefactPresentation(ar_stages_config[(int)f_gm_current_position].abs_pos);
			gm_state = gm_state_t.GMS_PRESENTATION_ARTEFACT;
			b_gm_pause = true;
			break;
		case 4u:
			gm_state = gm_state_t.GMS_CHANGE_STAGE;
			break;
		case 5u:
			f_levels_radius = 256f;
			gm_state = gm_state_t.GMS_APPEARANCE_LEVELS;
			break;
		case 0u:
			break;
		}
	}

	public static uint gmGetState()
	{
		return (uint)gm_state;
	}

	public static int gmGetCurLevel()
	{
		return i_gm_numactive_level;
	}

	public static void gmDrawAnimLevel(int num, int type, bool play)
	{
		level_config_t level_config_t2 = ar_levels_config[num];
		level_config_t2.play = play;
		dcolor_t c = new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		if (!level_config_t2.play && level_config_t2.frame == 0)
		{
			level_config_t2.timer = 0f;
		}
		if (level_config_t2.play || level_config_t2.frame != 0)
		{
			level_config_t2.timer += SCGlobals.ftickperframe;
			level_config_t2.frame = (int)(level_config_t2.timer / 30f);
			level_config_t2.frame %= 32;
		}
		float num2 = 1f - (float)level_config_t2.frame / 31f;
		float num3 = 1f - (float)Math.Cos((double)(1f - num2) * Math.PI * 4.0);
		gmDrawGlow(p_level_glow, num, level_config_t2.pos, new vec2_t(0.76f - num3 * 0.08f, 0.76f - num3 * 0.08f), 1f, 0.005f, rotated: false, new vec2_t(0f, 0f), 0f);
		if (type == 2)
		{
			c.assign(128, 128, 128, byte.MaxValue);
		}
		else
		{
			c.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}
		CowRender.BindTexture(p_map_level_mark);
		CowRender.DrawColorQuad2D(level_config_t2.pos, 32f - 2f * num3, 32f - 2f * num3, new frect_t(0f, 0f, 1f, 1f), c, 0f);
		ar_levels_config[num] = level_config_t2;
	}

	public static void gmDrawAnimStage(int num, int type, bool play)
	{
		stage_config_t stage_config_t2 = ar_stages_config[num];
		stage_config_t2.play = play;
		new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		if (!stage_config_t2.play && stage_config_t2.frame > 0)
		{
			stage_config_t2.timer -= SCGlobals.ftickperframe;
			if (stage_config_t2.timer > 1000f)
			{
				stage_config_t2.frame = (int)((stage_config_t2.timer - 1000f) / 45f);
				stage_config_t2.frame %= 16;
			}
		}
		if (stage_config_t2.play && stage_config_t2.frame < 15)
		{
			stage_config_t2.timer += SCGlobals.ftickperframe;
			if (stage_config_t2.timer > 1000f)
			{
				stage_config_t2.frame = (int)((stage_config_t2.timer - 1000f) / 45f);
				stage_config_t2.frame %= 16;
			}
		}
		switch (type)
		{
		case 0:
			gmDrawGlow(p_level_glow, num, stage_config_t2.abs_pos, new vec2_t(1.5f, 1.5f), 0.75f, 0.005f, rotated: false, new vec2_t(0f, 0f), 0f);
			CowRender.BindTexture(p_map_stage_question);
			CowRender.DrawColorQuad2D(stage_config_t2.abs_pos, p_map_stage_question.width, p_map_stage_question.height, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
			break;
		case 1:
			gmDrawGlow(p_stage_glow, num, stage_config_t2.abs_pos, new vec2_t(1.7f, 1.7f), 0.75f, 0.005f, rotated: false, new vec2_t(0f, 0f), 0f);
			CowRender.BindTexture(p_map_heads[num]);
			CowRender.DrawColorQuad2D(stage_config_t2.abs_pos, 85f, 85f, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
			break;
		case 2:
			CowRender.BindTexture(p_map_heads[num]);
			CowRender.DrawColorQuad2D(stage_config_t2.abs_pos, 80f, 80f, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
			break;
		}
		ar_stages_config[num] = stage_config_t2;
	}

	public static void gmStartGame()
	{
		b_gm_start = true;
		SCGlobals.gLevelLoading = true;
		SuperCow.Settings.SetPlayerCurLevel(i_gm_numselected_level);
		SuperCow.StartLevelLoadingThread();
	}

	public static void gmStartGameQuiet()
	{
		b_gm_start = true;
		SCGlobals.gLevelLoading = true;
	}

	public static bool gmSelectLevel(int theLevel)
	{
		i_gm_numselected_level = theLevel;
		return true;
	}
}
