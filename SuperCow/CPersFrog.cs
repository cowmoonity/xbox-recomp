using System;

namespace SuperCow;

public class CPersFrog : CMonster
{
	public float rangex0;

	public float rangex1;

	public override bool Load(string name)
	{
		base.Load(name);
		p_walk = SCGlobals.animhost.Load("frog_jump.anm");
		p_stand = SCGlobals.animhost.Load("frog_ik.anm");
		p_ataka = SCGlobals.animhost.Load("frog_plevok.anm");
		correct_pos.assign(0f, 0f, 0f);
		animspeed = 1f;
		pers_radius = 3f;
		monsterID = 8;
		attack_radius = 1f;
		fscale = 1f;
		monster_width = 3f;
		monster_height = 1.5f;
		base.Refresh();
		SetBehavior(behaviors_t.BT_WALK);
		direction.assign(1f, 0f, 0f);
		SetDirection(direction);
		fspeed = 0.12f;
		leye_index = GetBoneIndexByName("Bip01 Ponytail12");
		reye_index = GetBoneIndexByName("Bip01 Ponytail22");
		return true;
	}

	public float fsign(float f)
	{
		if (f >= 0f)
		{
			return 1f;
		}
		return -1f;
	}

	public override void Setup()
	{
		base.Setup();
		vec3_t vec3_t2 = default(vec3_t);
		vec3_t2 = endpos - startpos;
		if (vec3_t2.zero())
		{
			startpos.assign(position.x, position.y, 0f);
			endpos.assign(position.x + 6f, position.y + 3f, 0f);
			vec3_t2 = endpos - startpos;
		}
		attack_radius = vec3_t2.len();
		vec3_t2 /= attack_radius;
		if (attack_radius > 9f)
		{
			attack_radius = 9f;
			endpos = startpos + vec3_t2 * attack_radius;
		}
		SetDirection(new vec3_t(fsign(endpos.x - startpos.x), 0f, 0f));
		obstacle_t obstacleByPoint = SuperCowGame.GetObstacleByPoint(new vec2_t(position.x, position.y), -1f);
		if (obstacleByPoint.p_levelobj != null)
		{
			rangex0 = obstacleByPoint.rect.x0 + 1f;
			rangex1 = obstacleByPoint.rect.x1 - 1f;
		}
		else
		{
			rangex0 = position.x + 2f;
			rangex1 = position.x - 2f;
		}
		SetBehavior(behaviors_t.BT_WALK);
	}

	public override void Walk()
	{
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (behavior_time <= 0)
		{
			SetBehavior(behaviors_t.BT_WALK);
		}
		CheckForObstactle();
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

	public bool behaviortime(int x)
	{
		if (behavior_time < x)
		{
			return behavior_time + SCGlobals.engine_frequency >= x;
		}
		return false;
	}

	public override void Attack()
	{
		move.assign(0f, 0f, 0f);
		vec3_t vec3_t2 = new vec3_t(SCGlobals.hero.position.x, 0f, monster_width * 0.5f);
		vec3_t2 -= position;
		vec3_t2.y = 0f;
		vec3_t2.normalize();
		SetDirection(vec3_t2);
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (behaviortime(400))
		{
			vec2_t vec2_t2 = new vec2_t(endpos - startpos);
			float num = Math.Abs(vec2_t2.y);
			float num2 = Math.Abs(vec2_t2.x / 2f);
			vec2_t2.x = num2 * (float)Math.Sqrt(1f / (2f * num)) * SCGlobals.physics_speed;
			vec2_t2.y = (float)Math.Sqrt(2f * num) * SCGlobals.physics_speed;
			vec2_t2.x *= fsign(0f - position.x + SCGlobals.hero.position.x);
			Weapons.AddBullet(new vec2_t(position) + new vec2_t(direction.x * 0.9f * fscale, 0.7f * fscale), vec2_t2 * 0.048f, 34f, 0, 0);
		}
		if (behavior_time < 0)
		{
			SetBehavior(behaviors_t.BT_STAND);
		}
	}

	public override void CheckForObstactle()
	{
		move = direction;
		move *= fspeed * SCGlobals.physics_speed;
		float num = (float)(p_stage.iStageTime - behavior_time) / (float)(p_stage.iStageTime / 2);
		if (num > 1f)
		{
			num = 1f;
		}
		move *= 2f * num * num * num * num;
		vec2_t vec2_t2 = new vec2_t(position + move);
		default(frect_t).assign(vec2_t2.x - monster_width / 2f, vec2_t2.y + monster_height, vec2_t2.x + monster_width / 2f, vec2_t2.y);
		if (false)
		{
			vec2_t2.x = position.x;
			SetDirection(direction * -1f);
			SetAnimStage(p_walk);
			if (CRand.phys_rand(3) != 0)
			{
				SetBehavior(behaviors_t.BT_STAND);
			}
		}
		if (vec2_t2.x < rangex0 || vec2_t2.x > rangex1)
		{
			vec2_t2.y = position.y;
			vec2_t2.x = position.x;
			SetDirection(direction * -1f);
			SetBehavior(behaviors_t.BT_STAND);
		}
		move.assign(vec2_t2.x - position.x, vec2_t2.y - position.y, 0f);
	}

	public override void Calculate()
	{
		base.Calculate();
	}

	public override void Draw()
	{
		base.Draw();
	}

	public override int CheckAtaka()
	{
		float num = (new vec2_t(SCGlobals.hero.position) - new vec2_t(position)).len();
		if (num < attack_radius)
		{
			if (behavior != behaviors_t.BT_ATTACK)
			{
				SetBehavior(behaviors_t.BT_ATTACK);
			}
			return 1;
		}
		if (behavior == behaviors_t.BT_ATTACK)
		{
			if (CRand.phys_rand(2) != 0)
			{
				SetBehavior(behaviors_t.BT_STAND);
			}
			else
			{
				SetBehavior(behaviors_t.BT_WALK);
			}
		}
		return 0;
	}

	public override void SetBehavior(behaviors_t b)
	{
		switch (b)
		{
		case behaviors_t.BT_STAND:
			SetRotation(60f + ((CRand.phys_rand(2) != 0) ? 0f : 60f), 400f);
			behavior_time = p_stand.iStageTime / 2;
			SetAnimStage(p_stand);
			break;
		case behaviors_t.BT_WALK:
			behavior_time = p_walk.iStageTime;
			SetDirection(new vec3_t(fsign(direction.x), 0f, 0f));
			SetAnimStage(p_walk);
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
