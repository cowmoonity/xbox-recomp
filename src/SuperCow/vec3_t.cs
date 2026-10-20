using System;
using Microsoft.Xna.Framework;

namespace SuperCow;

public struct vec3_t
{
	public float v0;

	public float v1;

	public float v2;

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

	public float z
	{
		get
		{
			return v2;
		}
		set
		{
			v2 = value;
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

	public float Z
	{
		get
		{
			return v2;
		}
		set
		{
			v2 = value;
		}
	}

	public vec3_t(float _x, float _y, float _z)
	{
		v0 = _x;
		v1 = _y;
		v2 = _z;
	}

	public vec3_t(float[] _v)
	{
		v0 = _v[0];
		v1 = _v[1];
		v2 = _v[2];
	}

	public vec3_t(vec3_t _v)
	{
		v0 = _v.v0;
		v1 = _v.v1;
		v2 = _v.v2;
	}

	public vec3_t(vec2_t _v)
	{
		v0 = _v.v0;
		v1 = _v.v1;
		v2 = 0f;
	}

	public static vec3_t operator +(vec3_t _v1, vec3_t _v2)
	{
		return new vec3_t(_v1.v0 + _v2.v0, _v1.v1 + _v2.v1, _v1.v2 + _v2.v2);
	}

	public static vec3_t operator -(vec3_t _v1, vec3_t _v2)
	{
		return new vec3_t(_v1.v0 - _v2.v0, _v1.v1 - _v2.v1, _v1.v2 - _v2.v2);
	}

	public static vec3_t operator *(vec3_t _v1, vec3_t _v2)
	{
		return new vec3_t(_v1.v0 * _v2.v0, _v1.v1 * _v2.v1, _v1.v2 * _v2.v2);
	}

	public static vec3_t operator *(vec3_t _v1, float d)
	{
		return new vec3_t(_v1.v0 * d, _v1.v1 * d, _v1.v2 * d);
	}

	public static vec3_t operator *(vec3_t _v1, matrix_t m)
	{
		return new vec3_t(m.m0 * _v1.v0 + m.m4 * _v1.v1 + m.m8 * _v1.v2 + m.m12, m.m1 * _v1.v0 + m.m5 * _v1.v1 + m.m9 * _v1.v2 + m.m13, m.m2 * _v1.v0 + m.m6 * _v1.v1 + m.m10 * _v1.v2 + m.m14);
	}

	public static vec3_t operator /(vec3_t _v1, vec3_t _v2)
	{
		return new vec3_t(_v1.v0 / _v2.v0, _v1.v1 / _v2.v1, _v1.v2 / _v2.v2);
	}

	public static vec3_t operator /(vec3_t _v1, float d)
	{
		d = 1f / d;
		return new vec3_t(_v1.v0 * d, _v1.v1 * d, _v1.v2 * d);
	}

	public float len()
	{
		return (float)Math.Sqrt(v0 * v0 + v1 * v1 + v2 * v2);
	}

	public float lenlen()
	{
		return v0 * v0 + v1 * v1 + v2 * v2;
	}

	public void normalize()
	{
		float num = len();
		if (num > 0f)
		{
			num = 1f / num;
			v0 *= num;
			v1 *= num;
			v2 *= num;
		}
	}

	public vec3_t cross(vec3_t _v)
	{
		_v.assign(v1 * _v.v2 - v2 * _v.v1, v2 * _v.v0 - v0 * _v.v2, v0 * _v.v1 - v1 * _v.v0);
		return _v;
	}

	public float dot(vec3_t _v)
	{
		return v0 * _v.v0 + v1 * _v.v1 + v2 * _v.v2;
	}

	public float cos_between(vec3_t _v)
	{
		return dot(_v) / (len() * _v.len());
	}

	public vec3_t interp(vec3_t _v, float t)
	{
		float num = 1f - t;
		_v.assign(v0 * num + _v.v0 * t, v1 * num + _v.v1 * t, v2 * num + _v.v2 * t);
		return _v;
	}

	public bool zero()
	{
		if (v0 == 0f && v1 == 0f && v2 == 0f)
		{
			return true;
		}
		return false;
	}

	public void assign(float _x, float _y, float _z)
	{
		v0 = _x;
		v1 = _y;
		v2 = _z;
	}

	public void rotate(float angle, vec3_t _v)
	{
		len();
		vec3_t vec3_t2 = this;
		vec3_t vec3_t3 = _v;
		vec3_t3.normalize();
		vec3_t3 *= vec3_t2.dot(vec3_t3);
		vec3_t vec3_t4 = vec3_t2 - vec3_t3;
		vec3_t vec3_t5 = vec3_t4.cross(vec3_t2);
		vec3_t5.normalize();
		float num = vec3_t4.len();
		vec3_t4.normalize();
		float num2 = num * SuperCowHelper.fast_sin(angle);
		float num3 = num * SuperCowHelper.fast_cos(angle);
		vec3_t3 += vec3_t4 * num2 + vec3_t5 * num3;
		assign(vec3_t3.v0, vec3_t3.v1, vec3_t3.v2);
	}

	public Vector3 GetVector3()
	{
		return new Vector3(x, y, z);
	}

	public vec2_t v2t()
	{
		return new vec2_t(x, y);
	}
}
