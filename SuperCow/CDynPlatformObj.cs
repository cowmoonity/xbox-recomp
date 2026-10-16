using System.Globalization;

namespace SuperCow;

public class CDynPlatformObj : CLevelObj
{
	public CMonster[] ar_monsterlinks = new CMonster[5];

	public int nummonsterlinks;

	public bool b_stand;

	public bool b_soundon;

	public float platformspeed;

	public int process;

	public int snd_id;

	public override void Init()
	{
		nummonsterlinks = 0;
		base.Init();
		int1 = 0;
		b_stand = false;
		b_soundon = true;
		if (extratextparam.Length > 0)
		{
			string text = "";
			for (int i = 0; i < extratextparam.Length; i++)
			{
				if (extratextparam[i] >= '0' && extratextparam[i] <= '9')
				{
					text += extratextparam[i];
				}
			}
			if (text.Length > 0)
			{
				float1 = float.Parse(text, CultureInfo.InvariantCulture);
			}
			b_stand = (extratextparam.Contains("s") ? true : false);
		}
		else
		{
			float1 = 1f;
		}
		if (extratextparam == "0.2m")
		{
			float1 = 0.2f;
		}
		if (extratextparam == "0.5")
		{
			float1 = 0.5f;
		}
		if (extratextparam == "0.3m")
		{
			float1 = 0.3f;
		}
		if (extratextparam == "0.1m")
		{
			float1 = 0.1f;
		}
		if (extratextparam == "1.5")
		{
			float1 = 1.5f;
		}
		if (extratextparam == "2s")
		{
			float1 = 2f;
		}
		if (extratextparam == "3s")
		{
			float1 = 3f;
		}
		if (extratextparam == "4s")
		{
			float1 = 4f;
		}
		if (extratextparam == "6s")
		{
			float1 = 6f;
		}
		if (extratextparam == "2.5s")
		{
			float1 = 2.5f;
		}
		if (extratextparam == "2.5")
		{
			float1 = 2.5f;
		}
		if (b_stand)
		{
			platformspeed = 0f;
		}
		else
		{
			platformspeed = float1;
		}
		snd_id = 0;
	}

	public override void Draw(dcolor_t dc)
	{
		if (b_offscreen)
		{
			return;
		}
		dc.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		if (b_selected)
		{
			dc.assign(byte.MaxValue, 0, 0, byte.MaxValue);
		}
		if (SCGlobals.b_le_active)
		{
			CowRender.BindTexture(default(texture_t));
			CowRender.DisableRS(4u);
			frect_t frect_t2 = rect;
			frect_t2.translate(endpos - startpos);
			CowRender.DrawEmptyRect(frect_t2);
			vec3_t p = new vec3_t(startpos.x, startpos.y, 0f);
			vec3_t p2 = new vec3_t(endpos.x, endpos.y, 0f);
			CowRender.DrawLine(p, p2);
			CowRender.EnableRS(4u);
			SuperCowGame.DrawDynPlatform(rect, p_texture, dc, 0);
			return;
		}
		frame_rect = rect;
		frame_rect.translate(move * (SCGlobals.frameinterp / 2f));
		SuperCowGame.DrawDynPlatform(frame_rect, p_texture, dc, 0);
		if (float1 != 0f && (objsubtype == 1 || objsubtype == 2 || objsubtype == 3))
		{
			vec2_t vec2_t2 = frame_rect.pos();
			Particles.AddParticle(vec2_t2 + new vec2_t((0f - sizex) * 0.3f, -0.8f), 0f, 3f, new dcolor_t(155, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0, 12);
			Particles.AddParticle(vec2_t2 + new vec2_t(sizex * 0.3f, -0.8f), 0f, 3f, new dcolor_t(155, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0, 12);
			obj_time2 += SCGlobals.tickperframe;
			while (obj_time2 > 30f)
			{
				Particles.AddDynamicParticle(vec2_t2 + new vec2_t((0f - sizex) * 0.3f + CRand.frand(0.1f), -1f), 11, 1, -1);
				Particles.AddDynamicParticle(vec2_t2 + new vec2_t(sizex * 0.3f + CRand.frand(0.1f), -1f), 11, 1, -1);
				obj_time2 -= 30f;
			}
		}
	}

	public override void Touch()
	{
		if (b_stand)
		{
			platformspeed = float1;
			b_switch = 0;
		}
	}

	public override void Physics()
	{
		position += move;
		rect.assign((0f - sizex) / 2f + position.x, sizey / 2f + position.y, sizex / 2f + position.x, (0f - sizey) / 2f + position.y);
		if (p_obstacle_idx != -1)
		{
			if (objsubtype == 4)
			{
				SCGlobals.ar_obstacles[p_obstacle_idx].rect = rect;
				SCGlobals.ar_obstacles[p_obstacle_idx].rect.y0 = SCGlobals.ar_obstacles[p_obstacle_idx].rect.y1 + 1.7f;
				SCGlobals.ar_obstacles[p_obstacle_idx].pos = position;
			}
			else
			{
				SCGlobals.ar_obstacles[p_obstacle_idx].rect = rect;
				SCGlobals.ar_obstacles[p_obstacle_idx].rect.y0 = SCGlobals.ar_obstacles[p_obstacle_idx].rect.y0 * 0.7f + SCGlobals.ar_obstacles[p_obstacle_idx].rect.y1 * 0.3f;
				SCGlobals.ar_obstacles[p_obstacle_idx].pos = position;
			}
		}
		float num = ((iauxvar != 0) ? 1f : (-1f));
		obj_time += num * SCGlobals.physics_speed * 0.1f * platformspeed;
		if (obj_time > movedist)
		{
			obj_time = movedist;
			iauxvar = 1 - iauxvar;
		}
		if (obj_time < 0f)
		{
			obj_time = 0f;
			iauxvar = 1 - iauxvar;
			if (b_stand)
			{
				platformspeed = 0f;
				b_switch = 0;
			}
		}
		float num2 = ((movedist == 0f) ? 0f : (obj_time / movedist));
		vec2_t vec2_t2 = position;
		vec2_t vec2_t3 = startpos * (1f - num2) + endpos * num2;
		move = vec2_t3 - vec2_t2;
		if (rect.isPointInRect(new vec2_t(SCGlobals.hero.position)))
		{
			SCGlobals.hero.forcedmove = move.v3t();
		}
		for (int i = 0; i < nummonsterlinks; i++)
		{
			ar_monsterlinks[i].forcedmove = move.v3t();
		}
		for (int j = 0; j < numlinks; j++)
		{
			ar_linkedobjs[j].move = move;
		}
	}
}
