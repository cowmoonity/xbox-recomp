using System;
using Microsoft.Xna.Framework;

namespace SuperCow;

public struct frect_t
{
	public float x0;

	public float y0;

	public float x1;

	public float y1;

	public vec2_t p0
	{
		get
		{
			return new vec2_t(x0, y0);
		}
		set
		{
			x0 = value.x;
			y0 = value.y;
		}
	}

	public vec2_t p1
	{
		get
		{
			return new vec2_t(x1, y1);
		}
		set
		{
			x1 = value.x;
			y1 = value.y;
		}
	}

	public frect_t(float _x0, float _y0, float _x1, float _y1)
	{
		x0 = _x0;
		y0 = _y0;
		x1 = _x1;
		y1 = _y1;
	}

	public frect_t(vec2_t v0, vec2_t v1)
	{
		x0 = v0.x;
		y0 = v0.y;
		x1 = v1.x;
		y1 = v1.y;
	}

	public void assign(float _x0, float _y0, float _x1, float _y1)
	{
		x0 = _x0;
		y0 = _y0;
		x1 = _x1;
		y1 = _y1;
	}

	public void assign(vec2_t pos, float sizex, float sizey)
	{
		x0 = pos.x - sizex * 0.5f;
		y0 = pos.y + sizey * 0.5f;
		x1 = pos.x + sizex * 0.5f;
		y1 = pos.y - sizey * 0.5f;
	}

	public void translate(float dx, float dy)
	{
		x0 += dx;
		x1 += dx;
		y0 += dy;
		y1 += dy;
	}

	public void translate(vec2_t v)
	{
		x0 += v.x;
		x1 += v.x;
		y0 += v.y;
		y1 += v.y;
	}

	public void translate(vec3_t v)
	{
		x0 += v.x;
		x1 += v.x;
		y0 += v.y;
		y1 += v.y;
	}

	public void setpos(vec2_t v)
	{
		assign(v, sizex(), sizey());
	}

	public void setpos(float x, float y)
	{
		assign(new vec2_t(x, y), sizex(), sizey());
	}

	public void normalize()
	{
		if (x0 > x1)
		{
			float num = x0;
			x0 = x1;
			x1 = num;
		}
		if (y0 < y1)
		{
			float num = y0;
			y0 = y1;
			y1 = num;
		}
	}

	public float sizex()
	{
		return Math.Abs(x0 - x1);
	}

	public float sizey()
	{
		return Math.Abs(y0 - y1);
	}

	public vec2_t pos()
	{
		return new vec2_t((x0 + x1) * 0.5f, (y0 + y1) * 0.5f);
	}

	public bool isRectInRect(frect_t rc)
	{
		if (x0 > rc.x0 && x1 < rc.x1 && y0 < rc.y0)
		{
			return y1 > rc.y1;
		}
		return false;
	}

	public bool isPointInRect(vec2_t p)
	{
		if (p.x >= x0 && p.x <= x1 && p.y <= y0)
		{
			return p.y >= y1;
		}
		return false;
	}

	public bool isPointInRect(vec3_t p)
	{
		if (p.x >= x0 && p.x <= x1 && p.y <= y0)
		{
			return p.y >= y1;
		}
		return false;
	}

	public bool isPointInTriangle(vec2_t p, ref float newy)
	{
		frect_t frect_t2 = new frect_t(x0, y0, x1, y1);
		frect_t2.normalize();
		if (frect_t2.isPointInRect(p))
		{
			float num = (y0 - y1) / (x0 - x1);
			float num2 = y0 - num * x0;
			newy = num * p.x + num2;
			if (newy > p.y)
			{
				return true;
			}
		}
		return false;
	}

	public bool isRectTouchRect(frect_t rc)
	{
		if ((x0 >= rc.x0 && x0 <= rc.x1) || (x1 >= rc.x0 && x1 <= rc.x1) || (rc.x0 >= x0 && rc.x0 <= x1) || (rc.x1 >= x0 && rc.x1 <= x1))
		{
			if ((!(y0 <= rc.y0) || !(y0 >= rc.y1)) && (!(y1 <= rc.y0) || !(y1 >= rc.y1)) && (!(rc.y0 <= y0) || !(rc.y0 >= y1)))
			{
				if (rc.y1 <= y0)
				{
					return rc.y1 >= y1;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public bool isRectIntersectRect(frect_t rc)
	{
		if ((x0 > rc.x0 && x0 < rc.x1) || (x1 > rc.x0 && x1 < rc.x1) || (rc.x0 > x0 && rc.x0 < x1) || (rc.x1 > x0 && rc.x1 < x1))
		{
			if ((!(y0 < rc.y0) || !(y0 > rc.y1)) && (!(y1 < rc.y0) || !(y1 > rc.y1)) && (!(rc.y0 < y0) || !(rc.y0 > y1)))
			{
				if (rc.y1 < y0)
				{
					return rc.y1 > y1;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public Rectangle GetRectangle()
	{
		return new Rectangle((int)x0, (int)y0, (int)x1, (int)y1);
	}

	public frect_t Clone()
	{
		return new frect_t(x0, y0, x1, y1);
	}

	public Plane GetPlane()
	{
		return new Plane(x0, y0, x1, y1);
	}
}
