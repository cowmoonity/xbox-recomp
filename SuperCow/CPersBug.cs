namespace SuperCow;

public class CPersBug : CMonster
{
	public override bool Load(string name)
	{
		base.Load(name);
		p_walk = SCGlobals.animhost.Load("bug_attack.anm");
		p_stand = SCGlobals.animhost.Load("bug_run.anm");
		p_ataka = SCGlobals.animhost.Load("bug_attack.anm");
		correct_pos.assign(0f, 0f, 0.2f);
		animspeed = 1f;
		pers_radius = 3f;
		attack_radius = 1f;
		fscale = 1.5f;
		monster_width = 3f;
		monster_height = 0.7f;
		base.Refresh();
		SetBehavior(behaviors_t.BT_WALK);
		direction.assign((CRand.irand(2) != 0) ? 1f : (-1f), 0f, 0f);
		SetDirection(direction);
		fspeed = 0.12f;
		leye_index = GetBoneIndexByName("Bip01 Ponytail12");
		reye_index = GetBoneIndexByName("Bip01 Ponytail22");
		return true;
	}

	public override void Walk()
	{
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (direction.x < 0f)
		{
			SetRotation(150f);
		}
		else
		{
			SetRotation(30f);
		}
		CheckForObstactle();
		monster_sound_time -= SCGlobals.engine_frequency;
		if (monster_sound_time < 0f)
		{
			monster_sound_time += 500f;
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
		move.assign(0f, 0f, 0f);
		SCGlobals.hero.Punish(50f);
		vec3_t vec3_t2 = new vec3_t(SCGlobals.hero.position.x, 0f, monster_width * 0.5f);
		vec3_t2 -= position;
		vec3_t2.y = 0f;
		vec3_t2.normalize();
		SetDirection(vec3_t2);
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (behavior_time <= 0)
		{
			SetBehavior(behaviors_t.BT_WALK);
		}
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
		if (physpunishtimer > 0f)
		{
			return 0;
		}
		frect_t frect_t2 = bound_rect;
		frect_t2.x0 -= attack_radius;
		frect_t2.x1 += attack_radius;
		frect_t2.y1 -= 1f;
		vec2_t p = new vec2_t(SCGlobals.hero.position);
		if (frect_t2.isPointInRect(p) && SCGlobals.hero.IsPunishable())
		{
			SetBehavior(behaviors_t.BT_ATTACK);
		}
		return 0;
	}

	public override void SetBehavior(behaviors_t b)
	{
		switch (b)
		{
		case behaviors_t.BT_STAND:
			SetRotation(60f + ((CRand.phys_rand(2) != 0) ? 0f : 60f));
			behavior_time = p_stand.iStageTime * 2;
			SetAnimStage(p_stand);
			break;
		case behaviors_t.BT_WALK:
			behavior_time = 2000 + CRand.phys_rand(100) * 10;
			if (direction.x > 0f)
			{
				direction.assign(1f, 0f, 0f);
			}
			else
			{
				direction.assign(-1f, 0f, 0f);
			}
			SetAnimStage(p_walk);
			break;
		case behaviors_t.BT_ATTACK:
			SetAnimStage(p_ataka, 200f);
			behavior_time = p_ataka.iStageTime * 2;
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
