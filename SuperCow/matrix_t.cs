using Microsoft.Xna.Framework;

namespace SuperCow;

public struct matrix_t
{
	public float m0;

	public float m1;

	public float m2;

	public float m3;

	public float m4;

	public float m5;

	public float m6;

	public float m7;

	public float m8;

	public float m9;

	public float m10;

	public float m11;

	public float m12;

	public float m13;

	public float m14;

	public float m15;

	public matrix_t(matrix_t inm)
	{
		this = inm;
	}

	public void load_identity()
	{
		m0 = (m5 = (m10 = (m15 = 1f)));
		m1 = (m2 = (m3 = (m4 = (m6 = (m7 = (m8 = (m9 = (m11 = (m12 = (m13 = (m14 = 0f)))))))))));
	}

	public bool isNull()
	{
		if (m0 == 0f && m1 == 0f && m2 == 0f && m3 == 0f && m4 == 0f && m5 == 0f && m6 == 0f && m7 == 0f && m8 == 0f && m9 == 0f && m10 == 0f && m11 == 0f && m12 == 0f && m13 == 0f && m14 == 0f && m15 == 0f)
		{
			return true;
		}
		return false;
	}

	public void xrot(float angle)
	{
		float num = SuperCowHelper.fast_cos(angle);
		float num2 = SuperCowHelper.fast_sin(angle);
		m5 = num;
		m6 = num2;
		m9 = 0f - num2;
		m10 = num;
	}

	public void yrot(float angle)
	{
		float num = SuperCowHelper.fast_cos(angle);
		float num2 = SuperCowHelper.fast_sin(angle);
		m0 = num;
		m2 = 0f - num2;
		m8 = num2;
		m10 = num;
	}

	public void zrot(float angle)
	{
		float num = SuperCowHelper.fast_cos(angle);
		float num2 = SuperCowHelper.fast_sin(angle);
		m0 = num;
		m1 = num2;
		m4 = 0f - num2;
		m5 = num;
	}

	public void xrot_delta(float angle)
	{
		float num = SuperCowHelper.fast_cos(angle);
		float num2 = SuperCowHelper.fast_sin(angle);
		float num3 = m4 * num + m8 * num2;
		float num4 = m5 * num + m9 * num2;
		float num5 = m6 * num + m10 * num2;
		float num6 = m7 * num + m11 * num2;
		m8 = (0f - m4) * num2 + m8 * num;
		m9 = (0f - m5) * num2 + m9 * num;
		m10 = (0f - m6) * num2 + m10 * num;
		m11 = (0f - m7) * num2 + m11 * num;
		m4 = num3;
		m5 = num4;
		m6 = num5;
		m7 = num6;
	}

	public void yrot_delta(float angle)
	{
		float num = SuperCowHelper.fast_cos(angle);
		float num2 = SuperCowHelper.fast_sin(angle);
		float num3 = m0 * num - m8 * num2;
		float num4 = m1 * num - m9 * num2;
		float num5 = m2 * num - m10 * num2;
		float num6 = m3 * num - m11 * num2;
		m8 = m0 * num2 + m8 * num;
		m9 = m1 * num2 + m9 * num;
		m10 = m2 * num2 + m10 * num;
		m11 = m3 * num2 + m11 * num;
		m0 = num3;
		m1 = num4;
		m2 = num5;
		m3 = num6;
	}

	public void zrot_delta(float angle)
	{
		float num = SuperCowHelper.fast_cos(angle);
		float num2 = SuperCowHelper.fast_sin(angle);
		float num3 = m0 * num + m4 * num2;
		float num4 = m1 * num + m5 * num2;
		float num5 = m2 * num + m6 * num2;
		float num6 = m3 * num + m7 * num2;
		m4 = (0f - m0) * num2 + m4 * num;
		m5 = (0f - m1) * num2 + m5 * num;
		m6 = (0f - m2) * num2 + m6 * num;
		m7 = (0f - m3) * num2 + m7 * num;
		m0 = num3;
		m1 = num4;
		m2 = num5;
		m3 = num6;
	}

	private void rotate(float angle, vec3_t vec)
	{
		float num = SuperCowHelper.fast_cos(angle);
		float num2 = SuperCowHelper.fast_sin(angle);
		float num3 = 1f - num;
		float x = vec.x;
		float y = vec.y;
		float z = vec.z;
		m0 = x * x * num3 + num;
		m1 = y * x * num3 + z * num2;
		m2 = x * z * num3 - y * num2;
		m4 = x * y * num3 - z * num2;
		m5 = y * y * num3 + num;
		m6 = y * z * num3 + x * num2;
		m8 = x * z * num3 + y * num2;
		m9 = y * z * num3 - x * num2;
		m10 = z * z * num3 + num;
	}

