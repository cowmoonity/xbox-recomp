using System;

namespace SuperCow;

public class Ground
{
	public static int[,,] ar_ground = new int[6, 256, 64];

	public static int[,,] ar_groundtype = new int[6, 256, 64];

	public static int[,,] ar_ground_buf = new int[6, 256, 64];

	public static int[,,] ar_groundtype_buf = new int[6, 256, 64];

	public static int[,,] ar_groundselection = new int[6, 256, 64];

	public static int p_curground;

	public static int p_curgroundtype;

	public static byte curgroundtype;

	public static int curgroundPtr = 0;

	public static bool[,,] ar_groundoverlap = new bool[30, 20, 6];

	public static texture_t[] ar_groundtex = new texture_t[5];

	public static int[,,] ar_grounditemmask = new int[15, 3, 3]
	{
		{
			{ 1, 1, 1 },
			{ 1, 1, 1 },
			{ 1, 1, 1 }
		},
		{
			{ 0, 1, 1 },
			{ 1, 1, 1 },
			{ 1, 1, 1 }
		},
		{
			{ 1, 1, 0 },
			{ 1, 1, 1 },
			{ 1, 1, 1 }
		},
		{
			{ 1, 1, 1 },
			{ 1, 1, 1 },
			{ 0, 1, 1 }
		},
		{
			{ 1, 1, 1 },
			{ 1, 1, 1 },
			{ 1, 1, 0 }
		},
		{
			{ 2, 1, 1 },
			{ 0, 1, 1 },
			{ 2, 1, 1 }
		},
		{
			{ 1, 1, 2 },
			{ 1, 1, 0 },
			{ 1, 1, 2 }
		},
		{
			{ 1, 1, 1 },
			{ 1, 1, 1 },
			{ 2, 0, 2 }
		},
		{
			{ 2, 0, 2 },
			{ 1, 1, 1 },
			{ 1, 1, 1 }
		},
		{
			{ 2, 1, 1 },
			{ 0, 1, 1 },
			{ 3, 0, 2 }
		},
		{
			{ 1, 1, 2 },
			{ 1, 1, 0 },
			{ 2, 0, 3 }
		},
		{
			{ 3, 0, 2 },
			{ 0, 1, 1 },
			{ 2, 1, 1 }
		},
		{
			{ 2, 0, 3 },
			{ 1, 1, 0 },
			{ 1, 1, 2 }
		},
		{
			{ 0, 1, 1 },
			{ 1, 1, 1 },
			{ 1, 1, 0 }
		},
		{
			{ 1, 1, 0 },
			{ 1, 1, 1 },
			{ 0, 1, 1 }
		}
	};

	public static frect_t[] ar_groundcentertexcoords = new frect_t[4]
	{
		new frect_t(0.001953125f, 0.001953125f, 0.24804688f, 0.24804688f),
		new frect_t(0.25195312f, 0.001953125f, 0.49804688f, 0.24804688f),
		new frect_t(0.001953125f, 0.25195312f, 0.24804688f, 0.49804688f),
		new frect_t(0.25195312f, 0.25195312f, 0.49804688f, 0.49804688f)
	};

	public static frect_t[] ar_groundtexcoords = new frect_t[13]
	{
		new frect_t(0.001953125f, 0.001953125f, 0.24804688f, 0.24804688f),
		new frect_t(0.5019531f, 0.001953125f, 0.7480469f, 0.24804688f),
		new frect_t(0.7519531f, 0.001953125f, 0.9980469f, 0.24804688f),
		new frect_t(0.5019531f, 0.25195312f, 0.7480469f, 0.49804688f),
		new frect_t(0.7519531f, 0.25195312f, 0.9980469f, 0.49804688f),
		new frect_t(0.5019531f, 0.5019531f, 0.7480469f, 0.7480469f),
		new frect_t(0.7519531f, 0.5019531f, 0.9980469f, 0.7480469f),
		new frect_t(0.7519531f, 0.7519531f, 0.9980469f, 0.9980469f),
		new frect_t(0.5019531f, 0.7519531f, 0.7480469f, 0.9980469f),
		new frect_t(0.001953125f, 0.7519531f, 0.24804688f, 0.9980469f),
		new frect_t(0.25195312f, 0.7519531f, 0.49804688f, 0.9980469f),
		new frect_t(0.001953125f, 0.5019531f, 0.24804688f, 0.7480469f),
		new frect_t(0.25195312f, 0.5019531f, 0.49804688f, 0.7480469f)
	};

