using System;

namespace SuperCow;

public static class GlowFXHost
{
	public class natural_object_t
	{
		public vec2_t[] trajectory = new vec2_t[120];

		public vec2_t pos = default(vec2_t);

		public vec2_t size = default(vec2_t);

		public dcolor_t color = default(dcolor_t);

		public int count_trajectory_points;

		public float speed;

		public float distance;
	}

	public class natural_objects_t
	{
		public natural_object_t[] array = new natural_object_t[120];

		public int count;
	}

	public class discharge_t
	{
		public vec2_t[] vertices = new vec2_t[120];

		public int count_vertices;

		public dcolor_t plasma_color = default(dcolor_t);

		public dcolor_t halo_color = default(dcolor_t);

		public dcolor_t light_color = default(dcolor_t);

		public float lehgth;

		public float life_time;

		public float life_timer;

		public float plasma_thickness;

		public float halo_thickness;

		public float light_thickness;

		public float start_thickness;

		public float end_thickness;

		public bool attenuation;

		public bool draw_start;

		public bool draw_end;
	}

	public class discharges_t
	{
		public discharge_t[] array = new discharge_t[64];

		public int count;
	}

	public static texture_t p_flaretex;

	public static texture_t dottexture;

	public static texture_t bugtexture;

	public static natural_objects_t natural_objects = new natural_objects_t();

	public static discharges_t discharges = new discharges_t();

	public static float f_gfxh_randtimer = 0f;

	public static float f_gfxh_timer = 0f;

	public static void gfxhLoad()
	{
		Store.SetCurrentDirectory("", theAdd: false);
		for (int i = 0; i < discharges.array.Length; i++)
		{
			discharges.array[i] = new discharge_t();
		}
		for (int j = 0; j < natural_objects.array.Length; j++)
		{
			natural_objects.array[j] = new natural_object_t();
		}
		bugtexture = SCGlobals.texturehost.Load("bug.jpg");
		p_flaretex = SCGlobals.texturehost.Load("flare.jpg");
		dottexture = SCGlobals.texturehost.Load("dot.jpg");
	}

	public static void gfxhInitFractalTrajectory(vec2_t[] ar_vertices, vec2_t src, vec2_t dst, float std_dev, float factor, int iteration, int max_iteration)
	{
		vec2_t vec2_t2 = default(vec2_t);
		iteration++;
		if (iteration == max_iteration)
		{
			ar_vertices[natural_objects.array[natural_objects.count].count_trajectory_points] = src;
			ar_vertices[natural_objects.array[natural_objects.count].count_trajectory_points + 1] = dst;
			natural_objects.array[natural_objects.count].count_trajectory_points++;
			if (natural_objects.array[natural_objects.count].count_trajectory_points > 510)
			{
				natural_objects.array[natural_objects.count].count_trajectory_points--;
				SuperCowHelper.DebugOut("Out of bounds trajectory");
			}
		}
		else
		{
			float num = 0f;
			num = CRand.frand(1f);
			num *= std_dev;
			vec2_t2.x = 0.5f * (src.x + dst.x) - num * (dst.y - src.y);
			vec2_t2.y = 0.5f * (src.y + dst.y) + num * (dst.x - src.x);
			std_dev *= factor;
			gfxhInitFractalTrajectory(ar_vertices, src, vec2_t2, std_dev, factor, iteration, max_iteration);
			gfxhInitFractalTrajectory(ar_vertices, vec2_t2, dst, std_dev, factor, iteration, max_iteration);
		}
	}

	public static void gfxhAddNaturalObject(vec2_t src, vec2_t dst, vec2_t size, float speed, float dispersal, float evenness, dcolor_t color)
	{
		if (natural_objects.count < 120)
		{
			float factor = (float)Math.Pow(2.0, 1f - evenness / 2f);
			natural_objects.array[natural_objects.count].speed = speed;
			natural_objects.array[natural_objects.count].color = color;
			natural_objects.array[natural_objects.count].size = size;
			natural_objects.array[natural_objects.count].count_trajectory_points = 0;
			gfxhInitFractalTrajectory(natural_objects.array[natural_objects.count].trajectory, src, dst, dispersal, factor, 0, 6);
			natural_objects.array[natural_objects.count].distance = natural_objects.array[natural_objects.count].count_trajectory_points;
			natural_objects.count++;
		}
	}

	public static void gfxhCalculate()
	{
		f_gfxh_randtimer += SCGlobals.ftickperframe * (CRand.frand(0.5f) + 0.5f);
		if (f_gfxh_randtimer > 31415.926f || f_gfxh_randtimer < -31415.926f)
		{
			f_gfxh_randtimer = 0f;
		}
		f_gfxh_timer += SCGlobals.ftickperframe;
		gfxhCalculateNaturalObjects();
		gfxhCalculateDischarges();
	}

	public static void gfxhCalculateNaturalObjects()
	{
		for (int i = 0; i < natural_objects.count; i++)
		{
			gfxhCalculateNaturalObject(i);
		}
	}

