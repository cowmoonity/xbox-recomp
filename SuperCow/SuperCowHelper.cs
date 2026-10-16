using System;
using System.Globalization;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperCow;

public class SuperCowHelper
{
	public static float psin(float phase)
	{
		return psin(phase, -1000001f);
	}

	public static float psin(float phase, float t)
	{
		if (t < -1000000f)
		{
			t = SCGlobals.global_timer;
		}
		return 0.5f + 0.5f * (float)Math.Sin(phase * (float)Math.PI * 2f * t * 0.001f);
	}

	public static float Sin(float angle)
	{
		return (float)Math.Sin(DegreeToRadians(angle));
	}

	public static float Cos(float angle)
	{
		return (float)Math.Cos(DegreeToRadians(angle));
	}

	public static int Mystrlen(string theData)
	{
		return theData.Length;
	}

	public static int Mystrstr(string theString, string theFind, int theIndex)
	{
		return theString.IndexOf(theFind, theIndex);
	}

	public static bool str_equal(string str1, string str2)
	{
		return str1 == str2;
	}

	public static void up_case(ref string str)
	{
		str = str.ToUpper();
	}

	public static int get_script_int(string script, string varname, int theIndex)
	{
		string str = null;
		get_script_string(ref str, script, varname, theIndex);
		if (str == null)
		{
			return 0;
		}
		return int.Parse(str, CultureInfo.InvariantCulture);
	}

	public static void get_script_string(ref string str, string script, string varname, int theIndex)
	{
		int num = Mystrstr(script, varname, theIndex);
		if (num == -1)
		{
			str = null;
			return;
		}
		num += Mystrlen(varname);
		int num2 = script.IndexOf("\n", num);
		if (num2 == -1)
		{
			num2 = script.Length - 1;
		}
		str = script.Substring(num, num2 - num - 1);
	}

	public static void get_script_float_array(string script, ref float[] array, string varname, int theIndex)
	{
		string str = "";
		get_script_string(ref str, script, varname, theIndex);
		if (str.Length == 0)
		{
			return;
		}
		int num = 0;
		int num2 = Mystrstr(str, ")", 0);
		if (num2 == -1)
		{
			num2 = Mystrstr(str, "}", 0);
			if (num2 == -1)
			{
				return;
			}
		}
		int num3 = 1;
		int num4 = 0;
		while (true)
		{
			num4 = str.IndexOf(",", num3);
			if (num4 == -1)
			{
				num4 = str.IndexOf("}", num3);
			}
			if (num4 == -1)
			{
				num4 = str.IndexOf(")", num3);
			}
			if (num4 == -1)
			{
				break;
			}
			string s = str.Substring(num3, num4 - num3);
			array[num] = float.Parse(s, CultureInfo.InvariantCulture);
			num++;
			num3 = num4 + 1;
		}
	}

	public static float get_script_float(string script, string varname, int theIndex)
	{
		string str = "";
		get_script_string(ref str, script, varname, theIndex);
		if (str.Length == 0)
		{
			return 0f;
		}
		return float.Parse(str, CultureInfo.InvariantCulture);
	}

	public static vec3_t get_script_vec3_t(string script, string varname, int theIndex)
	{
		vec3_t result = default(vec3_t);
		string str = null;
		get_script_string(ref str, script, varname, theIndex);
		string text = str.Substring(1, str.Length - 1);
		result.X = float.Parse(text, CultureInfo.InvariantCulture);
		int num = text.IndexOf(",");
		text = text.Substring(num + 1, text.Length - (num + 1));
		result.Y = float.Parse(text, CultureInfo.InvariantCulture);
		num = text.IndexOf(",");
		text = text.Substring(num + 1, text.Length - (num + 1));
		result.Z = float.Parse(text, CultureInfo.InvariantCulture);
		return result;
	}

	public static void get_script_paragraph(ref string string_, string script, string varname, int theIndex)
	{
		int num = Mystrstr(script, varname, theIndex);
		if (num != -1)
		{
			num += Mystrlen(varname);
			int i;
			for (i = num; script[i] != '\r' || script[i + 2] != '\r'; i++)
			{
			}
			int num2 = i - num + 1;
			string_ = script.Substring(num, (num2 <= 1) ? 1 : (num2 - 1));
		}
	}

	public static void get_float_array(string str, ref float[] array)
	{
		int num = 0;
		int num2 = Mystrstr(str, ")", 0);
		if (num2 == -1)
		{
			num2 = Mystrstr(str, "}", 0);
			if (num2 == -1)
			{
				return;
			}
		}
		int num3 = 1;
		int num4 = 0;
		while (true)
		{
			num4 = str.IndexOf(",", num3);
			if (num4 == -1)
			{
				num4 = str.IndexOf("}", num3);
			}
			if (num4 == -1)
			{
				num4 = str.IndexOf(")", num3);
			}
			if (num4 == -1)
			{
				break;
			}
			string s = str.Substring(num3, num4 - num3);
			array[num] = float.Parse(s, CultureInfo.InvariantCulture);
			num++;
			num3 = num4 + 1;
		}
	}

	public static byte[] CharToByteArray(char[] str)
	{
		return new byte[str.Length];
	}

