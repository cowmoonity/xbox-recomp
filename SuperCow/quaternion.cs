using System;

namespace SuperCow;

public struct quaternion
{
	public static float[] p1 = new float[4];

	public float q0;

	public float q1;

	public float q2;

	public float q3;

	public float x
	{
		get
		{
			return q0;
		}
		set
		{
			q0 = value;
		}
	}

	public float y
	{
		get
		{
			return q1;
		}
		set
		{
			q1 = value;
		}
	}

	public float z
	{
		get
		{
			return q2;
		}
		set
		{
			q2 = value;
		}
	}

	public float w
	{
		get
		{
			return q3;
		}
		set
		{
			q3 = value;
		}
	}

	public float X
	{
		get
		{
			return q0;
		}
		set
		{
			q0 = value;
		}
	}

	public float Y
	{
		get
		{
			return q1;
		}
		set
		{
			q1 = value;
		}
	}

	public float Z
	{
		get
		{
			return q2;
		}
		set
		{
			q2 = value;
		}
	}

	public float W
	{
		get
		{
			return q3;
		}
		set
		{
			q3 = value;
		}
	}

	public vec3_t v
	{
		get
		{
			return new vec3_t(q0, q1, q2);
		}
		set
		{
			q0 = value.x;
			q1 = value.y;
			q2 = value.z;
		}
	}

	public quaternion(float x, float y, float z, float _w)
	{
		q0 = x;
		q1 = y;
		q2 = z;
		q3 = _w;
	}

	public quaternion(float[] _v)
	{
		q0 = _v[0];
		q1 = _v[1];
		q2 = _v[2];
		q3 = _v[3];
	}

	private quaternion(quaternion _v)
	{
		q0 = _v.q0;
		q1 = _v.q1;
		q2 = _v.q2;
		q3 = _v.q3;
	}

	public static quaternion operator +(quaternion _v1, quaternion _v2)
	{
		return new quaternion(_v1.q0 + _v2.q0, _v1.q1 + _v2.q1, _v1.q2 + _v2.q2, _v1.q3 + _v2.q3);
	}

	public static quaternion operator -(quaternion _v1, quaternion _v2)
	{
		return new quaternion(_v1.q0 - _v2.q0, _v1.q1 - _v2.q1, _v1.q2 - _v2.q2, _v1.q3 - _v2.q3);
	}

	public static quaternion operator *(quaternion _v1, quaternion _v2)
	{
		float num = (_v1.w + _v1.x) * (_v2.w + _v2.x);
		float num2 = (_v1.z - _v1.y) * (_v2.y - _v2.z);
		float num3 = (_v1.x - _v1.w) * (_v2.y - _v2.z);
		float num4 = (_v1.y + _v1.z) * (_v2.x - _v2.w);
		float num5 = (_v1.x + _v1.z) * (_v2.x + _v2.y);
		float num6 = (_v1.x - _v1.z) * (_v2.x - _v2.y);
		float num7 = (_v1.w + _v1.y) * (_v2.w - _v2.z);
		float num8 = (_v1.w - _v1.y) * (_v2.w + _v2.z);
		return new quaternion(num - (num5 + num6 + num7 + num8) * 0.5f, 0f - num3 + (num5 - num6 + num7 - num8) * 0.5f, 0f - num4 + (num5 - num6 - num7 + num8) * 0.5f, num2 + (0f - num5 - num6 + num7 + num8) * 0.5f);
	}

	public static quaternion operator *(quaternion _v1, float d)
	{
		return new quaternion(_v1.q0 * d, _v1.q1 * d, _v1.q2 * d, _v1.q3 * d);
	}

	public static quaternion operator /(quaternion _v1, float d)
	{
		d = 1f / d;
		return new quaternion(_v1.q0 * d, _v1.q1 * d, _v1.q2 * d, _v1.q3 * d);
	}

	public float len()
	{
		return (float)Math.Sqrt(q0 * q0 + q1 * q1 + q2 * q2 + q3 * q3);
	}

	public void normalize()
	{
		float num = len();
		if (num > 0f)
		{
			num = 1f / num;
			q0 *= num;
			q1 *= num;
			q2 *= num;
			q3 *= num;
		}
	}

	public bool zero()
	{
		if (q0 == 0f && q1 == 0f && q2 == 0f && q3 == 0f)
		{
			return true;
		}
		return false;
	}

	public void assign(float x, float y, float z, float w)
	{
		q0 = x;
		q1 = y;
		q2 = z;
		q3 = w;
	}

	public void assign(float w, vec3_t _v)
	{
		q0 = _v.v0;
		q1 = _v.v1;
		q2 = _v.v2;
		q3 = w;
	}

	public void assign(quaternion lq)
	{
		q0 = lq.q0;
		q1 = lq.q1;
		q2 = lq.q2;
		q3 = lq.q3;
	}

	public vec3_t rotatevector(vec3_t _v)
	{
		quaternion quaternion2 = new quaternion(q0, q1, q2, q3);
		quaternion2.normalize();
		quaternion quaternion3 = new quaternion(0f - quaternion2.q0, 0f - quaternion2.q1, 0f - quaternion2.q2, quaternion2.q3);
		quaternion quaternion4 = new quaternion(_v.v0, _v.v1, _v.v2, 0f);
		quaternion4 = quaternion2 * quaternion4 * quaternion3;
		_v.assign(quaternion4.q0, quaternion4.q1, quaternion4.q2);
		return _v;
	}

