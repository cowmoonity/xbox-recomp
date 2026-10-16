using System;

namespace SuperCow;

public class CPersSpider : CMonster
{
	public float web_radius;

	public float web_amplitude;

	public float phi0;

	public float web_angle;

	public float flytime;

	public int subtype;

	public vec3_t startpos2;

	public float prev_web_len;

	public override bool Load(string name)
	{
		base.Load(name);
		p_walk = SCGlobals.animhost.Load("spider_run.anm");
		p_stand = SCGlobals.animhost.Load("spider_run.anm");
		p_ataka = SCGlobals.animhost.Load("spider_run.anm");
		correct_pos.assign(0f, 0f, 0f);
		pers_radius = 3f;
		attack_radius = 2f;
		fscale = 1f;
		monster_width = 2f;
		monster_height = 2f;
		base.Refresh();
		animspeed = 1f;
		SetBehavior(behaviors_t.BT_WALK);
		direction.assign(1f, 0f, 0f);
		SetDirection(direction);
		fspeed = 0.27f;
		return true;
	}

	public override void Setup()
	{
		flytime = 0f;
		if (startpos.y < endpos.y)
		{
			vec3_t vec3_t2 = startpos;
			startpos = endpos;
			endpos = vec3_t2;
		}
		endpos.z = 0f;
		startpos.z = 0f;
		startpos2 = startpos * 0.3f + endpos * 0.7f;
		web_radius = (endpos - startpos).len();
		float num = Math.Abs(startpos.x - endpos.x) * 0.5f;
		web_amplitude = 2f * (float)Math.Asin(num / web_radius);
		phi0 = ((startpos.x > endpos.x) ? (-(float)Math.PI / 2f) : ((float)Math.PI / 2f));
		if (monsterID == 5)
		{
			animspeed = 3f;
		}
		prev_web_len = 0f;
		base.Setup();
	}

	public override void Walk()
	{
		flytime += SCGlobals.physics_speed;
		SetRotation(90f + 30f * (float)Math.Sin(flytime * 0.1f), 30f);
		vec3_t vec3_t2 = default(vec3_t);
		float num = 1f;
		if (monsterID == 5)
		{
			num = (1f - (float)Math.Cos(flytime * 0.1f)) / 2f;
			monster_sound_time -= SCGlobals.engine_frequency;
			if (monster_sound_time < 0f)
			{
				monster_sound_time += 2000f;
			}
		}
		float num2 = web_amplitude * (float)Math.Sin(phi0 + flytime * 0.05f);
		web_angle = -(float)Math.PI / 2f + num2;
		vec3_t2.assign((float)Math.Cos(web_angle), (float)Math.Sin(web_angle), 0f);
		startpos2 = startpos + vec3_t2 * 2.5f;
		vec3_t2 *= (web_radius - 2.5f) * num + 2.5f;
		position0 = position;
		vec3_t vec3_t3 = startpos + vec3_t2;
		move.assign(vec3_t3.x - position.x, vec3_t3.y - position.y, 0f);
	}

	public override void Stand()
	{
		move.assign(0f, 0f, 0f);
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (behavior_time <= 0)
		{
			SetDirection(direction);
			SetBehavior(behaviors_t.BT_WALK);
		}
	}

	public override void Punish(float power)
	{
		base.Punish(power);
	}

	public override void Attack()
	{
	}

	public override void Calculate()
	{
		base.Calculate();
	}

	public override void Draw()
	{
		if (b_offscreen || !b_alive)
		{
			return;
		}
		float num = 1f;
		if (behavior == behaviors_t.BT_DEATH)
		{
			num = (float)(3000 - behavior_time) / 500f;
			if (num > 1f)
			{
				num = 1f;
			}
			num = 1f - num;
		}
		CowRender.DisableRS(0u);
		CowRender.EnableRS(4u);
		CowRender.SetBlend(0);
		CowRender.LoadMatrixNull();
		CowRender.BindTexture(SCGlobals.flaretexture);
		vec3_t vec3_t2 = position + move * SCGlobals.frameinterp;
		vec3_t vec3_t3 = startpos2 + forcedmove * SCGlobals.frameinterp;
		vec3_t2.z = 20f;
		vec3_t3.z = 20f;
		CowRender.DrawLine(vec3_t2, vec3_t3, 0.04f, new dcolor_t(230, 230, 230, (byte)(255f * num)));
		CowRender.EnableRS(0u);
		CowRender.DisableRS(4u);
		matrix.yrot_delta(SuperCowHelper.RadiansToDegree(web_angle) + 90f);
		base.Draw();
		CowRender.EnableRS(4u);
	}

	public override void DrawDevices()
	{
		if (!b_alive)
		{
			return;
		}
		float num = 1f;
		if (behavior == behaviors_t.BT_DEATH)
		{
			num = (float)(3000 - behavior_time) / 500f;
			if (num > 1f)
			{
				num = 1f;
			}
			num = 1f - num;
		}
		CowRender.BindTexture(SCGlobals.flaretexture);
		vec3_t vec3_t2 = startpos + forcedmove * SCGlobals.frameinterp;
		vec3_t vec3_t3 = startpos2 + forcedmove * SCGlobals.frameinterp;
		vec3_t2.z = 20f;
		vec3_t3.z = 20f;
		CowRender.DrawLine(vec3_t2, vec3_t3, 0.04f, new dcolor_t(230, 230, 230, (byte)(num * 255f)));
		CowRender.SetBlend(0);
	}

	public override int CheckAtaka()
	{
		if (physpunishtimer > 0f)
		{
			return 0;
		}
		new vec2_t(SCGlobals.hero.position);
		frect_t frect_t2 = bound_rect;
		frect_t rc = SCGlobals.hero.bound_rect;
		rc.x0 += 0.5f;
		rc.x1 -= 0.5f;
		if (frect_t2.isRectTouchRect(rc))
		{
			SCGlobals.hero.Punish(25f);
			return 1;
		}
		if (behavior != behaviors_t.BT_WALK)
		{
			SetBehavior(behaviors_t.BT_WALK);
		}
		return 0;
	}

	public override void SetBehavior(behaviors_t b)
	{
		switch (b)
		{
		case behaviors_t.BT_STAND:
			if (direction.x > 0f)
			{
				SetRotation(60f);
			}
			else
			{
				SetRotation(120f);
			}
			behavior_time = p_stand.iStageTime;
			SetAnimStage(p_stand);
			break;
		case behaviors_t.BT_WALK:
			behavior_time = 0;
			SetDirection(direction);
			SetAnimStage(p_walk);
			stage_timef = CRand.prand((float)p_walk.iStageTime - 1f);
			break;
		case behaviors_t.BT_ATTACK:
			SetAnimStage(p_ataka, 200f);
			behavior_time = p_ataka.iStageTime;
			break;
		case behaviors_t.BT_DEATH:
			SCGlobals.levelstat.monsters++;
			SetAnimStage(p_stand);
			SetRotation(60f + CRand.prand(60f));
			behavior_time = 3000;
			break;
		}
		behavior = b;
	}
}
