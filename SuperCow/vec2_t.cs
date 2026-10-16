using System;
using Microsoft.Xna.Framework;

namespace SuperCow;

public struct vec2_t
{
	public float v0;

	public float v1;

	public float x
	{
		get
		{
			return v0;
		}
		set
		{
			v0 = value;
		}
	}

	public float y
	{
		get
		{
			return v1;
		}
		set
		{
			v1 = value;
		}
	}

	public float X
	{
		get
		{
			return v0;
		}
		set
		{
			v0 = value;
		}
	}

	public float Y
	{
		get
		{
			return v1;
		}
		set
		{
			v1 = value;
		}
	}

	public vec2_t(float x, float y)
	{
		v0 = x;
		v1 = y;
	}

	public vec2_t(float[] _v)
	{
		v0 = _v[0];
		v1 = _v[1];
	}

	public vec2_t(vec3_t _v)
	{
		v0 = _v.v0;
		v1 = _v.v1;
	}

	public vec2_t(vec2_t _v)
	{
		v0 = _v.v0;
		v1 = _v.v1;
	}

	public vec2_t interp(vec2_t _v, float t)
	{
		float num = 1f - t;
		_v.assign(x * num + _v.x * t, y * num + _v.y * t);
		return _v;
	}

	public static vec2_t operator +(vec2_t _v1, vec2_t _v2)
	{
		return new vec2_t(_v1.v0 + _v2.v0, _v1.v1 + _v2.v1);
	}

	public static vec2_t operator -(vec2_t _v1, vec2_t _v2)
	{
		return new vec2_t(_v1.v0 - _v2.v0, _v1.v1 - _v2.v1);
	}

	public static vec2_t operator *(vec2_t _v1, vec2_t _v2)
	{
		return new vec2_t(_v1.v0 * _v2.v0, _v1.v1 * _v2.v1);
	}

	public static vec2_t operator *(vec2_t _v1, float _v2)
	{
		return new vec2_t(_v1.v0 * _v2, _v1.v1 * _v2);
	}

	public static vec2_t operator /(vec2_t _v1, vec2_t _v2)
	{
		return new vec2_t(_v1.v0 / _v2.v0, _v1.v1 / _v2.v1);
	}

	public static vec2_t operator /(vec2_t _v1, float q)
	{
		float num = 1f / q;
		return new vec2_t(_v1.v0 * num, _v1.v1 * num);
	}

	public static vec2_t operator -(vec2_t _v)
	{
		return new vec2_t(0f - _v.v0, 0f - _v.v1);
	}

	public float len()
	{
		return (float)Math.Sqrt(v0 * v0 + v1 * v1);
	}

	public void normalize()
	{
		float num = len();
		if (num > 0f)
		{
			num = 1f / num;
			v0 *= num;
			v1 *= num;
		}
	}

	public float angle()
	{
		normalize();
		float num = x;
		if (num > 1f)
		{
			num = 1f;
		}
		else if (num < -1f)
		{
			num = -1f;
		}
		float num2 = SuperCowHelper.RadiansToDegree((float)Math.Acos(num));
		if (y < 0f)
		{
			return 360f - num2;
		}
		return num2;
	}

	public float dot(vec2_t _v)
	{
		return v0 * _v.v0 + v1 * _v.v1;
	}

	public float angle_between(vec2_t _v)
	{
		float num = dot(_v) / (len() * _v.len());
		if (num > 1f)
		{
			num = 1f;
		}
		else if (num < -1f)
		{
			num = -1f;
		}
		return SuperCowHelper.RadiansToDegree((float)Math.Acos(num));
	}

	public bool zero()
	{
		if (v0 == 0f && v1 == 0f)
		{
			return true;
		}
		return false;
	}

	public void assign(float x, float y)
	{
		v0 = x;
		v1 = y;
	}

	public void rotate(float angle, vec2_t _v)
	{
		vec2_t vec2_t2 = new vec2_t(v0, v1);
		vec2_t2 -= _v;
		float num = SuperCowHelper.fast_sin(angle);
		float num2 = SuperCowHelper.fast_cos(angle);
		vec2_t2.assign(vec2_t2.v0 * num2 - vec2_t2.v1 * num, vec2_t2.v0 * num + vec2_t2.v1 * num2);
		vec2_t2 += _v;
		assign(vec2_t2.v0, vec2_t2.v1);
	}

	public void rotate(float angle)
	{
		vec2_t vec2_t2 = default(vec2_t);
		float num = SuperCowHelper.fast_sin(angle);
		float num2 = SuperCowHelper.fast_cos(angle);
		vec2_t2.x = x * num2 - y * num;
		vec2_t2.y = x * num + y * num2;
		assign(vec2_t2.x, vec2_t2.y);
	}

	public void norm()
	{
		float num = x;
		x = y;
		y = 0f - num;
	}

	public vec3_t v3t()
	{
		return new vec3_t(x, y, 0f);
	}

	public Vector2 GetVector2()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(x, y);
	}
}
