using System;

namespace SuperCow;

public class CFrustum
{
	public float[][] m_Frustum = new float[6][]
	{
		new float[4],
		new float[4],
		new float[4],
		new float[4],
		new float[4],
		new float[4]
	};

	public void Calculate()
	{
		matrix_t gProjectionMatrix = SCGlobals.gProjectionMatrix;
		matrix_t gViewMatrix = SCGlobals.gViewMatrix;
		gProjectionMatrix *= gViewMatrix;
		for (int i = 0; i < 3; i++)
		{
			m_Frustum[i * 2][0] = gProjectionMatrix.m3 - gProjectionMatrix.GetValue(i);
			m_Frustum[i * 2][1] = gProjectionMatrix.m7 - gProjectionMatrix.GetValue(4 + i);
			m_Frustum[i * 2][2] = gProjectionMatrix.m11 - gProjectionMatrix.GetValue(8 + i);
			m_Frustum[i * 2][3] = gProjectionMatrix.m15 - gProjectionMatrix.GetValue(12 + i);
			NormalizePlane(ref m_Frustum, i * 2);
			m_Frustum[i * 2 + 1][0] = gProjectionMatrix.m3 + gProjectionMatrix.GetValue(i);
			m_Frustum[i * 2 + 1][1] = gProjectionMatrix.m7 + gProjectionMatrix.GetValue(4 + i);
			m_Frustum[i * 2 + 1][2] = gProjectionMatrix.m11 + gProjectionMatrix.GetValue(8 + i);
			m_Frustum[i * 2 + 1][3] = gProjectionMatrix.m15 + gProjectionMatrix.GetValue(12 + i);
			NormalizePlane(ref m_Frustum, i * 2 + 1);
		}
	}

	public bool PointInFrustum(float x, float y, float z)
	{
		for (int i = 0; i < 6; i++)
		{
			if (m_Frustum[i][0] * x + m_Frustum[i][1] * y + m_Frustum[i][2] * z + m_Frustum[i][3] <= 0f)
			{
				return false;
			}
		}
		return true;
	}

	public bool PointInFrustum(vec3_t vec)
	{
		return PointInFrustum(vec.X, vec.Y, vec.Z);
	}

	public bool CubeInFrustum(float x, float y, float z, float size)
	{
		for (int i = 0; i < 6; i++)
		{
			if (!(m_Frustum[i][0] * (x - size) + m_Frustum[i][1] * (y - size) + m_Frustum[i][2] * (z - size) + m_Frustum[i][3] > 0f) && !(m_Frustum[i][0] * (x + size) + m_Frustum[i][1] * (y - size) + m_Frustum[i][2] * (z - size) + m_Frustum[i][3] > 0f) && !(m_Frustum[i][0] * (x - size) + m_Frustum[i][1] * (y + size) + m_Frustum[i][2] * (z - size) + m_Frustum[i][3] > 0f) && !(m_Frustum[i][0] * (x + size) + m_Frustum[i][1] * (y + size) + m_Frustum[i][2] * (z - size) + m_Frustum[i][3] > 0f) && !(m_Frustum[i][0] * (x - size) + m_Frustum[i][1] * (y - size) + m_Frustum[i][2] * (z + size) + m_Frustum[i][3] > 0f) && !(m_Frustum[i][0] * (x + size) + m_Frustum[i][1] * (y - size) + m_Frustum[i][2] * (z + size) + m_Frustum[i][3] > 0f) && !(m_Frustum[i][0] * (x - size) + m_Frustum[i][1] * (y + size) + m_Frustum[i][2] * (z + size) + m_Frustum[i][3] > 0f) && !(m_Frustum[i][0] * (x + size) + m_Frustum[i][1] * (y + size) + m_Frustum[i][2] * (z + size) + m_Frustum[i][3] > 0f))
			{
				return false;
			}
		}
		return true;
	}

	public bool BoxInFrustum(bbox_t bbox)
	{
		for (int i = 0; i < 6; i++)
		{
			if (!(m_Frustum[i][0] * (bbox.center.X - bbox.size.X) + m_Frustum[i][1] * (bbox.center.Y - bbox.size.Y) + m_Frustum[i][2] * (bbox.center.Z - bbox.size.Z) + m_Frustum[i][3] > 0f) && !(m_Frustum[i][0] * (bbox.center.X + bbox.size.X) + m_Frustum[i][1] * (bbox.center.Y - bbox.size.Y) + m_Frustum[i][2] * (bbox.center.Z - bbox.size.Z) + m_Frustum[i][3] > 0f) && !(m_Frustum[i][0] * (bbox.center.X - bbox.size.X) + m_Frustum[i][1] * (bbox.center.Y + bbox.size.Y) + m_Frustum[i][2] * (bbox.center.Z - bbox.size.Z) + m_Frustum[i][3] > 0f) && !(m_Frustum[i][0] * (bbox.center.X + bbox.size.X) + m_Frustum[i][1] * (bbox.center.Y + bbox.size.Y) + m_Frustum[i][2] * (bbox.center.Z - bbox.size.Z) + m_Frustum[i][3] > 0f) && !(m_Frustum[i][0] * (bbox.center.X - bbox.size.X) + m_Frustum[i][1] * (bbox.center.Y - bbox.size.Y) + m_Frustum[i][2] * (bbox.center.Z + bbox.size.Z) + m_Frustum[i][3] > 0f) && !(m_Frustum[i][0] * (bbox.center.X + bbox.size.X) + m_Frustum[i][1] * (bbox.center.Y - bbox.size.Y) + m_Frustum[i][2] * (bbox.center.Z + bbox.size.Z) + m_Frustum[i][3] > 0f) && !(m_Frustum[i][0] * (bbox.center.X - bbox.size.X) + m_Frustum[i][1] * (bbox.center.Y + bbox.size.Y) + m_Frustum[i][2] * (bbox.center.Z + bbox.size.Z) + m_Frustum[i][3] > 0f) && !(m_Frustum[i][0] * (bbox.center.X + bbox.size.X) + m_Frustum[i][1] * (bbox.center.Y + bbox.size.Y) + m_Frustum[i][2] * (bbox.center.Z + bbox.size.Z) + m_Frustum[i][3] > 0f))
			{
				return false;
			}
		}
		return true;
	}

	public bool SphereInFrustum(float x, float y, float z, float radius)
	{
		for (int i = 0; i < 6; i++)
		{
			if (m_Frustum[i][0] * x + m_Frustum[i][1] * y + m_Frustum[i][2] * z + m_Frustum[i][3] <= 0f - radius)
			{
				return false;
			}
		}
		return true;
	}

	public void NormalizePlane(ref float[][] frustum, int side)
	{
		float num = 1f / (float)Math.Sqrt(frustum[side][0] * frustum[side][0] + frustum[side][1] * frustum[side][1] + frustum[side][2] * frustum[side][2]);
		frustum[side][0] *= num;
		frustum[side][1] *= num;
		frustum[side][2] *= num;
		frustum[side][3] *= num;
	}
}
