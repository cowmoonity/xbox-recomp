using System;

namespace SuperCow;

public class CPersGoat : CMonster
{
	public animstage_t p_run;

	public animstage_t p_ataka2;

	public float throw_timer;

	public bool b_throw_attack;

	public float punush_runtime;

	public override bool Load(string name)
	{
		base.Load(name);
		p_walk = SCGlobals.animhost.Load("goat_walk.anm");
		p_run = SCGlobals.animhost.Load("goat_run.anm");
		p_stand = SCGlobals.animhost.Load("goat_stand.anm");
		p_ataka = SCGlobals.animhost.Load("goat_attack.anm");
		p_ataka2 = SCGlobals.animhost.Load("goat_throw.anm");
		p_death = SCGlobals.animhost.Load("goat_death.anm");
		p_dance = SCGlobals.animhost.Load("goat_dance.anm");
		correct_pos.assign(0f, 0f, -0f);
		animspeed = 1f;
		pers_radius = CRand.frand(1f);
		attack_radius = 3f;
		fscale = 1.2f;
		monster_width = 2.7f;
		monster_height = 4.2f;
		base.Refresh();
		curlife = 100f;
		maxlife = 100f;
		SetBehavior(behaviors_t.BT_STAND);
		direction.assign(1f, 0f, 0f);
		SetDirection(direction);
		fspeed = 0.15f;
		leye_index = GetBoneIndexByName("Bip01 Ponytail12");
		reye_index = GetBoneIndexByName("Bip01 Ponytail22");
		throw_timer = 0f;
		b_throw_attack = false;
		punush_runtime = 0f;
		return true;
	}

	public override void Physics()
	{
		base.Physics();
	}

	public override void Run()
	{
		SetDirection(direction);
		move = direction;
		move *= fspeed * SCGlobals.physics_speed * 3f;
		vec2_t p = new vec2_t(position + move);
		float freeDirX = GetFreeDirX(p);
		if (freeDirX != 0f)
		{
			if (punush_runtime <= 0f)
			{
				SetBehavior(behaviors_t.BT_STAND);
			}
			direction.x = freeDirX;
			SetDirection(direction);
		}
	}

	public override void Walk()
	{
		SetDirection(direction);
		move = direction;
		move *= fspeed * SCGlobals.physics_speed;
		vec2_t p = new vec2_t(position + move);
		float freeDirX = GetFreeDirX(p);
		if (freeDirX != 0f)
		{
			direction.x = freeDirX;
			SetDirection(direction);
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
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (behavior_time < 0)
		{
			b_throw_attack = false;
			move.assign(0f, 0f, 0f);
			SetBehavior(behaviors_t.BT_WALK);
			return;
		}
		if (b_throw_attack)
		{
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
			vec2_t2.x *= SCGlobals.physics_speed;
			move.assign(0f, 0f, 0f);
			if (behavior_time < 250 && behavior_time + SCGlobals.engine_frequency >= 250 && SCGlobals.supercowgame.b_bossfight)
			{
				Weapons.AddBullet(new vec2_t(position) + new vec2_t(0f, monster_height), vec2_t2 * 0.3f, 34f, 1, 0);
			}
		}
		else
		{
			float num2 = 1f - (float)behavior_time / (float)p_ataka.iStageTime;
			num2 = ((!(num2 > 0.3f)) ? (num2 / 0.3f) : 1f);
			SetDirection(direction);
			move = direction;
			move *= fspeed * SCGlobals.physics_speed * num2 * (1f - num2) * 2f;
			vec2_t p = new vec2_t(position + move);
			float freeDirX = GetFreeDirX(p);
			if (freeDirX != 0f)
			{
				direction.x = freeDirX;
				move.assign(0f, 0f, 0f);
				if (Math.Abs(dist_to_main) > 4f)
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
			}
			freeDirX = GetFreeMonsterDirX(p);
			if (freeDirX != 0f)
			{
				direction.x = freeDirX;
				move.assign(0f, 0f, 0f);
			}
		}
		vec3_t vec3_t2 = new vec3_t(SCGlobals.hero.position.x, 0f, monster_width * 0.5f);
		vec3_t2 -= position;
		vec3_t2.y = 0f;
		vec3_t2.normalize();
		SetRotation(new vec2_t(vec3_t2.x, vec3_t2.z).angle());
	}

	public override int CheckAtaka()
	{
		throw_timer += SCGlobals.engine_frequency;
		punush_runtime -= SCGlobals.engine_frequency;
		frect_t rc = bound_rect;
		rc.y0 -= 2f;
		if (SCGlobals.hero.bound_rect.isRectIntersectRect(rc) && physpunishtimer <= 0f)
		{
			SCGlobals.hero.Punish(25f);
		}
		if (punush_runtime <= 0f && behavior != behaviors_t.BT_ATTACK && SCGlobals.hero.IsPunishable())
		{
			if (Math.Abs(dist_to_main) < attack_radius)
			{
				if (SCGlobals.hero.position.x > startpos.x && SCGlobals.hero.position.x < endpos.x)
				{
					direction.x = Math.Sign(dist_to_main);
					SetBehavior(behaviors_t.BT_ATTACK);
					return 1;
				}
			}
			else if (Math.Abs(dist_to_main) < 20f && behavior != behaviors_t.BT_RUN && throw_timer <= 3000f)
			{
				SetBehavior(behaviors_t.BT_RUN);
			}
			else if (throw_timer > 3000f)
			{
				throw_timer -= 3000f;
				b_throw_attack = true;
				SetBehavior(behaviors_t.BT_ATTACK);
			}
		}
		return 0;
	}

	public override void Punish(float power)
	{
		SetDirection(direction * ((CRand.phys_rand(2) != 0) ? 1f : (-1f)));
		SetBehavior(behaviors_t.BT_RUN);
		punush_runtime = 600f;
		if (!(physpunishtimer > 0f))
		{
			Particles.AddDynamicParticle(position + new vec3_t(0f, monster_height, 0f), 1, 20, -1);
			punishtimer = 1000f;
			physpunishtimer = 3000f;
			curlife -= power;
			SCGlobals.supercowgame.AddScore(position, 3000);
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
			float num = 1f - physpunishtimer / 3000f;
			float num2 = 1f - Math.Abs(1f + (float)Math.Cos((float)Math.PI * 30f * num)) * 0.5f * (1f - 0.7f * num);
			matcommon.Diffuse.assign(1f, 1f, 1f, 0.1f + num2 * 0.9f);
			CowRender.SetBlend(0);
			CowRender.EnableRS(4u);
			CowRender.SetMaterial(matcommon);
		}
		if (!b_offscreen && b_alive)
		{
			base.Draw();
			DrawLiveBar(10);
			CowRender.SetMaterial(SCGlobals.matcommon);
			CowRender.DisableRS(4u);
		}
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
