using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public class CPersFlyCatcher : CMonster
{
	public override bool Load(string name)
	{
		base.Load(name);
		p_walk = SCGlobals.animhost.Load("flycatcher_pause.anm");
		p_stand = SCGlobals.animhost.Load("flycatcher_pause.anm");
		p_ataka = SCGlobals.animhost.Load("flycatcher_attack.anm");
		p_death = SCGlobals.animhost.Load("flycatcher_death.anm");
		correct_pos.assign(0f, 0f, 0f);
		animspeed = 2f;
		pers_radius = 3f;
		attack_radius = 0.3f;
		fscale = 1f;
		monster_width = 2f;
		monster_height = 3f;
		base.Refresh();
		direction.assign((CRand.irand(2) != 0) ? 1f : (-1f), 0f, 0f);
		SetDirection(direction);
		behavior = behaviors_t.BT_STAND;
		SetBehavior(behaviors_t.BT_STAND);
		fspeed = 0.12f;
		return true;
	}

	public override void Stand()
	{
		move.assign(0f, 0f, 0f);
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (behavior_time <= 0)
		{
			SetRotation(90f + CRand.frand(30f), 2000f);
			behavior_time = 3000 + CRand.irand(1000);
		}
	}

	public override void Attack()
	{
		move.assign(0f, 0f, 0f);
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (behavior_time <= 0)
		{
			SetBehavior(behaviors_t.BT_STAND);
		}
		vec3_t vec3_t2 = new vec3_t(SCGlobals.hero.position.x, 0f, monster_width * 0.5f);
		vec3_t2 -= position;
		vec3_t2.y = 0f;
		vec3_t2.normalize();
		direction = vec3_t2;
		SetRotation(new vec2_t(direction.x, direction.z).angle() + 90f, 100f);
		bound_rect.assign(position.x - monster_width / 2f + vec3_t2.x * levelobjscale * 2f, position.y + monster_height, position.x + monster_width / 2f + vec3_t2.x * levelobjscale * 2f, position.y);
		if ((behavior_time < 100 && behavior_time + SCGlobals.engine_frequency >= 100) || (behavior_time < 700 && behavior_time + SCGlobals.engine_frequency >= 700))
		{
			frect_t frect_t2 = bound_rect;
			attack_radius = monster_width * 0.18f;
			frect_t2.x0 -= attack_radius;
			frect_t2.x1 += attack_radius;
			if (frect_t2.isRectTouchRect(SCGlobals.hero.bound_rect))
			{
				SCGlobals.hero.Punish(25f);
				Sound.Addsound("flycatcherbite");
			}
			else
			{
				Sound.Addsound("flycatcher");
			}
		}
	}

	public override void Draw()
	{
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		if (!b_offscreen && b_alive)
		{
			CowRender.EnableRS(4u);
			CowRender.EnableRS(5u);
			CowRender.SetBlend(0);
			CowRender.EnableRS(7u);
			// TODO: alpha test not implemented - 128f
			graphicsDevice.RasterizerState = RasterizerState.CullNone;
			base.Draw();
			// TODO: alpha test not implemented - 1f
			graphicsDevice.RasterizerState = RasterizerState.CullNone;
			CowRender.DisableRS(5u);
			CowRender.DisableRS(4u);
		}
	}

	public override int CheckAtaka()
	{
		frect_t frect_t2 = default(frect_t);
		frect_t2.assign(new vec2_t(position), monster_width * 5.5f, monster_height);
		if (frect_t2.isRectTouchRect(SCGlobals.hero.bound_rect) && SCGlobals.hero.IsPunishable() && behavior != behaviors_t.BT_ATTACK)
		{
			SetBehavior(behaviors_t.BT_ATTACK);
		}
		return 0;
	}

	public override void Death()
	{
		base.Death();
	}

	public override void Calculate()
	{
		base.Calculate();
	}

	public override void SetBehavior(behaviors_t b)
	{
		if (behavior != behaviors_t.BT_DEATH)
		{
			switch (b)
			{
			case behaviors_t.BT_STAND:
				SetRotation(90f + ((CRand.phys_rand(2) != 0) ? (-30f) : 30f));
				behavior_time = 3000;
				SetAnimStage(p_stand);
				break;
			case behaviors_t.BT_WALK:
				SetAnimStage(p_walk);
				SetDirection(direction);
				break;
			case behaviors_t.BT_ATTACK:
				SetAnimStage(p_ataka, 200f);
				behavior_time = p_ataka.iStageTime;
				break;
			case behaviors_t.BT_DEATH:
				SCGlobals.levelstat.monsters++;
				SetAnimStage(p_death);
				animspeed = 0.7f;
				SetRotation(180f + ((CRand.phys_rand(2) != 0) ? (-70f) : 70f));
				behavior_time = 3000;
				break;
			}
			behavior = b;
		}
	}
}
