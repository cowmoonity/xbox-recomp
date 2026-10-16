using System;
using System.Text;

namespace SuperCow;

public class AnimateBack
{
	public struct animbackobject_t
	{
		public string name;

		public CGeomObj p_cgo;

		public texture_t p_tex;

		public int group;
	}

	public struct animatedsquare_t
	{
		public int type;

		public animbackobject_t p_go;

		public frect_t rect;

		public float rotation;

		public bool b_flip;

		public vec2_t dtpos;

		public dcolor_t color;

		public float timer;

		public bool b_draw;

		public int laver;
	}

	public struct aleave_t
	{
		public vec2_t p;

		public vec2_t center;

		public float rot;

		public float sizex;

		public float sizey;

		public dcolor_t color;

		public int num;

		public texture_t tex;

		public bool b_flip;
	}

	public struct spray_t
	{
		public vec2_t p;

		public float timer;

		public int type;

		public texture_t tex;
	}

	private string[] animgroup_names = new string[32];

	private int numanimbackgroups;

	private animbackobject_t[] ar_animbackobjs = new animbackobject_t[512];

	private int numanimbackobjs;

	private float changebacktimer;

	private float changebackk;

	private int nextbackindex = -1;

	private int curbackindex = -1;

	private animatedsquare_t[] ar_animsquare = new animatedsquare_t[128];

	private int curasq;

	private aleave_t[] aleaves = new aleave_t[128];

	private int curleave;

	private static int NUM_SPRAYS = 256;

	private spray_t[] sprays = new spray_t[NUM_SPRAYS];

	private int curspray;

	private texture_t curback;

	private texture_t nextback;

	private texture_t[] waterfall = new texture_t[10];

	private texture_t[] water = new texture_t[10];

	private texture_t[] sky = new texture_t[10];

	private texture_t[] leaves = new texture_t[10];

	private texture_t[] spraystex = new texture_t[10];

	private float addleavestimer;

	private color_t anim_back_color = new color_t(1f, 1f, 1f, 1f);

	private color_t anim_back_color0 = new color_t(1f, 1f, 1f, 1f);

	public void NewAnimateBackgroundColor(int num)
	{
		color_t color_t2 = new color_t(0.67f, 0.67f, 1f, 1f);
		switch (num)
		{
		case 1:
			color_t2 = new color_t(1f, 0.67f, 0.67f, 1f);
			break;
		case 2:
			color_t2 = new color_t(1f, 1f, 1f, 1f);
			break;
		}
		anim_back_color0 = color_t2;
	}

	public void SetAnimateBackgroundColor(int num)
	{
		color_t color_t2 = new color_t(0.67f, 0.67f, 1f, 1f);
		switch (num)
		{
		case 1:
			color_t2 = new color_t(1f, 0.67f, 0.67f, 1f);
			break;
		case 2:
			color_t2 = new color_t(1f, 1f, 1f, 1f);
			break;
		}
		anim_back_color0 = color_t2;
		anim_back_color = color_t2;
	}

