using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public static class Vertexeffect
{
	public struct cnvertexf_t
	{
		public vec3_t xyz;

		public vec3_t normal;

		public dcolor_t c;

		public vec2_t uv;
	}

	public static int EFFECT_TIME = 3000;

	public static texture_t p_gametexcur;

	public static int WIDTH = 40;

	public static int VWIDTH = 41;

	public static int HEIGHT = 30;

	public static float TILEX = 0.01953125f;

	public static float TILEY = 0.01953125f;

	public static float FWIDTH = 160f;

	public static float FHEIGHT = 120f;

	public static float STEPX = 4f;

	public static float STEPY = 4f;

	public static int i_genum;

	public static float f_ge = -1f;

	public static float f_ge_nt = 0f;

	public static cnvertexf_t[] ar_vertices = new cnvertexf_t[7626];

	public static vec3_t dir_v2;

	public static vec3_t cross_dir_v2;

	public static float init_r_v2;

	public static vec3_t corner_v2;

	public static bool b_up_v2;

	public static vec3_t[,] ar_pos_v4 = new vec3_t[40, 30];

	public static float[,] ar_time_v4 = new float[40, 30];

	public static vec3_t[,] ar_speed_v4 = new vec3_t[40, 30];

	public static vec3_t gravity_v4;

	public static vec3_t[,] ar_pos_v6 = new vec3_t[40, 30];

	public static float[,] ar_time_v6 = new float[40, 30];

	public static vec3_t[,] ar_speed_v6 = new vec3_t[40, 30];

	public static vec3_t gravity_v6;

	public static float wave_v5;

	public static float amp_v5;

	public static float speed1_v5;

	public static float speed2_v5;

	public static float w1_v5;

	public static float w2_v5;

	public static void SetVertex(int i, int j, vec3_t pos, dcolor_t col)
	{
		cnvertexf_t cnvertexf_t = default(cnvertexf_t);
		cnvertexf_t.xyz = pos;
		cnvertexf_t.c = col;
		cnvertexf_t.normal.assign(0f, 0f, 1f);
		cnvertexf_t.uv.assign(TILEX * (float)i, TILEY * (float)j);
		if (i < WIDTH && j < HEIGHT)
		{
			ar_vertices[(j * WIDTH + i) * 6] = cnvertexf_t;
		}
		if (i > 0 && j > 0)
		{
			ar_vertices[((j - 1) * WIDTH + i - 1) * 6 + 5] = cnvertexf_t;
		}
		if (i > 0)
		{
			ar_vertices[(j * WIDTH + i - 1) * 6 + 1] = cnvertexf_t;
			ar_vertices[(j * WIDTH + i - 1) * 6 + 4] = cnvertexf_t;
		}
		if (j > 0)
		{
			ar_vertices[((j - 1) * WIDTH + i) * 6 + 2] = cnvertexf_t;
			ar_vertices[((j - 1) * WIDTH + i) * 6 + 3] = cnvertexf_t;
		}
	}

	public static void SetQuard(int i, int j, vec3_t pos, dcolor_t dc)
	{
		float num = STEPX / 2f;
		new vec3_t(pos + new vec3_t(0f - num, num, 0f));
		new vec3_t(pos + new vec3_t(num, num, 0f));
		new vec3_t(pos + new vec3_t(num, 0f - num, 0f));
		new vec3_t(pos + new vec3_t(0f - num, 0f - num, 0f));
		cnvertexf_t cnvertexf_t = ar_vertices[(j * WIDTH + i) * 6];
		ar_vertices[(j * WIDTH + i) * 6] = cnvertexf_t;
	}

	public static void VertexEffect01()
	{
		dcolor_t col = default(dcolor_t);
		vec3_t pos = default(vec3_t);
		vec2_t vec2_t2 = default(vec2_t);
		float num = f_ge_nt * (1f - f_ge_nt);
		for (int i = 0; i <= HEIGHT; i++)
		{
			for (int j = 0; j <= WIDTH; j++)
			{
				vec2_t2.assign((0f - FWIDTH) / 2f + (float)j * STEPX, FHEIGHT / 2f - (float)i * STEPY);
				float num2 = vec2_t2.len();
				pos.assign(vec2_t2.x, vec2_t2.y, 0f);
				float num3 = (float)Math.Sin(num2 / FHEIGHT * (float)Math.PI * 10f - f_ge_nt * (float)Math.PI * 2f * 8f);
				pos.z = 12f * num3 * num;
				pos.z -= 20f * f_ge_nt;
				float num4 = 0.5f * num3 * num;
				col.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255f * (1f - f_ge_nt + num4)));
				SetVertex(j, i, pos, col);
			}
		}
		CowRender.BindTexture(p_gametexcur);
		CowRender.DrawVertexArray(ar_vertices, WIDTH * HEIGHT * 2, 322uL, PrimitiveType.TriangleList);
	}

	public static void VertexEffect02()
	{
		float num = FWIDTH * 1.3f;
		if (f_ge_nt == 0f)
		{
			init_r_v2 = 15f + CRand.prand(5f);
			float num2 = ((CRand.irand(2) == 1) ? 1f : (-1f));
			dir_v2.assign(num2 * (0.1f + CRand.prand(1f)), (0f - num2) * (0.1f + CRand.prand(1f)), 0f);
			dir_v2.normalize();
			b_up_v2 = dir_v2.y < 0f;
			if (dir_v2.x >= 0f)
			{
				corner_v2.x = (0f - FWIDTH) / 2f;
			}
			else
			{
				corner_v2.x = FWIDTH / 2f;
			}
			if (dir_v2.y > 0f)
			{
				corner_v2.y = (0f - FHEIGHT) / 2f;
			}
			else
			{
				corner_v2.y = FHEIGHT / 2f;
			}
			corner_v2.z = 0f;
			cross_dir_v2 = dir_v2.cross(new vec3_t(0f, 0f, 1f));
			cross_dir_v2.normalize();
		}
		dcolor_t col = default(dcolor_t);
		vec3_t vec3_t2 = default(vec3_t);
		vec3_t vec3_t3 = corner_v2;
		vec3_t3 += dir_v2 * num * f_ge_nt;
		vec3_t vec3_t4 = new vec3_t(vec3_t3);
		vec3_t vec3_t5 = new vec3_t(vec3_t3 + cross_dir_v2);
		float num3 = (vec3_t5.y - vec3_t4.y) / (vec3_t5.x - vec3_t4.x);
		float num4 = (0f - vec3_t4.x) * num3 + vec3_t4.y;
		col.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		vec3_t vec3_t6 = default(vec3_t);
		for (int i = 0; i <= HEIGHT; i++)
		{
			for (int j = 0; j <= WIDTH; j++)
			{
				vec3_t6.assign((float)j * STEPX, (float)(-i) * STEPY, 0f);
				vec3_t2.assign((0f - FWIDTH) / 2f + (float)j * STEPX, FHEIGHT / 2f - (float)i * STEPY, 0f);
				if (b_up_v2 ? (vec3_t2.x * num3 + num4 <= vec3_t2.y) : (vec3_t2.x * num3 + num4 > vec3_t2.y))
				{
					float num5 = vec3_t6.dot(dir_v2) / num * 5f + init_r_v2;
					float num6 = (vec3_t3 - vec3_t2).dot(dir_v2);
					float num7 = num6 / num5;
					vec3_t2 = vec3_t2 + dir_v2 * num6 - new vec3_t(0f, 0f, num5);
					float num8 = (float)Math.Cos(num7);
					vec3_t2 -= dir_v2 * (float)Math.Sin(num7) * num5 - new vec3_t(0f, 0f, 1f) * num8 * num5;
					num8 = Math.Abs(num8) * 0.8f + 0.2f;
					byte b = (byte)(255f * num8);
					col.assign(b, b, b, byte.MaxValue);
				}
				else
				{
					col.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				}
				SetVertex(j, i, vec3_t2, col);
			}
		}
		CowRender.BindTexture(p_gametexcur);
		CowRender.DrawVertexArray(ar_vertices, WIDTH * HEIGHT * 2, 322uL, PrimitiveType.TriangleList);
	}

	public static void VertexEffect03()
	{
		vec3_t vec3_t2 = default(vec3_t);
		vec3_t vec3_t3 = default(vec3_t);
		vec3_t vec3_t4 = default(vec3_t);
		vec3_t vec3_t5 = default(vec3_t);
		vec3_t2.assign((0f - FWIDTH) / 2f, FHEIGHT / 2f, 0f);
		vec3_t3.assign(FWIDTH / 2f, FHEIGHT / 2f, 0f);
		vec3_t4.assign(FWIDTH / 2f, (0f - FHEIGHT) / 2f, 0f);
		vec3_t5.assign((0f - FWIDTH) / 2f, (0f - FHEIGHT) / 2f, 0f);
		vec3_t vec3_t6 = default(vec3_t);
		dcolor_t col = default(dcolor_t);
		float num = 0.1f;
		float num2 = (float)Math.Sqrt(f_ge_nt);
		float num3 = ((!(num2 >= num)) ? 0f : (FWIDTH * (num2 - num) * 0.7f));
		float num4 = FWIDTH * num2 * 0.7f;
		vec3_t v = default(vec3_t);
		new vec3_t(0f, 0f, 0f);
		for (int i = 0; i <= HEIGHT; i++)
		{
			for (int j = 0; j <= WIDTH; j++)
			{
				vec3_t6.assign((0f - FWIDTH) / 2f + (float)j * STEPX, FHEIGHT / 2f - (float)i * STEPY, 0f);
				vec3_t vec3_t7 = vec3_t6;
				vec3_t7.normalize();
				vec3_t6 = vec3_t6.interp(v, f_ge_nt * 0.75f);
				col.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				float num5 = vec3_t6.len();
				col.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				if (num5 < num4)
				{
					if (num5 > num3)
					{
						float num6 = (num5 - num3) / (num4 - num3);
						byte aa = (byte)(255f * num6);
						col.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, aa);
					}
					else
					{
						col.a = 0;
					}
				}
				SetVertex(j, i, vec3_t6, col);
			}
		}
		_ = SuperCow.DeviceManager.GraphicsDevice;
		CowRender.BindTexture(p_gametexcur);
		CowRender.EnableRS(4u);
		CowRender.SetBlend(0);
		CowRender.DrawVertexArray(ar_vertices, WIDTH * HEIGHT * 2, 322uL, PrimitiveType.TriangleList);
		CowRender.DisableRS(4u);
	}

	public static void VertexEffect04()
	{
		dcolor_t colorWhite = dcolor_t.ColorWhite;
		vec3_t vec3_t2 = default(vec3_t);
		if (f_ge_nt == 0f)
		{
			gravity_v4.assign(0f, -3f, 0f);
			for (int i = 0; i < HEIGHT; i++)
			{
				for (int j = 0; j < WIDTH; j++)
				{
					vec3_t2.assign((0f - FWIDTH) / 2f + (float)j * STEPX + STEPX / 2f, FHEIGHT / 2f - (float)i * STEPY - STEPY / 2f, 0f);
					float num = 1f - vec3_t2.len() / (FWIDTH * 1.2f);
					ar_time_v4[j, i] = num * 0.4f;
					ar_pos_v4[j, i] = vec3_t2;
					ar_speed_v4[j, i].assign(0f, 5f, -3f);
				}
			}
		}
		float num2 = FWIDTH * 1.3f;
		float num3 = num2 * (f_ge_nt * 2f);
		if (num3 < 0f)
		{
			num3 = 0f;
		}
		for (int k = 0; k < HEIGHT; k++)
		{
			for (int l = 0; l < WIDTH; l++)
			{
				colorWhite.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				vec3_t2 = ar_pos_v4[l, k];
				if (f_ge_nt > ar_time_v4[l, k])
				{
					vec3_t vec3_t3 = ar_speed_v4[l, k];
					vec3_t2 += vec3_t3 * SCGlobals.ftickperframe * 0.005f;
					vec3_t3 += gravity_v4 * SCGlobals.ftickperframe * 0.005f;
					ar_pos_v4[l, k] = vec3_t2;
					ar_speed_v4[l, k] = vec3_t3;
				}
				SetQuard(l, k, vec3_t2, colorWhite);
			}
		}
		_ = SuperCow.DeviceManager.GraphicsDevice;
		CowRender.BindTexture(p_gametexcur);
		CowRender.EnableRS(4u);
		CowRender.SetBlend(0);
		CowRender.DrawVertexArray(ar_vertices, WIDTH * HEIGHT * 2, 322uL, PrimitiveType.TriangleList);
		CowRender.DisableRS(4u);
	}

	public static void VertexEffect05()
	{
		dcolor_t colorWhite = dcolor_t.ColorWhite;
		vec3_t pos = default(vec3_t);
		if (f_ge_nt == 0f)
		{
			wave_v5 = (float)((CRand.irand(2) == 1) ? 1 : (-1)) * (float)Math.PI * (1f + CRand.prand(4f));
			amp_v5 = 20f + CRand.prand(20f);
			speed1_v5 = (float)((CRand.irand(2) == 1) ? 1 : (-1)) * (float)Math.PI * (1f + CRand.prand(6f));
			speed2_v5 = (float)((CRand.irand(2) == 1) ? 1 : (-1)) * (float)Math.PI * (1f + CRand.prand(6f));
			w1_v5 = CRand.prand((float)Math.PI);
			w2_v5 = CRand.prand((float)Math.PI);
		}
		float num = f_ge_nt * (1f - f_ge_nt);
		float num2 = 1f - f_ge_nt * f_ge_nt;
		float num3 = (float)Math.Sqrt(f_ge_nt);
		for (int i = 0; i <= HEIGHT; i++)
		{
			for (int j = 0; j <= WIDTH; j++)
			{
				pos.assign((0f - FWIDTH) / 2f + (float)j * STEPX, FHEIGHT / 2f - (float)i * STEPY, 0f);
				byte aa = (byte)(255f * num2);
				float num4 = f_ge_nt * speed1_v5;
				float num5 = f_ge_nt * speed2_v5;
				pos.z = (float)Math.Sin(pos.x / FWIDTH * wave_v5 + num4 + w1_v5) * (float)Math.Cos(pos.y / FHEIGHT * wave_v5 + num5 + w2_v5) * amp_v5 * num;
				byte b = (byte)(255f * (1f - ((float)Math.Sin(pos.x / FWIDTH * wave_v5 + num4 + w2_v5 + (float)Math.PI / 2f) * (float)Math.Cos(pos.y / FHEIGHT * wave_v5 + num5 + w2_v5 + (float)Math.PI / 2f) + 1f) * 0.7f * num));
				colorWhite.assign(b, b, b, aa);
				pos.z -= num3 * 20f;
				SetVertex(j, i, pos, colorWhite);
			}
		}
		_ = SuperCow.DeviceManager.GraphicsDevice;
		CowRender.BindTexture(p_gametexcur);
		CowRender.DisableRS(1u);
		CowRender.EnableRS(4u);
		CowRender.SetBlend(0);
		CowRender.DrawVertexArray(ar_vertices, WIDTH * HEIGHT * 2, 322uL, PrimitiveType.TriangleList);
		CowRender.DisableRS(4u);
	}

	public static void VertexEffect06()
	{
		float num = FWIDTH * 1.3f;
		dcolor_t colorWhite = dcolor_t.ColorWhite;
		vec3_t vec3_t2 = default(vec3_t);
		if (f_ge_nt == 0f)
		{
			gravity_v6.assign(0f, -3f, 0f);
			for (int i = 0; i <= HEIGHT; i++)
			{
				for (int j = 0; j <= WIDTH; j++)
				{
					vec3_t2.assign((0f - FWIDTH) / 2f + (float)j * STEPX, FHEIGHT / 2f - (float)i * STEPY, 0f);
					float num2 = 1f - vec3_t2.len() / (FWIDTH * 1.2f);
					ar_time_v6[j, i] = num2 * 0.4f;
					ar_pos_v6[j, i] = vec3_t2;
					ar_speed_v6[j, i].assign(0f, 5f, -3f);
				}
			}
		}
		float num3 = num * (f_ge_nt * 2f);
		if (num3 < 0f)
		{
			num3 = 0f;
		}
		for (int i = 0; i <= HEIGHT; i++)
		{
			for (int j = 0; j <= WIDTH; j++)
			{
				colorWhite.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				vec3_t2 = ar_pos_v6[j, i];
				if (f_ge_nt > ar_time_v6[j, i])
				{
					ar_pos_v6[j, i] = vec3_t2;
				}
				ar_pos_v6[j, i] = vec3_t2;
				SetVertex(j, i, vec3_t2, colorWhite);
			}
		}
		_ = SuperCow.DeviceManager.GraphicsDevice;
		CowRender.BindTexture(p_gametexcur);
		CowRender.EnableRS(4u);
		CowRender.SetBlend(0);
		CowRender.DrawVertexArray(ar_vertices, WIDTH * HEIGHT * 2, 322uL, PrimitiveType.TriangleList);
		CowRender.DisableRS(4u);
	}

	public static void EndVertexEffect()
	{
		f_ge = -1f;
	}

	public static void StartVertexEffect(int n)
	{
		i_genum = n;
		f_ge = 0f;
	}

	public static void CalculateVertexEffect()
	{
		if (!(f_ge < 0f))
		{
			f_ge_nt = f_ge / (float)EFFECT_TIME;
			f_ge += SCGlobals.ftickperframe;
			if (f_ge > (float)EFFECT_TIME)
			{
				f_ge = -1f;
			}
		}
	}

	public static void DrawVertexEffect()
	{
		if (!(f_ge < 0f))
		{
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			CowRender.EnableRS(4u);
			CowRender.DisableRS(5u);
			CowRender.SetBlend(0);
			graphicsDevice.RasterizerState = RasterizerState.CullNone;
			CowRender.EnableRS(1u);
			CowRender.EnableRS(2u);
			graphicsDevice.Clear(ClearOptions.DepthBuffer, Color.Black, 1f, 0);
			SCGlobals.cam.load_identity();
			SCGlobals.cam.translate(0f, 0f, 144f);
			CowRender.LoadCamera(SCGlobals.cam);
			CowRender.SetProjection(SCGlobals.projection);
			if (i_genum == 1)
			{
				VertexEffect01();
			}
			else if (i_genum == 2)
			{
				VertexEffect02();
			}
			else if (i_genum == 3)
			{
				VertexEffect03();
			}
			else if (i_genum == 4)
			{
				VertexEffect04();
			}
			else if (i_genum == 5)
			{
				VertexEffect05();
			}
			else if (i_genum == 6)
			{
				VertexEffect06();
			}
			SCGlobals.cam.load_identity();
			SCGlobals.cam.translate(0f, 0f, 150f);
			CowRender.LoadCamera(SCGlobals.cam);
			CowRender.SetOrtho(800f, 600f, 1f, 300f);
			CowRender.EnableRS(4u);
			CowRender.EnableRS(5u);
			CowRender.SetBlend(0);
			CowRender.DisableRS(1u);
			CowRender.DisableRS(2u);
		}
	}
}
