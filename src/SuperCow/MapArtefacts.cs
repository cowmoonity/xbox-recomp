using System;

namespace SuperCow;

internal class MapArtefacts
{
	public static texture_t[] ar_masks = new texture_t[11];

	public static texture_t[,] ar_masks_glow = new texture_t[11, 3];

	public static int i_current_artefact;

	public static int i_current_mask;

	public static int artefacr_vec_index;

	public static int i_view_artefact = 0;

	public static int i_show_artefact = 0;

	public static int diary_stage = -1;

	public static int diary_level = -1;

	public static float size;

	public static float f_new_percent = 0f;

	public static float f_current_percent = 0f;

	public static float f_new_mask = 0f;

	public static float f_current_mask = 0f;

	public static float f_ma_randtimer = 0f;

	public static float f_alfa_mask = 0f;

	public static float f_opening_mask_timer = 0f;

	public static float f_thunderbolt_timer = 0f;

	public static float gotocomicstimer = 0f;

	public static float diarytimer = 0f;

	public static int NUM_ARTEFACTS = 12;

	public static int NUM_MASK = 11;

	public static float BLINK_SPEED = 0.007f;

	public static int BLINK_PERIOD = 5;

	public static int MA_NUM_SHOT = 3;

	public static float MA_SPEED_GLOW = 0.005f;

	public static float MA_SPEED_MASK_OPENING = 0.001f;

	public static int MA_TIME_MASK_OPENING = 2000;

	public static CSprite animscene = new CSprite();

	public static frect_t artefact = default(frect_t);

	public static vec2_t thunderbolt_pos = default(vec2_t);

	public static float sounddiarytimer = 0f;

	public static int sounddiaryindex = 0;

	public static float diarypausetimer = 0f;

	public static int i_cartefactstep = -1;

	public static float f_t_cartefact = 0f;

	public static vec2_t ca_vec = default(vec2_t);

	public static string c_artefact_description;

	public static void maGetPosOpen()
	{
	}

	public static void maCalculateOpeningMask()
	{
	}

	public static void CA_NextStep()
	{
	}

	public static void maInit()
	{
		Store.SetCurrentDirectory("", theAdd: false);
		animscene.Load("Night.jpg", 32, 8);
		animscene.SetParams(230f, 115f, 150);
		Store.SetCurrentDirectory("", theAdd: false);
	}

	public static void maDraw()
	{
		if (SuperCow.IsHDTV)
		{
			animscene.Draw(new vec2_t(352f, 196f), -1, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f, 0f);
		}
		else
		{
			animscene.Draw(new vec2_t(232f, 196f), -1, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f, 0f);
		}
		CowRender.EnableRS(7u);
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		string name = $"diary_{diary_stage + 1:00}_{diary_level + 1:00}";
		string str = wstrstuff.GetStr(name);
		_ = str == "";
		c_artefact_description = wstrstuff.GetStr(name);
		int length = c_artefact_description.Length;
		int num = (int)Math.Ceiling(diarytimer);
		if (num > length)
		{
			num = -1;
		}
		if (num > 0 && diarypausetimer <= 0f)
		{
			if (c_artefact_description[num - 1] == '.')
			{
				diarypausetimer = 200f;
			}
			else if (c_artefact_description[num - 1] == ',')
			{
				diarypausetimer = 150f;
			}
			else
			{
				diarypausetimer = 0f;
			}
		}
		if (num > 0 && diarypausetimer <= 0f)
		{
			if (sounddiarytimer < 0f)
			{
				string sound = "write" + (sounddiaryindex + 1);
				Sound.Addsound(sound);
				sounddiaryindex = (sounddiaryindex + CRand.irand(4)) % 7;
				sounddiarytimer = 200f;
			}
			sounddiarytimer -= SCGlobals.ftickperframe;
		}
		if (diarypausetimer > 0f)
		{
			diarypausetimer -= SCGlobals.ftickperframe;
			if (diarypausetimer < 0f)
			{
				diarytimer += 1f;
			}
		}
		else if (num >= 0)
		{
			diarytimer += SCGlobals.ftickperframe * 0.015f;
		}
		Text.SetFont(1);
		float num2 = Text.PrepareWindowText(190f, c_artefact_description, 10f);
		Text.SetTextColor(78, 29, 0, byte.MaxValue);
		if (SuperCow.IsHDTV)
		{
			Text.DrawPreparedTextXY(255f, (0f - (260f - num2)) / 2f + 120f, 10f, num);
		}
		else
		{
			Text.DrawPreparedTextXY(140f, (0f - (260f - num2)) / 2f + 120f, 10f, num);
		}
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		Text.SetFont(0);
	}

	public static void maCalculate()
	{
	}

	public static void maArtefactSetPer(int current_artefact, float new_percent, float current_percent, bool animate)
	{
	}

	public static void maInitArtefactPresentation(vec2_t v)
	{
		ca_vec = v;
		i_cartefactstep = 0;
		f_t_cartefact = 0f;
		maArtefactSetPer(i_current_artefact, 0f, 0f, animate: false);
	}

	public static void maLButtonDown()
	{
		vec2_t pos = default(vec2_t);
		frect_t frect_t2 = default(frect_t);
		if (i_cartefactstep == 2)
		{
			float num = f_t_cartefact / 600f * 100f;
			if (num > 100f)
			{
				num = 100f;
			}
			pos.assign(0f, -220f);
			frect_t2.assign(pos, 1.6f * num, 0.56f * num);
			if (frect_t2.isPointInRect(SCGlobals.curspos))
			{
				maOkPressed();
			}
		}
	}

	public static void maOnkeyDown(int nChar)
	{
	}

	public static void maDrawPresentation()
	{
	}

	public static void maDrawOpeningMask()
	{
	}

	public static void maInitOpeningMask()
	{
		f_opening_mask_timer = MA_TIME_MASK_OPENING;
	}

	public static void maSetShowArtefact(int nstage, int nlevel)
	{
		if (diary_stage != nstage || diary_level != nlevel)
		{
			diary_stage = nstage;
			diary_level = nlevel;
			int absCurLevel = SuperCow.Settings.GetAbsCurLevel(diary_stage, diary_level);
			int playerMaxLevel = SuperCow.Settings.GetPlayerMaxLevel();
			if (absCurLevel < playerMaxLevel)
			{
				diarytimer = 1000f;
			}
			else
			{
				diarytimer = 0f;
			}
		}
	}

	public static void maOkPressed()
	{
		gotocomicstimer = 1f;
	}
}