	public static string StringFromByteArray(byte[] str)
	{
		return Encoding.ASCII.GetString(str, 0, str.Length);
	}

	public static float DegreeToRadians(float degree)
	{
		return (float)Math.PI / 180f * degree;
	}

	public static float RadiansToDegree(float rad)
	{
		return rad * (180f / (float)Math.PI);
	}

	public static float fast_cos(float angle)
	{
		return (float)Math.Cos(DegreeToRadians(angle));
	}

	public static float fast_sin(float angle)
	{
		return (float)Math.Sin(DegreeToRadians(angle));
	}

	public static void LocalMatrixToMatrix(matrix_t m, ref Matrix o)
	{
		o.M11 = m.m0;
		o.M12 = m.m1;
		o.M13 = m.m2;
		o.M14 = m.m3;
		o.M21 = m.m4;
		o.M22 = m.m5;
		o.M23 = m.m6;
		o.M24 = m.m7;
		o.M31 = m.m8;
		o.M32 = m.m9;
		o.M33 = m.m10;
		o.M34 = m.m11;
		o.M41 = m.m12;
		o.M42 = m.m13;
		o.M43 = m.m14;
		o.M44 = m.m15;
	}

	public static void MatrixToLocalMatrix(ref matrix_t m, Matrix o)
	{
		m.m0 = o.M11;
		m.m1 = o.M12;
		m.m2 = o.M13;
		m.m3 = o.M14;
		m.m4 = o.M21;
		m.m5 = o.M22;
		m.m6 = o.M23;
		m.m7 = o.M24;
		m.m8 = o.M31;
		m.m9 = o.M32;
		m.m10 = o.M33;
		m.m11 = o.M34;
		m.m12 = o.M41;
		m.m13 = o.M42;
		m.m14 = o.M43;
		m.m15 = o.M44;
	}

	public static matrix_t MultMatrices(matrix_t left, matrix_t right)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		Matrix o = default(Matrix);
		Matrix o2 = default(Matrix);
		LocalMatrixToMatrix(left, ref o);
		LocalMatrixToMatrix(right, ref o2);
		Matrix o3 = o * o2;
		MatrixToLocalMatrix(ref left, o3);
		return left;
	}

	public static vertex_t_d3d ToVertexD3D(vertex_t v)
	{
		return new vertex_t_d3d
		{
			normal = 
			{
				X = v.normal.X,
				Y = v.normal.Y,
				Z = v.normal.Z
			},
			xyz = 
			{
				X = v.xyz.X,
				Y = v.xyz.Y,
				Z = v.xyz.Z
			},
			uv = 
			{
				X = v.uv.X,
				Y = v.uv.Y
			}
		};
	}

	public static void SetCurrentDirectory(string theDir, bool theAdd)
	{
		if (!theAdd)
		{
			SCGlobals.gDirectory = theDir;
			return;
		}
		SCGlobals.gDirectory += theDir;
		SCGlobals.gDirectory += "\\";
	}

	public static void Memset<T>(T[] arr, T value)
	{
		for (int i = 0; i < arr.Length; i++)
		{
			arr[i] = value;
		}
	}

	public static void DebugOut(string str)
	{
	}

	public static vec2_t closestPointOnLine(vec2_t a, vec2_t b, vec2_t p)
	{
		vec2_t v = p - a;
		vec2_t vec2_t2 = b - a;
		float num = vec2_t2.len();
		vec2_t2 /= num;
		float num2 = vec2_t2.dot(v);
		if (num2 < 0f)
		{
			return a;
		}
		if (num2 > num)
		{
			return b;
		}
		return a + vec2_t2 * num2;
	}

	public static string StripTrash(string instring)
	{
		int num = 0;
		byte[] bytes = Encoding.ASCII.GetBytes(instring);
		for (int i = 0; i < bytes.Length && bytes[i] > 30; i++)
		{
			num++;
		}
		byte[] array = new byte[num];
		Buffer.BlockCopy(bytes, 0, array, 0, num);
		return Encoding.ASCII.GetString(array, 0, num);
	}

	public static void SetWinTexCoord(int id, float x0, float y0, float sizex, float sizey)
	{
		float num = 0.004f;
		SCGlobals.ar_wintexcoords[id].assign(x0 * 0.25f + num, y0 * 0.5f + num, (x0 + sizex) * 0.25f - num, (y0 + sizey) * 0.5f - num);
	}

	public static void SetMITexCoord(int id, float x0, float y0, float sizex, float sizey)
	{
		float num = 0.25f;
		float num2 = 0.125f;
		float num3 = 0.004f;
		SCGlobals.mitexcoords[id, 0].assign(x0 * num + num3, y0 * num2 + num3);
		SCGlobals.mitexcoords[id, 1].assign((x0 + sizex) * num - num3, y0 * num2 + num3);
		SCGlobals.mitexcoords[id, 2].assign((x0 + sizex) * num - num3, (y0 + sizey) * num2 - num3);
		SCGlobals.mitexcoords[id, 3].assign(x0 * num + num3, (y0 + sizey) * num2 - num3);
		SCGlobals.misizes[id].assign(sizex * 64f, sizey * 64f);
	}
}
