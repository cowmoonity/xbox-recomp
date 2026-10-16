using System;

namespace SuperCow;

public class CPersPig : CMonster
{
	public animstage_t p_stand2;

	public int generation;

	public float division_timer;

	public int step_sound_index;

	public CPersPig p_pigbrother;

	public override bool Load(string name)
	{
		base.Load(name);
		p_pigbrother = null;
		p_walk = SCGlobals.animhost.Load("pig_run.anm");
		p_stand = SCGlobals.animhost.Load("pig_stand.anm");
		p_stand2 = SCGlobals.animhost.Load("pig_stand.anm");
		p_ataka = SCGlobals.animhost.Load("pig_attack.anm");
		correct_pos.assign(0f, 1f, -0.3f);
		animspeed = 1f;
		pers_radius = CRand.frand(1f);
		attack_radius = 8f;
		fscale = 1.2f;
		monster_width = 3.4f;
		monster_height = 4.6f;
		base.Refresh();
		curlife = 50f;
		maxlife = 50f;
		SetBehavior(behaviors_t.BT_STAND);
		direction.assign(1f, 0f, 0f);
		SetDirection(direction);
		fspeed = 0.1f;
		leye_index = GetBoneIndexByName("Bip01 Ponytail12");
		reye_index = GetBoneIndexByName("Bip01 Ponytail22");
		generation = 0;
		division_timer = 0f;
		step_sound_index = 0;
		return true;
	}

	public override void Walk()
	{
		SetDirection(direction);
		base.CheckForObstactle();
	}

	public override void Stand()
	{
		move.assign(0f, 0f, 0f);
		if (division_timer > 0f)
		{
			move = direction * 20f / 400f;
			division_timer -= SCGlobals.engine_frequency;
			Particles.AddDynamicParticle(position + new vec3_t(CRand.frand(1f), CRand.prand(2f), 0f), 9, 1, -1);
			if (division_timer <= 0f)
			{
				fscale *= 0.7f;
				division_timer = 0f;
			}
		}
		else
		{
			behavior_time -= (int)SCGlobals.engine_frequency;
			if (behavior_time <= 0)
			{
				SetDirection(direction);
				SetBehavior(behaviors_t.BT_WALK);
			}
		}
	}

	public override void Attack()
	{
		SetDirection(direction);
		move = direction;
		move *= fspeed * SCGlobals.physics_speed * 4f;
		frect_t rc = bound_rect;
		rc.y0 -= 2f;
		if (SCGlobals.hero.bound_rect.isRectIntersectRect(rc) && physpunishtimer <= 0f)
		{
			SCGlobals.hero.Punish(30f);
		}
		vec2_t vec2_t2 = new vec2_t(position + move);
		frect_t rc2 = default(frect_t);
		rc2.assign(vec2_t2.x - monster_width / 2f, vec2_t2.y + monster_height, vec2_t2.x + monster_width / 2f, vec2_t2.y);
		bool flag = false;
		float x = 0f;
		for (int i = 0; i < SCGlobals.nummonsters; i++)
		{
			CMonster cMonster = SCGlobals.ar_monsters[i];
			if (cMonster != this)
			{
				frect_t frect_t2 = cMonster.bound_rect;
				if (frect_t2.isRectTouchRect(rc2))
				{
					flag = true;
					x = ((cMonster.position.x > position.x) ? (-1f) : 1f);
					break;
				}
			}
		}
		for (int i = 0; i < SCGlobals.numobstacles; i++)
		{
			obstacle_t obstacle_t2 = SCGlobals.ar_obstacles[i];
			if (obstacle_t2.obsflag == obstacle_flags_t.RIGID_OBF && obstacle_t2.rect.isRectTouchRect(rc2))
			{
				flag = true;
				x = ((obstacle_t2.pos.x > position.x) ? (-1f) : 1f);
				break;
			}
		}
		if (flag)
		{
			direction.x = x;
			b_aimcaptured = false;
			SetDirection(direction);
		}
		if (vec2_t2.x < startpos.x)
		{
			vec2_t2.x = startpos.x;
			direction.x = 1f;
			SetDirection(direction);
			SetBehavior(behaviors_t.BT_STAND);
		}
		if (vec2_t2.x > endpos.x)
		{
			vec2_t2.x = endpos.x;
			direction.x = -1f;
			SetDirection(direction);
			SetBehavior(behaviors_t.BT_STAND);
		}
		move.assign(vec2_t2.x - position.x, vec2_t2.y - position.y, 0f);
		behavior_time += (int)SCGlobals.engine_frequency;
		if (p_pigbrother == null && behavior_time > 400)
		{
			string sound = $"pigstep{step_sound_index + 1:D}";
			Sound.Addsound(sound);
			step_sound_index = (step_sound_index + 1) % 3;
			SCGlobals.supercowgame.SetCameraEffect(2);
			behavior_time -= 400;
		}
	}

	public override int CheckAtaka()
	{
		if (division_timer > 0f)
		{
			return 0;
		}
		if (Math.Abs(dist_to_main) < attack_radius && behavior != behaviors_t.BT_ATTACK && SCGlobals.hero.position.x > startpos.x && SCGlobals.hero.position.x < endpos.x)
		{
			direction.x = Math.Sign(dist_to_main);
			SetBehavior(behaviors_t.BT_ATTACK);
			return 1;
		}
		return 0;
	}

