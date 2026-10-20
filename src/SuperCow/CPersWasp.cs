using System;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public class CPersWasp : CMonster
{
	public vec3_t target_point;

	public float flydist;

	public override bool Load(string name)
	{
		base.Load(name);
		p_walk = SCGlobals.animhost.Load("wasp_run.anm");
		p_stand = SCGlobals.animhost.Load("wasp_run.anm");
		p_ataka = SCGlobals.animhost.Load("wasp_run.anm");
		correct_pos.assign(0f, 0f, -0.2f);
		animspeed = 1f;
		pers_radius = 3f;
		attack_radius = 2f;
		fscale = 0.7f;
		monster_width = 2f;
		monster_height = 1.5f;
		base.Refresh();
		SetBehavior(behaviors_t.BT_STAND);
		behavior_time = p_stand.iStageTime * (1 + CRand.phys_rand(2));
		direction.assign(1f, 0f, 0f);
		SetDirection(direction);
		SetRotation(90f);
		fspeed = 0.27f;
		animspeed = 2f;
		leye_index = GetBoneIndexByName("Bip01 Ponytail12");
		reye_index = GetBoneIndexByName("Bip01 Ponytail22");
		return true;
	}

	public override void Setup()
	{
		monster_sound_time = 0f;
		if (startpos.x == endpos.x && startpos.y == endpos.y)
		{
			startpos.assign(position.x - 10f, position.y + 10f, 0f);
			endpos.assign(position.x + 10f, position.y - 10f, 0f);
		}
		position.assign((startpos.x + endpos.x) / 2f, (startpos.y + endpos.y) / 2f, 0f);
		base.Setup();
	}

	public override void Walk()
	{
		move = target_point - position;
		float num = move.len();
		if (num < 0.5f)
		{
			SetBehavior(behaviors_t.BT_STAND);
			return;
		}
		float num2 = ((!(flydist > 0.2f)) ? 0f : (num / flydist));
		move.normalize();
		direction = move;
		move *= 0.1f + num2 * (1f - num2);
		if (move.x < 0f)
		{
			SetRotation(140f);
		}
		else
		{
			SetRotation(40f);
		}
		monster_sound_time -= SCGlobals.engine_frequency;
		if (monster_sound_time < 0f)
		{
			monster_sound_time += 3000f;
		}
	}

	public override void Stand()
	{
		monster_sound_time -= SCGlobals.engine_frequency;
		if (monster_sound_time < 0f)
		{
			monster_sound_time += 3000f;
		}
		move.assign(0f, 0.15f * (float)Math.Sin((float)behavior_time * 0.007f), 0f);
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (behavior_time <= 0)
		{
			float num = 0f - (startpos.x - endpos.x);
			float num2 = 0f - (startpos.y - endpos.y);
			float num3 = (float)CRand.phys_rand(100) / 100f;
			float num4 = (float)CRand.phys_rand(100) / 100f;
			target_point = startpos + new vec3_t(num3 * num, num4 * num2, 0f);
			flydist = (position - target_point).len();
			move.assign(0f, 0f, 0f);
			SetBehavior(behaviors_t.BT_WALK);
		}
	}

	public override void Attack()
	{
		monster_sound_time -= SCGlobals.engine_frequency;
		if (monster_sound_time < 0f)
		{
			monster_sound_time += 3000f;
		}
		SetDirection(move);
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (!SCGlobals.hero.IsPunishable() || behavior_time <= 0)
		{
			SetBehavior(behaviors_t.BT_STAND);
		}
		else if (SCGlobals.hero.bound_rect.isPointInRect(new vec2_t(position) + new vec2_t(0f, -1f)))
		{
			SCGlobals.hero.Punish(25f);
			SetBehavior(behaviors_t.BT_STAND);
		}
		else
		{
			move = SCGlobals.hero.position + new vec3_t(0f, 1.5f, 0f) - position;
			move.normalize();
			move *= 0.25f;
		}
	}

	public override void Calculate()
	{
		base.Calculate();
	}

	public override void Draw()
	{
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		graphicsDevice.RasterizerState = RasterizerState.CullNone;
		CowRender.EnableRS(7u);
		base.Draw();
		graphicsDevice.RasterizerState = RasterizerState.CullNone;
	}

	public override int CheckAtaka()
	{
		frect_t rc = new frect_t(startpos.v2t(), endpos.v2t());
		rc.normalize();
		if (SCGlobals.hero.bound_rect.isRectIntersectRect(rc) && SCGlobals.hero.IsPunishable() && behavior == behaviors_t.BT_STAND)
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
			if (CRand.irand(2) == 0)
			{
				SetRotation(60f, 500f);
			}
			else
			{
				SetRotation(120f, 500f);
			}
			behavior_time = 1000;
			SetAnimStage(p_stand);
			break;
		case behaviors_t.BT_WALK:
			behavior_time = 0;
			SetAnimStage(p_walk);
			break;
		case behaviors_t.BT_ATTACK:
			SetAnimStage(p_ataka, 200f);
			behavior_time = 5000;
			break;
		case behaviors_t.BT_DEATH:
			SCGlobals.levelstat.monsters++;
			animspeed = 0.2f;
			SetAnimStage(p_stand);
			SetRotation(60f + CRand.prand(60f));
			behavior_time = 3000;
			break;
		}
		behavior = b;
	}
}
