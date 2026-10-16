using System;

namespace SuperCow;

public static class VisualEffects
{
	public struct ve_fireworkparticle_t
	{
		public float timer;

		public float timer2;

		public vec2_t pos;

		public vec2_t dpos;

		public float grav;

		public float ftickmn;

		public int type;

		public dcolor_t color;
	}

	public static int VE_NUMFIREFORKPARTICLES = 4096;

	public static int ve_numfireworkparticle = 0;

	public static ve_fireworkparticle_t[] ve_fireworkparticles = new ve_fireworkparticle_t[4096];

	public static texture_t p_ve_firework1;

	public static void VisualEffects_Load()
	{
		p_ve_firework1 = SCGlobals.texturehost.Load("firework1.jpg");
	}

	public static void VisualEffects_Init()
	{
		for (int i = 0; i < VE_NUMFIREFORKPARTICLES; i++)
		{
			ve_fireworkparticles[i].timer = -1f;
		}
	}

	public static void VisualEffects_Draw()
	{
		_ = SuperCow.DeviceManager.GraphicsDevice;
		CowRender.DisableRS(5u);
		VisualEffects_DrawFireWorks();
	}

	public static void VisualEffects_AddFireWork(vec2_t pos, int type)
	{
		dcolor_t c = default(dcolor_t);
		switch (type)
		{
		case 0:
		{
			int num = CRand.irand(8);
			int num2 = CRand.irand(3);
			switch (num)
			{
			case 0:
			{
				for (int i = 0; i < 64; i++)
				{
					c.assign((byte)(100f + CRand.prand(155f)), (byte)(100f + CRand.prand(155f)), (byte)(100f + CRand.prand(155f)), byte.MaxValue);
					VisualEffects_AddFireWorkParticle(pos, num2, 0, c);
				}
				return;
			}
			case 1:
				c.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				break;
			case 2:
				c.assign(byte.MaxValue, byte.MaxValue, 100, byte.MaxValue);
				break;
			case 3:
				c.assign(byte.MaxValue, 100, byte.MaxValue, byte.MaxValue);
				break;
			case 4:
				c.assign(100, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				break;
			case 5:
				c.assign(byte.MaxValue, 200, 200, byte.MaxValue);
				break;
			case 6:
				c.assign(200, byte.MaxValue, 200, byte.MaxValue);
				break;
			case 7:
				c.assign(200, 200, byte.MaxValue, byte.MaxValue);
				break;
			}
			for (int i = 0; i < 64; i++)
			{
				VisualEffects_AddFireWorkParticle(pos, num2, 0, c);
			}
			break;
		}
		case 1:
		{
			for (int i = 0; i < 16; i++)
			{
				c.assign((byte)(100f + CRand.prand(155f)), (byte)(100f + CRand.prand(155f)), (byte)(100f + CRand.prand(155f)), byte.MaxValue);
				VisualEffects_AddFireWorkParticle(pos, 3, 0, c);
			}
			break;
		}
		case 2:
		{
			for (int i = 0; i < 8; i++)
			{
				c.assign((byte)(100f + CRand.prand(155f)), (byte)(100f + CRand.prand(155f)), (byte)(100f + CRand.prand(155f)), byte.MaxValue);
				VisualEffects_AddFireWorkParticle(pos + new vec2_t(CRand.frand(16f), CRand.frand(16f)), 3, 0, c);
			}
			break;
		}
		}
	}

	public static void VisualEffects_AddSmallFireWork(vec2_t pos, int type)
	{
		if (type != 0)
		{
			return;
		}
		int num = CRand.irand(8);
		int num2 = CRand.irand(3);
		dcolor_t c = default(dcolor_t);
		switch (num)
		{
		case 0:
		{
			for (int i = 0; i < 64; i++)
			{
				c.assign((byte)(100f + CRand.prand(155f)), (byte)(100f + CRand.prand(155f)), (byte)(100f + CRand.prand(155f)), byte.MaxValue);
				VisualEffects_AddFireWorkParticle(pos, num2, 0, c);
			}
			return;
		}
		case 1:
			c.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			break;
		case 2:
			c.assign(byte.MaxValue, byte.MaxValue, 100, byte.MaxValue);
			break;
		case 3:
			c.assign(byte.MaxValue, 100, byte.MaxValue, byte.MaxValue);
			break;
		case 4:
			c.assign(100, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			break;
		case 5:
			c.assign(byte.MaxValue, 200, 200, byte.MaxValue);
			break;
		case 6:
			c.assign(200, byte.MaxValue, 200, byte.MaxValue);
			break;
		case 7:
			c.assign(200, 200, byte.MaxValue, byte.MaxValue);
			break;
		}
		for (int i = 0; i < 64; i++)
		{
			VisualEffects_AddFireWorkParticle(pos, num2, 0, c);
		}
	}

	public static void VisualEffects_DrawFireWorks()
	{
		CowRender.SetBlend(1);
		CowRender.BindTexture(p_ve_firework1);
		for (int i = 0; i < VE_NUMFIREFORKPARTICLES; i++)
		{
			VisualEffects_DrawFireWorkParticle(ve_fireworkparticles[i]);
		}
		CowRender.SetBlend(0);
	}

	public static void VisualEffects_DrawFireWorkParticle(ve_fireworkparticle_t p_fwp)
	{
		if (p_fwp.timer <= 0f)
		{
			return;
		}
		float num = 1f;
		float num2 = SCGlobals.ftickperframe * p_fwp.ftickmn;
		if (p_fwp.type == 0)
		{
			p_fwp.pos += p_fwp.dpos * num2;
			p_fwp.dpos.y -= p_fwp.grav * num2;
			p_fwp.timer -= num2;
			p_fwp.timer2 -= num2;
			if (p_fwp.timer2 <= 0f)
			{
				p_fwp.timer2 = 75f + CRand.prand(50f);
				if (p_fwp.type == 0)
				{
					VisualEffects_AddFireWorkParticle(p_fwp.pos, 0, 1, p_fwp.color);
				}
			}
			if (p_fwp.timer < 400f)
			{
				num = 1f + (float)Math.Sin(p_fwp.timer / 50f * (float)Math.PI);
			}
			if (p_fwp.timer < 200f)
			{
				num *= p_fwp.timer / 200f;
			}
			CowRender.DrawColorQuad2D(p_fwp.pos, 32f * num, 32f * num, new frect_t(0f, 0f, 1f, 1f), p_fwp.color, 0f);
		}
		else if (p_fwp.type == 1)
		{
			p_fwp.pos.y -= p_fwp.grav * num2;
			p_fwp.grav += 0.0001f * num2;
			p_fwp.timer -= num2;
			p_fwp.timer2 -= num2;
			num *= p_fwp.timer / 300f;
			CowRender.DrawColorQuad2D(p_fwp.pos, 20f * num, 20f * num, new frect_t(0f, 0f, 1f, 1f), p_fwp.color, 0f);
		}
		else if (p_fwp.type == 2)
		{
			p_fwp.pos.y -= p_fwp.grav * num2;
			p_fwp.grav -= 0.001f * num2;
			p_fwp.timer -= num2;
			p_fwp.timer2 -= num2;
			num *= p_fwp.timer / 300f;
			CowRender.DrawColorQuad2D(p_fwp.pos, 20f * num, 20f * num, new frect_t(0f, 0f, 1f, 1f), p_fwp.color, 0f);
		}
		else if (p_fwp.type == 3)
		{
			p_fwp.pos.y -= p_fwp.grav * num2;
			p_fwp.grav += 0.001f * num2;
			p_fwp.timer -= num2;
			p_fwp.timer2 -= num2;
			num *= p_fwp.timer / 300f;
			CowRender.DrawColorQuad2D(p_fwp.pos, 20f * num, 20f * num, new frect_t(0f, 0f, 1f, 1f), p_fwp.color, 0f);
		}
	}

	public static void VisualEffects_AddFireWorkParticle(vec2_t pos, int num, int type, dcolor_t c)
	{
		ve_fireworkparticle_t ve_fireworkparticle_t = ve_fireworkparticles[ve_numfireworkparticle];
		ve_fireworkparticle_t.pos = pos;
		ve_fireworkparticle_t.dpos.assign(1f, 0f);
		ve_fireworkparticle_t.dpos.rotate(CRand.frand(360f));
		if (num == 0)
		{
			ve_fireworkparticle_t.dpos *= 0.2f * CRand.prand(1f);
		}
		if (num == 1)
		{
			ve_fireworkparticle_t.dpos *= 0.16f * (0.7f + CRand.prand(0.3f));
		}
		if (num == 2)
		{
			ve_fireworkparticle_t.dpos *= 0.1f + 0.1f * (float)CRand.irand(2);
		}
		if (num == 3)
		{
			ve_fireworkparticle_t.dpos *= 0.1f * CRand.prand(1f);
		}
		ve_fireworkparticle_t.grav = 0.0002f;
		if (num == 2)
		{
			ve_fireworkparticle_t.grav = 0.0001f;
		}
		if (num == 1)
		{
			ve_fireworkparticle_t.grav = 5E-05f + CRand.prand(5E-05f);
		}
		if (num == 3)
		{
			ve_fireworkparticle_t.grav = 7E-05f + CRand.prand(5E-05f);
		}
		ve_fireworkparticle_t.color = c;
		if (type == 0)
		{
			ve_fireworkparticle_t.timer = 1200f;
		}
		if (num == 3)
		{
			ve_fireworkparticle_t.timer = 800f;
		}
		if (type == 1)
		{
			ve_fireworkparticle_t.timer = 500f;
		}
		if (type == 2)
		{
			ve_fireworkparticle_t.timer = 500f;
			ve_fireworkparticle_t.grav = -0.01f;
		}
		if (type == 3)
		{
			ve_fireworkparticle_t.timer = 500f;
			ve_fireworkparticle_t.grav = 0.01f;
		}
		ve_fireworkparticle_t.timer2 = 100f;
		ve_fireworkparticle_t.ftickmn = 1f + CRand.frand(0.3f);
		ve_fireworkparticle_t.type = type;
		ve_fireworkparticles[ve_numfireworkparticle] = ve_fireworkparticle_t;
		ve_numfireworkparticle = (ve_numfireworkparticle + 1) % VE_NUMFIREFORKPARTICLES;
	}
}