	public void SphericalToQuaternion(float latitude, float longitude, float angle)
	{
		float num = SuperCowHelper.fast_sin(angle / 2f);
		float num2 = SuperCowHelper.fast_cos(angle / 2f);
		float num3 = SuperCowHelper.fast_sin(latitude);
		float num4 = SuperCowHelper.fast_cos(latitude);
		float num5 = SuperCowHelper.fast_sin(longitude);
		float num6 = SuperCowHelper.fast_cos(longitude);
		assign(num * num4 * num5, num * num3, num * num3 * num6, num2);
	}

	public void EulerToQuaternion(float yaw, float pitch, float roll)
	{
		quaternion quaternion2 = new quaternion((float)Math.Sin(yaw / 2f), 0f, 0f, (float)Math.Cos(yaw / 2f));
		quaternion quaternion3 = new quaternion(0f, (float)Math.Sin(pitch / 2f), 0f, (float)Math.Cos(pitch / 2f));
		quaternion quaternion4 = new quaternion(0f, 0f, (float)Math.Sin(roll / 2f), (float)Math.Cos(roll / 2f));
		assign(quaternion4 * quaternion3 * quaternion2);
	}

	public void EulerToQuaternion(vec3_t v)
	{
		EulerToQuaternion(v.x, v.y, v.z);
	}

	public void RotateToQuaternion(float angle, vec3_t _v)
	{
		float num = SuperCowHelper.fast_sin(angle * 0.5f);
		assign(num * _v.v0, num * _v.v1, num * _v.v2, SuperCowHelper.fast_cos(angle * 0.5f));
		normalize();
	}

	public void MatrixToQuaternion(matrix_t m)
	{
		float[] array = new float[4];
		int[] array2 = new int[4] { 1, 2, 3, 0 };
		float num = m.m0 + m.m5 + m.m10;
		float num2;
		if ((double)num > 0.0)
		{
			num2 = (float)Math.Sqrt(num + 1f);
			q3 = num2 / 2f;
			num2 = 0.5f / num2;
			q0 = (m.m9 - m.m6) * num2;
			q1 = (m.m2 - m.m8) * num2;
			q2 = (m.m4 - m.m1) * num2;
			return;
		}
		int num3 = 0;
		if (m.m5 > m.m0)
		{
			num3 = 1;
		}
		if (m.m10 > m.GetValue(num3 * 5))
		{
			num3 = 2;
		}
		int num4 = array2[num3];
		int num5 = array2[num4];
		num2 = (float)Math.Sqrt(m.GetValue(num3 * 5) - (m.GetValue(num4 * 5) + m.GetValue(num5 * 5)) + 1f);
		array[num3] = num2 * 0.5f;
		if ((double)num2 != 0.0)
		{
			num2 = 0.5f / num2;
		}
		array[3] = (m.GetValue(num4 + num5 * 4) - m.GetValue(num5 + num4 * 4)) * num2;
		array[num4] = (m.GetValue(num3 + num4 * 4) + m.GetValue(num4 + num3 * 4)) * num2;
		array[num5] = (m.GetValue(num3 + num5 * 4) + m.GetValue(num5 + num3 * 4)) * num2;
		q0 = array[0];
		q1 = array[1];
		q2 = array[2];
		q3 = array[3];
	}

	public void QuaternionToMatrix(ref matrix_t m)
	{
		float num = x + x;
		float num2 = y + y;
		float num3 = z + z;
		float num4 = x * num;
		float num5 = x * num2;
		float num6 = x * num3;
		float num7 = y * num2;
		float num8 = y * num3;
		float num9 = z * num3;
		float num10 = w * num;
		float num11 = w * num2;
		float num12 = w * num3;
		m.m0 = 1f - (num7 + num9);
		m.m4 = num5 - num12;
		m.m8 = num6 + num11;
		m.m1 = num5 + num12;
		m.m5 = 1f - (num4 + num9);
		m.m9 = num8 - num10;
		m.m2 = num6 - num11;
		m.m6 = num8 + num10;
		m.m10 = 1f - (num4 + num7);
	}

	public quaternion Slerp(quaternion p, float t)
	{
		quaternion result = default(quaternion);
		float num = x * p.x + y * p.y + z * p.z + w * p.w;
		if ((double)num < 0.0)
		{
			num = 0f - num;
			p1[0] = 0f - p.x;
			p1[1] = 0f - p.y;
			p1[2] = 0f - p.z;
			p1[3] = 0f - p.w;
		}
		else
		{
			p1[0] = p.x;
			p1[1] = p.y;
			p1[2] = p.z;
			p1[3] = p.w;
		}
		float num4;
		float num5;
		if (1f - num > 0.001f)
		{
			float num2 = (float)Math.Acos(num);
			float num3 = (float)Math.Sin(num2);
			num4 = (float)Math.Sin((1f - t) * num2) / num3;
			num5 = (float)Math.Sin(t * num2) / num3;
		}
		else
		{
			num4 = 1f - t;
			num5 = t;
		}
		result.x = num4 * x + num5 * p1[0];
		result.y = num4 * y + num5 * p1[1];
		result.z = num4 * z + num5 * p1[2];
		result.w = num4 * w + num5 * p1[3];
		return result;
	}
}
