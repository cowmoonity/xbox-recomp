using System;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public static class Particles
{
	public static color_t stcolor2 = new color_t(0.1f, 0.1f, 0.1f, 1f);

	public static vec2_t temp = default(vec2_t);

	public static vec2_t dir = default(vec2_t);

	public static int particles_sizex;

	public static int particles_sizey;

	public static vec2_t t = default(vec2_t);

	public static vec2_t rt = default(vec2_t);

	public static vec2_t ut = default(vec2_t);

	public static vec3_t vector = new vec3_t(0f, 2f, 0f);

	public static vec3_t emptyvector = new vec3_t(0f, 0f, 0f);

	public static vec3_t vec = default(vec3_t);

	public static dcolor_t stcolor = default(dcolor_t);

	public static int MAX_DYNAMIC_PARTICLES = 2048;

	public static int MAX_DYNAMIC_SOURCES = 256;

	public static int MAX_NUM_PARTICLES = 4096;

	public static int PARTICLE_SIZE_IN_PIXELS = 64;

	public static dynamicparticle_t[] ar_dynamicparticles = new dynamicparticle_t[2048];

	public static int dynamicparticlesindex = 0;

	public static dynamicsource_t[] ar_dynamicsources = new dynamicsource_t[256];

	public static int dynamicsourcesindex = 0;

	public static particle_t[] ar_partcles = new particle_t[MAX_NUM_PARTICLES];

	public static int numparticles_ = 0;

	public static particle_t curp = ar_partcles[0];

	public static particle_t firstp = null;

	public static int MAX_DYNAMIC_TEXT = 128;

	public static string[] ar_dynamictext = new string[MAX_DYNAMIC_TEXT];

	public static int dynamictextindex = 0;

	public static int curparticlelayer = 0;

	public static void SetParticleLayer(int nlayer)
	{
		curparticlelayer = nlayer;
	}

	public static void DrawParticle(vec3_t p, float rot, float size, dcolor_t color, int num, bool toBuffer)
	{
		vec3_t vec3_t2 = SCGlobals.cam.up_vector() * (size * 0.5f);
		vec3_t vec3_t3 = SCGlobals.cam.right_vector() * (size * 0.5f);
		vec3_t vec3_t4 = vec3_t3 * SuperCowHelper.fast_sin(rot) + vec3_t2 * SuperCowHelper.fast_cos(rot);
		vec3_t vec3_t5 = vec3_t3 * SuperCowHelper.fast_cos(rot) - vec3_t2 * SuperCowHelper.fast_sin(rot);
		float num2 = 1f / (float)particles_sizex;
		float num3 = 1f / (float)particles_sizey;
		int num4 = num % particles_sizex;
		int num5 = num / particles_sizex;
		float v = num2 * (float)num4 + 0.001f;
		float v2 = num3 * (float)num5 + 0.001f;
		t.v0 = v;
		t.v1 = v2;
		rt.v0 = num2 - 0.001f;
		rt.v1 = 0f;
		ut.v0 = 0f;
		ut.v1 = num3 - 0.001f;
		CowRender.DrawColorQuadDynBuffer(p + vec3_t5 + vec3_t4, p + vec3_t5 - vec3_t4, p - vec3_t5 - vec3_t4, p - vec3_t5 + vec3_t4, t, t + rt, t + ut + rt, t + ut, color);
	}

	public static void InitParticles()
	{
		numparticles_ = 0;
		curp = ar_partcles[0];
		firstp = null;
		Array.Clear(ar_dynamicparticles, 0, ar_dynamicparticles.Length);
		Array.Clear(ar_dynamicsources, 0, ar_dynamicsources.Length);
		for (int i = 0; i < ar_partcles.Length; i++)
		{
			ar_partcles[i] = new particle_t();
		}
		for (int j = 0; j < ar_dynamicparticles.Length; j++)
		{
			ar_dynamicparticles[j].c0 = color_t.ColorWhite;
			ar_dynamicparticles[j].c1 = color_t.ColorWhite;
			ar_dynamicparticles[j].dpos = default(vec3_t);
			ar_dynamicparticles[j].pos = default(vec3_t);
			ar_dynamicparticles[j].dpos0 = default(vec3_t);
		}
		dynamicparticlesindex = 0;
		dynamicsourcesindex = 0;
		SCGlobals.p_particlestex = SCGlobals.texturehost.Load("particles.tga");
		particles_sizex = (int)SCGlobals.p_particlestex.width / PARTICLE_SIZE_IN_PIXELS;
		particles_sizey = (int)SCGlobals.p_particlestex.height / PARTICLE_SIZE_IN_PIXELS;
	}

	public static void AddParticle(vec3_t p, float rot, float size, dcolor_t color, int blendtype, int num)
	{
		if (!SCGlobals.frustum.SphereInFrustum(p.x, p.y, p.z, size) || numparticles_ >= MAX_NUM_PARTICLES - 1)
		{
			return;
		}
		particle_t particle_t2 = ar_partcles[numparticles_];
		particle_t2.pos = p;
		particle_t2.rot = rot;
		particle_t2.size = size;
		particle_t2.color = color;
		particle_t2.num = num;
		particle_t2.blendtype = blendtype;
		float num2 = (particle_t2.dist = SCGlobals.frustum.m_Frustum[5][0] * p.x + SCGlobals.frustum.m_Frustum[5][1] * p.y + SCGlobals.frustum.m_Frustum[5][2] * p.z + SCGlobals.frustum.m_Frustum[5][3]);
		ar_partcles[numparticles_] = particle_t2;
		if (numparticles_ == 0)
		{
			curp = ar_partcles[0];
			curp.next = null;
			curp.prev = null;
			firstp = curp;
			numparticles_ = 1;
			return;
		}
		if (num2 < curp.dist)
		{
			while (true)
			{
				if (curp.next == null)
				{
					curp.next = ar_partcles[numparticles_];
					ar_partcles[numparticles_].prev = curp;
					ar_partcles[numparticles_].next = null;
					curp = ar_partcles[numparticles_];
					break;
				}
				if (num2 > curp.next.dist)
				{
					particle_t next = curp.next;
					curp.next = ar_partcles[numparticles_];
					ar_partcles[numparticles_].prev = curp;
					ar_partcles[numparticles_].next = next;
					next.prev = ar_partcles[numparticles_];
					curp = ar_partcles[numparticles_];
					break;
				}
				curp = curp.next;
			}
		}
		else
		{
			while (true)
			{
				if (curp.prev == null)
				{
					curp.prev = ar_partcles[numparticles_];
					ar_partcles[numparticles_].next = curp;
					ar_partcles[numparticles_].prev = null;
					curp = ar_partcles[numparticles_];
					firstp = curp;
					break;
				}
				if (num2 < curp.prev.dist)
				{
					particle_t next = curp.prev;
					curp.prev = ar_partcles[numparticles_];
					ar_partcles[numparticles_].next = curp;
					ar_partcles[numparticles_].prev = next;
					next.next = ar_partcles[numparticles_];
					curp = ar_partcles[numparticles_];
					break;
				}
				curp = curp.prev;
			}
		}
		numparticles_++;
	}

	public static void AddParticle(vec2_t p, float rot, float size, dcolor_t color, int blendtype, int num)
	{
		vec.assign(p.x, p.y, 0f);
		AddParticle(vec, rot, size, color, blendtype, num);
	}

	public static void DrawParticles()
	{
		_ = SuperCow.DeviceManager.GraphicsDevice;
		CowRender.LoadMatrixNull();
		if (!SCGlobals.b_shadows)
		{
			CowRender.SetLighting(theEnable: false);
		}
		CowRender.EnableRS(4u);
		CowRender.SetBlend(1);
		CowRender.DisableRS(5u);
		CowRender.EnableRS(7u);
		CowRender.DisableRS(2u);
		CowRender.EnableRS(1u);
		CowRender.BindTexture(SCGlobals.p_particlestex);
		SCGlobals.numVisParticles = 0;
		SCGlobals.numBlendingChanges = 0;
		while (firstp != null)
		{
			bool flag = false;
			if (firstp.prev == null)
			{
				flag = true;
			}
			else if (firstp.blendtype != firstp.prev.blendtype)
			{
				flag = true;
			}
			if (flag)
			{
				CowRender.DrawDynBuffer();
				switch (firstp.blendtype)
				{
				case 0:
					CowRender.SetBlend(1);
					break;
				case 1:
					CowRender.SetBlend(5);
					break;
				case 2:
					CowRender.SetBlend(0);
					break;
				case 3:
					CowRender.SetBlend(1);
					break;
				case 4:
					CowRender.SetBlend(3);
					break;
				}
				SCGlobals.numBlendingChanges++;
			}
			DrawParticle(firstp.pos, firstp.rot, firstp.size, firstp.color, firstp.num, toBuffer: true);
			firstp = firstp.next;
			SCGlobals.numVisParticles++;
		}
		CowRender.DrawDynBuffer();
		numparticles_ = 0;
		firstp = null;
		CowRender.EnableRS(5u);
		CowRender.DisableRS(1u);
		CowRender.EnableRS(2u);
		CowRender.SetBlend(0);
		CowRender.DisableRS(7u);
	}

	public static void AddDynamicText(vec2_t pos, string str, int type)
	{
		vec.assign(pos.x, pos.y, 0f);
		AddDynamicText3D(vec, str, type);
	}

	public static void AddDynamicText(vec3_t pos, string str, int type)
	{
		AddDynamicText3D(pos, str, type);
	}

	public static void AddDynamicText3D(vec3_t pos, string str, int type)
	{
		string text = ar_dynamictext[dynamictextindex];
		text = str;
		dynamictextindex = (dynamictextindex + 1) % MAX_DYNAMIC_TEXT;
		ar_dynamictext[dynamictextindex] = text;
		dynamicparticle_t dynamicparticle_t2 = ar_dynamicparticles[dynamicparticlesindex];
		dynamicparticlesindex = (dynamicparticlesindex + 1) % MAX_DYNAMIC_PARTICLES;
		ref color_t c = ref dynamicparticle_t2.c0;
		ref color_t c2 = ref dynamicparticle_t2.c0;
		ref color_t c3 = ref dynamicparticle_t2.c0;
		float num = (dynamicparticle_t2.c0.a = 1f);
		float num3 = (c3.b = num);
		float r = (c2.g = num3);
		c.r = r;
		ref color_t c4 = ref dynamicparticle_t2.c1;
		ref color_t c5 = ref dynamicparticle_t2.c1;
		ref color_t c6 = ref dynamicparticle_t2.c1;
		float num6 = (dynamicparticle_t2.c1.a = 1f);
		float num8 = (c6.b = num6);
		float r2 = (c5.g = num8);
		c4.r = r2;
		dynamicparticle_t2.type = type;
		dynamicparticle_t2.curtime = 0f;
		dynamicparticle_t2.text = text;
		dynamicparticle_t2.blendtype = -1;
		switch (type)
		{
		case 0:
			dynamicparticle_t2.pos = pos;
			dynamicparticle_t2.dpos.assign(CRand.frand(0.002f), 0.006f + CRand.prand(0.002f), 0f);
			dynamicparticle_t2.rot = -90f + CRand.frand(20f);
			dynamicparticle_t2.drot = CRand.frand(0.1f);
			dynamicparticle_t2.size = 0.7f;
			dynamicparticle_t2.dsize = dynamicparticle_t2.size * 0.5f;
			dynamicparticle_t2.c0.a = 0.8f;
			dynamicparticle_t2.lifetime = 1300f;
			dynamicparticle_t2.grav = -0.0001f;
			break;
		case 1:
			dynamicparticle_t2.pos = pos;
			dynamicparticle_t2.dpos.assign(CRand.frand(0.002f), 0.002f + CRand.prand(0.002f), 0f);
			dynamicparticle_t2.rot = -90f + CRand.frand(20f);
			dynamicparticle_t2.drot = CRand.frand(0.1f);
			dynamicparticle_t2.size = 1f;
			dynamicparticle_t2.dsize = dynamicparticle_t2.size * 0.5f;
			dynamicparticle_t2.c0.a = 0.8f;
			dynamicparticle_t2.lifetime = 1300f;
			dynamicparticle_t2.grav = -0.0001f;
			break;
		case 2:
		case 3:
			dynamicparticle_t2.pos = pos;
			dynamicparticle_t2.dpos.assign(0.004f + CRand.prand(0.001f), 0.001f, 0f);
			if (type == 3)
			{
				dynamicparticle_t2.dpos.x *= -1f;
			}
			dynamicparticle_t2.rot = -90f + CRand.frand(20f);
			dynamicparticle_t2.drot = CRand.frand(0.1f);
			dynamicparticle_t2.size = 0.7f;
			dynamicparticle_t2.dsize = dynamicparticle_t2.size * 0.5f;
			dynamicparticle_t2.c0.a = 0.8f;
			dynamicparticle_t2.lifetime = 1300f;
			dynamicparticle_t2.grav = 0.00015f;
			break;
		case 4:
			dynamicparticle_t2.pos = pos;
			dynamicparticle_t2.dpos.assign(0f, 0.001f, 0f);
			dynamicparticle_t2.size = 0.7f;
			dynamicparticle_t2.dsize = dynamicparticle_t2.size * 0.5f;
			dynamicparticle_t2.c0.a = 0.8f;
			dynamicparticle_t2.lifetime = 1300f;
			dynamicparticle_t2.grav = -0.0001f;
			break;
		}
		int num11 = ((dynamicparticlesindex > 0) ? (dynamicparticlesindex - 1) : 0);
		ar_dynamicparticles[num11] = dynamicparticle_t2;
	}

	public static void AddDynamicParticle2D(vec2_t pos, vec2_t dpos, color_t c0, color_t c1, float rot, float drot, float size, float dsize, float grav, float lifetime, int texnum, int blendtype, int type)
	{
		dynamicparticle_t dynamicparticle_t2 = ar_dynamicparticles[dynamicparticlesindex];
		dynamicparticlesindex = (dynamicparticlesindex + 1) % MAX_DYNAMIC_PARTICLES;
		dynamicparticle_t2.pos.assign(pos.x, pos.y, 0f);
		dynamicparticle_t2.dpos.assign(dpos.x, dpos.y, 0f);
		dynamicparticle_t2.c0 = c0;
		dynamicparticle_t2.c1 = c1;
		dynamicparticle_t2.grav = grav;
		dynamicparticle_t2.lifetime = lifetime;
		dynamicparticle_t2.dpos0 = dpos.v3t();
		dynamicparticle_t2.rot = rot;
		dynamicparticle_t2.size = size;
		dynamicparticle_t2.dsize = dsize;
		dynamicparticle_t2.num = texnum;
		dynamicparticle_t2.blendtype = blendtype;
		dynamicparticle_t2.type = type;
		dynamicparticle_t2.curtime = 0f;
		dynamicparticle_t2.dpos0 = dynamicparticle_t2.dpos;
		dynamicparticle_t2.drot0 = dynamicparticle_t2.drot;
		dynamicparticle_t2.nlayer = curparticlelayer;
		int num = ((dynamicparticlesindex > 0) ? (dynamicparticlesindex - 1) : 0);
		ar_dynamicparticles[num] = dynamicparticle_t2;
	}

	public static void CalculateDynamicParticles()
	{
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		CowRender.SetBlend(0);
		CowRender.EnableRS(4u);
		CowRender.EnableRS(5u);
		graphicsDevice.RenderState.ReferenceAlpha = 1;
		vec3_t vec3_t2 = SCGlobals.hero.framepos + vector;
		for (int i = 0; i < MAX_DYNAMIC_PARTICLES; i++)
		{
			dynamicparticle_t dynamicparticle_t2 = ar_dynamicparticles[i];
			if (dynamicparticle_t2.nlayer != curparticlelayer)
			{
				continue;
			}
			if (dynamicparticle_t2.curtime < dynamicparticle_t2.lifetime)
			{
				float num = dynamicparticle_t2.curtime / dynamicparticle_t2.lifetime;
				float num2 = 1f - num;
				color_t color_t2 = dynamicparticle_t2.c0 * num2 + dynamicparticle_t2.c1 * num;
				dcolor_t dcolor_t2 = color_t2.dcolor();
				if (dynamicparticle_t2.type == 18 || dynamicparticle_t2.type == 19)
				{
					dynamicparticle_t2.pos = dynamicparticle_t2.pos.interp(vec3_t2, num * num * num);
					dynamicparticle_t2.drot = dynamicparticle_t2.drot0 * num2;
					dynamicparticle_t2.dpos = dynamicparticle_t2.dpos0 * num2;
					if ((dynamicparticle_t2.pos - vec3_t2).lenlen() < 0.5f && num > 0.5f)
					{
						dynamicparticle_t2.curtime = dynamicparticle_t2.lifetime + 1f;
						ar_dynamicparticles[i] = dynamicparticle_t2;
						continue;
					}
				}
				if (dynamicparticle_t2.type == 20)
				{
					dcolor_t2 = (color_t2 * (float)Math.Sin((float)Math.PI * num)).dcolor();
				}
				if (dynamicparticle_t2.blendtype == -1)
				{
					Text.SetTextColor(dcolor_t2);
					Text.OutTextCenterXY(dynamicparticle_t2.pos.x, dynamicparticle_t2.pos.y, dynamicparticle_t2.size * num2 + dynamicparticle_t2.dsize * num, dynamicparticle_t2.text);
				}
				else
				{
					AddParticle(dynamicparticle_t2.pos, dynamicparticle_t2.rot, dynamicparticle_t2.size * num2 + dynamicparticle_t2.dsize * num, dcolor_t2, dynamicparticle_t2.blendtype, dynamicparticle_t2.num);
				}
				dynamicparticle_t2.pos += dynamicparticle_t2.dpos * SCGlobals.ftickperframe;
				dynamicparticle_t2.dpos.y += dynamicparticle_t2.grav * SCGlobals.ftickperframe * 0.05f;
				dynamicparticle_t2.rot += dynamicparticle_t2.drot * SCGlobals.ftickperframe;
				dynamicparticle_t2.curtime += SCGlobals.ftickperframe;
			}
			ar_dynamicparticles[i] = dynamicparticle_t2;
		}
	}

	public static void AddParticleSource(vec3_t pos, int type)
	{
		dynamicsource_t dynamicsource_t2 = ar_dynamicsources[dynamicsourcesindex];
		dynamicsourcesindex = (dynamicsourcesindex + 1) % MAX_DYNAMIC_SOURCES;
		dynamicsource_t2.pos = pos;
		dynamicsource_t2.type = type;
		switch (type)
		{
		case 0:
			dynamicsource_t2.num = 6;
			break;
		case 1:
			dynamicsource_t2.num = 8;
			break;
		case 2:
			dynamicsource_t2.num = 10;
			break;
		case 3:
		case 4:
		case 5:
			dynamicsource_t2.num = 5;
			break;
		}
		dynamicsource_t2.lifetime = 1f;
		dynamicsource_t2.intensivity = 1f;
		if (type >= 3)
		{
			dynamicsource_t2.intensivity = 199f;
			dynamicsource_t2.lifetime = 400f;
		}
		dynamicsource_t2.dintensivity = dynamicsource_t2.intensivity;
		dynamicsource_t2.intensivity = 0f;
		int num = ((dynamicsourcesindex != 0) ? (dynamicsourcesindex - 1) : 0);
		ar_dynamicsources[num] = dynamicsource_t2;
	}

	public static void CalculateDynamicSource()
	{
		stcolor.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
		color_t colorWhite = color_t.ColorWhite;
		float grav = 0f;
		for (int i = 0; i < MAX_DYNAMIC_SOURCES; i++)
		{
			if (!(ar_dynamicsources[i].lifetime > 0f))
			{
				continue;
			}
			dynamicsource_t dynamicsource_t2 = ar_dynamicsources[i];
			dynamicsource_t2.intensivity -= SCGlobals.ftickperframe;
			dynamicsource_t2.lifetime -= SCGlobals.ftickperframe;
			if (dynamicsource_t2.intensivity < 0f)
			{
				dynamicsource_t2.intensivity += dynamicsource_t2.dintensivity;
				for (int j = 0; j < dynamicsource_t2.num; j++)
				{
					float num = (float)Math.PI * 2f / (float)dynamicsource_t2.num;
					float num2 = num * (float)j + num / 2f * dynamicsource_t2.lifetime / dynamicsource_t2.dintensivity;
					dir.assign((float)Math.Cos(num2), (float)Math.Sin(num2));
					dir.normalize();
					dir *= 0.008f;
					if (dynamicsource_t2.type >= 3)
					{
						switch (dynamicsource_t2.type)
						{
						case 3:
							colorWhite.assign(1f, 0.5f, 0.5f, 1f);
							break;
						case 4:
							colorWhite.assign(0.5f, 1f, 0.5f, 1f);
							break;
						case 5:
							colorWhite.assign(0.5f, 0.5f, 1f, 1f);
							break;
						}
						dir *= 0.6f;
					}
					else
					{
						colorWhite.assign(0.5f + CRand.prand(0.5f), 0.5f + CRand.prand(0.5f), 0.5f + CRand.prand(0.5f), 1f);
						dir *= 0.5f + 0.5f * (float)dynamicsource_t2.type / 3f;
					}
					temp.assign(dynamicsource_t2.pos.x, dynamicsource_t2.pos.y);
					AddDynamicParticle2D(temp, dir, colorWhite, stcolor2, 0f, 0f, 2.2f, 1f, grav, 1200f, 12, 0, 27);
				}
			}
			ar_dynamicsources[i] = dynamicsource_t2;
		}
	}

	public static void CalculateParticles()
	{
		CalculateDynamicSource();
		CalculateDynamicParticles();
	}

	public static void AddNewDynamicParticle(vec3_t pos, int type, int texnum, vec3_t dir)
	{
		dynamicparticle_t dynamicparticle_t2 = ar_dynamicparticles[dynamicparticlesindex];
		dynamicparticlesindex = (dynamicparticlesindex + 1) % MAX_DYNAMIC_PARTICLES;
		dynamicparticle_t2.c0 = color_t.ColorWhite;
		dynamicparticle_t2.c1 = color_t.ColorWhite;
		dynamicparticle_t2.type = type;
		dynamicparticle_t2.curtime = 0f;
		dynamicparticle_t2.pos = pos;
		dynamicparticle_t2.dpos = dir;
		dynamicparticle_t2.nlayer = curparticlelayer;
		switch (type)
		{
		case 0:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.002f), 0.004f + CRand.prand(0.002f), 0f);
			dynamicparticle_t2.rot = -90f + CRand.frand(20f);
			dynamicparticle_t2.drot = CRand.frand(0.1f);
			dynamicparticle_t2.size = 1f + CRand.prand(0.4f);
			dynamicparticle_t2.dsize = dynamicparticle_t2.size * 0.5f;
			dynamicparticle_t2.c0.a = 0f;
			dynamicparticle_t2.lifetime = 1300f;
			dynamicparticle_t2.grav = -0.0001f;
			dynamicparticle_t2.num = 28;
			dynamicparticle_t2.blendtype = 2;
			break;
		case 1:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.006f), 0.004f + CRand.prand(0.01f), 0f);
			dynamicparticle_t2.rot = 0f;
			dynamicparticle_t2.drot = 0f;
			dynamicparticle_t2.size = 0.5f + CRand.prand(0.4f);
			dynamicparticle_t2.dsize = 0f;
			dynamicparticle_t2.c1.r = 0f;
			dynamicparticle_t2.c1.g = 0f;
			dynamicparticle_t2.c1.b = 0f;
			dynamicparticle_t2.lifetime = 1300f;
			dynamicparticle_t2.grav = -0.0004f;
			dynamicparticle_t2.num = ((type == 1) ? 8 : 10);
			dynamicparticle_t2.blendtype = 0;
			break;
		case 14:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.003f), 0.003f + CRand.prand(0.006f), 0f);
			dynamicparticle_t2.rot = CRand.frand(180f);
			dynamicparticle_t2.drot = CRand.frand(1f);
			dynamicparticle_t2.size = 0.5f + CRand.prand(0.4f);
			dynamicparticle_t2.dsize = dynamicparticle_t2.size * 0.5f;
			dynamicparticle_t2.c1.a = 0.5f;
			dynamicparticle_t2.lifetime = 1000f;
			dynamicparticle_t2.grav = -0.0002f;
			dynamicparticle_t2.num = (type = 10);
			dynamicparticle_t2.blendtype = 2;
			break;
		case 3:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.004f), CRand.frand(0.002f), 0f);
			dynamicparticle_t2.rot = CRand.frand(360f);
			dynamicparticle_t2.drot = CRand.frand(1f);
			dynamicparticle_t2.size = 1f + CRand.prand(0.5f);
			dynamicparticle_t2.dsize = dynamicparticle_t2.size + 1f;
			dynamicparticle_t2.c0.a = 0.5f;
			dynamicparticle_t2.c1.a = 0f;
			dynamicparticle_t2.lifetime = 750f;
			dynamicparticle_t2.grav = 0f;
			dynamicparticle_t2.num = 25;
			dynamicparticle_t2.blendtype = 2;
			break;
		case 4:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.006f), 0.008f + CRand.prand(0.008f), 0f);
			dynamicparticle_t2.rot = CRand.prand(360f);
			dynamicparticle_t2.drot = CRand.frand(2f);
			dynamicparticle_t2.size = 0.8f + CRand.prand(0.1f);
			dynamicparticle_t2.dsize = dynamicparticle_t2.size * 0.4f;
			dynamicparticle_t2.c0.a = 0.5f;
			dynamicparticle_t2.lifetime = 700f;
			dynamicparticle_t2.grav = -0.0005f;
			dynamicparticle_t2.num = 27;
			dynamicparticle_t2.blendtype = 2;
			break;
		case 5:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.006f), 0.008f + CRand.prand(0.008f), 0f);
			dynamicparticle_t2.rot = CRand.prand(360f);
			dynamicparticle_t2.drot = CRand.frand(2f);
			dynamicparticle_t2.size = 0.8f + CRand.prand(0.3f);
			dynamicparticle_t2.dsize = dynamicparticle_t2.size * 0.1f;
			dynamicparticle_t2.c0.a = 0.5f;
			dynamicparticle_t2.lifetime = 700f;
			dynamicparticle_t2.grav = -0.0005f;
			dynamicparticle_t2.num = 26;
			dynamicparticle_t2.blendtype = 2;
			break;
		case 6:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.003f), 0.005f + CRand.prand(0.005f), 0f);
			dynamicparticle_t2.rot = 0f;
			dynamicparticle_t2.drot = 0f;
			dynamicparticle_t2.size = 0.6f + CRand.prand(0.4f);
			dynamicparticle_t2.dsize = 0f;
			dynamicparticle_t2.c1.r = 0f;
			dynamicparticle_t2.c1.g = 0f;
			dynamicparticle_t2.c1.b = 0f;
			dynamicparticle_t2.lifetime = 2000f;
			dynamicparticle_t2.grav = -0.0002f;
			dynamicparticle_t2.num = 8;
			dynamicparticle_t2.blendtype = 0;
			break;
		case 7:
		case 8:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.002f), 0.003f + CRand.prand(0.001f), 0f);
			dynamicparticle_t2.rot = CRand.frand(360f);
			dynamicparticle_t2.drot = CRand.frand(2f);
			dynamicparticle_t2.size = 0.6f + CRand.prand(0.1f);
			dynamicparticle_t2.dsize = 0f;
			dynamicparticle_t2.c1.a = 1f;
			dynamicparticle_t2.lifetime = 1500f;
			dynamicparticle_t2.grav = -0.0001f;
			dynamicparticle_t2.num = ((type == 7) ? 24 : 16);
			dynamicparticle_t2.blendtype = 2;
			break;
		case 9:
			dynamicparticle_t2.pos.x += CRand.frand(1f);
			dynamicparticle_t2.dpos.assign(CRand.frand(0.002f), 0.004f + CRand.prand(0.003f), 0f);
			dynamicparticle_t2.rot = CRand.frand(360f);
			dynamicparticle_t2.drot = CRand.frand(2f);
			dynamicparticle_t2.size = 0.8f + CRand.prand(0.8f);
			dynamicparticle_t2.dsize = dynamicparticle_t2.size;
			dynamicparticle_t2.c0.r = CRand.prand(1f);
			dynamicparticle_t2.c0.g = CRand.prand(1f);
			dynamicparticle_t2.c0.b = CRand.prand(1f);
			dynamicparticle_t2.c1.r = CRand.prand(1f);
			dynamicparticle_t2.c1.g = CRand.prand(1f);
			dynamicparticle_t2.c1.b = CRand.prand(1f);
			dynamicparticle_t2.c1.a = 0f;
			dynamicparticle_t2.lifetime = 1500f;
			dynamicparticle_t2.grav = -0.00015f;
			dynamicparticle_t2.num = 12;
			dynamicparticle_t2.blendtype = 0;
			break;
		case 10:
			dynamicparticle_t2.pos.x += CRand.frand(0.5f);
			dynamicparticle_t2.pos.y += 1.3f + CRand.prand(3f);
			dynamicparticle_t2.pos.z = 5f;
			dynamicparticle_t2.dpos.assign(CRand.frand(0.003f), 0f, CRand.frand(0.002f));
			dynamicparticle_t2.rot = -90f + CRand.frand(10f);
			dynamicparticle_t2.drot = CRand.frand(1f);
			dynamicparticle_t2.size = 0.5f + CRand.prand(0.1f);
			dynamicparticle_t2.dsize = 0f;
			dynamicparticle_t2.c0.r = 0.1f + CRand.prand(0.1f);
			dynamicparticle_t2.c0.g = 1f;
			dynamicparticle_t2.c0.b = 0.1f + CRand.prand(0.1f);
			dynamicparticle_t2.c1.r = dynamicparticle_t2.c0.r;
			dynamicparticle_t2.c1.g = 1f;
			dynamicparticle_t2.c1.b = dynamicparticle_t2.c0.b;
			dynamicparticle_t2.c1.a = 0f;
			dynamicparticle_t2.lifetime = 1000f;
			dynamicparticle_t2.grav = 0.0002f;
			dynamicparticle_t2.num = 7;
			dynamicparticle_t2.blendtype = 2;
			break;
		case 11:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.001f), -0.011f - CRand.prand(0.001f), 0f);
			dynamicparticle_t2.rot = CRand.frand(180f);
			dynamicparticle_t2.pos.z = 45f;
			dynamicparticle_t2.drot = CRand.frand(1f);
			dynamicparticle_t2.size = 0.8f + CRand.prand(0.1f);
			dynamicparticle_t2.dsize = dynamicparticle_t2.size / 2f;
			dynamicparticle_t2.c0.assign(1f, 0.5f, 0.5f, 1f);
			dynamicparticle_t2.c1.assign(0f, 0f, 0f, 0f);
			dynamicparticle_t2.lifetime = 150f;
			dynamicparticle_t2.grav = 0f;
			dynamicparticle_t2.num = 9;
			dynamicparticle_t2.blendtype = 0;
			break;
		case 12:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.002f), 0.002f + CRand.prand(0.002f), 0f);
			dynamicparticle_t2.rot = CRand.frand(360f);
			dynamicparticle_t2.drot = CRand.frand(0.5f);
			dynamicparticle_t2.size = 0.8f + CRand.prand(0.6f);
			dynamicparticle_t2.dsize = dynamicparticle_t2.size * 0.2f;
			dynamicparticle_t2.c0.r = CRand.prand(1f);
			dynamicparticle_t2.c0.g = CRand.prand(1f);
			dynamicparticle_t2.c0.b = CRand.prand(1f);
			dynamicparticle_t2.c1.r = dynamicparticle_t2.c0.r;
			dynamicparticle_t2.c1.g = dynamicparticle_t2.c0.g;
			dynamicparticle_t2.c1.b = dynamicparticle_t2.c0.b;
			dynamicparticle_t2.c1.a = 1f;
			dynamicparticle_t2.lifetime = 1000f;
			dynamicparticle_t2.grav = -0.0003f;
			dynamicparticle_t2.num = 13;
			dynamicparticle_t2.blendtype = 0;
			break;
		case 13:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.002f), 0.002f + CRand.prand(0.002f), 0f);
			dynamicparticle_t2.rot = CRand.frand(360f);
			dynamicparticle_t2.drot = CRand.frand(0.5f);
			dynamicparticle_t2.size = 0.3f + CRand.prand(0.2f);
			dynamicparticle_t2.dsize = dynamicparticle_t2.size * 0.2f;
			dynamicparticle_t2.c1.a = 1f;
			dynamicparticle_t2.lifetime = 1000f;
			dynamicparticle_t2.grav = -0.0003f;
			dynamicparticle_t2.num = 4;
			dynamicparticle_t2.blendtype = 0;
			break;
		case 15:
			dynamicparticle_t2.pos += new vec3_t(CRand.frand(1.5f), CRand.frand(0.5f), 0f);
			dynamicparticle_t2.dpos.assign(CRand.frand(0.005f), 0.005f + CRand.prand(0.005f), 0f);
			dynamicparticle_t2.rot = 0f;
			dynamicparticle_t2.drot = 0f;
			dynamicparticle_t2.size = 0.8f + CRand.prand(0.5f);
			dynamicparticle_t2.dsize = 0f;
			dynamicparticle_t2.c1.a = 0f;
			dynamicparticle_t2.lifetime = 2000f;
			dynamicparticle_t2.grav = -0.0003f;
			dynamicparticle_t2.num = 26;
			dynamicparticle_t2.blendtype = 2;
			break;
		case 16:
			dynamicparticle_t2.pos += new vec3_t(CRand.frand(1.5f), CRand.frand(0.5f), 0f);
			dynamicparticle_t2.dpos.assign(CRand.frand(0.005f), 0.005f + CRand.prand(0.005f), 0f);
			dynamicparticle_t2.rot = CRand.frand(180f);
			dynamicparticle_t2.drot = CRand.frand(1f);
			dynamicparticle_t2.size = 1.2f;
			dynamicparticle_t2.dsize = 2f;
			dynamicparticle_t2.c0.a = 1f;
			dynamicparticle_t2.c1.a = 1f;
			dynamicparticle_t2.lifetime = 1500f;
			dynamicparticle_t2.grav = -0.0003f;
			dynamicparticle_t2.num = 1;
			dynamicparticle_t2.blendtype = 2;
			break;
		case 17:
			dynamicparticle_t2.pos += new vec3_t(CRand.frand(0.5f), CRand.frand(0.5f), -4f);
			dynamicparticle_t2.dpos.assign(CRand.frand(0.001f), 0.002f + CRand.frand(0.002f), 0f);
			dynamicparticle_t2.rot = CRand.frand(180f);
			dynamicparticle_t2.drot = CRand.frand(0.1f);
			dynamicparticle_t2.size = 2.5f + CRand.prand(0.5f);
			dynamicparticle_t2.dsize = dynamicparticle_t2.size * 0.5f;
			dynamicparticle_t2.c0.a = 1f;
			dynamicparticle_t2.c1.a = 0f;
			dynamicparticle_t2.lifetime = 1500f;
			dynamicparticle_t2.grav = -5E-05f;
			dynamicparticle_t2.num = 22;
			dynamicparticle_t2.blendtype = 2;
			break;
		case 18:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.005f), 0.005f + CRand.prand(0.004f), 0f);
			dynamicparticle_t2.rot = CRand.frand(360f);
			dynamicparticle_t2.drot = ((CRand.irand(2) != 0) ? 1f : (-1f)) * (0.2f + CRand.prand(1f));
			dynamicparticle_t2.size = 2f;
			dynamicparticle_t2.dsize = 0.3f;
			dynamicparticle_t2.c1.a = 0f;
			dynamicparticle_t2.lifetime = 2000f;
			dynamicparticle_t2.grav = -0.0003f;
			dynamicparticle_t2.num = 1;
			dynamicparticle_t2.blendtype = 2;
			break;
		case 19:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.003f), 0.005f + CRand.prand(0.004f), 0f);
			dynamicparticle_t2.rot = CRand.frand(360f);
			dynamicparticle_t2.drot = ((CRand.irand(2) != 0) ? 1f : (-1f)) * (0.2f + CRand.prand(0.2f));
			dynamicparticle_t2.size = 2f;
			dynamicparticle_t2.dsize = 2f;
			dynamicparticle_t2.c1.r = 0.2f;
			dynamicparticle_t2.c1.b = 0.2f;
			dynamicparticle_t2.c1.g = 0.2f;
			dynamicparticle_t2.lifetime = 1500f;
			dynamicparticle_t2.grav = -0.0003f;
			dynamicparticle_t2.num = 18;
			dynamicparticle_t2.blendtype = 0;
			break;
		case 20:
			dynamicparticle_t2.dpos.assign(0f, 0f, 0f);
			dynamicparticle_t2.rot = CRand.frand(360f);
			dynamicparticle_t2.drot = 0f;
			dynamicparticle_t2.size = 0.9f + CRand.prand(0.2f);
			dynamicparticle_t2.dsize = dynamicparticle_t2.size;
			dynamicparticle_t2.c0.assign(1f, 1f, 1f, 1f);
			dynamicparticle_t2.c1.assign(1f, 1f, 1f, 1f);
			dynamicparticle_t2.lifetime = 700f;
			dynamicparticle_t2.grav = 0f;
			dynamicparticle_t2.num = 13;
			dynamicparticle_t2.blendtype = 0;
			break;
		case 21:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.003f), CRand.prand(0.003f), 0f);
			dynamicparticle_t2.rot = CRand.frand(360f);
			dynamicparticle_t2.drot = CRand.frand(1f);
			dynamicparticle_t2.size = 0.4f + CRand.prand(0.2f);
			dynamicparticle_t2.dsize = 0f;
			dynamicparticle_t2.c0.assign(1f, 1f, 1f, 1f);
			dynamicparticle_t2.c1.assign(1f, 1f, 1f, 1f);
			dynamicparticle_t2.lifetime = 500f;
			dynamicparticle_t2.grav = -0.0005f;
			dynamicparticle_t2.num = 21;
			dynamicparticle_t2.blendtype = 2;
			break;
		case 22:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.002f), 0.0001f + CRand.prand(0.002f), 0f);
			dynamicparticle_t2.rot = CRand.frand(180f);
			dynamicparticle_t2.pos.z = -15f;
			dynamicparticle_t2.pos.y += CRand.frand(0.1f);
			dynamicparticle_t2.drot = CRand.frand(1f);
			dynamicparticle_t2.size = 1.8f + CRand.prand(0.1f);
			dynamicparticle_t2.dsize = 0f;
			dynamicparticle_t2.c0.assign(1f, 1f, 1f, 1f);
			dynamicparticle_t2.c1.assign(1f, 0f, 1f, 0f);
			dynamicparticle_t2.lifetime = 600f + CRand.prand(300f);
			dynamicparticle_t2.grav = 0.0001f;
			dynamicparticle_t2.num = 20;
			dynamicparticle_t2.blendtype = 0;
			break;
		case 23:
			dynamicparticle_t2.dpos.assign(0f, 0f, 0f);
			dynamicparticle_t2.rot = -90f;
			dynamicparticle_t2.pos.z = -15f;
			dynamicparticle_t2.drot = 0f;
			dynamicparticle_t2.size = 1f;
			dynamicparticle_t2.dsize = 1f;
			dynamicparticle_t2.c0.assign(1f, 1f, 1f, 1f);
			dynamicparticle_t2.c1.assign(0f, 0f, 0.1f, 0f);
			dynamicparticle_t2.lifetime = 1000f;
			dynamicparticle_t2.grav = 0f;
			dynamicparticle_t2.num = 14;
			dynamicparticle_t2.blendtype = 0;
			break;
		case 24:
			dynamicparticle_t2.dpos.assign(0f, 0.003f + CRand.prand(0.002f), 0f);
			dynamicparticle_t2.rot = CRand.frand(180f);
			dynamicparticle_t2.pos.z = -15f;
			dynamicparticle_t2.drot = CRand.frand(1f);
			dynamicparticle_t2.size = 0.1f;
			dynamicparticle_t2.dsize = 1.9f + CRand.prand(0.2f);
			dynamicparticle_t2.c0.assign(0.5f, 0.2f, 0f, 0.6f);
			dynamicparticle_t2.c1.assign(0f, 0f, 0f, 0f);
			dynamicparticle_t2.lifetime = 600f + CRand.prand(200f);
			dynamicparticle_t2.grav = 0f;
			dynamicparticle_t2.num = 25;
			dynamicparticle_t2.blendtype = 2;
			break;
		case 25:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.001f), 0.002f + CRand.prand(0.001f), 0f);
			dynamicparticle_t2.rot = CRand.frand(180f);
			dynamicparticle_t2.pos.z = -15f;
			dynamicparticle_t2.drot = CRand.frand(1f);
			dynamicparticle_t2.size = 0.3f + CRand.prand(0.6f);
			dynamicparticle_t2.dsize = dynamicparticle_t2.size;
			dynamicparticle_t2.c0.assign(0.8f, 1f, 0.8f, 1f);
			dynamicparticle_t2.c1.assign(0.8f, 1f, 0.8f, 1f);
			dynamicparticle_t2.lifetime = 800f + CRand.prand(200f);
			dynamicparticle_t2.grav = 0f;
			dynamicparticle_t2.num = 8;
			dynamicparticle_t2.blendtype = 0;
			break;
		case 26:
			dynamicparticle_t2.dpos.assign(0f, 0f, 0f);
			dynamicparticle_t2.size = 0.1f;
			dynamicparticle_t2.dsize = 6f;
			dynamicparticle_t2.c0.assign(1f, 1f, 1f, 1f);
			dynamicparticle_t2.c1.assign(0.5f, 0.5f, 0.5f, 1f);
			dynamicparticle_t2.lifetime = 500f;
			dynamicparticle_t2.num = 34;
			dynamicparticle_t2.grav = 0f;
			dynamicparticle_t2.blendtype = 0;
			break;
		case 27:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.002f), CRand.prand(0.001f), 0f);
			dynamicparticle_t2.rot = 0f;
			dynamicparticle_t2.pos.z = -15f;
			dynamicparticle_t2.drot = ((CRand.irand(2) != 0) ? 0.1f : (-0.1f));
			dynamicparticle_t2.size = 2f;
			dynamicparticle_t2.dsize = 3f;
			dynamicparticle_t2.c0.assign(1f, 1f, 1f, 1f);
			dynamicparticle_t2.c1.assign(0.6f, 0.6f, 0.6f, 1f);
			dynamicparticle_t2.lifetime = 2000f;
			dynamicparticle_t2.grav = -0f;
			dynamicparticle_t2.num = 37;
			dynamicparticle_t2.blendtype = 0;
			break;
		case 28:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.01f), 0.03f + CRand.prand(0.01f), 0f);
			dynamicparticle_t2.rot = 0f;
			dynamicparticle_t2.drot = ((CRand.irand(2) != 0) ? 0.1f : (-0.1f));
			dynamicparticle_t2.size = 50f;
			dynamicparticle_t2.dsize = 50f;
			dynamicparticle_t2.c0.assign(0.7f + CRand.prand(0.3f), 0.7f + CRand.prand(0.3f), 0.7f + CRand.prand(0.3f), 1f);
			dynamicparticle_t2.c1.assign(0f, 0f, 0f, 1f);
			dynamicparticle_t2.lifetime = 2000f;
			dynamicparticle_t2.grav = -0.001f;
			dynamicparticle_t2.num = 12;
			dynamicparticle_t2.blendtype = 0;
			break;
		case 29:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.01f), 0.03f + CRand.prand(0.01f), 0f);
			dynamicparticle_t2.rot = 0f;
			dynamicparticle_t2.drot = ((CRand.irand(2) != 0) ? 0.1f : (-0.1f));
			dynamicparticle_t2.size = 30f;
			dynamicparticle_t2.dsize = 30f;
			dynamicparticle_t2.c0.assign(0.7f + CRand.prand(0.3f), 0.7f + CRand.prand(0.3f), 0.7f + CRand.prand(0.3f), 1f);
			dynamicparticle_t2.c1.assign(0f, 0f, 0f, 1f);
			dynamicparticle_t2.lifetime = 2000f;
			dynamicparticle_t2.grav = 0.001f;
			dynamicparticle_t2.num = 12;
			dynamicparticle_t2.blendtype = 0;
			break;
		case 30:
			dynamicparticle_t2.dpos.assign(CRand.prand(0.003f), 0f, 0f);
			dynamicparticle_t2.rot = -90f + CRand.frand(10f);
			dynamicparticle_t2.drot = CRand.frand(1f);
			dynamicparticle_t2.size = 14f + CRand.prand(5f);
			dynamicparticle_t2.dsize = 0f;
			dynamicparticle_t2.c0.assign(1f, 1f, 1f, 1f);
			dynamicparticle_t2.c1.assign(0f, 0f, 0f, 1f);
			dynamicparticle_t2.lifetime = 1000f;
			dynamicparticle_t2.grav = 0f;
			dynamicparticle_t2.num = 23;
			dynamicparticle_t2.blendtype = 0;
			break;
		case 31:
			dynamicparticle_t2.dpos.assign(0f, 0f, 0f);
			dynamicparticle_t2.rot = 90f;
			dynamicparticle_t2.drot = 0f;
			dynamicparticle_t2.size = 8f;
			dynamicparticle_t2.dsize = 1f;
			dynamicparticle_t2.c0.assign(1f, 1f, 1f, 1f);
			dynamicparticle_t2.c1.assign(0.5f, 0f, 0f, 1f);
			dynamicparticle_t2.lifetime = 1200f;
			dynamicparticle_t2.grav = 0f;
			dynamicparticle_t2.num = 13;
			dynamicparticle_t2.blendtype = 0;
			break;
		case 32:
			dynamicparticle_t2.dpos.assign(CRand.frand(0.05f), 0.1f + CRand.prand(0.1f), 0f);
			dynamicparticle_t2.rot = 90f;
			dynamicparticle_t2.drot = 0f;
			dynamicparticle_t2.size = 32f;
			dynamicparticle_t2.dsize = 2f;
			dynamicparticle_t2.c0.assign(1f, CRand.prand(1f), CRand.prand(1f), 1f);
			dynamicparticle_t2.c1 = dynamicparticle_t2.c0;
			dynamicparticle_t2.lifetime = 1200f;
			dynamicparticle_t2.grav = -0.01f;
			dynamicparticle_t2.num = 12;
			dynamicparticle_t2.blendtype = 0;
			break;
		case 33:
			dynamicparticle_t2.dpos.assign(0f, 0f, 0f);
			dynamicparticle_t2.rot = 90f;
			dynamicparticle_t2.drot = 0f;
			dynamicparticle_t2.size = 0f;
			dynamicparticle_t2.dsize = 300f;
			dynamicparticle_t2.c0.assign(1f, 1f, 1f, 1f);
			dynamicparticle_t2.c1.assign(0f, 0f, 0f, 1f);
			dynamicparticle_t2.lifetime = 1100f;
			dynamicparticle_t2.grav = 0f;
			dynamicparticle_t2.num = 8;
			dynamicparticle_t2.blendtype = 0;
			break;
		}
		dynamicparticle_t2.dpos0 = dynamicparticle_t2.dpos;
		dynamicparticle_t2.drot0 = dynamicparticle_t2.drot;
		if (texnum != -1)
		{
			dynamicparticle_t2.num = texnum;
		}
		int num = ((dynamicparticlesindex > 0) ? (dynamicparticlesindex - 1) : 0);
		ar_dynamicparticles[num] = dynamicparticle_t2;
	}

	public static void AddDynamicParticle(vec3_t pos, int type, int amount, int texnum)
	{
		for (int i = 0; i < amount; i++)
		{
			AddNewDynamicParticle(pos, type, texnum, emptyvector);
		}
	}

	public static void AddDynamicParticle(vec2_t v, int type, int amount, int texnum)
	{
		vec.assign(v.x, v.y, 0f);
		for (int i = 0; i < amount; i++)
		{
			AddNewDynamicParticle(vec, type, texnum, emptyvector);
		}
	}
}