	public static void gfxhCalculateNaturalObject(int num_natural_object)
	{
		natural_object_t natural_object_t = natural_objects.array[num_natural_object];
		if (natural_object_t.distance < 0f)
		{
			gfxhDeleteNaturalObject(num_natural_object);
			return;
		}
		int num = (int)natural_object_t.distance;
		natural_object_t.distance -= SCGlobals.ftickperframe * natural_object_t.speed;
		float t = natural_object_t.distance - (float)num;
		natural_objects.array[num_natural_object] = natural_object_t;
		if (num - 1 >= 0 && num + 2 <= natural_object_t.count_trajectory_points)
		{
			natural_object_t.pos = NurbsInterpolate(natural_object_t.trajectory[num - 1], natural_object_t.trajectory[num], natural_object_t.trajectory[num + 1], natural_object_t.trajectory[num + 2], t);
			natural_objects.array[num_natural_object] = natural_object_t;
		}
	}

	public static void gfxhDeleteNaturalObject(int num_natural_object)
	{
		if (num_natural_object >= 0 && num_natural_object < 512 && natural_objects.count != 0)
		{
			natural_objects.array[num_natural_object] = natural_objects.array[natural_objects.count - 1];
			natural_objects.array[natural_objects.count - 1].count_trajectory_points = 0;
			natural_objects.count--;
		}
	}

	public static vec2_t NurbsInterpolate(vec2_t p0, vec2_t p1, vec2_t p2, vec2_t p3, float t)
	{
		vec2_t result = default(vec2_t);
		float num = (0f - p0.x + 3f * p1.x - 3f * p2.x + p3.x) / 6f;
		float num2 = (p0.x - 2f * p1.x + p2.x) / 2f;
		float num3 = (0f - p0.x + p2.x) / 2f;
		float num4 = (p0.x + 4f * p1.x + p2.x) / 6f;
		float num5 = (0f - p0.y + 3f * p1.y - 3f * p2.y + p3.y) / 6f;
		float num6 = (p0.y - 2f * p1.y + p2.y) / 2f;
		float num7 = (0f - p0.y + p2.y) / 2f;
		float num8 = (p0.y + 4f * p1.y + p2.y) / 6f;
		result.x = ((num * t + num2) * t + num3) * t + num4;
		result.y = ((num5 * t + num6) * t + num7) * t + num8;
		return result;
	}

	public static vec2_t LinearInterpolate(vec2_t a, vec2_t b, float x)
	{
		return a * (1f - x) + b * x;
	}

	public static void gfxhDraw()
	{
		_ = SuperCow.DeviceManager.GraphicsDevice;
		CowRender.EnableRS(4u);
		CowRender.SetBlend(3);
		gfxhDrawDischarges();
		CowRender.SetBlend(1);
		gfxhDrawNaturalObjects();
		CowRender.SetBlend(0);
	}

	public static void gfxhDrawNaturalObjects()
	{
		CowRender.BindTexture(dottexture);
		for (int i = 0; i < natural_objects.count; i++)
		{
			gfxhDrawNaturalObject(natural_objects.array[i]);
		}
	}

	public static void gfxhDrawNaturalObject(natural_object_t current_object)
	{
		CowRender.DrawColorQuad2D(current_object.pos, current_object.size.x, current_object.size.y, new frect_t(0f, 0f, 1f, 1f), current_object.color, 0f);
	}

	public static void gfxhInitDischarges()
	{
		for (int i = 0; i < 64; i++)
		{
			discharges.array[i].count_vertices = 0;
		}
		discharges.count = 0;
	}

	public static void gfxhDrawDischarges()
	{
		for (int i = 0; i < discharges.count; i++)
		{
			gfxhDrawDischarge(i);
		}
	}

	public static void gfxhDrawDischarge(int num_discharge)
	{
		if (discharges.array[num_discharge].plasma_thickness > 0f)
		{
			gfxhDrawPlasma(discharges.array[num_discharge].vertices, discharges.array[num_discharge].count_vertices, discharges.array[num_discharge].plasma_thickness, discharges.array[num_discharge].draw_start, discharges.array[num_discharge].draw_end, discharges.array[num_discharge].plasma_color);
		}
		if (discharges.array[num_discharge].halo_thickness > 0f)
		{
			gfxhDrawPlasma(discharges.array[num_discharge].vertices, discharges.array[num_discharge].count_vertices, discharges.array[num_discharge].halo_thickness, discharges.array[num_discharge].draw_start, discharges.array[num_discharge].draw_end, discharges.array[num_discharge].halo_color);
		}
		if (discharges.array[num_discharge].light_thickness > 0f)
		{
			gfxhDrawLighting(discharges.array[num_discharge].vertices, discharges.array[num_discharge].count_vertices, discharges.array[num_discharge].lehgth, discharges.array[num_discharge].light_thickness, discharges.array[num_discharge].light_color);
		}
	}

