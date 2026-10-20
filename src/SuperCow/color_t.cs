using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public struct color_t
{
	public float c0;

	public float c1;

	public float c2;

	public float c3;

	public float r
	{
		get
		{
			return c0;
		}
		set
		{
			c0 = value;
		}
	}

	public float g
	{
		get
		{
			return c1;
		}
		set
		{
			c1 = value;
		}
	}

	public float b
	{
		get
		{
			return c2;
		}
		set
		{
			c2 = value;
		}
	}

	public float a
	{
		get
		{
			return c3;
		}
		set
		{
			c3 = value;
		}
	}

	public static color_t ColorWhite => new color_t(1f, 1f, 1f, 1f);

	public color_t(float rr, float gg, float bb, float aa)
	{
		c0 = rr;
		c1 = gg;
		c2 = bb;
		c3 = aa;
		check_max();
	}

	public color_t(float rr, float gg, float bb)
	{
		c0 = rr;
		c1 = gg;
		c2 = bb;
		c3 = 1f;
		check_max();
	}

	public void check_max()
	{
		if (r > 1f)
		{
			r = 1f;
		}
		if (g > 1f)
		{
			g = 1f;
		}
		if (b > 1f)
		{
			b = 1f;
		}
		if (a > 1f)
		{
			a = 1f;
		}
		if (r < 0f)
		{
			r = 0f;
		}
		if (g < 0f)
		{
			g = 0f;
		}
		if (b < 0f)
		{
			b = 0f;
		}
		if (a < 0f)
		{
			a = 0f;
		}
	}

	public static color_t operator *(color_t col, float x)
	{
		color_t result = new color_t(col.c0 * x, col.c1 * x, col.c2 * x, col.c3 * x);
		result.check_max();
		return result;
	}

	public static color_t operator +(color_t _c1, color_t _c2)
	{
		color_t result = new color_t(_c1.c0 + _c2.c0, _c1.c1 + _c2.c1, _c1.c2 + _c2.c2, _c1.c3 + _c2.c3);
		result.check_max();
		return result;
	}

	public static color_t operator -(color_t _c1, color_t _c2)
	{
		color_t result = new color_t(_c1.c0 - _c2.c0, _c1.c1 - _c2.c1, _c1.c2 - _c2.c2, _c1.c3 - _c2.c3);
		result.check_max();
		return result;
	}

	public static color_t operator /(color_t col, float x)
	{
		color_t result = new color_t(col.c0 / x, col.c1 / x, col.c2 / x, col.c3 / x);
		result.check_max();
		return result;
	}

	public int dword()
	{
		dcolor_t dcolor_t2 = dcolor();
		return (dcolor_t2.r << 24) | (dcolor_t2.g << 16) | (dcolor_t2.b << 8) | dcolor_t2.a;
	}

	public dcolor_t dcolor()
	{
		return new dcolor_t((byte)(255f * r), (byte)(255f * g), (byte)(255f * b), (byte)(255f * a));
	}

	public void assign(float rr, float gg, float bb, float aa)
	{
		c0 = rr;
		c1 = gg;
		c2 = bb;
		c3 = aa;
		check_max();
	}

	public void assign(float rr, float gg, float bb)
	{
		c0 = rr;
		c1 = gg;
		c2 = bb;
		check_max();
	}

	public color_t interp(color_t _c, float t)
	{
		float num = 1f - t;
		_c.assign(r * num + _c.r * t, g * num + _c.g * t, b * num + _c.b * t, a * num + _c.a * t);
		return _c;
	}

	public Color GetD3DColor()
	{
		dcolor_t dcolor_t2 = dcolor();
		return new Color(dcolor_t2.r, dcolor_t2.g, dcolor_t2.b, dcolor_t2.a);
	}

	public void ToVector4(ref Vector4 outCol)
	{
		float num = 1f;
		float w = a / num;
		float x = r / num;
		float y = g / num;
		float z = b / num;
		outCol.X = x;
		outCol.Y = y;
		outCol.Z = z;
		outCol.W = w;
	}

	public void ToArray(ref float[] outResult)
	{
		outResult[0] = c0;
		outResult[1] = c1;
		outResult[2] = c2;
		outResult[3] = c3;
	}
}
