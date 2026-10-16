using System;

namespace SuperCow;

public class CPersProf : CMonster
{
	public animstage_t p_run;

	public animstage_t p_ataka2;

	public float throw_timer;

	public bool b_throw_attack;

	public float punush_runtime;

	public vec3_t target_point;

	public override bool Load(string name)
	{
		base.Load(name);
		p_walk = SCGlobals.animhost.Load("prof_walk.anm");
		p_run = SCGlobals.animhost.Load("prof_walk.anm");
		p_stand = SCGlobals.animhost.Load("prof_walk.anm");
		p_ataka = SCGlobals.animhost.Load("prof_attack.anm");
		p_ataka2 = SCGlobals.animhost.Load("prof_attack.anm");
		p_death = SCGlobals.animhost.Load("prof_attack.anm");
		correct_pos.assign(0f, 0f, -0f);
		animspeed = 1f;
		pers_radius = CRand.frand(1f);
		attack_radius = 15f;
		fscale = 0.9f;
		monster_width = 2f;
		monster_height = 4f;
		base.Refresh();
		curlife = 100f;
		maxlife = 100f;
		SetBehavior(behaviors_t.BT_STAND);
		direction.assign(1f, 0f, 0f);
		SetDirection(direction);
		fspeed = 0.18f;
		leye_index = GetBoneIndexByName("Bip01 Ponytail12");
		reye_index = GetBoneIndexByName("Bip01 Ponytail22");
		throw_timer = 0f;
		b_throw_attack = false;
		punush_runtime = 0f;
		return true;
	}

	public override void Setup()
	{
		target_point = position;
		base.Setup();
	}

	public override void Physics()
	{
		if (SCGlobals.supercowgame.b_bossfight)
		{
			base.Physics();
			Particles.AddDynamicParticle(position, 11, 3, -1);
		}
	}

	public override void Run()
	{
		direction.assign(1f, 1f, 0f);
		SetDirection(direction);
		move = direction;
		move *= fspeed * SCGlobals.physics_speed * 1f;
	}

	public override void Walk()
	{
		vec3_t vec3_t2 = target_point - position;
		behavior_time += (int)SCGlobals.engine_frequency;
		float num = vec3_t2.len();
		if (num < 1f)
		{
			move.assign(0f, 0f, 0f);
			if (SCGlobals.hero.behavior != behaviors_t.BT_DEATH)
			{
				if (punush_runtime <= 0f && behavior != behaviors_t.BT_ATTACK && SCGlobals.hero.IsPunishable())
				{
					SetBehavior(behaviors_t.BT_ATTACK);
				}
				target_point.assign(SCGlobals.hero.position.x + 3f * (1f - (float)CRand.phys_rand(100) / 50f), SCGlobals.hero.position.y + 5f + (float)CRand.phys_rand(16) / 4f, 0f);
			}
		}
		vec3_t2.normalize();
		vec3_t2 *= (fspeed + num / 40f) * SCGlobals.physics_speed * 0.1f;
		move += vec3_t2;
		move *= 0.9f;
		direction = move;
		SetDirection(direction);
	}

	public override void Stand()
	{
		move.assign(0f, 0f, 0f);
		behavior_time -= (int)SCGlobals.engine_frequency;
		move.assign(0f, 0.1f * (float)Math.Sin((float)behavior_time * 0.004f), 0f);
		if (behavior_time <= 0)
		{
			SetDirection(direction);
			SetBehavior(behaviors_t.BT_WALK);
		}
	}

	public override void Attack()
	{
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (behavior_time < 0)
		{
			move.assign(0f, 0f, 0f);
			SetBehavior(behaviors_t.BT_WALK);
			return;
		}
		vec2_t vec2_t2 = new vec2_t(SCGlobals.hero.position - position);
		vec2_t2.normalize();
		float num = 0.8f;
		if (vec2_t2.y > num)
		{
			vec2_t2.assign((float)Math.Sign(vec2_t2.x) * (float)Math.Sqrt(1f - num * num), num);
		}
		if (vec2_t2.y < 0f - num)
		{
			vec2_t2.assign((float)Math.Sign(vec2_t2.x) * (float)Math.Sqrt(1f - num * num), 0f - num);
		}
		vec2_t2.normalize();
		move.assign(0f, 0.1f * (float)Math.Sin((float)behavior_time * 0.004f), 0f);
		if (behavior_time < 200 && behavior_time + SCGlobals.engine_frequency >= 200)
		{
			Weapons.AddBullet(new vec2_t(position) + new vec2_t(0f, monster_height), vec2_t2 * 0.1f, 34f, 4, 2);
		}
		vec3_t vec3_t2 = new vec3_t(SCGlobals.hero.position.x, 0f, monster_width * 0.5f);
		vec3_t2 -= position;
		vec3_t2.y = 0f;
		vec3_t2.normalize();
		SetRotation(new vec2_t(vec3_t2.x, vec3_t2.z).angle());
	}

	public override int CheckAtaka()
	{
		punush_runtime -= SCGlobals.engine_frequency;
		if (full_dist_to_main < attack_radius)
		{
			throw_timer += SCGlobals.engine_frequency;
		}
		return 0;
	}

	public override void Punish(float power)
	{
		SetDirection(direction * ((CRand.phys_rand(2) != 0) ? 1f : (-1f)));
		punush_runtime = 600f;
		if (!(physpunishtimer > 0f))
		{
			punishtimer = 1000f;
			physpunishtimer = 3000f;
			curlife -= 10f;
			SCGlobals.supercowgame.AddScore(position, 5000);
			if (curlife <= 0f)
			{
				SetBehavior(behaviors_t.BT_DEATH);
			}
		}
	}

	public override void CheckForObstactle()
	{
	}

	public override void Calculate()
	{
		base.Calculate();
	}

	public override void Draw()
	{
		if (physpunishtimer > 0f)
		{
			material_t matcommon = SCGlobals.matcommon;
			float num = 1f - (physpunishtimer - SCGlobals.frameinterp * (float)SCGlobals.engine_frequency) / 3000f;
			float num2 = 1f - Math.Abs(1f + (float)Math.Cos((float)Math.PI * 30f * num)) * 0.5f * (1f - 0.7f * num);
			matcommon.Diffuse.assign(1f, 1f, 1f, 0.1f + num2 * 0.9f);
			CowRender.SetBlend(0);
			CowRender.EnableRS(4u);
			CowRender.SetMaterial(matcommon);
		}
		base.Draw();
		DrawLiveBar(10);
		CowRender.SetMaterial(SCGlobals.matcommon);
		CowRender.DisableRS(4u);
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
			behavior_time = p_stand.iStageTime / 10 * (5 + CRand.phys_rand(5));
			SetAnimStage(p_stand);
			break;
		case behaviors_t.BT_WALK:
			behavior_time = 2000;
			SetDirection(direction);
			SetAnimStage(p_walk);
			break;
		case behaviors_t.BT_RUN:
			behavior_time = 2000;
			SetDirection(direction);
			SetAnimStage(p_run);
			break;
		case behaviors_t.BT_ATTACK:
			SetAnimStage(b_throw_attack ? p_ataka2 : p_ataka, 200f);
			behavior_time = (b_throw_attack ? p_ataka2.iStageTime : p_ataka.iStageTime);
			break;
		case behaviors_t.BT_DEATH:
			SCGlobals.levelstat.monsters++;
			SetAnimStage(p_death, 500f);
			SetRotation(90f);
			behavior_time = 3000;
			break;
		}
		behavior = b;
	}
}
