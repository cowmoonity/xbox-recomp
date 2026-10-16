using System;

namespace SuperCow;

public class CPersBat : CMonster
{
	public float flytime;

	public bool b_flystarted;

	public override bool Load(string name)
	{
		base.Load(name);
		p_walk = SCGlobals.animhost.Load("bat_run.anm");
		p_stand = SCGlobals.animhost.Load("bat_pause.anm");
		p_ataka = SCGlobals.animhost.Load("bat_run.anm");
		correct_pos.assign(0f, 0f, 1f);
		animspeed = 1f;
		pers_radius = 3f;
		attack_radius = 2f;
		fscale = 0.55f;
		monster_width = 3.5f;
		monster_height = 1.8f;
		base.Refresh();
		b_flystarted = false;
		direction.assign(1f, 0f, 0f);
		SetDirection(direction);
		SetBehavior(behaviors_t.BT_STAND);
		fspeed = 0.27f;
		flytime = 0f;
		leye_index = GetBoneIndexByName("Bip01 Ponytail12");
		reye_index = GetBoneIndexByName("Bip01 Ponytail22");
		return true;
	}

	public override void Setup()
	{
		if (startpos.x == endpos.x && startpos.y == endpos.y)
		{
			startpos.assign(position.x - 6f, position.y + 6f, 0f);
			endpos.assign(position.x + 6f, position.y - 6f, 0f);
		}
		attack_radius = new vec2_t(startpos - endpos).len();
		float num = attack_radius / 15f;
		if (num > 1f)
		{
			num = 1f;
		}
		fscale = 0.55f + num * 0.2f;
		scalematrix.scale(fscale, fscale, fscale);
		monster_width = 3.5f * fscale;
		monster_height = 1.8f * fscale;
		flytime = 0f;
		base.Setup();
	}

	public override void Walk()
	{
		b_flystarted = true;
		direction = move;
		direction.y *= 0.5f;
		direction.normalize();
		direction.z = direction.y;
		float angle = 0f - new vec2_t(direction.x, direction.z).angle();
		SetRotation(angle, 100f);
		flytime += SCGlobals.physics_speed;
		vec3_t vec3_t2 = default(vec3_t);
		vec2_t vec2_t2 = default(vec2_t);
		float num = Math.Abs(startpos.x - endpos.x) * 0.5f;
		float num2 = Math.Abs(startpos.y - endpos.y) * 0.5f;
		float num3 = 1f;
		float num4 = 2f;
		float num5 = flytime / (num + num2) * 0.3f;
		position0 = position;
		vec3_t vec3_t3 = default(vec3_t);
		vec3_t2.assign(num * (float)Math.Cos((float)Math.PI + num3 * num5), num2 * (float)Math.Sin((float)Math.PI + num4 * num5), 0f);
		vec2_t2 = (startpos + endpos).v2t() / 2f;
		vec3_t3.assign(vec3_t2.x + vec2_t2.x, vec3_t2.y + vec2_t2.y, 0f);
		move.assign(vec3_t3.x - position0.x, vec3_t3.y - position0.y, 0f);
		if (move.len() > 0.7f)
		{
			move.normalize();
			move *= 0.3f;
		}
		animspeed = 1.4f + 0.5f * (float)Math.Cos(num4 * num5);
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (behavior_time < 0)
		{
			behavior_time += 10000;
		}
		monster_sound_time -= SCGlobals.engine_frequency;
		if (monster_sound_time < 0f)
		{
			monster_sound_time += 800f / animspeed;
		}
	}

	public override void Stand()
	{
		move.assign(0f, 0f, 0f);
		if (Math.Abs(SCGlobals.hero.position.x - position.x) < 5f)
		{
			SetDirection(direction);
			SetBehavior(behaviors_t.BT_WALK);
		}
	}

	public override void Attack()
	{
	}

	public override int CheckAtaka()
	{
		new vec2_t(SCGlobals.hero.position);
		frect_t frect_t2 = bound_rect;
		frect_t rc = SCGlobals.hero.bound_rect;
		rc.x0 += 0.5f;
		rc.x1 -= 0.5f;
		if (frect_t2.isRectTouchRect(rc) && physpunishtimer <= 0f)
		{
			SCGlobals.hero.Punish(25f);
			return 1;
		}
		return 0;
	}

	public override void Death()
	{
		base.Death();
	}

	public override void SetBehavior(behaviors_t b)
	{
		switch (b)
		{
		case behaviors_t.BT_STAND:
			if (!b_flystarted)
			{
				SetRotation(90f);
				position = startpos;
				behavior_time = p_stand.iStageTime;
				SetAnimStage(p_stand);
				break;
			}
			return;
		case behaviors_t.BT_WALK:
			behavior_time = 0;
			SetDirection(direction);
			SetAnimStage(p_walk, 600f);
			break;
		case behaviors_t.BT_ATTACK:
			SetAnimStage(p_ataka, 200f);
			behavior_time = p_ataka.iStageTime;
			break;
		case behaviors_t.BT_DEATH:
			SCGlobals.levelstat.monsters++;
			SetAnimStage(p_stand, 700f);
			SetRotation(60f + CRand.prand(60f));
			behavior_time = 3000;
			break;
		}
		behavior = b;
	}
}
