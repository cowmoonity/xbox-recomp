using System;

namespace SuperCow;

public class CPersMole : CMonster
{
	public vec3_t holepos;

	public bool b_appear;

	public float punish_time;

	public animstage_t p_fall;

	public float mole_appk;

	public override bool Load(string name)
	{
		base.Load(name);
		p_walk = SCGlobals.animhost.Load("mole_void.anm");
		p_stand = SCGlobals.animhost.Load("mole_look.anm");
		p_ataka = SCGlobals.animhost.Load("mole_attack.anm");
		p_fall = SCGlobals.animhost.Load("mole_fall.anm");
		correct_pos.assign(0f, 0f, 0f);
		animspeed = 1f;
		pers_radius = 3f;
		attack_radius = 0.5f;
		fscale = 1f;
		monster_width = 3f;
		monster_height = 2.7f;
		base.Refresh();
		SetBehavior(behaviors_t.BT_STAND);
		direction.assign(-1f, 0f, 0f);
		fspeed = 0.16f;
		b_appear = true;
		return true;
	}

	public override void Setup()
	{
		holepos = position;
		base.Setup();
	}

	public override void Walk()
	{
		mole_appk = (3000f - (float)behavior_time) / 3000f;
		vec3_t pos = new vec3_t(position);
		pos.y += 0.5f;
		pos.z += 12f;
		float num = monster_height * 0.9f * (float)SCGlobals.engine_frequency / 3000f;
		num *= (float)(b_appear ? 1 : (-1));
		move.assign(0f, num, 0f);
		if (behavior_time <= 0)
		{
			b_appear = false;
			SetBehavior(behaviors_t.BT_ATTACK);
			return;
		}
		float num2 = 1f - mole_appk;
		SetRotation(num2 * num2 * 360f * 2f + 90f);
		if (b_appear)
		{
			behavior_time -= (int)SCGlobals.engine_frequency;
		}
		else
		{
			behavior_time += (int)SCGlobals.engine_frequency;
			if (behavior_time > 3000)
			{
				SetBehavior(behaviors_t.BT_STAND);
				pos.y = coord_y;
				Particles.AddDynamicParticle(pos, 5, 10, -1);
			}
		}
		monster_sound_time -= SCGlobals.engine_frequency;
		if (monster_sound_time < 0f)
		{
			monster_sound_time += 600f;
		}
	}

	public override void Fall()
	{
		float num = 1f - (float)behavior_time / 1000f;
		float y = 0.2f - num;
		if (position.y <= coord_y - monster_height)
		{
			y = 0f;
			position.y = coord_y - monster_height;
		}
		move.assign(0f, y, 0f);
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (behavior_time <= 0)
		{
			SetBehavior(behaviors_t.BT_STAND);
		}
	}

	public override void Shock()
	{
	}

	public bool CheckForFreeSpace()
	{
		return true;
	}

	public override void Stand()
	{
		move.assign(0f, 0f, 0f);
		behavior_time -= (int)SCGlobals.engine_frequency;
		position.y = coord_y - monster_height;
		if (behavior_time <= 0)
		{
			if (CheckForFreeSpace())
			{
				SetDirection(direction);
				b_appear = true;
				SetBehavior(behaviors_t.BT_WALK);
			}
			else
			{
				behavior_time = CRand.irand(1000);
			}
		}
	}

	public override void Attack()
	{
		move.assign(0f, 0f, 0f);
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (behavior_time < 0 && p_stage != p_ataka)
		{
			SetAnimStage(p_ataka);
			behavior_time = p_ataka.iStageTime;
		}
		if (p_stage == p_ataka)
		{
			vec3_t vec3_t2 = new vec3_t(SCGlobals.hero.position.x, 0f, monster_width * 0.5f);
			vec3_t2 -= position;
			vec3_t2.y = 0f;
			vec3_t2.normalize();
			SetDirection(vec3_t2);
		}
		if (behavior_time < 300 && behavior_time + SCGlobals.engine_frequency >= 300 && p_stage == p_ataka)
		{
			vec2_t vec2_t2 = new vec2_t(SCGlobals.hero.position + new vec3_t(0f, 5f, 0f) - position);
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
			Weapons.AddBullet(new vec2_t(position.x, coord_y + 1f), vec2_t2 * 0.15f, 34f, 3, 0);
		}
		if (behavior_time < 0 && p_stage == p_ataka)
		{
			SetBehavior(behaviors_t.BT_FALL);
		}
	}

	public override void Calculate()
	{
		base.Calculate();
	}

	public override void Draw()
	{
		_ = SuperCow.DeviceManager.GraphicsDevice;
		if (!b_offscreen && b_alive && behavior != behaviors_t.BT_STAND)
		{
			frect_t vc = bound_rect.Clone();
			if (behavior != behaviors_t.BT_DEATH || !((float)behavior_time <= 2700f))
			{
				vc.y0 = coord_y;
				vc.y1 = coord_y - monster_height;
				CowRender.EnableRS(4u);
				CowRender.LoadMatrixNull();
				CowRender.BindTextureNull();
				CowRender.SetBlend(2);
				CowRender.DrawColorQuad2D(vc, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 5f);
				CowRender.SetBlend(0);
				CowRender.DisableRS(4u);
			}
			base.Draw();
		}
		if (behavior == behaviors_t.BT_DEATH)
		{
			return;
		}
		CRand.pushrandpos();
		CRand.frandomize(1000);
		vec3_t vec3_t2 = position;
		vec3_t2.y = coord_y;
		int num = 25;
		float num2 = 1f;
		for (int i = 0; i < num; i++)
		{
			float num3 = mole_appk * (1000f + CRand.frand(500f)) * ((i % 2 != 0) ? 1f : (-1f));
			Particles.AddParticle(vec3_t2 + new vec3_t(CRand.frand(monster_width * 0.3f), 0f, 5f), CRand.frand(360f) + num3, num2 * (CRand.prand(0.5f) + 0.5f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 2, 26);
			if (i > 10)
			{
				float num4 = (float)(i - 10) / 10f;
				float x = ((i % 2 != 0) ? 1f : (-1f)) * monster_width * (0f + 0.2f * num4);
				Particles.AddParticle(vec3_t2 + new vec3_t(x, num4 * 0.2f, 5f), CRand.frand(360f) + num3, CRand.prand(0.5f) + 0.5f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 2, 26);
			}
		}
		CRand.poprandpos();
	}

	public override int CheckAtaka()
	{
		return 0;
	}

	public override void Punish(float power)
	{
		base.Punish(power);
		if (curlife <= 0f)
		{
			vec3_t pos = position;
			pos.y = coord_y;
			Particles.AddDynamicParticle(pos, 4, 10, -1);
			Particles.AddDynamicParticle(pos, 5, 20, -1);
		}
	}

	public override void SetBehavior(behaviors_t b)
	{
		switch (b)
		{
		case behaviors_t.BT_STAND:
			behavior_time = 1000 + CRand.phys_rand(100) * 10;
			SetAnimStage(p_stand);
			break;
		case behaviors_t.BT_WALK:
			behavior_time = 3000;
			SetAnimStage(p_walk);
			break;
		case behaviors_t.BT_ATTACK:
			SetAnimStage(p_stand);
			behavior_time = p_ataka.iStageTime;
			break;
		case behaviors_t.BT_SHOCK:
			punish_time = 0f;
			break;
		case behaviors_t.BT_FALL:
			SetAnimStage(p_fall);
			SetRotation(90f);
			behavior_time = 1000;
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
