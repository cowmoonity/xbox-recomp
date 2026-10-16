namespace SuperCow;

public class bbox_t
{
	public vec3_t center = default(vec3_t);

	public vec3_t size = default(vec3_t);

	public bbox_t()
	{
		center.X = (center.Y = (center.Z = (size.X = (size.Y = (size.Z = 0f)))));
	}

	public bbox_t(vec3_t[] p_vertexes, int numverts)
	{
		Calculate(p_vertexes, numverts);
	}

	public bbox_t(vec3_t _center, vec3_t _size)
	{
		center = _center;
		size = _size;
	}

	public void assign(vec3_t _center, vec3_t _size)
	{
		center = _center;
		size = _size;
	}

	public void assign(vec3_t _center, float dx, float dy, float dz)
	{
		assign(_center, new vec3_t(dx, dy, dz));
	}

	public void Calculate(vec3_t[] p_vertexes, int numverts)
	{
		if (numverts == 0)
		{
			assign(new vec3_t(0f, 0f, 0f), new vec3_t(0f, 0f, 0f));
			return;
		}
		float x;
		float num = (x = p_vertexes[0].X);
		float y;
		float num2 = (y = p_vertexes[0].Y);
		float z;
		float num3 = (z = p_vertexes[0].Z);
		for (int i = 0; i < numverts; i++)
		{
			if (num < p_vertexes[i].X)
			{
				num = p_vertexes[i].X;
			}
			if (x > p_vertexes[i].X)
			{
				x = p_vertexes[i].X;
			}
			if (y < p_vertexes[i].Y)
			{
				y = p_vertexes[i].Y;
			}
			if (num2 > p_vertexes[i].Y)
			{
				num2 = p_vertexes[i].Y;
			}
			if (z < p_vertexes[i].Z)
			{
				z = p_vertexes[i].Z;
			}
			if (num3 > p_vertexes[i].Z)
			{
				num3 = p_vertexes[i].Z;
			}
		}
		size.assign((num - x) * 0.5f, (y - num2) * 0.5f, (z - num3) * 0.5f);
		center.assign(x, num2, num3);
		center += size;
	}

	public bool PointInBBox(vec3_t point)
	{
		vec3_t vec3_t2 = default(vec3_t);
		vec3_t2 = point - center;
		if (vec3_t2.X < size.X && vec3_t2.X > 0f - size.X && vec3_t2.Y < size.Y && vec3_t2.Y > 0f - size.Y)
		{
			if (vec3_t2.Z < size.Z)
			{
				return vec3_t2.Z > 0f - size.Z;
			}
			return false;
		}
		return false;
	}
}