	public void rotate(float angle, float x, float y, float z)
	{
		float num = SuperCowHelper.fast_cos(angle);
		float num2 = SuperCowHelper.fast_sin(angle);
		float num3 = 1f - num;
		m0 = x * x * num3 + num;
		m1 = y * x * num3 + z * num2;
		m2 = x * z * num3 - y * num2;
		m4 = x * y * num3 - z * num2;
		m5 = y * y * num3 + num;
		m6 = y * z * num3 + x * num2;
		m8 = x * z * num3 + y * num2;
		m9 = y * z * num3 - x * num2;
		m10 = z * z * num3 + num;
	}

	public void xyzrotate(vec3_t vec)
	{
		xrot(vec.x);
		yrot(vec.y);
		zrot(vec.z);
	}

	public void xyzrotate(float xangle, float yangle, float zangle)
	{
		xrot(xangle);
		yrot(yangle);
		zrot(zangle);
	}

	public void xyzrotate_delta(vec3_t vec)
	{
		xrot_delta(vec.x);
		yrot_delta(vec.y);
		zrot_delta(vec.z);
	}

	public void xyzrotate_delta(float xangle, float yangle, float zangle)
	{
		xrot_delta(xangle);
		yrot_delta(yangle);
		zrot_delta(zangle);
	}

	public void scale(float x, float y, float z)
	{
		m0 = x;
		m5 = y;
		m10 = z;
	}

	public matrix_t interp(matrix_t ma2, float k)
	{
		ma2.m0 = m0 * (1f - k) + ma2.m0 * k;
		ma2.m1 = m1 * (1f - k) + ma2.m1 * k;
		ma2.m2 = m2 * (1f - k) + ma2.m2 * k;
		ma2.m3 = m3 * (1f - k) + ma2.m3 * k;
		ma2.m4 = m4 * (1f - k) + ma2.m4 * k;
		ma2.m5 = m5 * (1f - k) + ma2.m5 * k;
		ma2.m6 = m6 * (1f - k) + ma2.m6 * k;
		ma2.m7 = m7 * (1f - k) + ma2.m7 * k;
		ma2.m8 = m8 * (1f - k) + ma2.m8 * k;
		ma2.m9 = m9 * (1f - k) + ma2.m9 * k;
		ma2.m10 = m10 * (1f - k) + ma2.m10 * k;
		ma2.m11 = m11 * (1f - k) + ma2.m11 * k;
		ma2.m12 = m12 * (1f - k) + ma2.m12 * k;
		ma2.m13 = m13 * (1f - k) + ma2.m13 * k;
		ma2.m14 = m14 * (1f - k) + ma2.m14 * k;
		ma2.m15 = m15 * (1f - k) + ma2.m15 * k;
		return ma2;
	}

	public void SetValue(float val, int index)
	{
		switch (index)
		{
		case 0:
			m0 = val;
			break;
		case 1:
			m1 = val;
			break;
		case 2:
			m2 = val;
			break;
		case 3:
			m3 = val;
			break;
		case 4:
			m4 = val;
			break;
		case 5:
			m5 = val;
			break;
		case 6:
			m6 = val;
			break;
		case 7:
			m7 = val;
			break;
		case 8:
			m8 = val;
			break;
		case 9:
			m9 = val;
			break;
		case 10:
			m10 = val;
			break;
		case 11:
			m11 = val;
			break;
		case 12:
			m12 = val;
			break;
		case 13:
			m13 = val;
			break;
		case 14:
			m14 = val;
			break;
		case 15:
			m15 = val;
			break;
		}
	}

	public float GetValue(int index)
	{
		return index switch
		{
			0 => m0, 
			1 => m1, 
			2 => m2, 
			3 => m3, 
			4 => m4, 
			5 => m5, 
			6 => m6, 
			7 => m7, 
			8 => m8, 
			9 => m9, 
			10 => m10, 
			11 => m11, 
			12 => m12, 
			13 => m13, 
			14 => m14, 
			15 => m15, 
			_ => 0f, 
		};
	}

