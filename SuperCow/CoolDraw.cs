using System;

namespace SuperCow;

public static class CoolDraw
{
	public struct lightning_t
	{
		public vec3_t[] ar_lightlines;

		public int numlines;

		public dcolor_t col1;

		public dcolor_t col2;

		public float fsize1;

		public float fsize2;
	}

	public static int MAX_NUM_LIGHTING = 64;

	public static lightning_t[] ar_lightings = new lightning_t[MAX_NUM_LIGHTING];

	public static int numlightings = 0;

	public static void AddLightning(vec3_t src, vec3_t dst, float lighting_step, float fsize1, float fsize2, float dev, dcolor_t col1, dcolor_t col2)
	{
		if (numlightings != MAX_NUM_LIGHTING - 1)
		{
			CRand.pushrandpos();
			CRand.frandomize(Environment.TickCount % 256);
			lightning_t lightning_t = new lightning_t
			{
				ar_lightlines = new vec3_t[101]
			};
			vec3_t vec3_t2 = dst - src;
			vec3_t vec3_t3 = vec3_t2.cross(new vec3_t(0f, 0f, 1f));
			float num = vec3_t2.len();
			vec3_t2 /= num;
			lightning_t.numlines = (int)(num / lighting_step) + 1;
			if (lightning_t.numlines >= 100)
			{
				lightning_t.numlines = 100;
				lighting_step = num / 100f;
			}
			lightning_t.col1 = col1;
			lightning_t.col2 = col2;
			lightning_t.fsize1 = fsize1;
			lightning_t.fsize2 = fsize2;
			vec3_t3.normalize();
			lightning_t.ar_lightlines[0] = src;
			for (int i = 1; i < lightning_t.numlines; i++)
			{
				ref vec3_t reference = ref lightning_t.ar_lightlines[i];
				reference = src + vec3_t2 * lighting_step * i;
				float angle = CRand.frand(180f);
				vec3_t vec3_t4 = vec3_t3;
				vec3_t4.rotate(angle, vec3_t2);
				vec3_t4 *= CRand.frand(dev);
				lightning_t.ar_lightlines[i] += vec3_t4;
			}
			lightning_t.ar_lightlines[lightning_t.numlines] = dst;
			CRand.poprandpos();
			ar_lightings[numlightings] = lightning_t;
			numlightings++;
		}
	}

	public static void DrawLightning()
	{
		_ = SuperCow.DeviceManager.GraphicsDevice;
		CowRender.SetLighting(theEnable: false);
		CowRender.EnableRS(4u);
		CowRender.SetBlend(3);
		CowRender.DisableRS(2u);
		CowRender.BindTexture(SCGlobals.flaretexture);
		for (int i = 0; i < numlightings; i++)
		{
			lightning_t lightning_t = ar_lightings[i];
			vec3_t vec3_t2 = lightning_t.ar_lightlines[lightning_t.numlines - 1] - lightning_t.ar_lightlines[0];
			vec3_t2.normalize();
			vec3_t2 *= 0.05f;
			CowRender.DrawLine(lightning_t.ar_lightlines[0], lightning_t.ar_lightlines[lightning_t.numlines], lightning_t.fsize2, lightning_t.col2);
			for (int j = 0; j < lightning_t.numlines; j++)
			{
				CowRender.DrawLine(lightning_t.ar_lightlines[j], lightning_t.ar_lightlines[j + 1] + vec3_t2, lightning_t.fsize1, lightning_t.col1);
			}
		}
		numlightings = 0;
		CowRender.EnableRS(2u);
		CowRender.SetBlend(0);
	}
}