	public static void LoadGround()
	{
		Store.SetCurrentDirectory("levelobjs", theAdd: false);
		for (int i = 0; i < 5; i++)
		{
			string name = $"ground{i + 1:D}.tga";
			ref texture_t reference = ref ar_groundtex[i];
			reference = SCGlobals.texturehost.Load(name);
		}
		Store.SetCurrentDirectory("", theAdd: false);
		for (int j = 0; j < 6; j++)
		{
			for (int k = 0; k < 256; k++)
			{
				for (int l = 0; l < 64; l++)
				{
					ar_ground_buf[j, k, l] = -2;
					ar_groundtype_buf[j, k, l] = -2;
				}
			}
		}
	}

	public static void InitGround()
	{
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 256; j++)
			{
				for (int k = 0; k < 64; k++)
				{
					ar_ground[i, j, k] = -2;
					ar_groundtype[i, j, k] = -2;
				}
			}
		}
		curgroundPtr = 1;
		p_curground = ar_ground[curgroundPtr, 0, 0];
		p_curgroundtype = ar_groundtype[curgroundPtr, 0, 0];
	}

	public static void DrawGroundBrush(vec2_t p, bool b_del)
	{
		vec2_t vec2_t2 = new vec2_t
		{
			x = (float)Math.Floor(p.x / 4f),
			y = (float)Math.Ceiling(p.y / 4f)
		};
		int num = (int)vec2_t2.x - 1 + 128;
		int num2 = (int)vec2_t2.y + 1 + 32;
		int num3 = (int)vec2_t2.x + 1 + 128;
		int num4 = (int)vec2_t2.y - 1 + 32;
		int num5 = (num + num3) / 2;
		int num6 = (num2 + num4) / 2;
		if (num5 >= 0 && num6 >= 0 && num5 < 256 && num6 < 64)
		{
			CowRender.BindTexture(SCGlobals.p_white);
			frect_t vc = default(frect_t);
			dcolor_t c = new dcolor_t(byte.MaxValue, 100, 100, 100);
			num3--;
			num4++;
			if (b_del)
			{
				num++;
				num2--;
			}
			vc.assign(4f * (float)(-128 + num), 4f * (float)(-32 + num2), 4f * (float)(-128 + num3 + 1), 4f * (float)(-32 + num4 - 1));
			CowRender.DrawColorQuad2D(vc, new frect_t(0f, 0f, 1f, 1f), c, 0f);
		}
	}

	public static void RecalculateGround()
	{
		for (int i = 0; i < 6; i++)
		{
			p_curground = ar_ground[i, 0, 0];
			curgroundPtr = i;
			for (int j = 1; j < 255; j++)
			{
				for (int k = 1; k < 63; k++)
				{
					SetGroundSquare(j, k);
				}
			}
		}
	}

	public static bool ISM(int dx, int dy, int gi, int x, int y)
	{
		bool flag = ar_grounditemmask[gi, -(dy - 1), 1 + dx] == 1;
		bool flag2 = ar_ground[curgroundPtr, x + dx, y + dy] >= 0;
		bool flag3 = ar_grounditemmask[gi, -(dy - 1), 1 + dx] == 0;
		bool flag4 = ar_ground[curgroundPtr, x + dx, y + dy] < 0;
		return flag ? flag2 : (!flag3 || flag4);
	}

	public static void SetGroundSquare(int x, int y)
	{
		bool flag = false;
		for (int i = 0; i < 13; i++)
		{
			if (ISM(-1, 1, i, x, y) && ISM(0, 1, i, x, y) && ISM(1, 1, i, x, y) && ISM(-1, 0, i, x, y) && ISM(0, 0, i, x, y) && ISM(1, 0, i, x, y) && ISM(-1, -1, i, x, y) && ISM(0, -1, i, x, y) && ISM(1, -1, i, x, y))
			{
				ar_ground[curgroundPtr, x, y] = i;
				if (ar_groundtype[curgroundPtr, x, y] < 0)
				{
					ar_groundtype[curgroundPtr, x, y] = curgroundtype;
				}
				flag = true;
				break;
			}
		}
		if (flag || ar_ground[curgroundPtr, x, y] < 0)
		{
			return;
		}
		ar_ground[curgroundPtr, x, y] = -1;
		for (int j = x - 1; j <= x + 1; j++)
		{
			for (int num = y + 1; num >= y - 1; num--)
			{
				ar_ground[curgroundPtr, x, y] = 1;
				if (ar_groundtype[curgroundPtr, x, y] < 0)
				{
					ar_groundtype[curgroundPtr, x, y] = curgroundtype;
				}
			}
		}
	}

	public static bool GroundOverlapAllScreen()
	{
		Array.Clear(ar_groundoverlap, 0, ar_groundoverlap.Length);
		int num = 7;
		int num2 = 5;
		bool[,] array = new bool[20, 20];
		for (int i = 0; i < 6; i++)
		{
			SuperCowGame.SetGameCameraByPlane(SCGlobals.camerapos, i);
			vec2_t p = SCGlobals.viewport_rect.p0;
			vec2_t p2 = SCGlobals.viewport_rect.p1;
			p.x = (float)Math.Floor(p.x / 4f);
			p.y = (float)Math.Ceiling(p.y / 4f);
			p2.x = (float)Math.Ceiling(p2.x / 4f);
			p2.y = (float)Math.Floor(p2.y / 4f);
			int num3 = (int)p.x + 127;
			int num4 = (int)p.y + 32;
			int num5 = (int)p2.x + 128;
			int num6 = (int)p2.y + 32;
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num6 < 0)
			{
				num6 = 0;
			}
			if (num5 >= 256)
			{
				num5 = 255;
			}
			if (num4 >= 64)
			{
				num4 = 63;
			}
			for (int j = num3; j < num5; j++)
			{
				for (int num7 = num4; num7 > num6; num7--)
				{
					int num8 = ar_ground[i, j, num7];
					int num9 = j - num3;
					int num10 = num4 - num7;
					if (num8 == 0)
					{
						ref bool reference = ref array[num9, num10];
						_ = reference;
						reference = true;
						if (ar_ground[i, j - 1, num7] <= 4 && ar_ground[i, j + 1, num7] <= 4 && ar_ground[i, j, num7 - 1] <= 4 && ar_ground[i, j, num7 + 1] <= 4 && ar_ground[i, j + 1, num7 + 1] <= 4 && ar_ground[i, j - 1, num7 - 1] <= 4 && ar_ground[i, j + 1, num7 - 1] <= 4 && ar_ground[i, j - 1, num7 + 1] <= 4)
						{
							ar_groundoverlap[num9, num10, i] = true;
						}
					}
				}
			}
		}
		for (int k = 0; k < num; k++)
		{
			for (int l = 0; l < num2; l++)
			{
				if (!array[k, l])
				{
					return false;
				}
			}
		}
		return true;
	}

	public static void DrawGroundCust(int nplane, frect_t custom_vp)
	{
		CowRender.DisableRS(7u);
		vec2_t p = custom_vp.p0;
		vec2_t p2 = custom_vp.p1;
		p.x = (float)Math.Floor(p.x / 4f);
		p.y = (float)Math.Ceiling(p.y / 4f);
		p2.x = (float)Math.Ceiling(p2.x / 4f);
		p2.y = (float)Math.Floor(p2.y / 4f);
		int num = (int)p.x + 128;
		int num2 = (int)p.y + 32;
		int num3 = (int)p2.x + 128;
		int num4 = (int)p2.y + 32;
		if (num < 0)
		{
			num = 0;
		}
		if (num4 < 0)
		{
			num4 = 0;
		}
		if (num3 >= 256)
		{
			num3 = 255;
		}
		if (num2 >= 64)
		{
			num2 = 63;
		}
		frect_t vc = default(frect_t);
		byte b = byte.MaxValue;
		switch (nplane)
		{
		case 0:
		case 1:
			b = byte.MaxValue;
			break;
		case 2:
			b = 220;
			break;
		case 3:
			b = 170;
			break;
		case 4:
			b = 140;
			break;
		case 5:
			b = 100;
			break;
		}
		dcolor_t dcolor_t2 = new dcolor_t(b, b, b, byte.MaxValue);
		dcolor_t c = default(dcolor_t);
		int num5 = -100;
		int num6 = -100;
		for (int i = num; i < num3; i++)
		{
			for (int num7 = num2; num7 > num4; num7--)
			{
				int num8 = ar_ground[nplane, i, num7];
				if (num8 >= 0)
				{
					bool flag = false;
					for (int num9 = nplane - 1; num9 >= 0; num9--)
					{
						if (ar_groundoverlap[i - num, num2 - num7, num9])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						num5 = ar_groundtype[nplane, i, num7];
						if (num5 >= 5)
						{
							num5 = 0;
						}
						if (num5 >= 0 && num5 != num6)
						{
							num6 = num5;
							CowRender.BindTexture(ar_groundtex[num6]);
						}
						vc.assign(4f * (float)(-128 + i), 4f * (float)(-32 + num7), 4f * (float)(-128 + i + 1), 4f * (float)(-32 + num7 - 1));
						frect_t tc;
						if (num8 == 0)
						{
							CowRender.DisableRS(4u);
							tc = ar_groundcentertexcoords[i % 2 + num7 % 2 * 2];
						}
						else
						{
							tc = ar_groundtexcoords[num8];
						}
						if (ar_groundselection[nplane, i, num7] != 0)
						{
							c.assign(byte.MaxValue, 155, 155, byte.MaxValue);
						}
						else
						{
							c = dcolor_t2;
						}
						CowRender.DrawColorQuad2D(vc, tc, c, 0f);
						CowRender.EnableRS(4u);
					}
				}
			}
		}
	}

	public static void DrawGround(int nplane, frect_t rect)
	{
		CowRender.DisableRS(7u);
		float num = 0f;
		float num2 = 0f;
		vec2_t vec2_t2 = rect.p0 + new vec2_t(0f - num, 0f - num2);
		vec2_t vec2_t3 = rect.p1 + new vec2_t(0f - num, 0f - num2);
		vec2_t2.x = (float)Math.Floor(vec2_t2.x / 4f);
		vec2_t2.y = (float)Math.Ceiling(vec2_t2.y / 4f);
		vec2_t3.x = (float)Math.Ceiling(vec2_t3.x / 4f);
		vec2_t3.y = (float)Math.Floor(vec2_t3.y / 4f);
		int num3 = (int)vec2_t2.x + 128;
		int num4 = (int)vec2_t2.y + 32;
		int num5 = (int)vec2_t3.x + 128;
		int num6 = (int)vec2_t3.y + 32;
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num6 < 0)
		{
			num6 = 0;
		}
		if (num5 >= 256)
		{
			num5 = 255;
		}
		if (num4 >= 64)
		{
			num4 = 63;
		}
		frect_t vc = default(frect_t);
		byte b = byte.MaxValue;
		switch (nplane)
		{
		case 0:
		case 1:
			b = byte.MaxValue;
			break;
		case 2:
			b = 220;
			break;
		case 3:
			b = 170;
			break;
		case 4:
			b = 140;
			break;
		case 5:
			b = 100;
			break;
		}
		dcolor_t dcolor_t2 = new dcolor_t(b, b, b, byte.MaxValue);
		dcolor_t c = default(dcolor_t);
		int num7 = -100;
		int num8 = -100;
		for (int i = num3; i < num5; i++)
		{
			for (int num9 = num4; num9 > num6; num9--)
			{
				int num10 = ar_ground[nplane, i, num9];
				if (num10 >= 0)
				{
					num7 = ar_groundtype[nplane, i, num9];
					if (num7 >= 5)
					{
						num7 = 0;
					}
					if (num7 >= 0 && num7 != num8)
					{
						num8 = num7;
					}
					CowRender.BindTexture(ar_groundtex[num8]);
					vc.assign(4f * (float)(-128 + i), 4f * (float)(-32 + num9), 4f * (float)(-128 + i + 1), 4f * (float)(-32 + num9 - 1));
					frect_t tc;
					if (num10 == 0)
					{
						CowRender.DisableRS(4u);
						tc = ar_groundcentertexcoords[i % 2 + num9 % 2 * 2];
					}
					else
					{
						tc = ar_groundtexcoords[num10];
					}
					if (ar_groundselection[nplane, i, num9] != 0)
					{
						c.assign(byte.MaxValue, 155, 155, byte.MaxValue);
					}
					else
					{
						c = dcolor_t2;
					}
					CowRender.DrawColorQuad2DStaticBuffer(vc, tc, c, nplane);
					CowRender.EnableRS(4u);
				}
			}
		}
	}

	public static void DrawGroundOld(int nplane)
	{
		CowRender.DisableRS(7u);
		float num = 0f;
		float num2 = 0f;
		vec2_t vec2_t2 = SCGlobals.viewport_rect.p0 + new vec2_t(0f - num, 0f - num2);
		vec2_t vec2_t3 = SCGlobals.viewport_rect.p1 + new vec2_t(0f - num, 0f - num2);
		vec2_t2.x = (float)Math.Floor(vec2_t2.x / 4f);
		vec2_t2.y = (float)Math.Ceiling(vec2_t2.y / 4f);
		vec2_t3.x = (float)Math.Ceiling(vec2_t3.x / 4f);
		vec2_t3.y = (float)Math.Floor(vec2_t3.y / 4f);
		int num3 = (int)vec2_t2.x + 128;
		int num4 = (int)vec2_t2.y + 32;
		int num5 = (int)vec2_t3.x + 128;
		int num6 = (int)vec2_t3.y + 32;
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num6 < 0)
		{
			num6 = 0;
		}
		if (num5 >= 256)
		{
			num5 = 255;
		}
		if (num4 >= 64)
		{
			num4 = 63;
		}
		frect_t vc = default(frect_t);
		byte b = byte.MaxValue;
		switch (nplane)
		{
		case 0:
		case 1:
			b = byte.MaxValue;
			break;
		case 2:
			b = 220;
			break;
		case 3:
			b = 170;
			break;
		case 4:
			b = 140;
			break;
		case 5:
			b = 100;
			break;
		}
		dcolor_t dcolor_t2 = new dcolor_t(b, b, b, byte.MaxValue);
		dcolor_t c = default(dcolor_t);
		int num7 = -100;
		int num8 = -100;
		for (int i = num3; i < num5; i++)
		{
			for (int num9 = num4; num9 > num6; num9--)
			{
				int num10 = ar_ground[nplane, i, num9];
				if (num10 >= 0)
				{
					num7 = ar_groundtype[nplane, i, num9];
					if (num7 >= 5)
					{
						num7 = 0;
					}
					if (num7 >= 0 && num7 != num8)
					{
						num8 = num7;
					}
					CowRender.BindTexture(ar_groundtex[num8]);
					vc.assign(4f * (float)(-128 + i), 4f * (float)(-32 + num9), 4f * (float)(-128 + i + 1), 4f * (float)(-32 + num9 - 1));
					frect_t tc;
					if (num10 == 0)
					{
						CowRender.DisableRS(4u);
						tc = ar_groundcentertexcoords[i % 2 + num9 % 2 * 2];
					}
					else
					{
						tc = ar_groundtexcoords[num10];
					}
					if (ar_groundselection[nplane, i, num9] != 0)
					{
						c.assign(byte.MaxValue, 155, 155, byte.MaxValue);
					}
					else
					{
						c = dcolor_t2;
					}
					CowRender.DrawColorQuad2D(vc, tc, c, nplane);
					CowRender.EnableRS(4u);
				}
			}
		}
	}
}