	public void translate(float x, float y, float z)
	{
		m12 = m0 * x + m4 * y + m8 * z + m12;
		m13 = m1 * x + m5 * y + m9 * z + m13;
		m14 = m2 * x + m6 * y + m10 * z + m14;
		m15 = m3 * x + m7 * y + m11 * z + m15;
	}

	public void translate(float[] v)
	{
		m12 = m0 * v[0] + m4 * v[1] + m8 * v[2] + m12;
		m13 = m1 * v[0] + m5 * v[1] + m9 * v[2] + m13;
		m14 = m2 * v[0] + m6 * v[1] + m10 * v[2] + m14;
		m15 = m3 * v[0] + m7 * v[1] + m11 * v[2] + m15;
	}

	public void translate(vec3_t v)
	{
		m12 = m0 * v.v0 + m4 * v.v1 + m8 * v.v2 + m12;
		m13 = m1 * v.v0 + m5 * v.v1 + m9 * v.v2 + m13;
		m14 = m2 * v.v0 + m6 * v.v1 + m10 * v.v2 + m14;
		m15 = m3 * v.v0 + m7 * v.v1 + m11 * v.v2 + m15;
	}

	public void invert()
	{
		for (int i = 0; i < 4; i++)
		{
			float value = GetValue(i * 4 + i);
			if (value != 1f && value != 0f)
			{
				for (int j = 0; j < 4; j++)
				{
					SetValue(GetValue(i * 4 + j) / value, i * 4 + j);
				}
			}
			for (int j = 0; j < 4; j++)
			{
				if (i != j && GetValue(j * 4 + i) != 0f)
				{
					float value2 = GetValue(j * 4 + i);
					for (int k = 0; k < 4; k++)
					{
						float num = value2 * GetValue(i * 4 + k);
						SetValue(GetValue(j * 4 + k) - num, j * 4 + k);
					}
				}
			}
		}
	}

	public static matrix_t operator *(matrix_t mm, matrix_t s)
	{
		matrix_t result = default(matrix_t);
		result.m0 = mm.m0 * s.m0 + mm.m4 * s.m1 + mm.m8 * s.m2 + mm.m12 * s.m3;
		result.m1 = mm.m1 * s.m0 + mm.m5 * s.m1 + mm.m9 * s.m2 + mm.m13 * s.m3;
		result.m2 = mm.m2 * s.m0 + mm.m6 * s.m1 + mm.m10 * s.m2 + mm.m14 * s.m3;
		result.m3 = mm.m3 * s.m0 + mm.m7 * s.m1 + mm.m11 * s.m2 + mm.m15 * s.m3;
		result.m4 = mm.m0 * s.m4 + mm.m4 * s.m5 + mm.m8 * s.m6 + mm.m12 * s.m7;
		result.m5 = mm.m1 * s.m4 + mm.m5 * s.m5 + mm.m9 * s.m6 + mm.m13 * s.m7;
		result.m6 = mm.m2 * s.m4 + mm.m6 * s.m5 + mm.m10 * s.m6 + mm.m14 * s.m7;
		result.m7 = mm.m3 * s.m4 + mm.m7 * s.m5 + mm.m11 * s.m6 + mm.m15 * s.m7;
		result.m8 = mm.m0 * s.m8 + mm.m4 * s.m9 + mm.m8 * s.m10 + mm.m12 * s.m11;
		result.m9 = mm.m1 * s.m8 + mm.m5 * s.m9 + mm.m9 * s.m10 + mm.m13 * s.m11;
		result.m10 = mm.m2 * s.m8 + mm.m6 * s.m9 + mm.m10 * s.m10 + mm.m14 * s.m11;
		result.m11 = mm.m3 * s.m8 + mm.m7 * s.m9 + mm.m11 * s.m10 + mm.m15 * s.m11;
		result.m12 = mm.m0 * s.m12 + mm.m4 * s.m13 + mm.m8 * s.m14 + mm.m12 * s.m15;
		result.m13 = mm.m1 * s.m12 + mm.m5 * s.m13 + mm.m9 * s.m14 + mm.m13 * s.m15;
		result.m14 = mm.m2 * s.m12 + mm.m6 * s.m13 + mm.m10 * s.m14 + mm.m14 * s.m15;
		result.m15 = mm.m3 * s.m12 + mm.m7 * s.m13 + mm.m11 * s.m14 + mm.m15 * s.m15;
		return result;
	}

	public static vec3_t operator *(matrix_t mm, vec3_t v)
	{
		return new vec3_t(mm.m0 * v.v0 + mm.m1 * v.v1 + mm.m2 * v.v2, mm.m4 * v.v0 + mm.m5 * v.v1 + mm.m6 * v.v2, mm.m8 * v.v0 + mm.m9 * v.v1 + mm.m10 * v.v2);
	}

