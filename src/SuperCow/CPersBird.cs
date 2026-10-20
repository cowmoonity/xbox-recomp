using System;

namespace SuperCow;

public class CPersBird : CMonster
{
	public animstage_t p_stand2;

	public float flytime;

	public override bool Load(string name)
	{
		base.Load(name);
		p_walk = SCGlobals.animhost.Load("bird_run.anm");
		p_stand = SCGlobals.animhost.Load("bird_run.anm");
		p_stand2 = SCGlobals.animhost.Load("bird_run.anm");
		p_ataka = SCGlobals.animhost.Load("bird_attack.anm");
		correct_pos.assign(0f, 0f, -0.2f);
		animspeed = 1f;
		pers_radius = 3f;
		attack_radius = 2f;
		fscale = 0.85f;
		monster_width = 2.7f;
		monster_height = 1.5f;
		base.Refresh();
		SetBehavior(behaviors_t.BT_WALK);
		direction.assign(1f, 0f, 0f);
		SetDirection(direction);
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
			startpos.assign(position.x - 10f, position.y + 5f, 0f);
			endpos.assign(position.x + 10f, position.y - 5f, 0f);
		}
		monster_sound_time = 0f;
		base.Setup();
	}

	public override void Walk()
	{
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
		animspeed = 1.6f + 0.7f * (float)Math.Sin(num4 * num5);
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (behavior_time < 200 && behavior_time + SCGlobals.engine_frequency >= 200)
		{
			SetAnimStage(p_ataka);
		}
		if (behavior_time < 0)
		{
			behavior_time += 4500;
		}
		monster_sound_time -= SCGlobals.engine_frequency;
		if (monster_sound_time < 0f)
		{
			if (p_stage == p_ataka && behavior_time > 200)
			{
				SetAnimStage(p_walk);
			}
			monster_sound_time += 800f / animspeed;
		}
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

	public override void Attack()
	{
		monster_sound_time -= SCGlobals.engine_frequency;
		if (monster_sound_time < 0f)
		{
			monster_sound_time += 800f;
		}
		SetDirection(move);
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (!SCGlobals.hero.IsPunishable() || behavior_time <= 0)
		{
			SetBehavior(behaviors_t.BT_WALK);
			return;
		}
		move = SCGlobals.hero.position + new vec3_t(0f, 1.5f, 0f) - position;
		move.normalize();
		if (SCGlobals.hero.bound_rect.isPointInRect(new vec2_t(position + move)))
		{
			SCGlobals.hero.Punish(25f);
			SetBehavior(behaviors_t.BT_WALK);
		}
		move *= 0.23f;
	}

	public override int CheckAtaka()
	{
		if (physpunishtimer > 0f)
		{
			return 0;
		}
		frect_t rc = new frect_t(startpos.v2t(), endpos.v2t());
		rc.normalize();
		if (SCGlobals.hero.bound_rect.isRectIntersectRect(rc) && SCGlobals.hero.IsPunishable() && behavior == behaviors_t.BT_WALK)
		{
			SetBehavior(behaviors_t.BT_ATTACK);
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
			if (direction.x > 0f)
			{
				SetRotation(60f);
			}
			else
			{
				SetRotation(120f);
			}
			behavior_time = p_stand.iStageTime;
			SetAnimStage((CRand.irand(2) != 0) ? p_stand : p_stand2);
			break;
		case behaviors_t.BT_WALK:
			behavior_time = 0;
			SetDirection(direction);
			SetAnimStage(p_walk);
			break;
		case behaviors_t.BT_ATTACK:
			SetAnimStage(p_ataka, 200f);
			behavior_time = 5000;
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
