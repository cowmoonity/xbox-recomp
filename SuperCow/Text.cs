using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public static class Text
{
	public struct funnychar_t
	{
		public char c;

		public vec2_t pos;

		public float rot;

		public float drot;

		public float size;

		public float dsize;

		public vec2_t addpos;

		public vec2_t dpos;

		public vec2_t dpos2;

		public int type;

		public int numprig;

		public float timer;

		public float totaltime;
	}

	public struct eventtext_t
	{
		public string str;

		public float stringlen;

		public float size;

		public float timer;

		public vec2_t pos;

		public float timescale;

		public float partimer;

		public float partime;
	}

	public struct charinfo_t
	{
		public ushort width;

		public ushort conform;
	}

	public struct fontinfo_t
	{
		public ushort sizex;

		public ushort sizey;

		public ushort def;

		public ushort nop;

		public charinfo_t[] ci;
	}

	public struct font_t
	{
		public int defx;

		public int defy;

		public float font_step_x;

		public float font_step_y;

		public float scalex;

		public float scaley;

		public float[] charsize;

		public ushort[] conform;

		public texture_t p_fonttex;
	}

	public static int MAX_FONTS = 8;

	public static float OUTLINE_SPACE = 0.9f;

	public static float INFO_TEXT_SIZE = 1.5f;

	public static vec3_t uv = default(vec3_t);

	public static vec3_t rv = default(vec3_t);

	public static dcolor_t dc = default(dcolor_t);

	public static vec3_t carriage = default(vec3_t);

	public static vec3_t uv0 = default(vec3_t);

	public static vec3_t rv0 = default(vec3_t);

	public static int FUNNY_TIME = 6;

	public static funnychar_t[] ar_funnychars = new funnychar_t[256];

	public static eventtext_t[] ar_eventtexts = new eventtext_t[16];

	public static fontinfo_t p_fontinfo = default(fontinfo_t);

	public static font_t p_curfont = default(font_t);

	public static font_t[] ar_fonts = new font_t[MAX_FONTS];

	public static int numfonts = 0;

	public static int curfontindex = 0;

	public static int curfunnychar = 0;

	public static int cureventtext = 0;

	public static string strbuf = "";

	public static List<string> buff = new List<string>();

	public static string lit;

	public static float font_step;

	public static float x_carriage;

	public static float fontsize;

	public static float imgFontSize;

	public static float littimer;

	public static CowRender.Quad_VertexF3[] letters = new CowRender.Quad_VertexF3[4];

	public static vec3_t carriage_pos = new vec3_t(0f, 0f, 45f);

	public static dcolor_t dtextcolor = default(dcolor_t);

	public static bool LoadFont(string fontname)
	{
		Store.SetCurrentDirectory("", theAdd: false);
		p_curfont = default(font_t);
		string name = fontname + ".tga";
		p_curfont.p_fonttex = SCGlobals.texturehost.Load(name);
		name = fontname + ".dat";
		int len = 0;
		byte[] dataBuffer = Store.storeLoadFileArray(name, ref len, b_errorexit: false);
		p_fontinfo = default(fontinfo_t);
		CBuffer cBuffer = new CBuffer();
		cBuffer.SetDataBuffer(dataBuffer);
		p_fontinfo.sizex = cBuffer.ReadWord();
		p_fontinfo.sizey = cBuffer.ReadWord();
		p_fontinfo.def = cBuffer.ReadWord();
		p_fontinfo.nop = cBuffer.ReadWord();
		p_fontinfo.ci = new charinfo_t[1024];
		for (int i = 0; i < 1024; i++)
		{
			p_fontinfo.ci[i] = default(charinfo_t);
			p_fontinfo.ci[i].width = cBuffer.ReadWord();
			p_fontinfo.ci[i].conform = cBuffer.ReadWord();
		}
		p_curfont.defx = p_fontinfo.def;
		p_curfont.defy = p_curfont.defx;
		if (p_fontinfo.sizex > p_fontinfo.sizey)
		{
			p_curfont.defy /= 2;
		}
		p_curfont.font_step_x = 1f / (float)p_curfont.defx;
		p_curfont.font_step_y = 1f / (float)p_curfont.defy;
		p_curfont.scalex = 1f;
		p_curfont.scaley = 1f;
		float num = (float)(int)p_fontinfo.sizex / (float)p_curfont.defx;
		p_curfont.charsize = new float[65536];
		p_curfont.conform = new ushort[65536];
		for (int j = 0; j < 1024; j++)
		{
			int conform = p_fontinfo.ci[j].conform;
			p_curfont.charsize[conform] = 2f * (float)(int)p_fontinfo.ci[j].width;
			p_curfont.charsize[conform] /= num;
			p_curfont.charsize[conform] *= p_curfont.scalex;
			p_curfont.conform[conform] = (ushort)j;
		}
		SCGlobals.p_podkladka = SCGlobals.texturehost.Load("podsvet.jpg");
		x_carriage = 0f;
		ClearFunnyText();
		SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		ref font_t reference = ref ar_fonts[numfonts++];
		reference = p_curfont;
		letters[0].uw = default(vec2_t);
		letters[1].uw = default(vec2_t);
		letters[2].uw = default(vec2_t);
		letters[3].uw = default(vec2_t);
		letters[0].xyz = default(vec3_t);
		letters[1].xyz = default(vec3_t);
		letters[2].xyz = default(vec3_t);
		letters[3].xyz = default(vec3_t);
		return true;
	}

	public static void SetFont(int num)
	{
		curfontindex = num % numfonts;
		p_curfont = ar_fonts[curfontindex];
	}

	public static void SetTextCoordZ(float z)
	{
		carriage_pos.z = z;
	}

	public static void SetFontSize(float fs)
	{
		fontsize = fs;
	}

	public static void SetTextColor(dcolor_t tc)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		dtextcolor = tc;
		letters[0].c = tc.GetD3DColor();
		letters[1].c = tc.GetD3DColor();
		letters[2].c = tc.GetD3DColor();
		letters[3].c = tc.GetD3DColor();
	}

	public static void SetTextColor(byte r, byte g, byte b, byte a)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		dtextcolor.assign(r, g, b, a);
		letters[0].c = dtextcolor.GetD3DColor();
		letters[1].c = dtextcolor.GetD3DColor();
		letters[2].c = dtextcolor.GetD3DColor();
		letters[3].c = dtextcolor.GetD3DColor();
	}

	public static void OutTextXY(float x, float y, float font_size, string str, bool b_center)
	{
		if (b_center)
		{
			OutTextCenterXY(x, y, font_size, str);
			return;
		}
		carriage_pos.assign(x, y, carriage_pos.z);
		fontsize = font_size;
		x_carriage = 0f;
		OutText(str);
	}

	public static void OutTextCenterXY(float x, float y, float font_size, string str)
	{
		carriage_pos.assign(x, y, carriage_pos.z);
		x_carriage = 0f;
		fontsize = font_size;
		OutTextCenter(str);
	}

	public static void OutText(string str)
	{
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		if (str.Length == 0)
		{
			return;
		}
		uv.assign(0f, fontsize * p_curfont.scalex * p_curfont.scaley, 0f);
		rv.assign(fontsize * p_curfont.scalex, 0f, 0f);
		dc.r = (byte)(SCGlobals.global_2d_color.r * (float)(int)dtextcolor.r);
		dc.g = (byte)(SCGlobals.global_2d_color.g * (float)(int)dtextcolor.g);
		dc.b = (byte)(SCGlobals.global_2d_color.b * (float)(int)dtextcolor.b);
		dc.a = (byte)(SCGlobals.global_2d_color.a * (float)(int)dtextcolor.a);
		letters[0].c = dc.GetD3DColor();
		letters[1].c = dc.GetD3DColor();
		letters[2].c = dc.GetD3DColor();
		letters[3].c = dc.GetD3DColor();
		CowRender.BindTexture(p_curfont.p_fonttex);
		int num = 0;
		while (num < str.Length)
		{
			if (num != 0)
			{
				x_carriage += fontsize * p_curfont.charsize[(uint)str[num]] * 0.5f;
			}
			if (str[num] == '\r')
			{
				num++;
				x_carriage = (0f - fontsize) * p_curfont.charsize[(uint)str[num]] * 0.5f;
				carriage_pos.y -= OUTLINE_SPACE * fontsize * 2f;
				continue;
			}
			if (str[num] == '\n')
			{
				num++;
				continue;
			}
			vec3_t vec3_t2 = carriage_pos;
			vec3_t2.x += x_carriage;
			int num2 = p_curfont.conform[(uint)str[num]];
			float num3 = p_curfont.font_step_x * (float)(num2 % p_curfont.defx);
			float num4 = p_curfont.font_step_y * (float)(num2 / p_curfont.defx);
			letters[0].xyz = vec3_t2 - rv + uv;
			letters[1].xyz = vec3_t2 + rv + uv;
			letters[2].xyz = vec3_t2 - rv - uv;
			letters[3].xyz = vec3_t2 + rv - uv;
			letters[0].uw.assign(num3, num4);
			letters[1].uw.assign(num3 + p_curfont.font_step_x, num4);
			letters[2].uw.assign(num3, num4 + p_curfont.font_step_y);
			letters[3].uw.assign(num3 + p_curfont.font_step_x, num4 + p_curfont.font_step_y);
			CowRender.DrawTextDynBuffer(ref letters, 2);
			x_carriage += fontsize * p_curfont.charsize[(uint)str[num]] * 0.5f;
			num++;
		}
		CowRender.DrawDynBuffer();
	}

	public static void OutTextCenter(string str)
	{
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		x_carriage = 0f;
		int i = 0;
		int num = 0;
		uv.assign(0f, fontsize * p_curfont.scalex * p_curfont.scaley, 0f);
		rv.assign(fontsize * p_curfont.scalex, 0f, 0f);
		dc.r = (byte)(SCGlobals.global_2d_color.r * (float)(int)dtextcolor.r);
		dc.g = (byte)(SCGlobals.global_2d_color.g * (float)(int)dtextcolor.g);
		dc.b = (byte)(SCGlobals.global_2d_color.b * (float)(int)dtextcolor.b);
		dc.a = (byte)(SCGlobals.global_2d_color.a * (float)(int)dtextcolor.a);
		letters[0].c = dc.GetD3DColor();
		letters[1].c = dc.GetD3DColor();
		letters[2].c = dc.GetD3DColor();
		letters[3].c = dc.GetD3DColor();
		CowRender.BindTextureForced(p_curfont.p_fonttex);
		do
		{
			float num2 = 0f;
			for (; i < str.Length && str[i] != '\r' && str[i] != '\n'; i++)
			{
				num2 += fontsize * p_curfont.charsize[(uint)str[i]];
			}
			x_carriage = (0f - num2) / 2f + fontsize / 2f;
			for (int j = num; j < i; j++)
			{
				int num3 = p_curfont.conform[(uint)str[j]];
				if (j != 0)
				{
					x_carriage += fontsize * p_curfont.charsize[(uint)str[j]] * 0.5f;
				}
				vec3_t vec3_t2 = carriage_pos;
				vec3_t2.x += x_carriage;
				float num4 = p_curfont.font_step_x * (float)(num3 % p_curfont.defx);
				float num5 = p_curfont.font_step_y * (float)(num3 / p_curfont.defx);
				letters[0].xyz = vec3_t2 - rv + uv;
				letters[1].xyz = vec3_t2 + rv + uv;
				letters[2].xyz = vec3_t2 - rv - uv;
				letters[3].xyz = vec3_t2 + rv - uv;
				letters[0].uw.assign(num4, num5);
				letters[1].uw.assign(num4 + p_curfont.font_step_x, num5);
				letters[2].uw.assign(num4, num5 + p_curfont.font_step_y);
				letters[3].uw.assign(num4 + p_curfont.font_step_x, num5 + p_curfont.font_step_y);
				CowRender.DrawTextDynBuffer(ref letters, 2);
				x_carriage += fontsize * p_curfont.charsize[(uint)str[j]] * 0.5f;
			}
			if (i < str.Length)
			{
				if (str[i] == '\r')
				{
					i += 2;
					num = i;
					carriage_pos.y -= OUTLINE_SPACE * fontsize * 2f;
				}
				else if (str[i] == '\n')
				{
					i++;
					num = i;
					carriage_pos.y -= OUTLINE_SPACE * fontsize * 2f;
				}
			}
		}
		while (i < str.Length);
		CowRender.DrawDynBuffer();
	}

	public static float GetStringWidth(string str, float font_size)
	{
		int i = 0;
		float num = 0f;
		for (; i < str.Length; i++)
		{
			num += font_size * p_curfont.charsize[(uint)str[i]];
		}
		return num;
	}

	public static void InitGameTexts()
	{
		littimer = 0f;
		for (int i = 0; i < 16; i++)
		{
			ar_eventtexts[i].timer = -1f;
		}
		ClearFunnyText();
	}

	public static void RemovEventText()
	{
		ClearFunnyText();
		for (int i = 0; i < 16; i++)
		{
			ar_eventtexts[i].timer = -1f;
		}
	}

	public static void AddEventText(string str, vec2_t pos, float size, float effecttime)
	{
		if (!(effecttime <= 0f))
		{
			ar_eventtexts[cureventtext].str = str;
			ar_eventtexts[cureventtext].pos = pos;
			ar_eventtexts[cureventtext].size = size;
			ar_eventtexts[cureventtext].stringlen = GetStringWidth(str, size);
			ar_eventtexts[cureventtext].timer = 2000f;
			ar_eventtexts[cureventtext].timescale = 2000f / effecttime;
			ar_eventtexts[cureventtext].partimer = 0f;
			ar_eventtexts[cureventtext].partime = 5000f / (1f + ar_eventtexts[cureventtext].stringlen);
			cureventtext = (cureventtext + 1) % 16;
		}
	}

	public static void DrawEventTexts()
	{
		CowRender.BindTexture(p_curfont.p_fonttex);
		dcolor_t textColor = default(dcolor_t);
		for (int i = 0; i < 16; i++)
		{
			if (!(ar_eventtexts[i].timer > 0f))
			{
				continue;
			}
			float timer = ar_eventtexts[i].timer;
			textColor.assign(byte.MaxValue, (byte)(230f + 25f * SuperCowHelper.psin(3f, -1000001f)), (byte)(100f + 155f * SuperCowHelper.psin(4f, -1000001f)), byte.MaxValue);
			if (timer > 1400f)
			{
				float num = (2000f - timer) / 600f;
				int num2 = (int)((2000f - timer) * 256f / 600f);
				if (num2 > 255)
				{
					num2 = 255;
				}
				textColor.a = (byte)num2;
				SetTextColor(textColor);
				OutTextCenterXY(ar_eventtexts[i].pos.x, ar_eventtexts[i].pos.y, ar_eventtexts[i].size * num, ar_eventtexts[i].str);
				ar_eventtexts[i].partimer += SCGlobals.ftickperframe;
				while (ar_eventtexts[i].partimer > ar_eventtexts[i].partime)
				{
					Particles.AddDynamicParticle(new vec2_t(ar_eventtexts[i].pos.x + CRand.frand(ar_eventtexts[i].stringlen / 2f), ar_eventtexts[i].pos.y + CRand.frand(ar_eventtexts[i].size / 2f)), 28, 1, 18);
					ar_eventtexts[i].partimer -= ar_eventtexts[i].partime;
				}
			}
			else if (timer > 600f)
			{
				float num = (timer - 600f) / 800f;
				SetTextColor(textColor);
				OutTextCenterXY(ar_eventtexts[i].pos.x, ar_eventtexts[i].pos.y, ar_eventtexts[i].size * (1f + 0.1f * (float)Math.Sin((double)num * Math.PI * 3.0)), ar_eventtexts[i].str);
			}
			else
			{
				float num = timer / 600f;
				int num2 = (int)(timer * 256f / 600f);
				if (num2 > 255)
				{
					num2 = 255;
				}
				textColor.a = (byte)num2;
				SetTextColor(textColor);
				OutTextCenterXY(ar_eventtexts[i].pos.x, ar_eventtexts[i].pos.y, ar_eventtexts[i].size * num, ar_eventtexts[i].str);
			}
			ar_eventtexts[i].timer -= SCGlobals.ftickperframe * ar_eventtexts[i].timescale;
		}
		SetTextColor(new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
	}

	public static void OutBitmap(float x, float y, float sizex, float sizey, texture_t p_tex)
	{
		carriage_pos.assign(x, y, 45f);
		fontsize = 0f;
		vec3_t vec3_t2 = new vec3_t(0f, sizey, 0f);
		vec3_t vec3_t3 = new vec3_t(sizex, 0f, 0f);
		CowRender.BindTexture(p_tex);
		letters[0].xyz = carriage_pos - vec3_t3 + vec3_t2;
		letters[1].xyz = carriage_pos + vec3_t3 + vec3_t2;
		letters[2].xyz = carriage_pos - vec3_t3 - vec3_t2;
		letters[3].xyz = carriage_pos + vec3_t3 - vec3_t2;
		letters[0].uw.assign(0f, 0f);
		letters[1].uw.assign(0.999f, 0f);
		letters[2].uw.assign(0f, 0.999f);
		letters[3].uw.assign(0.999f, 0.999f);
		CowRender.DrawVertexArray(ref letters, 2, 322uL, (PrimitiveType)5);
	}

	public static void AddLInfoText(string text)
	{
		if (littimer <= 0f)
		{
			littimer = 1400f;
		}
		lit = text;
	}

	public static void DrawLInfoTexts()
	{
		CowRender.BindTexture(p_curfont.p_fonttex);
		if (!(littimer <= 0f))
		{
			if (littimer < 500f || littimer > 1000f)
			{
				littimer -= SCGlobals.ftickperframe;
			}
			else
			{
				littimer -= SCGlobals.ftickperframe * 0.3f;
			}
			float num = 127f + 127f * (float)Math.Sin(littimer * (float)Math.PI / 1500f);
			SetTextColor(new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)num));
			float num2 = 30f;
			if (littimer < 500f)
			{
				OutTextCenterXY(0f, -270f - 0.1f * (500f - littimer) + num2, INFO_TEXT_SIZE * 16.6f, lit);
			}
			else if (littimer > 1000f)
			{
				OutTextCenterXY(0f, -270f - 0.1f * (littimer - 1000f) + num2, INFO_TEXT_SIZE * (6f + 12f * (float)Math.Sin(littimer * (float)Math.PI / 1500f)), lit);
			}
			else
			{
				OutTextCenterXY(0f, -270f + num2, INFO_TEXT_SIZE * 16.6f + (float)Math.Sin((littimer - 500f) * (float)Math.PI * 2f / 125f), lit);
			}
		}
	}

	public static void DrawFunnyTexts()
	{
		float num = 1f;
		dcolor_t colorWhite = dcolor_t.ColorWhite;
		SetTextColor(colorWhite);
		CowRender.BindTexture(p_curfont.p_fonttex);
		for (int i = 0; i < 256; i++)
		{
			float num2 = ar_funnychars[i].totaltime / (float)FUNNY_TIME;
			if (!(ar_funnychars[i].timer > 0f))
			{
				continue;
			}
			if (ar_funnychars[i].timer <= 6f * num2)
			{
				float num4;
				if (ar_funnychars[i].timer >= 5f * num2)
				{
					float num3 = (6f * num2 - ar_funnychars[i].timer) / num2;
					colorWhite.a = (byte)(num3 * 255f);
					num = 2f - num3;
					num4 = 0f;
				}
				else if (ar_funnychars[i].timer <= 1.5f * num2)
				{
					float num3 = ar_funnychars[i].timer / (1.5f * num2);
					colorWhite.a = (byte)(num3 * 255f);
					num = num3;
					num4 = 0f;
				}
				else
				{
					float num3 = (ar_funnychars[i].timer - 1.5f * num2) / (3.5f * num2) * ((float)Math.PI * 3f);
					colorWhite.a = byte.MaxValue;
					num = 1f + 0.08f * (float)Math.Sin(num3);
					num4 = 2f * (float)Math.Sin(num3 + num3);
				}
				num *= num;
				vec2_t pos = ar_funnychars[i].pos;
				pos.x *= num;
				pos.y += num4;
				SetTextColor(colorWhite);
				OutChar(ar_funnychars[i].c, pos, ar_funnychars[i].rot, ar_funnychars[i].size * num, colorWhite);
			}
			ar_funnychars[i].timer -= SCGlobals.ftickperframe;
		}
		CowRender.DrawDynBuffer();
		SetTextColor(dcolor_t.ColorWhite);
	}

	public static void ClearFunnyText()
	{
		Array.Clear(ar_funnychars, 0, ar_funnychars.Length);
	}

	public static void OutChar(char c, vec2_t pos, float rot, float size, dcolor_t dc)
	{
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		uv0.assign(0f, size * p_curfont.scalex * p_curfont.scaley, 0f);
		rv0.assign(size * p_curfont.scalex, 0f, 0f);
		rv = rv0 * SuperCowHelper.fast_cos(rot) - uv0 * SuperCowHelper.fast_sin(rot);
		uv = rv0 * SuperCowHelper.fast_sin(rot) + uv0 * SuperCowHelper.fast_cos(rot);
		x_carriage = 0f;
		int num = p_curfont.conform[(uint)c];
		carriage.assign(pos.x, pos.y, 10f);
		float num2 = p_curfont.font_step_x * (float)(num % p_curfont.defx);
		float num3 = p_curfont.font_step_y * (float)(num / p_curfont.defx);
		dc.r = (byte)(SCGlobals.global_2d_color.r * (float)(int)dc.r);
		dc.g = (byte)(SCGlobals.global_2d_color.g * (float)(int)dc.g);
		dc.b = (byte)(SCGlobals.global_2d_color.b * (float)(int)dc.b);
		dc.a = (byte)(SCGlobals.global_2d_color.a * (float)(int)dc.a);
		letters[0].c = dc.GetD3DColor();
		letters[1].c = dc.GetD3DColor();
		letters[2].c = dc.GetD3DColor();
		letters[3].c = dc.GetD3DColor();
		letters[0].xyz = carriage - rv + uv;
		letters[1].xyz = carriage + rv + uv;
		letters[2].xyz = carriage - rv - uv;
		letters[3].xyz = carriage + rv - uv;
		letters[0].uw.assign(num2, num3);
		letters[1].uw.assign(num2 + p_curfont.font_step_x, num3);
		letters[2].uw.assign(num2, num3 + p_curfont.font_step_y);
		letters[3].uw.assign(num2 + p_curfont.font_step_x, num3 + p_curfont.font_step_y);
		CowRender.DrawTextDynBuffer(ref letters, 2);
	}

	public static void AddFunnyText(string text, vec2_t coord, int type, float size, float totaltime)
	{
		funnychar_t funnychar_t = new funnychar_t
		{
			addpos = default(vec2_t),
			dpos = default(vec2_t),
			dpos2 = default(vec2_t)
		};
		int num = 0;
		float num2 = (0f - GetStringWidth(text, size)) * 0.5f;
		while (num < text.Length)
		{
			funnychar_t.c = text[num];
			if (num != 0)
			{
				num2 += size * p_curfont.charsize[(uint)text[num]] * 0.5f;
			}
			funnychar_t.pos = coord;
			funnychar_t.pos.x += num2;
			funnychar_t.addpos.assign(0f, 0f);
			funnychar_t.dpos.assign(0f, 0f);
			funnychar_t.numprig = 0;
			funnychar_t.dpos2.assign((funnychar_t.pos.x - coord.x) * 0.001f, 0.7f + CRand.frand(0.1f));
			funnychar_t.type = type;
			funnychar_t.size = size;
			funnychar_t.rot = 0f;
			funnychar_t.drot = 0f;
			funnychar_t.totaltime = totaltime;
			funnychar_t.timer = totaltime + (float)(num * 40);
			num2 += size * p_curfont.charsize[(uint)text[num]] * 0.5f;
			ar_funnychars[curfunnychar] = funnychar_t;
			num++;
			curfunnychar = (curfunnychar + 1) % 256;
		}
	}

	public static float PrepareWindowText(float window_width, string str, float font_size)
	{
		fontsize = font_size;
		float num = 0f;
		buff.Clear();
		strbuf = str;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		float num5 = OUTLINE_SPACE * fontsize * 2f;
		while (num3 < str.Length)
		{
			num += fontsize * p_curfont.charsize[(uint)str[num3]];
			if (num > window_width)
			{
				string text = str.Substring(num2, num4 - num2);
				num3 = num4 + 1;
				num2 = num3;
				buff.Add(text + "@\r\n");
				num = 0f;
				num5 += OUTLINE_SPACE * fontsize * 2f;
			}
			if (str[num3] == '\n')
			{
				num = 0f;
				string item = str.Substring(num2, num3 - num2 + 1);
				buff.Add(item);
				num2 = num3 + 1;
				num5 += OUTLINE_SPACE * fontsize * 2f;
			}
			else if (str[num3] == '\r')
			{
				num = 0f;
			}
			num3++;
			if (num3 != str.Length && strbuf[num3] == ' ')
			{
				num4 = num3;
			}
		}
		string item2 = str.Substring(num2, num3 - num2);
		buff.Add(item2);
		return num5;
	}

	public static void DrawPreparedTextCenterXY(float x, float y, float font_size)
	{
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		carriage_pos.assign(x, y, carriage_pos.z);
		x_carriage = 0f;
		fontsize = font_size;
		uv.assign(0f, fontsize * p_curfont.scalex * p_curfont.scaley, 0f);
		rv.assign(fontsize * p_curfont.scalex, 0f, 0f);
		dc.r = (byte)(SCGlobals.global_2d_color.r * (float)(int)dtextcolor.r);
		dc.g = (byte)(SCGlobals.global_2d_color.g * (float)(int)dtextcolor.g);
		dc.b = (byte)(SCGlobals.global_2d_color.b * (float)(int)dtextcolor.b);
		dc.a = (byte)(SCGlobals.global_2d_color.a * (float)(int)dtextcolor.a);
		letters[0].c = dc.GetD3DColor();
		letters[1].c = dc.GetD3DColor();
		letters[2].c = dc.GetD3DColor();
		letters[3].c = dc.GetD3DColor();
		CowRender.BindTexture(p_curfont.p_fonttex);
		int i = 0;
		int num = 0;
		for (; i < buff.Count; i++)
		{
			string text = buff[i];
			int j = 0;
			float num2 = 0f;
			for (; j < text.Length; j++)
			{
				num2 += fontsize * p_curfont.charsize[(uint)text[j]];
			}
			x_carriage = (0f - num2) / 2f + fontsize / 2f;
			j = 0;
			while (j < text.Length)
			{
				num++;
				if (text[j] == '@')
				{
					num -= 2;
					j++;
					continue;
				}
				if (j != 0)
				{
					x_carriage += fontsize * p_curfont.charsize[(uint)text[j]] * 0.5f;
				}
				if (text[j] == '\r')
				{
					j++;
					x_carriage = (0f - fontsize) * p_curfont.charsize[(uint)text[j]] * 0.5f;
					carriage_pos.y -= OUTLINE_SPACE * fontsize * 2f;
					continue;
				}
				if (text[j] == '\n')
				{
					j++;
					continue;
				}
				carriage = carriage_pos;
				carriage.x += x_carriage;
				int num3 = p_curfont.conform[(uint)text[j]];
				float num4 = p_curfont.font_step_x * (float)(num3 % p_curfont.defx);
				float num5 = p_curfont.font_step_y * (float)(num3 / p_curfont.defx);
				letters[0].xyz = carriage - rv + uv;
				letters[1].xyz = carriage + rv + uv;
				letters[2].xyz = carriage - rv - uv;
				letters[3].xyz = carriage + rv - uv;
				letters[0].uw.assign(num4, num5);
				letters[1].uw.assign(num4 + p_curfont.font_step_x, num5);
				letters[2].uw.assign(num4, num5 + p_curfont.font_step_y);
				letters[3].uw.assign(num4 + p_curfont.font_step_x, num5 + p_curfont.font_step_y);
				CowRender.DrawTextDynBuffer(ref letters, 2);
				x_carriage += fontsize * p_curfont.charsize[(uint)text[j]] * 0.5f;
				j++;
			}
		}
		CowRender.DrawDynBuffer();
	}

	public static void DrawPreparedTextXY(float x, float y, float font_size, int lastchar)
	{
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		carriage_pos.assign(x, y, carriage_pos.z);
		fontsize = font_size;
		x_carriage = 0f;
		uv.assign(0f, fontsize * p_curfont.scalex * p_curfont.scaley, 0f);
		rv.assign(fontsize * p_curfont.scalex, 0f, 0f);
		dc.r = (byte)(SCGlobals.global_2d_color.r * (float)(int)dtextcolor.r);
		dc.g = (byte)(SCGlobals.global_2d_color.g * (float)(int)dtextcolor.g);
		dc.b = (byte)(SCGlobals.global_2d_color.b * (float)(int)dtextcolor.b);
		dc.a = (byte)(SCGlobals.global_2d_color.a * (float)(int)dtextcolor.a);
		letters[0].c = dc.GetD3DColor();
		letters[1].c = dc.GetD3DColor();
		letters[2].c = dc.GetD3DColor();
		letters[3].c = dc.GetD3DColor();
		CowRender.BindTexture(p_curfont.p_fonttex);
		int i = 0;
		int num = 0;
		int num2 = lastchar;
		if (lastchar == -1)
		{
			num2 = 1000000;
		}
		for (; i < buff.Count; i++)
		{
			string text = buff[i];
			int num3 = 0;
			while (num3 < text.Length && num < num2)
			{
				num++;
				if (text[num3] == '@')
				{
					num -= 2;
					num3++;
					continue;
				}
				if (num3 != 0)
				{
					x_carriage += fontsize * p_curfont.charsize[(uint)text[num3]] * 0.5f;
				}
				if (text[num3] == '\r')
				{
					num3++;
					x_carriage = (0f - fontsize) * p_curfont.charsize[(uint)text[num3]] * 0.5f;
					carriage_pos.y -= OUTLINE_SPACE * fontsize * 2f;
					continue;
				}
				if (text[num3] == '\n')
				{
					num3++;
					continue;
				}
				carriage = carriage_pos;
				carriage.x += x_carriage;
				int num4 = p_curfont.conform[(uint)text[num3]];
				float num5 = p_curfont.font_step_x * (float)(num4 % p_curfont.defx);
				float num6 = p_curfont.font_step_y * (float)(num4 / p_curfont.defx);
				letters[0].xyz = carriage - rv + uv;
				letters[1].xyz = carriage + rv + uv;
				letters[2].xyz = carriage - rv - uv;
				letters[3].xyz = carriage + rv - uv;
				letters[0].uw.assign(num5, num6);
				letters[1].uw.assign(num5 + p_curfont.font_step_x, num6);
				letters[2].uw.assign(num5, num6 + p_curfont.font_step_y);
				letters[3].uw.assign(num5 + p_curfont.font_step_x, num6 + p_curfont.font_step_y);
				CowRender.DrawTextDynBuffer(ref letters, 2);
				x_carriage += fontsize * p_curfont.charsize[(uint)text[num3]] * 0.5f;
				num3++;
			}
		}
		CowRender.DrawDynBuffer();
	}
}
