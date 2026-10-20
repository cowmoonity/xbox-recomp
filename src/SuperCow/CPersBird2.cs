namespace SuperCow;

public class CPersBird2 : CMonster
{
	public override bool Load(string name)
	{
		base.Load(name);
		p_walk = SCGlobals.animhost.Load("bird_run.anm");
		p_stand = SCGlobals.animhost.Load("bird_run.anm");
		p_ataka = SCGlobals.animhost.Load("bird_attack.anm");
		correct_pos.assign(0f, 0f, -0.2f);
		animspeed = 1f;
		pers_radius = 3f;
		attack_radius = 2f;
		fscale = 0.85f;
		monster_width = 2.5f;
		monster_height = 1.5f;
		base.Refresh();
		SetBehavior(behaviors_t.BT_STAND);
		direction.assign(1f, 0f, 0f);
		SetDirection(direction);
		fspeed = 0.27f;
		leye_index = GetBoneIndexByName("Bip01 Ponytail12");
		reye_index = GetBoneIndexByName("Bip01 Ponytail22");
		monster_sound_time = 0f;
		return true;
	}

	public override void Walk()
	{
		move = direction * 0.3f;
		behavior_time += (int)SCGlobals.engine_frequency;
		if (behavior_time > 800)
		{
			behavior_time -= 800;
			vec2_t dir = default(vec2_t);
			dir.assign(direction.x * 0.08f, 0f);
			Weapons.AddBullet(new vec2_t(position), dir, 34f, 2, 0);
		}
		frect_t frect_t2 = new frect_t(startpos.v2t(), endpos.v2t());
		frect_t2.normalize();
		if (!frect_t2.isPointInRect(new vec2_t(position + move)))
		{
			SetDirection(direction * -1f);
			SetRotation(dest_rotation, 700f);
			SetBehavior(behaviors_t.BT_STAND);
		}
		monster_sound_time -= SCGlobals.engine_frequency;
		if (monster_sound_time < 0f)
		{
			monster_sound_time += 800f;
		}
	}

	public override void Stand()
	{
		move.assign(0f, 0f, 0f);
		behavior_time -= (int)SCGlobals.engine_frequency;
		new frect_t(startpos.v2t(), endpos.v2t()).normalize();
		monster_sound_time -= SCGlobals.engine_frequency;
		if (monster_sound_time < 0f)
		{
			monster_sound_time += 300f;
		}
		if (behavior_time <= 0)
		{
			SetBehavior(behaviors_t.BT_WALK);
		}
	}

	public override int CheckAtaka()
	{
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
			behavior_time = 400;
			animspeed = 3f;
			SetAnimStage(p_stand);
			break;
		case behaviors_t.BT_WALK:
			animspeed = 1f;
			behavior_time = CRand.phys_rand(80) * 10;
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