	public static void gfxhDrawPlasma(vec2_t[] ar_vertices, int count_vertices, float thickness, bool draw_start, bool draw_end, dcolor_t color)
	{
		CowRender.BindTexture(p_flaretex);
		for (int i = 0; i < count_vertices; i++)
		{
			vec3_t vec3_t2 = ((i >= 1) ? (ar_vertices[i - 1].v3t() - ar_vertices[i].v3t()) : (ar_vertices[i].v3t() - ar_vertices[i + 1].v3t())).cross(SCGlobals.cam.forward_vector() * -1f);
			vec3_t2.normalize();
			vec3_t vec3_t3 = (ar_vertices[i].v3t() - ar_vertices[i + 1].v3t()).cross(SCGlobals.cam.forward_vector() * -1f);
			vec3_t3.normalize();
			vec3_t vec3_t4 = ((i <= count_vertices - 2) ? (ar_vertices[i + 1].v3t() - ar_vertices[i + 2].v3t()) : (ar_vertices[i].v3t() - ar_vertices[i + 1].v3t())).cross(SCGlobals.cam.forward_vector() * -1f);
			vec3_t4.normalize();
			vec3_t vec3_t5 = (vec3_t2 + vec3_t3) * 0.5f;
			vec3_t vec3_t6 = (vec3_t3 + vec3_t4) * 0.5f;
			vec3_t5 *= thickness;
			vec3_t6 *= thickness;
			CowRender.DrawColorQuad(vec3_t6 * -1f + ar_vertices[i + 1].v3t(), vec3_t6 + ar_vertices[i + 1].v3t(), vec3_t5 + ar_vertices[i].v3t(), vec3_t5 * -1f + ar_vertices[i].v3t(), color);
		}
		if (draw_end)
		{
			CowRender.BindTexture(dottexture);
			CowRender.DrawColorQuad2D(ar_vertices[count_vertices], thickness * 2f, thickness * 2f, new frect_t(0f, 0f, 1f, 1f), color, 0f);
		}
		if (draw_start)
		{
			CowRender.BindTexture(dottexture);
			CowRender.DrawColorQuad2D(ar_vertices[0], thickness * 2f, thickness * 2f, new frect_t(0f, 0f, 1f, 1f), color, 0f);
		}
	}

	public static void gfxhDrawLighting(vec2_t[] ar_vertices, int count_vertices, float length, float light_thickness, dcolor_t color)
	{
		float num = length;
		vec2_t vec2_t2 = default(vec2_t);
		CowRender.BindTexture(dottexture);
		while (num > 0f)
		{
			int num2 = (int)(num / length * (float)count_vertices);
			float x = num / length * (float)count_vertices - (float)num2;
			vec2_t2 = LinearInterpolate(ar_vertices[num2], ar_vertices[num2 + 1], x);
			num -= light_thickness * 0.5f;
			CowRender.DrawColorQuad2D(vec2_t2, light_thickness, light_thickness, new frect_t(0f, 0f, 1f, 1f), color, 0f);
		}
	}

	public static void gfxhAddDischarges(vec2_t src, vec2_t dst, float life_time, float plasma_thickness, float halo_thickness, float light_thickness, float start_thickness, float end_thickness, float dispersal, float evenness, int lod, bool attenuation, bool draw_start, bool draw_end, dcolor_t plasma_color, dcolor_t halo_color, dcolor_t light_color)
	{
	}

	public static void gfxhCalculateDischarge(int num_discharge)
	{
		if (discharges.array[num_discharge].life_timer < 0f)
		{
			gfxhDeleteDischarges(num_discharge);
			return;
		}
		discharges.array[num_discharge].life_timer -= SCGlobals.ftickperframe;
		if (discharges.array[num_discharge].attenuation)
		{
			float num = discharges.array[num_discharge].life_timer / discharges.array[num_discharge].life_time;
			if (num < 0f)
			{
				num = 0f;
			}
			if (num > 1f)
			{
				num = 1f;
			}
			discharges.array[num_discharge].plasma_color.a = (byte)(num * 255f);
			discharges.array[num_discharge].halo_color.a = (byte)(num * 255f);
			discharges.array[num_discharge].light_color.a = (byte)(num * 255f);
		}
	}

	public static void gfxhDeleteDischarges(int num_discharge)
	{
		if (num_discharge >= 0 && num_discharge < 64 && discharges.count != 0)
		{
			discharges.array[num_discharge].count_vertices = 0;
			discharges.array[discharges.count].lehgth = 0f;
			for (int i = num_discharge; i < discharges.count; i++)
			{
				discharges.array[i] = discharges.array[i + 1];
			}
			discharges.count--;
		}
	}

	public static void gfxhCalculateDischarges()
	{
		for (int i = 0; i < discharges.count; i++)
		{
			gfxhCalculateDischarge(i);
		}
	}
}
