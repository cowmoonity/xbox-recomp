namespace SuperCow;

public class bsphere_t
{
	public vec3_t center = default(vec3_t);

	public float radius;

	public bsphere_t()
	{
		center.X = (center.Y = (center.Z = (radius = 0f)));
	}

	public bsphere_t(vec3_t[] p_vertexes, int numvert)
	{
		Calculate(p_vertexes, numvert);
	}

	public bsphere_t(vec3_t _center, float _radius)
	{
		center = _center;
	}

	public void assign(vec3_t _center, float _radius)
	{
		center = _center;
		_radius = radius;
	}

	public void Calculate(vec3_t[] p_vertexes, int numvert)
	{
		if (numvert == 0)
		{
			assign(new vec3_t(0f, 0f, 0f), 0f);
			return;
		}
		float x;
		float num = (x = p_vertexes[0].X);
		float y;
		float num2 = (y = p_vertexes[0].Y);
		float z;
		float num3 = (z = p_vertexes[0].Z);
		for (int i = 0; i < numvert; i++)
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
		vec3_t vec3_t2 = new vec3_t((num - x) * 0.5f, (y - num2) * 0.5f, (z - num3) * 0.5f);
		center.assign(x, num2, num3);
		center += vec3_t2;
		radius = vec3_t2.len();
	}
}
