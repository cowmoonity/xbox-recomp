namespace SuperCow;

public static class Collision
{
	public static float intersetRayLine(ref vec2_t result, vec2_t a, vec2_t b, vec2_t origin, vec2_t ray)
	{
		vec2_t vec2_t2 = b - a;
		vec2_t v = origin - a;
		float num = vec2_t2.len();
		vec2_t2 /= num;
		float num2 = vec2_t2.dot(v);
		vec2_t v2 = a + vec2_t2 * num2 - origin;
		float num3 = v2.len();
		v2 /= num3;
		float num4 = ray.dot(v2);
		if (num4 < 0.0001f)
		{
			return -1f;
		}
		float num5 = num3 / num4;
		if (num5 <= 0f)
		{
			return -1f;
		}
		result = origin + ray * num5;
		float num6 = (result - a).dot(vec2_t2);
		if (num6 < 0f || num6 > num)
		{
			return -1f;
		}
		return num5;
	}
}
