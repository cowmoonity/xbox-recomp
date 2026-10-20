using System;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public static class Drawpers
{
	public static int WIDTH = 30;

	public static int HEIGHT = 30;

	public static float TILEX = 1f / 30f;

	public static float TILEY = 1f / 30f;

	public static float FWIDTH = 102.1f;

	public static float FHEIGHT = 102.1f;

	public static float STEPX = 3.4033332f;

	public static float STEPY = 3.4033332f;

	public static float f_pers_timer;

	public static float f_pers_timer_nt;

	public static int i_pers_effect_type = 0;

	public static matrix_t pers_matrix;

	public static color_t pers_col;

	public static float wave;

	public static float amp;

	public static float speed1;

	public static float speed2;

	public static float w1;

	public static float w2;

	public static Vertexeffect.cnvertexf_t[] ar_pers_vert = new Vertexeffect.cnvertexf_t[5766];

	public static void SetPersVertex(int i, int j, vec3_t pos, dcolor_t col)
	{
		Vertexeffect.cnvertexf_t cnvertexf_t = default(Vertexeffect.cnvertexf_t);
		cnvertexf_t.xyz = pos;
		cnvertexf_t.c.r = (byte)((float)(int)col.r * pers_col.r);
		cnvertexf_t.c.g = (byte)((float)(int)col.g * pers_col.g);
		cnvertexf_t.c.b = (byte)((float)(int)col.b * pers_col.b);
		cnvertexf_t.c.a = (byte)((float)(int)col.a * pers_col.a);
		cnvertexf_t.normal.assign(0f, 0f, 1f);
		cnvertexf_t.uv.assign(TILEX * (float)i, TILEY * (float)j);
		if (i < WIDTH && j < HEIGHT)
		{
			ar_pers_vert[(j * WIDTH + i) * 6] = cnvertexf_t;
		}
		if (i > 0 && j > 0)
		{
			ar_pers_vert[((j - 1) * WIDTH + i - 1) * 6 + 5] = cnvertexf_t;
		}
		if (i > 0)
		{
			ar_pers_vert[(j * WIDTH + i - 1) * 6 + 1] = cnvertexf_t;
			ar_pers_vert[(j * WIDTH + i - 1) * 6 + 4] = cnvertexf_t;
		}
		if (j > 0)
		{
			ar_pers_vert[((j - 1) * WIDTH + i) * 6 + 2] = cnvertexf_t;
			ar_pers_vert[((j - 1) * WIDTH + i) * 6 + 3] = cnvertexf_t;
		}
	}

	public static void PersInit()
	{
		vec3_t pos = default(vec3_t);
		dcolor_t col = default(dcolor_t);
		col.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		for (int i = 0; i <= HEIGHT; i++)
		{
			for (int j = 0; j <= WIDTH; j++)
			{
				pos.assign((0f - FWIDTH) / 2f + (float)j * STEPX, FHEIGHT / 2f - (float)i * STEPY, 0f);
				SetPersVertex(j, i, pos, col);
			}
		}
	}

	public static void PersCalculate()
	{
		dcolor_t col = default(dcolor_t);
		vec2_t vec2_t2 = default(vec2_t);
		vec3_t pos = default(vec3_t);
		f_pers_timer_nt = f_pers_timer / 3000f;
		if (i_pers_effect_type == 0)
		{
			PersInit();
		}
		else if (i_pers_effect_type == 1)
		{
			float num = f_pers_timer_nt * (1f - f_pers_timer_nt);
			for (int i = 0; i <= HEIGHT; i++)
			{
				for (int j = 0; j <= WIDTH; j++)
				{
					vec2_t2.assign((0f - FWIDTH) / 2f + (float)j * STEPX, FHEIGHT / 2f - (float)i * STEPY);
					pos.assign(vec2_t2.x, vec2_t2.y, 0f);
					float num2 = vec2_t2.len();
					float num3 = (float)Math.Sin(num2 / FHEIGHT * (float)Math.PI * 10f - f_pers_timer_nt * (float)Math.PI * 2f * 8f);
					pos.z = 8f * num3 * num;
					float num4 = 0.5f * num3 * num;
					col.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255f * (f_pers_timer_nt + num4)));
					SetPersVertex(j, i, pos, col);
				}
			}
		}
		else if (i_pers_effect_type == 2)
		{
			col.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			if (f_pers_timer_nt == 0f)
			{
				wave = (float)((CRand.irand(2) == 1) ? 1 : (-1)) * (float)Math.PI * (1f + CRand.prand(4f));
				amp = 20f + CRand.prand(10f);
				speed1 = (float)((CRand.irand(2) == 1) ? 1 : (-1)) * (float)Math.PI * (1f + CRand.prand(6f));
				speed2 = (float)((CRand.irand(2) == 1) ? 1 : (-1)) * (float)Math.PI * (1f + CRand.prand(6f));
				w1 = CRand.prand((float)Math.PI);
				w2 = CRand.prand((float)Math.PI);
			}
			float num = f_pers_timer_nt * (1f - f_pers_timer_nt);
			float num5 = (float)Math.Sqrt(f_pers_timer_nt);
			Math.Sqrt(f_pers_timer_nt);
			for (int i = 0; i <= HEIGHT; i++)
			{
				for (int j = 0; j <= WIDTH; j++)
				{
					pos.assign((0f - FWIDTH) / 2f + (float)j * STEPX, FHEIGHT / 2f - (float)i * STEPY, 0f);
					byte aa = (byte)(255f * num5);
					float num6 = f_pers_timer_nt * speed1;
					float num7 = f_pers_timer_nt * speed2;
					pos.z = (float)Math.Sin(pos.x / FWIDTH * wave + num6 + w1) * (float)Math.Cos(pos.y / FHEIGHT * wave + num7 + w2) * amp * num;
					byte b = (byte)(255f * (1f - ((float)Math.Sin(pos.x / FWIDTH * wave + num6 + w2 + (float)Math.PI / 2f) * (float)Math.Cos(pos.y / FHEIGHT * wave + num7 + w2 + (float)Math.PI / 2f) + 1f) * 0.7f * num));
					col.assign(b, b, b, aa);
					SetPersVertex(j, i, pos, col);
				}
			}
		}
		else if (i_pers_effect_type == 3)
		{
			float num = f_pers_timer_nt * (1f - f_pers_timer_nt);
			for (int i = 0; i <= HEIGHT; i++)
			{
				for (int j = 0; j <= WIDTH; j++)
				{
					vec2_t2.assign((0f - FWIDTH) / 2f + (float)j * STEPX, FHEIGHT / 2f - (float)i * STEPY);
					pos.assign(vec2_t2.x, vec2_t2.y, 0f);
					float num2 = vec2_t2.len();
					float num3 = (float)Math.Sin(num2 / FHEIGHT * (float)Math.PI * 10f - f_pers_timer_nt * (float)Math.PI * 2f * 8f);
					pos.z = 8f * num3 * num;
					float num4 = 0.5f * num3 * num;
					col.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255f * (1f - f_pers_timer_nt - num4)));
					SetPersVertex(j, i, pos, col);
				}
			}
		}
		else if (i_pers_effect_type == 4)
		{
			col.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			if (f_pers_timer_nt == 0f)
			{
				wave = (float)((CRand.irand(2) == 1) ? 1 : (-1)) * (float)Math.PI * (1f + CRand.prand(4f));
				amp = 20f + CRand.prand(10f);
				speed1 = (float)((CRand.irand(2) == 1) ? 1 : (-1)) * (float)Math.PI * (1f + CRand.prand(6f));
				speed2 = (float)((CRand.irand(2) == 1) ? 1 : (-1)) * (float)Math.PI * (1f + CRand.prand(6f));
				w1 = CRand.prand((float)Math.PI);
				w2 = CRand.prand((float)Math.PI);
			}
			float num = f_pers_timer_nt * (1f - f_pers_timer_nt);
			float num5 = (float)Math.Sqrt(f_pers_timer_nt);
			Math.Sqrt(f_pers_timer_nt);
			for (int i = 0; i <= HEIGHT; i++)
			{
				for (int j = 0; j <= WIDTH; j++)
				{
					pos.assign((0f - FWIDTH) / 2f + (float)j * STEPX, FHEIGHT / 2f - (float)i * STEPY, 0f);
					float num6 = f_pers_timer_nt * speed1;
					float num7 = f_pers_timer_nt * speed2;
					pos.z = (float)Math.Sin(pos.x / FWIDTH * wave + num6 + w1) * (float)Math.Cos(pos.y / FHEIGHT * wave + num7 + w2) * amp * num;
					byte aa = (byte)(255f * (1f - num5));
					byte b = (byte)(255f * (1f - ((float)Math.Sin(pos.x / FWIDTH * wave + num6 + w2 + (float)Math.PI / 2f) * (float)Math.Cos(pos.y / FHEIGHT * wave + num7 + w2 + (float)Math.PI / 2f) + 1f) * 0.7f * num));
					col.assign(b, b, b, aa);
					SetPersVertex(j, i, pos, col);
				}
			}
		}
		f_pers_timer += SCGlobals.ftickperframe;
	}

	public static void PersDraw()
	{
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		CowRender.SetBlend(0);
		CowRender.EnableRS(4u);
		CowRender.DisableRS(5u);
		graphicsDevice.RasterizerState = RasterizerState.CullNone;
		CowRender.DisableRS(1u);
		CowRender.DisableRS(2u);
		SCGlobals.cam.load_identity();
		SCGlobals.cam.translate(0f, 0f, 144f);
		CowRender.LoadCamera(SCGlobals.cam);
		CowRender.LoadMatrix(pers_matrix);
		CowRender.SetProjection(SCGlobals.projection);
		PersCalculate();
		CowRender.BindTexture(Comics.p_curpers);
		CowRender.DrawVertexArray(ar_pers_vert, WIDTH * HEIGHT * 2, 322uL, PrimitiveType.TriangleList);
		SCGlobals.cam.load_identity();
		SCGlobals.cam.translate(0f, 0f, 150f);
		CowRender.LoadCamera(SCGlobals.cam);
		CowRender.LoadMatrixNull();
		CowRender.SetOrtho(800f, 600f, 1f, 300f);
		CowRender.EnableRS(5u);
	}

	public static void PersSetEffect(int i)
	{
		i_pers_effect_type = i;
		f_pers_timer = 0f;
	}

	public static void PersSetParams(matrix_t m, color_t col)
	{
		pers_matrix = m;
		pers_col = col;
	}
}