	private void DrawAnimateSquare(ref animatedsquare_t as_t)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 1f;
		float num4 = 1f;
		texture_t p_tex = as_t.p_go.p_tex;
		as_t.b_draw = false;
		if (as_t.p_go.name.Contains("sky"))
		{
			num3 = (as_t.rect.x1 - as_t.rect.x0) / (as_t.rect.y1 - as_t.rect.y0) * ((float)p_tex.height / (float)p_tex.width);
			num4 = 1f;
			num = (0f - SCGlobals.global_timer) * 3E-06f;
			num2 = 0f;
			as_t.b_draw = true;
		}
		else if (as_t.p_go.name.Contains("waterfall"))
		{
			num = (0f - as_t.dtpos.x) * SCGlobals.global_timer * 1E-05f;
			num2 = (0f - as_t.dtpos.y) * SCGlobals.global_timer * 1E-05f;
			num3 = (as_t.rect.x1 - as_t.rect.x0) / (float)p_tex.width;
			num4 = (as_t.rect.y1 - as_t.rect.y0) / (float)p_tex.height;
			as_t.b_draw = true;
		}
		else if (as_t.p_go.name.Contains("water"))
		{
			num = (0f - as_t.dtpos.x) * SCGlobals.global_timer * 1E-05f;
			num2 = (0f - as_t.dtpos.y) * SCGlobals.global_timer * 1E-05f;
			num *= 0.2f;
			num2 *= 0.2f;
			num3 = (as_t.rect.x1 - as_t.rect.x0) / (float)p_tex.width;
			num4 = (as_t.rect.y1 - as_t.rect.y0) / (float)p_tex.height;
			as_t.b_draw = true;
		}
		else
		{
			if (as_t.p_go.name.Contains("spray"))
			{
				float num5 = Math.Abs((as_t.rect.x1 - as_t.rect.x0) * (as_t.rect.y1 - as_t.rect.y0));
				as_t.timer += SCGlobals.ftickperframe;
				if (as_t.timer > 300000f / num5)
				{
					as_t.timer = 0f;
					AddSpray(as_t.rect.pos() + new vec2_t(CRand.frand(as_t.rect.sizex() * 0.5f), CRand.frand(as_t.rect.sizey() * 0.5f)), 0, p_tex);
				}
				as_t.b_draw = true;
				return;
			}
			if (as_t.p_go.name.Contains("leaves"))
			{
				as_t.timer += SCGlobals.ftickperframe;
				vec2_t vec2_t2 = as_t.rect.pos();
				float rotation = as_t.rotation;
				float num6 = as_t.timer + addleavestimer;
				addleavestimer += 400f;
				byte b = (byte)(255 - (byte)as_t.dtpos.len());
				dcolor_t color = new dcolor_t(b, b, b, byte.MaxValue);
				float num7 = (as_t.b_flip ? (-1f) : 1f);
				if (as_t.p_go.name.Contains("leaves1"))
				{
					AddLeave(vec2_t2, vec2_t2 + new vec2_t((0f - as_t.rect.sizex()) / 2f, 0f) * num7, rotation + 0.5f * (float)Math.Sin(num6 * 0.003f) + 0.2f * (float)Math.Sin(num6 * 0.0043f), as_t.rect.sizex(), as_t.rect.sizey(), color, p_tex, as_t.b_flip);
				}
				else if (as_t.p_go.name.Contains("leaves2"))
				{
					AddLeave(vec2_t2, vec2_t2 + new vec2_t((0f - as_t.rect.sizex()) / 2f, 0f) * num7, rotation + 0.18f * (float)Math.Sin(num6 * 0.004f) + 0.11f * (float)Math.Sin(num6 * 0.0024f), as_t.rect.sizex(), as_t.rect.sizey(), color, p_tex, as_t.b_flip);
				}
				else
				{
					AddLeave(vec2_t2, vec2_t2 + new vec2_t((0f - as_t.rect.sizex()) / 2f, 0f) * num7, rotation + 0.6f * (float)Math.Sin(num6 * 0.0022f) + 0.24f * (float)Math.Sin(num6 * 0.0038f), as_t.rect.sizex(), as_t.rect.sizey(), color, p_tex, as_t.b_flip);
				}
				as_t.b_draw = true;
				return;
			}
		}
		if (as_t.laver != 0 || as_t.b_draw)
		{
			as_t.b_draw = true;
			CowRender.BindTexture(as_t.p_go.p_tex);
			frect_t tc = default(frect_t);
			if (!as_t.b_flip)
			{
				tc.assign(num, num2, num + num3, num2 + num4);
			}
			else
			{
				tc.assign(num + num3, num2, num, num2 + num4);
			}
			dcolor_t colorWhite = dcolor_t.ColorWhite;
			if (changebacktimer > 0f && changebacktimer <= 3000f)
			{
				float num8 = changebackk;
				colorWhite.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255f * num8 * num8 * num8));
			}
			CowRender.DrawRotatedColorQuad2D(as_t.rect.pos(), as_t.rect.pos(), as_t.rotation, as_t.rect.sizex(), as_t.rect.sizey(), tc, colorWhite, 0f);
		}
	}

	public void DrawSquareAboveAll(animatedsquare_t as_t)
	{
		if (!as_t.b_draw)
		{
			CowRender.BindTexture(as_t.p_go.p_tex);
			frect_t tc = default(frect_t);
			if (!as_t.b_flip)
			{
				tc.assign(0f, 0f, 1f, 1f);
			}
			else
			{
				tc.assign(1f, 0f, 0f, 1f);
			}
			CowRender.DrawRotatedColorQuad2D(as_t.rect.pos(), as_t.rect.pos(), as_t.rotation, as_t.rect.sizex(), as_t.rect.sizey(), tc, dcolor_t.ColorWhite, 0f);
		}
	}

	public void AddLeave(vec2_t p, vec2_t center, float rot, float sizex, float sizey, dcolor_t color, texture_t tex, bool b_flip)
	{
		if (curleave < 128)
		{
			aleaves[curleave].p = p;
			aleaves[curleave].center = center;
			if (tex.name.Contains("0007") || tex.name.Contains("0031"))
			{
				aleaves[curleave].center -= new vec2_t(0f, 50f);
			}
			aleaves[curleave].rot = rot;
			aleaves[curleave].sizex = sizex;
			aleaves[curleave].sizey = sizey;
			aleaves[curleave].color = color;
			aleaves[curleave].tex = tex;
			aleaves[curleave].b_flip = b_flip;
			curleave++;
		}
	}

	public void DrawLeaves()
	{
		_ = SuperCow.DeviceManager.GraphicsDevice;
		CowRender.EnableRS(4u);
		CowRender.SetBlend(0);
		for (int i = 0; i < curleave; i++)
		{
			aleave_t aleave_t = aleaves[i];
			CowRender.BindTexture(aleave_t.tex);
			CowRender.DrawRotatedColorQuad2D(aleave_t.p, aleave_t.center, aleave_t.rot, aleave_t.sizex, aleave_t.sizey, aleave_t.b_flip ? SCGlobals.tc_inverse : SCGlobals.tc_normal, aleave_t.color, 0f);
		}
	}

	public void DrawAnimateBacks()
	{
		_ = SuperCow.DeviceManager.GraphicsDevice;
		color_t color_t2 = anim_back_color;
		if (SCGlobals.b_gamemenu)
		{
			float num = 0.8f + 0.2f * SCGlobals.menusetk;
			color_t2 = anim_back_color * num;
			color_t2.a = 1f;
			color_t2.check_max();
		}
		CowRender.SetGlobal2DColor(color_t2);
		anim_back_color = anim_back_color.interp(anim_back_color0, 0.0015f * SCGlobals.ftickperframe);
		anim_back_color.check_max();
		CowRender.SetGlobal2DColor(anim_back_color);
		int num2 = 0;
		curleave = 0;
		float num3 = 0f;
		if (changebacktimer > 0f)
		{
			num3 = 1f - changebacktimer / 6000f;
			if (changebacktimer < 3000f)
			{
				changebackk = 1f - changebacktimer / 3000f;
			}
			else
			{
				changebackk = 0f;
				changebacktimer -= SCGlobals.ftickperframe;
			}
		}
		else
		{
			changebackk = 0f;
			num3 = 0f;
		}
		matrix_t cam = SCGlobals.cam;
		float num4 = num3 * num3 * 30f;
		cam.translate(num4 * (float)Math.Cos(4.712389f * num3), num4 * (float)Math.Sin((float)Math.PI * num3), 0f);
		num4 = num3 * num3;
		cam.scale(1f + num4 * 1.5f, 1f + num4 * 1.5f, 1f);
		CowRender.LoadCamera(cam);
		CowRender.DisableRS(4u);
		CowRender.DisableRS(5u);
		addleavestimer = 0f;
		for (num2 = 0; num2 < curasq; num2++)
		{
			animatedsquare_t as_t = ar_animsquare[num2];
			DrawAnimateSquare(ref as_t);
			ar_animsquare[num2] = as_t;
		}
		CowRender.EnableRS(4u);
		CowRender.SetBlend(0);
		CowRender.BindTexture(curback);
		if (changebacktimer == 0f)
		{
			CowRender.DisableRS(7u);
		}
		CowRender.DrawColorQuad2D(curback.mImage, dcolor_t.ColorWhite, 0f);
		CowRender.EnableRS(7u);
		for (num2 = 0; num2 < curasq; num2++)
		{
			DrawSquareAboveAll(ar_animsquare[num2]);
		}
		DrawSprays();
		DrawLeaves();
		CowRender.SetGlobal2DColor(new color_t(1f, 1f, 1f, 1f));
		CowRender.LoadCamera(SCGlobals.cam);
		if (changebacktimer > 0f && changebacktimer <= 3000f)
		{
			changebacktimer -= SCGlobals.ftickperframe;
			InitAnimateBackFromFile(nextbackindex);
			color_t global2DColor = color_t2;
			global2DColor.a = changebackk;
			CowRender.SetGlobal2DColor(global2DColor);
			curleave = 0;
			CowRender.DisableRS(4u);
			CowRender.DisableRS(5u);
			addleavestimer = 0f;
			CowRender.EnableRS(4u);
			CowRender.SetBlend(0);
			for (num2 = 0; num2 < curasq; num2++)
			{
				animatedsquare_t as_t2 = ar_animsquare[num2];
				DrawAnimateSquare(ref as_t2);
				ar_animsquare[num2] = as_t2;
			}
			CowRender.EnableRS(4u);
			CowRender.SetBlend(0);
			CowRender.BindTexture(nextback);
			if (SuperCow.IsHDTV)
			{
				CowRender.DrawColorQuad2D(curback.mImage, 0f, 0f, 1067f, 600f, dcolor_t.ColorWhite, 0f);
			}
			else
			{
				CowRender.DrawColorQuad2D(curback.mImage, dcolor_t.ColorWhite, 0f);
			}
			for (num2 = 0; num2 < curasq; num2++)
			{
				DrawSquareAboveAll(ar_animsquare[num2]);
			}
			DrawSprays();
			DrawLeaves();
			CowRender.SetGlobal2DColor(color_t.ColorWhite);
			InitAnimateBackFromFile(curbackindex);
			if (changebacktimer <= 0f)
			{
				InitAnimateBackFromFile(nextbackindex);
				curbackindex = nextbackindex;
				curback = nextback;
				changebacktimer = 0f;
			}
		}
	}

	public void InitAnimateBacksAtInitLevel(int numlevel)
	{
		changebacktimer = 0f;
		Store.SetCurrentDirectory("animbacks", theAdd: true);
		int num = numlevel / 5;
		if (numlevel < 0)
		{
			num = -1;
		}
		string name = $"_a_back{numlevel:D}.jpg";
		curbackindex = num + 1;
		curback = SCGlobals.texturehost.LoadTGAJPGWithoutScale(name, 800, 600);
		curasq = 0;
		InitAnimateBackFromFile(numlevel);
		Store.SetCurrentDirectory("", theAdd: false);
	}

	public void LoadNewAnimateBack()
	{
		changebacktimer = 6000f;
		Store.SetCurrentDirectory("animbacks", theAdd: true);
		string name = $"_a_back{nextbackindex:D}.jpg";
		nextback = SCGlobals.texturehost.LoadTGAJPGWithoutScale(name, 800, 600);
		Store.SetCurrentDirectory("", theAdd: true);
	}

	public void InitAnimateBacks()
	{
		Store.SetCurrentDirectory("animbacks", theAdd: true);
		InitAnimBackObjs();
		int len = 0;
		Store.storeLoadFileArray("level0.levv", ref len, b_errorexit: false);
		Store.SetCurrentDirectory("", theAdd: false);
	}

	public void InitAnimateBackFromFile(int num)
	{
		string filename = $"level{num:D}.levv";
		int len = 0;
		byte[] array = Store.storeLoadFileArray(filename, ref len, b_errorexit: false);
		if (array.Length == 0)
		{
			return;
		}
		CBuffer cBuffer = new CBuffer();
		cBuffer.SetDataBuffer(array);
		string text = cBuffer.ReadString(64);
		curasq = (int)cBuffer.ReadLong();
		for (int i = 0; i < curasq; i++)
		{
			text = cBuffer.ReadString(32);
			ar_animsquare[i].type = 0;
			for (int j = 0; j < numanimbackobjs; j++)
			{
				if (ar_animbackobjs[j].name == text)
				{
					ar_animsquare[i].p_go = ar_animbackobjs[j];
				}
			}
			frect_t rect = new frect_t
			{
				x0 = cBuffer.ReadFloat(),
				y0 = cBuffer.ReadFloat(),
				x1 = cBuffer.ReadFloat(),
				y1 = cBuffer.ReadFloat()
			};
			ar_animsquare[i].rect = rect;
			ar_animsquare[i].rotation = cBuffer.ReadFloat();
			vec2_t dtpos = new vec2_t
			{
				x = cBuffer.ReadFloat(),
				y = cBuffer.ReadFloat()
			};
			ar_animsquare[i].dtpos = dtpos;
			ar_animsquare[i].b_flip = cBuffer.ReadBool();
			ar_animsquare[i].laver = (int)cBuffer.ReadLong();
			if (changebacktimer == 0f)
			{
				ar_animsquare[i].timer = CRand.prand(500f);
			}
			ar_animsquare[i].color = dcolor_t.ColorWhite;
			ar_animsquare[i].b_draw = false;
		}
	}

	public void InitAnimateBackFromFile2(string filename)
	{
	}

	public void DrawJpgParticle(vec2_t pos, float rot, float size, dcolor_t color)
	{
		rot = 90f - rot;
		vec3_t vec3_t2 = SCGlobals.cam.up_vector() * size;
		vec3_t vec3_t3 = SCGlobals.cam.right_vector() * size;
		vec3_t vec3_t4 = vec3_t2 * SuperCowHelper.fast_sin(rot) + vec3_t3 * SuperCowHelper.fast_cos(rot);
		vec3_t vec3_t5 = vec3_t2 * SuperCowHelper.fast_cos(rot) - vec3_t3 * SuperCowHelper.fast_sin(rot);
		vec3_t vec3_t6 = new vec3_t(pos.x, pos.y, 0f);
		CowRender.DrawColorQuad(vec3_t6 + vec3_t5 + vec3_t4, vec3_t6 + vec3_t5 - vec3_t4, vec3_t6 - vec3_t5 - vec3_t4, vec3_t6 - vec3_t5 + vec3_t4, color, color, color, color);
	}

	public void AddSpray(vec2_t pos, int type, texture_t tex)
	{
		if (!(changebacktimer > 0f))
		{
			sprays[curspray].p = pos;
			sprays[curspray].type = type;
			sprays[curspray].timer = 2000f;
			sprays[curspray].tex = tex;
			curspray = (curspray + 1) % 256;
		}
	}

	public void DrawSprays()
	{
		_ = SuperCow.DeviceManager.GraphicsDevice;
		CowRender.EnableRS(4u);
		CowRender.SetBlend(1);
		CowRender.BindTexture(spraystex[0]);
		for (int i = 0; i < NUM_SPRAYS; i++)
		{
			if (sprays[i].timer > 0f)
			{
				float timer = sprays[i].timer;
				vec2_t p = sprays[i].p;
				_ = sprays[i].type;
				vec2_t vec2_t2 = new vec2_t(0f, 20f * (float)Math.Sin(timer / 2000f * (float)Math.PI));
				vec2_t2 *= vec2_t2;
				vec2_t2 *= 0.02f;
				float size = (2000f - timer) * 0.011f + 15f;
				float num = 255f * (float)Math.Sin(timer / 2000f * (float)Math.PI);
				int num2 = (int)num;
				dcolor_t color = new dcolor_t((byte)num2, (byte)num2, (byte)num2, byte.MaxValue);
				CowRender.BindTexture(sprays[i].tex);
				DrawJpgParticle(p + vec2_t2, 0f, size, color);
				sprays[i].timer -= SCGlobals.ftickperframe;
			}
		}
	}

	private string memcpy_forstring(byte[] array, int array_ptr, int size)
	{
		byte[] array2 = new byte[size];
		Buffer.BlockCopy(array, array_ptr, array2, 0, size);
		return Encoding.ASCII.GetString(array2, 0, size);
	}

	public void InitAnimBackObjs()
	{
		int len = 0;
		byte[] array = Store.storeLoadFileArray("animbackobjs.txt", ref len, b_errorexit: false);
		Array.Clear(animgroup_names, 0, animgroup_names.Length);
		byte[] array2 = array;
		int num2;
		int num = (num2 = 0);
		while (true)
		{
			if (array2[num] != 91)
			{
				num++;
				if (num == len)
				{
					break;
				}
				continue;
			}
			byte[] array3 = array2;
			num++;
			num2 = num;
			while (array3[num2] != 47)
			{
				num2++;
				if (num2 == len)
				{
					return;
				}
			}
			string text = memcpy_forstring(array, num, num2 - num);
			animgroup_names[numanimbackgroups++] = text;
			num2 += 2;
			while (array3[num2] <= 32)
			{
				num2++;
				if (num2 == len)
				{
					return;
				}
			}
			do
			{
				array2 = array3;
				num = num2;
				while (array3[num2] != 61)
				{
					num2++;
					if (num2 == len)
					{
						return;
					}
				}
				string name = memcpy_forstring(array, num, num2 - num);
				num2++;
				while (array3[num2] <= 32)
				{
					num2++;
					if (num2 == len)
					{
						return;
					}
				}
				animbackobject_t animbackobject_t = new animbackobject_t
				{
					name = name,
					group = numanimbackgroups - 1
				};
				ar_animbackobjs[numanimbackobjs] = animbackobject_t;
				array2 = array3;
				num = num2;
				while (array3[num2] > 32 && array3[num2] != 44)
				{
					num2++;
					if (num2 == len)
					{
						numanimbackobjs++;
						return;
					}
				}
				string text2 = memcpy_forstring(array, num, num2 - num);
				if (text2.Contains(".tga") || text2.Contains(".jpg"))
				{
					animbackobject_t.p_tex = SCGlobals.texturehost.Load(text2);
				}
				else if (text2.Contains(".cgo"))
				{
					animbackobject_t.p_cgo = SCGlobals.geomobjhost.Load(text2);
				}
				ar_animbackobjs[numanimbackobjs] = animbackobject_t;
				while (array3[num2] <= 32)
				{
					num2++;
					if (num2 == len)
					{
						numanimbackobjs++;
						return;
					}
				}
				numanimbackobjs++;
			}
			while (array3[num2] != 91 || array3[++num2] != 47);
			while (array3[num2] > 32)
			{
				num2++;
				if (num2 == len)
				{
					return;
				}
			}
			array2 = array3;
			num = num2;
		}
	}
}
