using System;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public static class TrophyRoom
{
	public struct trophiesconf_t
	{
		public vec2_t size;

		public vec2_t pos;

		public int group;

		public float scale;

		public string name;

		public bool cursontrophy;
	}

	public struct popuptip_t
	{
		public bool b;

		public int num;

		public bool button;

		public bool arrow;

		public vec2_t pos;

		public float timer;

		public string str;
	}

	public struct trophies_indexes_t
	{
		public uint trophy_index;
	}

	public static int trophyescapetimer = 0;

	public static WindowRect winRect = new WindowRect();

	public static uint nTrophyRoomCommand;

	public static int TROPHY_MENU_APP_TIME = 700;

	public static texture_t p_trophyback;

	public static trophiesconf_t[] ar_trophiesconf = new trophiesconf_t[13];

	public static trophies_indexes_t[] ar_trophiesindexes = new trophies_indexes_t[13];

	public static texture_t[] ar_trophiestex = new texture_t[13];

	public static texture_t[] ar_trophiestexlum = new texture_t[13];

	public static frect_t okbuttonrect = new frect_t(0f, 0f, 0f, 0f);

	public static trophiesconf_t activetrophy;

	public static trophiesconf_t selectedtrophy;

	public static frect_t buttonback;

	public static popuptip_t trophytip;

	public static vec2_t CursPos;

	public static vec2_t ClickPos;

	public static vec2_t DeltaPos;

	public static bool b_editormode = false;

	public static bool b_click = false;

	public static bool b_drag = false;

	public static bool b_presentation = false;

	public static bool b_overpresentation = false;

	public static bool b_ingame = false;

	public static int i_numcurplayer;

	public static int TEMPcounttrophies = -1;

	public static int i_gameback = 0;

	public static int i_numselectedtrophytex = 0;

	public static float f_blinkingtimer = 0f;

	public static float f_trophyroomtimer = 0f;

	public static float f_trophyaddtimer = 0f;

	public static string c_playername;

	public static int NUM_TROPHIES = 13;

	public static int partimer = 0;

	public static void SetTrophyRoomCommand(uint nCom)
	{
		nTrophyRoomCommand = nCom;
	}

	public static void ProcessTrophyRoomCommand(uint nCom)
	{
		switch (nCom)
		{
		case 8u:
			ar_trophiesconf[ar_trophiesindexes[i_numselectedtrophytex--].trophy_index].cursontrophy = false;
			if (i_numselectedtrophytex < 0)
			{
				i_numselectedtrophytex = 12;
			}
			ar_trophiesconf[ar_trophiesindexes[i_numselectedtrophytex].trophy_index].cursontrophy = true;
			break;
		case 9u:
			ar_trophiesconf[ar_trophiesindexes[i_numselectedtrophytex++].trophy_index].cursontrophy = false;
			i_numselectedtrophytex %= 13;
			ar_trophiesconf[ar_trophiesindexes[i_numselectedtrophytex].trophy_index].cursontrophy = true;
			break;
		}
	}

	public static void CalculateTrophyRoom()
	{
		CalculateTrophies();
		ProcessTrophyRoomCommand(666u);
		CalculateTrophyTips(b_click);
		f_trophyroomtimer += SCGlobals.ftickperframe;
	}

	public static void CalculateTrophies()
	{
		bool b = false;
		for (int i = 0; i < NUM_TROPHIES; i++)
		{
			if (CheckForCursOnTrophy(i))
			{
				if (!b_editormode && !b_presentation)
				{
					AddTrophyTip(i, 300f, button: false, arrow: true);
					b = true;
				}
			}
			else
			{
				ar_trophiesconf[i].cursontrophy = false;
			}
		}
		trophytip.b = b;
	}

	public static void InitTrophiesTex()
	{
		int len = 0;
		Store.SetCurrentDirectory("", theAdd: false);
		p_trophyback = SCGlobals.texturehost.LoadTextureWithoutScale("trophyback.jpg", 800, 600);
		byte[] array = Store.storeLoadFileArray("trophy.txt", ref len, b_errorexit: false);
		Store.SetCurrentDirectory("trophies", theAdd: false);
		string script = Encoding.ASCII.GetString(array, 0, array.Length);
		for (int i = 0; i < NUM_TROPHIES; i++)
		{
			string varname = $"trophyname{i + 1}=";
			SuperCowHelper.get_script_string(ref ar_trophiesconf[i].name, script, varname, 0);
			string name = $"tr{i + 1}.tga";
			ref texture_t reference = ref ar_trophiestex[i];
			reference = SCGlobals.texturehost.Load(name);
			string name2 = $"tr{i + 1}.jpg";
			ref texture_t reference2 = ref ar_trophiestexlum[i];
			reference2 = SCGlobals.texturehost.Load(name2);
			ar_trophiesconf[i].size = new vec2_t(ar_trophiestex[i].width, ar_trophiestex[i].height);
		}
		Store.SetCurrentDirectory("", theAdd: false);
	}

	public static void InitTrophiesConf()
	{
		for (int i = 0; i < NUM_TROPHIES; i++)
		{
			ar_trophiesconf[i].pos.assign(760f / (float)NUM_TROPHIES * (float)i - 380f, 280f - 560f / (float)NUM_TROPHIES * (float)i);
			ar_trophiesconf[i].size = new vec2_t(ar_trophiestex[i].width, ar_trophiestex[i].height);
			ar_trophiesconf[i].scale = 1f;
			ar_trophiesconf[i].group = 0;
			ar_trophiesconf[i].cursontrophy = false;
		}
		ar_trophiesconf[0].size.assign(128f, 64f);
		ar_trophiesconf[0].pos.assign(244.24f, 50.94f);
		ar_trophiesconf[1].size.assign(128f, 128f);
		ar_trophiesconf[1].pos.assign(-219.51f, 227.8f);
		ar_trophiesconf[2].size.assign(64f, 128f);
		ar_trophiesconf[2].pos.assign(-68.12f, -153.78f);
		ar_trophiesconf[3].size.assign(128f, 128f);
		ar_trophiesconf[3].pos.assign(-87.44f, 105.16f);
		ar_trophiesconf[4].size.assign(128f, 128f);
		ar_trophiesconf[4].pos.assign(-227.97f, 87.42f);
		ar_trophiesconf[5].size.assign(64f, 128f);
		ar_trophiesconf[5].pos.assign(19.37f, 99.07f);
		ar_trophiesconf[6].size.assign(128f, 128f);
		ar_trophiesconf[6].pos.assign(190.97f, 203.97f);
		ar_trophiesconf[7].size.assign(128f, 256f);
		ar_trophiesconf[7].pos.assign(98.92f, -121.87f);
		ar_trophiesconf[8].size.assign(128f, 128f);
		ar_trophiesconf[8].pos.assign(-114.32f, 246.53f);
		ar_trophiesconf[9].size.assign(128f, 128f);
		ar_trophiesconf[9].pos.assign(123.93f, 75.75f);
		ar_trophiesconf[10].size.assign(64f, 64f);
		ar_trophiesconf[10].pos.assign(41.98f, 238.41f);
		ar_trophiesconf[11].size.assign(128f, 128f);
		ar_trophiesconf[11].pos.assign(342.87f, 230.81f);
		ar_trophiesconf[12].size.assign(256f, 256f);
		ar_trophiesconf[12].pos.assign(-199.67f, -132.53f);
		i_numselectedtrophytex = 0;
		ar_trophiesindexes[0].trophy_index = 1u;
		ar_trophiesindexes[1].trophy_index = 8u;
		ar_trophiesindexes[2].trophy_index = 10u;
		ar_trophiesindexes[3].trophy_index = 6u;
		ar_trophiesindexes[4].trophy_index = 11u;
		ar_trophiesindexes[5].trophy_index = 4u;
		ar_trophiesindexes[6].trophy_index = 3u;
		ar_trophiesindexes[7].trophy_index = 5u;
		ar_trophiesindexes[8].trophy_index = 9u;
		ar_trophiesindexes[9].trophy_index = 0u;
		ar_trophiesindexes[10].trophy_index = 12u;
		ar_trophiesindexes[11].trophy_index = 2u;
		ar_trophiesindexes[12].trophy_index = 7u;
		ar_trophiesconf[ar_trophiesindexes[0].trophy_index].cursontrophy = true;
	}

	public static void InitTrophyRoom(int numback, string playername, bool ingame)
	{
		float x = 0f;
		float y = 0f;
		SuperCow.GetCursorPoint(x, y);
		CursPos.x = x;
		CursPos.y = y;
		b_ingame = ingame;
		b_click = false;
		b_drag = false;
		trophyescapetimer = 0;
		ar_trophiesindexes[0].trophy_index = 1u;
		ar_trophiesindexes[1].trophy_index = 8u;
		ar_trophiesindexes[2].trophy_index = 10u;
		ar_trophiesindexes[3].trophy_index = 6u;
		ar_trophiesindexes[4].trophy_index = 11u;
		ar_trophiesindexes[5].trophy_index = 4u;
		ar_trophiesindexes[6].trophy_index = 3u;
		ar_trophiesindexes[7].trophy_index = 5u;
		ar_trophiesindexes[8].trophy_index = 9u;
		ar_trophiesindexes[9].trophy_index = 0u;
		ar_trophiesindexes[10].trophy_index = 12u;
		ar_trophiesindexes[11].trophy_index = 2u;
		ar_trophiesindexes[12].trophy_index = 7u;
	}

	public static void InitTrophies()
	{
		SuperCow.Settings.ClearPlayerTrophies();
	}

	public static bool AddTrophy(int num, string playername)
	{
		if (!SuperCow.Settings.IsTrophyActive(num))
		{
			SuperCow.Settings.AddTrophy(num);
			ar_trophiesconf[ar_trophiesindexes[i_numselectedtrophytex].trophy_index].cursontrophy = false;
			ar_trophiesconf[num].cursontrophy = true;
			for (int i = 0; i < 13; i++)
			{
				if (ar_trophiesindexes[i].trophy_index == num)
				{
					i_numselectedtrophytex = i;
				}
			}
			f_trophyaddtimer = 1f;
			return true;
		}
		return false;
	}

	public static void AddTrophyTip(int num, float delay, bool button, bool arrow)
	{
		string name = $"trophytip{num + 1}";
		string name2 = $"trophyname{num + 1}";
		if (SuperCow.Settings.GetTrophy(num).active)
		{
			trophytip.str = wstrstuff.GetStr(name2) + "\r\n(" + wstrstuff.GetStrDelta("trophysubname1", num) + ")\r\n" + wstrstuff.GetStr("havegottrophy") + " " + wstrstuff.GetStr(name);
		}
		else
		{
			trophytip.str = wstrstuff.GetStr("willgettrophy") + " " + wstrstuff.GetStr(name);
		}
		trophytip.button = button;
		trophytip.arrow = arrow;
		if (!trophytip.b || trophytip.num != num)
		{
			trophytip.timer = delay;
		}
		trophytip.num = num;
		trophytip.b = true;
		vec2_t pos = ar_trophiesconf[num].pos;
		pos.normalize();
		if (ar_trophiesconf[num].pos.x > ar_trophiesconf[num].pos.y)
		{
			pos *= ar_trophiesconf[num].size.x * 0.5f;
		}
		else
		{
			pos *= ar_trophiesconf[num].size.y * 0.5f;
		}
		trophytip.pos = ar_trophiesconf[num].pos - pos;
	}

	public static string GetTrophyName(int num)
	{
		string name = $"trophyname{num}";
		return wstrstuff.GetStr(name);
	}

	public static void CalculateTrophyTips(bool b_click)
	{
		if (!trophytip.b || f_trophyaddtimer > 0f)
		{
			return;
		}
		if (trophytip.timer > 0f)
		{
			trophytip.timer -= SCGlobals.ftickperframe;
			Balloon.visible = false;
			return;
		}
		if (!trophytip.button)
		{
			Balloon.SetBalloon(trophytip.pos, trophytip.str, 300f, trophytip.arrow, -1000f, b_grow: true);
		}
		else
		{
			Balloon.SetBalloonTipWithButton(trophytip.pos, trophytip.str, 300f, b_arrow: false, -1000f, b_grow: true);
		}
		if (trophytip.timer > -250f)
		{
			trophytip.timer -= SCGlobals.ftickperframe;
		}
		else if (b_click && Balloon.balloon_button_rect.isPointInRect(CursPos))
		{
			trophytip.b = false;
		}
	}

	public static void DrawTrophyRoom()
	{
		buttonback.assign(new vec2_t(318f, -85f), 120f, 30f);
		CowRender.DisableRS(7u);
		CowRender.BindTexture(p_trophyback);
		CowRender.DrawColorQuad2D(p_trophyback.mImage, dcolor_t.ColorWhite, 10f);
		CowRender.EnableRS(7u);
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		Text.OutTextXY(310f, -85f, 15f, "    Back", b_center: true);
		CowRender.BindTexture(SCGlobals.p_b);
		CowRender.DrawColorQuad2D(SCGlobals.p_b.mImage, 290f, -85f, 21f, 21f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 10f);
		DrawTrophiesTrophyRoom();
		if (f_trophyaddtimer > 0f)
		{
			DrawAddTrophInTrophyRoom();
		}
		CowRender.DisableRS(7u);
		if (trophyescapetimer > 0)
		{
			trophyescapetimer += (int)SCGlobals.tickperframe;
			if (trophyescapetimer > 800)
			{
				trophyescapetimer = 0;
				TrophyRoomEscape();
			}
		}
	}

	public static void DrawTrophiesTrophyRoom()
	{
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		f_blinkingtimer += SCGlobals.ftickperframe;
		partimer = 0;
		dcolor_t colorWhite = dcolor_t.ColorWhite;
		for (int i = 0; i < NUM_TROPHIES; i++)
		{
			float num = ((!ar_trophiesconf[i].cursontrophy || f_trophyaddtimer != 0f) ? (205f + 50f * (float)Math.Sin(SCGlobals.global_timer * 0.005f)) : (205f + 50f * (float)Math.Sin(SCGlobals.global_timer * 0.015f)));
			colorWhite = dcolor_t.ColorWhite;
			if (SuperCow.Settings.GetTrophy(i).active && !SuperCow.Settings.GetTrophy(i).state)
			{
				float num2 = 1f;
				colorWhite.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(num2 * 255f));
				CowRender.BindTexture(ar_trophiestex[i]);
				CowRender.DrawColorQuad2D(ar_trophiesconf[i].pos, ar_trophiesconf[i].size.x * ar_trophiesconf[i].scale, ar_trophiesconf[i].size.y * ar_trophiesconf[i].scale, new frect_t(0f, 0f, 1f, 1f), colorWhite, 0f);
				CowRender.SetBlend(1);
				CowRender.BindTexture(ar_trophiestexlum[i]);
				CowRender.DrawColorQuad2D(ar_trophiesconf[i].pos, ar_trophiesconf[i].size.x * ar_trophiesconf[i].scale, ar_trophiesconf[i].size.y * ar_trophiesconf[i].scale, new frect_t(0f, 0f, 1f, 1f), dcolor_t.ColorWhite, 0f);
				CowRender.SetBlend(0);
				continue;
			}
			if (b_editormode)
			{
				CowRender.BindTexture(ar_trophiestex[i]);
				CowRender.DrawColorQuad2D(ar_trophiesconf[i].pos, ar_trophiesconf[i].size.x * ar_trophiesconf[i].scale, ar_trophiesconf[i].size.y * ar_trophiesconf[i].scale, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
			}
			graphicsDevice.RenderState.SourceBlend = (Blend)3;
			graphicsDevice.RenderState.DestinationBlend = (Blend)2;
			CowRender.BindTexture(ar_trophiestexlum[i]);
			CowRender.DrawColorQuad2D(ar_trophiesconf[i].pos, ar_trophiesconf[i].size.x * ar_trophiesconf[i].scale, ar_trophiesconf[i].size.y * ar_trophiesconf[i].scale, new frect_t(0f, 0f, 1f, 1f), new dcolor_t((byte)num, (byte)(127 + (byte)(num * 0.5f)), byte.MaxValue, (byte)num), 0f);
			graphicsDevice.RenderState.SourceBlend = (Blend)5;
			graphicsDevice.RenderState.DestinationBlend = (Blend)6;
		}
	}

	public static void DrawAddTrophInTrophyRoom()
	{
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		float num = 600f;
		float num2 = 290f;
		matrix_t m = default(matrix_t);
		int num3 = CheckForStateTrophies();
		if (num3 >= 0)
		{
			float num4 = Text.PrepareWindowText(280f, wstrstuff.GetStrDelta("trophytip1", num3), 12f);
			num2 += num4;
			frect_t rect = default(frect_t);
			rect.x0 = (0f - num) / 2f;
			rect.x1 = num;
			rect.y0 = num2 / 2f;
			rect.y1 = num2;
			b_presentation = true;
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			m.load_identity();
			if (f_trophyaddtimer > 0f && f_trophyaddtimer <= (float)TROPHY_MENU_APP_TIME)
			{
				float num5 = 1f - f_trophyaddtimer / (float)TROPHY_MENU_APP_TIME;
				m.scale(1f - num5 * num5, 1f - num5 * num5, 1f);
				f_trophyaddtimer += SCGlobals.ftickperframe;
			}
			else if (f_trophyaddtimer >= (float)(TROPHY_MENU_APP_TIME * 2) && f_trophyaddtimer <= (float)(TROPHY_MENU_APP_TIME * 3))
			{
				float num5 = (f_trophyaddtimer - (float)(TROPHY_MENU_APP_TIME * 2)) / (float)TROPHY_MENU_APP_TIME;
				m.scale(1f - num5 * num5, 1f - num5 * num5, 1f);
				f_trophyaddtimer += SCGlobals.ftickperframe;
			}
			CowRender.LoadMatrix(m);
			winRect.SetRect(rect);
			winRect.Draw();
			CowRender.BindTexture(ar_trophiestex[num3]);
			CowRender.DrawColorQuad2D(new vec2_t(-170f, 0f), ar_trophiesconf[num3].size.x * ar_trophiesconf[num3].scale, ar_trophiesconf[num3].size.y * ar_trophiesconf[num3].scale, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
			graphicsDevice.RenderState.SourceBlend = (Blend)3;
			graphicsDevice.RenderState.DestinationBlend = (Blend)2;
			CowRender.BindTexture(ar_trophiestexlum[num3]);
			CowRender.DrawColorQuad2D(new vec2_t(-170f, 0f), ar_trophiesconf[num3].size.x * ar_trophiesconf[num3].scale, ar_trophiesconf[num3].size.y * ar_trophiesconf[num3].scale, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
			graphicsDevice.RenderState.SourceBlend = (Blend)5;
			graphicsDevice.RenderState.DestinationBlend = (Blend)6;
			float x = 70f;
			float num6 = rect.y0 - 60f;
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, 155, byte.MaxValue);
			Text.OutTextCenterXY(x, num6, 22f, wstrstuff.GetStr("congrat"));
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			Text.OutTextCenterXY(x, num6 - 40f, 12f, wstrstuff.GetStr("youhavegot"));
			Text.SetTextColor(155, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			Text.OutTextCenterXY(x, num6 - 80f, 16f, GetTrophyName(num3 + 1));
			Text.OutTextCenterXY(x, num6 - 110f, 12f, wstrstuff.GetStrDelta("trophysubname1", num3));
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			Text.DrawPreparedTextCenterXY(x, num6 - 140f, 12f);
			Text.SetTextColor(120, byte.MaxValue, 120, byte.MaxValue);
			Text.OutTextCenterXY(x, rect.y0 - rect.y1 + 50f, 15f, "Press    to continue");
			CowRender.BindTexture(SCGlobals.p_a);
			CowRender.DrawColorQuad2D(SCGlobals.p_a.mImage, 42f, rect.y0 - rect.y1 + 50f, 21f, 21f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 10f);
			CowRender.LoadMatrixNull();
			if (f_trophyaddtimer > (float)(TROPHY_MENU_APP_TIME * 3))
			{
				f_trophyaddtimer = 0f;
				SuperCow.Settings.GetTrophies()[num3].state = false;
				SuperCow.Settings.GetTrophies()[num3].active = true;
				SuperCow.Settings.GetTrophies()[num3].apptimer = 1f;
				b_overpresentation = false;
				b_presentation = false;
				Sound.Addsound("addtrophy");
			}
		}
		else
		{
			b_presentation = false;
		}
	}

	public static int CheckForStateTrophies()
	{
		for (int i = 0; i < NUM_TROPHIES; i++)
		{
			if (SuperCow.Settings.GetTrophies()[i].state)
			{
				return i;
			}
		}
		return -1;
	}

	public static bool CheckForCursOnTrophy(int num)
	{
		return ar_trophiesconf[num].cursontrophy;
	}

	public static void SetStateTrophy(bool state, int num)
	{
		if (num == -1)
		{
			for (int i = 0; i < NUM_TROPHIES; i++)
			{
				SuperCow.Settings.GetTrophies()[i].state = state;
			}
		}
		else
		{
			SuperCow.Settings.GetTrophies()[num].state = state;
		}
	}

	public static void TrophyRoomEscape()
	{
		if (b_ingame)
		{
			SCGlobals.animbacks.InitAnimateBacksAtInitLevel(i_gameback);
			Particles.InitParticles();
		}
		else
		{
			SCGlobals.animbacks.InitAnimateBacksAtInitLevel(9);
		}
		SetStateTrophy(state: false, -1);
		b_overpresentation = false;
		b_drag = false;
		b_click = false;
		f_trophyroomtimer = 0f;
		f_trophyaddtimer = 0f;
	}

	public static void SkipWindow()
	{
		if (f_trophyaddtimer > (float)TROPHY_MENU_APP_TIME)
		{
			f_trophyaddtimer = TROPHY_MENU_APP_TIME * 2;
			Sound.Addsound("menu");
		}
	}
}
