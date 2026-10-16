using System;

namespace SuperCow;

public class CCloudObj : CLevelObj
{
	public vec3_t light1;

	public vec3_t light2;

	public float flighttime;

	public int particlenumber;

	public override void Init()
	{
		flighttime = 0f;
		base.Init();
		int1 = CRand.irand(4);
		flighttime = CRand.prand(800f);
		float num = 10000f;
		CLevelObj cLevelObj = null;
		for (int i = 0; i < SCGlobals.level.numlevelobjs; i++)
		{
			CLevelObj cLevelObj2 = SCGlobals.level.ar_levelobjs[i];
			if (cLevelObj2 != this && (cLevelObj2.objID == 0 || cLevelObj2.objID == 2) && !(position.x < cLevelObj2.rect.x0) && !(position.x > cLevelObj2.rect.x1) && !(position.y < cLevelObj2.rect.y0))
			{
				float num2 = Math.Abs(position.y - cLevelObj2.rect.y0);
				if (num > num2)
				{
					cLevelObj = cLevelObj2;
					num = num2;
				}
			}
		}
		if (cLevelObj != null)
		{
			AddLinkedObjs(cLevelObj);
		}
		if (objsubtype == 0)
		{
			particlenumber = 22;
		}
		else
		{
			particlenumber = 43;
		}
	}

	public override void Draw(dcolor_t dc)
	{
		frect_t frect_t2 = rect;
		if (numlinks > 0)
		{
			frect_t2.y1 = ar_linkedobjs[0].rect.y1;
		}
		if (numlinks > 0 && frect_t2.isRectTouchRect(SCGlobals.viewport_rect) && flighttime > 800f)
		{
			float num = (flighttime - 800f) / 500f;
			float num2 = (float)Math.Sin(num * (float)Math.PI);
			CoolDraw.AddLightning(light1, light2, 1.5f, 0.1f, 1.8f, 1f, new dcolor_t(byte.MaxValue, 155, 155, (byte)(num2 * 155f)), new dcolor_t(0, byte.MaxValue, byte.MaxValue, (byte)(num2 * 55f)));
			Particles.AddParticle(light2, 0f, 3.5f, new dcolor_t(byte.MaxValue, byte.MaxValue, 0, (byte)(num2 * 75f)), 4, 15);
		}
		frect_t2 = rect;
		frect_t2.x0 -= 3f;
		frect_t2.x1 += 3f;
		frect_t2.y0 += 3f;
		frect_t2.y1 -= 3f;
		b_offscreen = !frect_t2.isRectTouchRect(SCGlobals.viewport_rect);
		if (!b_offscreen)
		{
			obj_time += SCGlobals.ftickperframe;
			CowRender.BindTexture(p_texture);
			int height = (int)p_texture.height;
			int num3 = (int)Math.Round(rect.sizex() * 32f / (float)height);
			float num4 = rect.sizey();
			vec2_t p = rect.p0;
			p.y -= num4 * 0.5f;
			CRand.randomize2((int)(Math.Abs(frandom1) * 256f));
			for (int i = 0; i <= num3; i++)
			{
				float rot = CRand.frand2(360f) + obj_time * ((CRand.irand2(2) == 1) ? (-1f) : 1f) * (0.05f + CRand.prand2(0.05f)) * 0.1f;
				vec2_t p2 = p;
				Particles.AddParticle(p2, rot, num4 * 1.5f + CRand.prand2(0.5f), dc, 2, particlenumber);
				p.x += num4;
			}
			p = rect.p0;
			p.y -= num4 * 0.5f;
			p.x += num4 * 0.5f;
			float num5 = ((CRand.irand2(2) != 0) ? 1f : (-1f));
			for (int j = 1; j <= num3; j++)
			{
				float rot2 = CRand.frand2(360f) + obj_time * ((CRand.irand2(2) == 1) ? (-1f) : 1f) * (0.05f + CRand.prand2(0.05f)) * 0.1f;
				vec2_t p2 = p;
				p2.y += num5 * ((j % 2 != 0) ? 1f : (-1f)) * (0.9f + CRand.prand2(0.3f));
				Particles.AddParticle(p2, rot2, num4 * 0.9f + CRand.prand2(0.5f), dc, 2, particlenumber);
				p.x += num4;
			}
		}
	}

	public override void Physics()
	{
		if (objsubtype == 0)
		{
			flighttime += SCGlobals.engine_frequency;
			if (flighttime < 800f && flighttime + (float)SCGlobals.engine_frequency >= 800f)
			{
				if (numlinks > 0)
				{
					light1 = position.v3t();
					light2 = ar_linkedobjs[0].rect.pos().v3t();
					float num = ((!(rect.x0 < ar_linkedobjs[0].rect.x0)) ? rect.x0 : ar_linkedobjs[0].rect.x0);
					float num2 = ((!(rect.x1 > ar_linkedobjs[0].rect.x1)) ? rect.x1 : ar_linkedobjs[0].rect.x1);
					float num3 = (float)int1 / 3f;
					light2.x = num * (1f - num3) + num2 * num3;
					light1.x = light2.x + ((CRand.phys_rand(2) != 0) ? 1f : (-1f)) * 1.5f * (float)CRand.phys_rand(100) / 100f;
					if (light1.x < num)
					{
						light1.x = num;
					}
					if (light1.x > num2)
					{
						light1.x = num2;
					}
					int1 = (int1 + 1) % 4;
					if (!b_offscreen)
					{
						Sound.Addsound("lightning");
					}
				}
				else
				{
					flighttime = 0f;
				}
			}
			if (flighttime > 1300f)
			{
				flighttime = 0f;
			}
		}
		if (flighttime > 800f && numlinks > 0)
		{
			frect_t rc = default(frect_t);
			vec2_t pos = new vec2_t(light2);
			pos.y += SCGlobals.hero.monster_height / 2f;
			rc.assign(pos, SCGlobals.hero.monster_width * 0.6f, SCGlobals.hero.monster_height);
			if (SCGlobals.hero.bound_rect.isRectTouchRect(rc) && SCGlobals.hero.punishtime <= 0f)
			{
				SCGlobals.hero.b_setsparks = true;
				SCGlobals.hero.Punish(30f);
			}
		}
		if (rect.isRectTouchRect(SCGlobals.hero.bound_rect) && SCGlobals.hero.behavior != behaviors_t.BT_JUMP)
		{
			SCGlobals.hero.forcedmove += new vec3_t(0f, -0.02f, 0f);
		}
		if (rect.isPointInRect(new vec2_t(SCGlobals.hero.position)))
		{
			if (SCGlobals.hero.behavior != behaviors_t.BT_STAND && iauxvar == 0)
			{
				Particles.AddDynamicParticle(SCGlobals.hero.position, 17, 1, particlenumber);
			}
			iauxvar = (iauxvar + 1) % 5;
		}
	}

	public override void Touch()
	{
		if (SCGlobals.hero.b_landing)
		{
			Particles.AddDynamicParticle(SCGlobals.hero.position, 17, 3, particlenumber);
		}
	}
}
