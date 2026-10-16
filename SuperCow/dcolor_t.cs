using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public struct dcolor_t
{
	public byte c0;

	public byte c1;

	public byte c2;

	public byte c3;

	public byte r
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

	public byte g
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

	public byte b
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

	public byte a
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

	public static dcolor_t ColorWhite => new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	public dcolor_t(byte rr, byte gg, byte bb, byte aa)
	{
		c0 = rr;
		c1 = gg;
		c2 = bb;
		c3 = aa;
		check_max();
	}

	public void assign(byte rr, byte gg, byte bb, byte aa)
	{
		c0 = rr;
		c1 = gg;
		c2 = bb;
		c3 = aa;
		check_max();
	}

	public void check_max()
	{
		if (r > byte.MaxValue)
		{
			r = byte.MaxValue;
		}
		if (g > byte.MaxValue)
		{
			g = byte.MaxValue;
		}
		if (b > byte.MaxValue)
		{
			b = byte.MaxValue;
		}
		if (a > byte.MaxValue)
		{
			a = byte.MaxValue;
		}
	}

	public static dcolor_t operator *(dcolor_t col, float x)
	{
		dcolor_t result = new dcolor_t((byte)((float)(int)col.c0 * x), (byte)((float)(int)col.c1 * x), (byte)((float)(int)col.c2 * x), (byte)((float)(int)col.c3 * x));
		result.check_max();
		return result;
	}

	public static dcolor_t operator /(dcolor_t col, float x)
	{
		dcolor_t result = new dcolor_t((byte)((float)(int)col.c0 / x), (byte)((float)(int)col.c1 / x), (byte)((float)(int)col.c2 / x), (byte)((float)(int)col.c3 / x));
		result.check_max();
		return result;
	}

	public static dcolor_t operator +(dcolor_t col, dcolor_t col1)
	{
		dcolor_t result = new dcolor_t((byte)(col.c0 + col1.c0), (byte)(col.c1 + col1.c1), (byte)(col.c2 + col1.c2), (byte)(col.c3 + col1.c3));
		result.check_max();
		return result;
	}

	public static dcolor_t operator -(dcolor_t col, dcolor_t col1)
	{
		dcolor_t result = new dcolor_t((byte)(col.c0 - col1.c0), (byte)(col.c1 - col1.c1), (byte)(col.c2 - col1.c2), (byte)(col.c3 - col1.c3));
		result.check_max();
		return result;
	}

	public Color GetD3DColor()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		return new Color(r, g, b, a);
	}
}
