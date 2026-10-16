namespace SuperCow;

public class CPersHedgehog : CMonster
{
	public override bool Load(string name)
	{
		base.Load(name);
		p_walk = SCGlobals.animhost.Load("hedgehog.anm");
		p_stand = SCGlobals.animhost.Load("hedgehog.anm");
		p_ataka = SCGlobals.animhost.Load("hedgehog.anm");
		correct_pos.assign(0f, 1.5f, 0f);
		animspeed = 2f;
		pers_radius = 3f;
		attack_radius = 1f;
		fscale = 0.6f;
		monster_width = 1.8f;
		monster_height = 0.9f;
		base.Refresh();
		SetBehavior(behaviors_t.BT_WALK);
		direction.assign((CRand.irand(2) != 0) ? 1f : (-1f), 0f, 0f);
		SetDirection(direction);
		fspeed = 0.12f;
		return true;
	}

	public override void Walk()
	{
		behavior_time -= (int)SCGlobals.engine_frequency;
		CheckForObstactle();
		monster_sound_time -= SCGlobals.engine_frequency;
		if (monster_sound_time < 0f)
		{
			monster_sound_time += 3500f;
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
			SetAnimStage(p_walk);
		}
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
		CowRender.EnableRS(4u);
		CowRender.EnableRS(5u);
		CowRender.SetBlend(0);
		CowRender.EnableRS(7u);
		base.Draw();
		CowRender.DisableRS(7u);
		CowRender.DisableRS(5u);
		CowRender.DisableRS(4u);
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
		if (frect_t2.isPointInRect(p))
		{
			SCGlobals.hero.Punish(25f);
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
			SetDirection(direction);
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
