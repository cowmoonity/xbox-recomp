namespace SuperCow;

public class CPersSnail : CMonster
{
	public animstage_t p_walk_right;

	public override bool Load(string name)
	{
		base.Load(name);
		p_walk = SCGlobals.animhost.Load("snail_left.anm");
		p_walk_right = SCGlobals.animhost.Load("snail_right.anm");
		p_stand = SCGlobals.animhost.Load("snail_pause.anm");
		p_ataka = SCGlobals.animhost.Load("snail_attack.anm");
		correct_pos.assign(0f, 0f, 0f);
		animspeed = 1f;
		pers_radius = 3f;
		monsterID = 0;
		attack_radius = 0.7f;
		fscale = 0.14f;
		monster_width = 3f;
		monster_height = 1.5f;
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
			if (direction.x < 0f)
			{
				SetAnimStage(p_walk_right);
			}
			else
			{
				SetAnimStage(p_walk);
			}
		}
	}

	public override void Attack()
	{
		move.assign(0f, 0f, 0f);
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (behavior_time <= 0)
		{
			behavior_time += p_ataka.iStageTime;
		}
		float num = (float)p_ataka.iStageTime * 0.7f;
		if ((float)behavior_time > num && (float)(behavior_time - SCGlobals.engine_frequency) < num)
		{
			SCGlobals.hero.Punish(25f);
		}
		vec3_t vec3_t2 = new vec3_t(SCGlobals.hero.position.x, 0f, monster_width * 0.5f);
		vec3_t2 -= position;
		vec3_t2.y = 0f;
		vec3_t2.normalize();
		SetRotation(new vec2_t(vec3_t2.x, vec3_t2.z).angle());
	}

	public override void CheckForObstactle()
	{
		SetDirection(direction);
		move = direction;
		move *= fspeed * SCGlobals.physics_speed;
		vec2_t vec2_t2 = new vec2_t(position + move);
		frect_t rc = default(frect_t);
		rc.assign(vec2_t2.x - monster_width / 2f, vec2_t2.y + monster_height, vec2_t2.x + monster_width / 2f, vec2_t2.y);
		bool flag = false;
		for (int i = 0; i < SCGlobals.numobstacles; i++)
		{
			obstacle_t obstacle_t2 = SCGlobals.ar_obstacles[i];
			if (obstacle_t2.obsflag == obstacle_flags_t.RIGID_OBF && obstacle_t2.rect.isRectTouchRect(rc))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			vec2_t2.x = position.x;
			SetDirection(direction * -1f);
			if (direction.x < 0f)
			{
				SetAnimStage(p_walk_right);
			}
			else
			{
				SetAnimStage(p_walk);
			}
		}
		if (vec2_t2.x < startpos.x || vec2_t2.x > endpos.x)
		{
			vec2_t2.y = position.y;
			vec2_t2.x = position.x;
			SetDirection(direction * -1f);
			if (direction.x < 0f)
			{
				SetAnimStage(p_walk_right);
			}
			else
			{
				SetAnimStage(p_walk);
			}
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
		if (physpunishtimer > 0f)
		{
			return 0;
		}
		frect_t frect_t2 = bound_rect.Clone();
		frect_t2.x0 -= attack_radius;
		frect_t2.x1 += attack_radius;
		frect_t2.y1 -= 1f;
		vec2_t p = new vec2_t(SCGlobals.hero.position);
		if (frect_t2.isPointInRect(p))
		{
			if (behavior != behaviors_t.BT_ATTACK)
			{
				SetBehavior(behaviors_t.BT_ATTACK);
			}
			return 1;
		}
		if (behavior == behaviors_t.BT_ATTACK)
		{
			SetBehavior(behaviors_t.BT_STAND);
		}
		return 0;
	}

	public override void SetBehavior(behaviors_t b)
	{
		switch (b)
		{
		case behaviors_t.BT_STAND:
			SetRotation(60f + ((CRand.phys_rand(2) != 0) ? 0f : 60f));
			behavior_time = p_stand.iStageTime / 2;
			SetAnimStage(p_stand);
			break;
		case behaviors_t.BT_WALK:
			behavior_time = 2000 + CRand.phys_rand(100) * 10;
			SetAnimStage(p_walk);
			break;
		case behaviors_t.BT_ATTACK:
			SetAnimStage(p_ataka, 200f);
			behavior_time = p_ataka.iStageTime;
			break;
		case behaviors_t.BT_DEATH:
			SetAnimStage(p_stand);
			SetRotation(60f + CRand.prand(60f));
			behavior_time = 3000;
			SCGlobals.levelstat.monsters++;
			break;
		}
		behavior = b;
	}
}