	public vec3_t eye()
	{
		return new vec3_t((0f - m0) * m12 - m1 * m13 - m2 * m14, (0f - m4) * m12 - m5 * m13 - m6 * m14, (0f - m8) * m12 - m9 * m13 - m10 * m14);
	}

	public void perspective(float fov, float aspect, float zn, float zf)
	{
		float num = SuperCowHelper.fast_cos(fov / 2f) / SuperCowHelper.fast_sin(fov / 2f);
		float num2 = num / aspect;
		m0 = num2;
		m1 = (m2 = (m3 = 0f));
		m4 = 0f;
		m5 = num;
		m6 = (m7 = 0f);
		m8 = (m9 = 0f);
		m10 = zf / (zf - zn);
		m11 = 1f;
		m12 = (m13 = 0f);
		m14 = zn * zf / (zn - zf);
		m15 = 0f;
	}

	public void ortho(float w, float h, float zn, float zf)
	{
		m0 = 2f / w;
		m1 = (m2 = (m3 = 0f));
		m4 = 0f;
		m5 = 2f / h;
		m6 = (m7 = 0f);
		m8 = (m9 = 0f);
		m10 = 1f / (zf - zn);
		m11 = 0f;
		m12 = (m13 = 0f);
		m14 = zn / (zn - zf);
		m15 = 1f;
	}

	public void lookatdot(vec3_t d, float dist)
	{
		m12 = (m13 = (m14 = 0f));
		translate(d);
		vec3_t vec3_t2 = forward_vector();
		translate(vec3_t2 * dist);
	}

	public void lookat(vec3_t eye, vec3_t center, vec3_t up)
	{
		vec3_t v = center - eye;
		v.normalize();
		up.normalize();
		vec3_t vec3_t2 = up.cross(v);
		m0 = vec3_t2.v0;
		m4 = vec3_t2.v1;
		m8 = vec3_t2.v2;
		m1 = up.v0;
		m5 = up.v1;
		m9 = up.v2;
		m2 = v.v0;
		m6 = v.v1;
		m10 = v.v2;
		m12 = (0f - m0) * eye.v0 - m4 * eye.v1 - m8 * eye.v2;
		m13 = (0f - m1) * eye.v0 - m5 * eye.v1 - m9 * eye.v2;
		m14 = (0f - m2) * eye.v0 - m6 * eye.v1 - m10 * eye.v2;
	}

	public void rotate_around_dot(vec3_t v, vec3_t d, float angle)
	{
	}

	public vec3_t right_vector()
	{
		return new vec3_t(m0, m4, m8);
	}

	public vec3_t get_trans()
	{
		return new vec3_t(m12, m13, m14);
	}

	public vec3_t up_vector()
	{
		return new vec3_t(m1, m5, m9);
	}

	public vec3_t forward_vector()
	{
		return new vec3_t(m2, m6, m10);
	}

	public void set_right_vector(vec3_t v)
	{
		m0 = v.x;
		m4 = v.y;
		m8 = v.z;
	}

	public void set_up_vector(vec3_t v)
	{
		m1 = v.x;
		m5 = v.y;
		m9 = v.z;
	}

	public void set_forward_vector(vec3_t v)
	{
		m2 = v.x;
		m6 = v.y;
		m10 = v.z;
	}

	public void normalize()
	{
		vec3_t vec3_t2 = right_vector();
		vec3_t2.normalize();
		set_right_vector(vec3_t2);
		vec3_t2 = up_vector();
		vec3_t2.normalize();
		set_up_vector(vec3_t2);
		vec3_t2 = forward_vector();
		vec3_t2.normalize();
		set_forward_vector(vec3_t2);
	}

	public void translate(vec2_t v)
	{
		m12 = m0 * v.v0 + m4 * v.v1 + m8 * 0f + m12;
		m13 = m1 * v.v0 + m5 * v.v1 + m9 * 0f + m13;
		m14 = m2 * v.v0 + m6 * v.v1 + m10 * 0f + m14;
		m15 = m3 * v.v0 + m7 * v.v1 + m11 * 0f + m15;
	}

	public Matrix GetMatrix()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		Matrix result = default(Matrix);
		((Matrix)(ref result))._002Ector(m0, m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12, m13, m14, m15);
		return result;
	}
}
