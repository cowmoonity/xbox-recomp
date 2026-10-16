namespace SuperCow;

public class CPersDog : CMonster
{
	public animstage_t p_stand2;

	public override bool Load(string name)
	{
		base.Load(name);
		p_walk = SCGlobals.animhost.Load("dog_run.anm");
		p_stand = SCGlobals.animhost.Load("dog_pausa.anm");
		p_stand2 = SCGlobals.animhost.Load("dog_pausa2.anm");
		p_ataka = SCGlobals.animhost.Load("dog_atack.anm");
		p_dance = SCGlobals.animhost.Load("dog_dance.anm");
		correct_pos.assign(0f, 0f, -0.2f);
		animspeed = 1f;
		pers_radius = 3f;
		attack_radius = 1f;
		fscale = 1f;
		monster_width = 3.5f;
		monster_height = 1.5f;
		base.Refresh();
		SetBehavior(behaviors_t.BT_STAND);
		direction.assign((CRand.irand(2) != 0) ? 1f : (-1f), 0f, 0f);
		SetDirection(direction);
		fspeed = 0.27f;
		leye_index = GetBoneIndexByName("Bip01 Ponytail12");
		reye_index = GetBoneIndexByName("Bip01 Ponytail22");
		return true;
	}

	public override void Walk()
	{
		SetDirection(direction);
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (behavior_time <= 0)
		{
			behavior_time = 3000;
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

	public override void Attack()
	{
		move.assign(0f, 0f, 0f);
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (behavior_time <= 0)
		{
			behavior_time += p_ataka.iStageTime;
		}
		float num = (float)p_ataka.iStageTime * 0.5f;
		if ((float)behavior_time > num && (float)(behavior_time - SCGlobals.engine_frequency) < num)
		{
			Sound.Addsound("dogvoice");
			SCGlobals.hero.Punish(25f);
		}
		vec3_t vec3_t2 = new vec3_t(SCGlobals.hero.position.x, 0f, monster_width * 0.5f);
		vec3_t2 -= position;
		vec3_t2.y = 0f;
		vec3_t2.normalize();
		SetDirection(vec3_t2);
	}

	public override void Punish(float power)
	{
		SetBehavior(behaviors_t.BT_WALK);
		SetDirection(new vec3_t((direction.x > 0f) ? (-1f) : 1f, 0f, 0f));
		base.Punish(power);
	}

	public override void Calculate()
	{
		base.Calculate();
	}

	public override void Draw()
	{
		CowRender.EnableRS(4u);
		base.Draw();
		CowRender.DisableRS(4u);
	}

	public override int CheckAtaka()
	{
		if (physpunishtimer > 0f)
		{
			return 0;
		}
		vec2_t p = new vec2_t(SCGlobals.hero.position);
		float num = ((p.x - position.x > 0f) ? 1f : (-1f));
		frect_t frect_t2 = bound_rect;
		frect_t2.x0 -= attack_radius;
		frect_t2.x1 += attack_radius;
		frect_t2.y1 -= 1f;
		if (frect_t2.isPointInRect(p))
		{
			if (behavior != behaviors_t.BT_ATTACK)
			{
				SetBehavior(behaviors_t.BT_ATTACK);
			}
			return 1;
		}
		if (behavior == behaviors_t.BT_STAND)
		{
			frect_t2 = bound_rect;
			frect_t2.x0 = startpos.x;
			frect_t2.x1 = endpos.x;
			frect_t2.y1 -= 1f;
			if (frect_t2.isPointInRect(p))
			{
				SetDirection(new vec3_t(num, 0f, 0f));
				SetBehavior(behaviors_t.BT_WALK);
				b_aimcaptured = true;
			}
			else
			{
				b_aimcaptured = false;
			}
		}
		else if (behavior == behaviors_t.BT_WALK && b_aimcaptured)
		{
			frect_t2 = bound_rect;
			frect_t2.x0 = startpos.x;
			frect_t2.x1 = endpos.x;
			frect_t2.y0 += 10f;
			frect_t2.y1 -= 1f;
			if (!frect_t2.isPointInRect(p) || num * direction.x < 0f)
			{
				SetBehavior(behaviors_t.BT_STAND);
				b_aimcaptured = false;
			}
		}
		else if (behavior == behaviors_t.BT_ATTACK)
		{
			SetDirection(new vec3_t((direction.x > 0f) ? 1f : (-1f), 0f, 0f));
			SetBehavior(behaviors_t.BT_STAND);
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
			behavior_time = 3000;
			SetDirection(direction);
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