	public override void Punish(float power)
	{
		if (physpunishtimer > 0f)
		{
			return;
		}
		curlife -= power;
		if (curlife <= 0f && generation == 1)
		{
			SetBehavior(behaviors_t.BT_DEATH);
			if (SCGlobals.supercowgame.p_bossmonster == this && p_pigbrother != null && p_pigbrother.b_alive)
			{
				SCGlobals.supercowgame.p_bossmonster = p_pigbrother;
			}
			return;
		}
		if (generation == 0)
		{
			SCGlobals.supercowgame.AddScore(position, 2000);
		}
		else
		{
			SCGlobals.supercowgame.AddScore(position, 1000);
		}
		Particles.AddDynamicParticle(position + new vec3_t(0f, monster_height, 0f), 1, 20, -1);
		punishtimer = 1000f;
		physpunishtimer = 3000f;
		if (curlife <= 0f)
		{
			GlowFXHost.gfxhAddDischarges(new vec2_t(position), new vec2_t(position) + new vec2_t(0f, 20f), 1000f, 0.02f, 0.7f, 4f, 0.05f, 0.05f, 0.2f, 2.4f, 2, attenuation: true, draw_start: true, draw_end: true, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), new dcolor_t(128, 36, 36, byte.MaxValue), new dcolor_t(72, 72, 128, byte.MaxValue));
			GlowFXHost.gfxhAddDischarges(new vec2_t(position), new vec2_t(position) + new vec2_t(0f, 20f), 1000f, 0.02f, 0.7f, 4f, 0.05f, 0.05f, 0.2f, 2.4f, 2, attenuation: true, draw_start: true, draw_end: true, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), new dcolor_t(128, 36, 36, byte.MaxValue), new dcolor_t(72, 72, 128, byte.MaxValue));
			float num = 0.7f;
			fspeed *= num * 1.1f;
			correct_pos.z -= 0.2f;
			monster_width *= num;
			monster_height *= num;
			generation++;
			SetBehavior(behaviors_t.BT_STAND);
			SetRotation(120f, 400f);
			direction.x = -1f;
			curlife = maxlife;
			CMonster cMonster = SCGlobals.pershost.AddMonster(monsterID);
			division_timer = 400f;
			Sound.Addsound("addthunder");
			if (cMonster != null)
			{
				cMonster.position = position;
				cMonster.Setup();
				cMonster.SetBehavior(behaviors_t.BT_STAND);
				cMonster.SetRotation(60f, 400f);
				cMonster.scalematrix = scalematrix;
				cMonster.startpos = startpos;
				cMonster.endpos = endpos;
				cMonster.coord_y = coord_y;
				cMonster.curlife = curlife;
				cMonster.maxlife = maxlife;
				cMonster.correct_pos = correct_pos;
				cMonster.monster_width = monster_width;
				cMonster.monster_height = monster_height;
				cMonster.fspeed = fspeed;
				cMonster.fscale = fscale;
				cMonster.physpunishtimer = 3000f;
				cMonster.direction.x = 1f;
				((CPersPig)cMonster).division_timer = 400f;
				((CPersPig)cMonster).generation = generation;
				p_pigbrother = (CPersPig)cMonster;
			}
		}
	}

	public override void CheckForObstactle()
	{
		move = direction;
		move *= fspeed * SCGlobals.physics_speed;
		vec2_t vec2_t2 = new vec2_t(position + move);
		frect_t rc = default(frect_t);
		rc.assign(vec2_t2.x - monster_width / 2f, vec2_t2.y + monster_height, vec2_t2.x + monster_width / 2f, vec2_t2.y);
		for (int i = 0; i < SCGlobals.numobstacles; i++)
		{
			obstacle_t obstacle_t2 = SCGlobals.ar_obstacles[i];
			if (obstacle_t2.obsflag == obstacle_flags_t.RIGID_OBF && obstacle_t2.rect.isRectTouchRect(rc))
			{
				float x = ((obstacle_t2.pos.x > position.x) ? (-1f) : 1f);
				direction.x = x;
				SetDirection(direction);
				break;
			}
		}
		if (vec2_t2.x < startpos.x || vec2_t2.x > endpos.x)
		{
			vec2_t2.y = position.y;
			vec2_t2.x = position.x;
			SetDirection(direction * -1f);
		}
		move.assign(vec2_t2.x - position.x, vec2_t2.y - position.y, 0f);
	}

	public override void Calculate()
	{
		if (behavior == behaviors_t.BT_DEATH)
		{
			base.Calculate();
			return;
		}
		if (division_timer > 0f)
		{
			float num = (division_timer - SCGlobals.frameinterp * (float)SCGlobals.engine_frequency) / 400f;
			float num2 = fscale * (num + 0.7f * (1f - num));
			scalematrix.scale(num2, num2, num2);
		}
		else if (punishtimer > 0f)
		{
			float num3 = 1f - punishtimer / 1000f;
			float num4 = 1f + (1f - num3) * 0.3f * (float)Math.Sin(num3 * 5f * (float)Math.PI);
			float num5 = 1f - (1f - num3) * 0.3f * (float)Math.Sin(num3 * 5f * (float)Math.PI);
			scalematrix.scale(fscale * num4, fscale * num4, fscale * num5);
			punishtimer -= SCGlobals.ftickperframe;
		}
		else
		{
			scalematrix.scale(fscale, fscale, fscale);
		}
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
		case behaviors_t.BT_ATTACK:
			SetAnimStage(p_ataka, 200f);
			behavior_time = 0;
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
