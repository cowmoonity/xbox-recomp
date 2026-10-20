using System;

namespace SuperCow;

public class CMonster : CAnimGeomObj
{
	public int monsterID;

	public animstage_t p_walk = new animstage_t();

	public animstage_t p_walk2 = new animstage_t();

	public animstage_t p_stand = new animstage_t();

	public animstage_t p_look = new animstage_t();

	public animstage_t p_ataka = new animstage_t();

	public animstage_t p_shock = new animstage_t();

	public animstage_t p_death = new animstage_t();

	public animstage_t p_dance = new animstage_t();

	public float curlife;

	public float maxlife;

	public float pers_radius;

	public float attack_radius;

	public float fspeed;

	public float levelobjscale;

	public int ilevelobjscale;

	public vec3_t dir_to_main = default(vec3_t);

	public float dist_to_main;

	public float full_dist_to_main;

	public vec3_t direction = default(vec3_t);

	public vec3_t forcedmove = default(vec3_t);

	public float head_rot;

	public bool b_aimcaptured;

	public bool b_alive;

	public behaviors_t behavior;

	public int behavior_time;

	public bool b_punished;

	public float punishtimer;

	public float physpunishtimer;

	public float punishsoundtimer;

	public float walksoundtimer;

	public int monster_sound_id;

	public float monster_sound_time;

	public float monster_width;

	public float monster_height;

	public frect_t bound_rect = default(frect_t);

	public float coord_y;

	public vec3_t startpos = default(vec3_t);

	public vec3_t endpos = default(vec3_t);

	public CMonster()
	{
		levelobjscale = 1f;
		full_dist_to_main = 10000f;
	}

	public virtual void Setup()
	{
		if (ilevelobjscale == 2)
		{
			levelobjscale = 1.4f;
		}
		else if (ilevelobjscale == 3)
		{
			levelobjscale = 1.8f;
		}
		else
		{
			levelobjscale = 1f;
		}
		curlife *= ilevelobjscale;
		maxlife *= ilevelobjscale;
		fscale *= levelobjscale;
		monster_width *= levelobjscale;
		monster_height *= levelobjscale;
		scalematrix.load_identity();
		scalematrix.scale(fscale, fscale, fscale);
		forcedmove.assign(0f, 0f, 0f);
		curpos.X = position.X;
		curpos.Z = position.Y;
		full_dist_to_main = 10000f;
	}

	public virtual void Refresh()
	{
		coord_z = 10f;
		b_punished = false;
		b_alive = true;
		numdirection = 0;
		move.assign(0f, 0f, 0f);
		p_stage = p_stand;
		b_aimcaptured = false;
		scalematrix.load_identity();
		fscale *= 0.9f;
		monster_width *= 0.9f;
		monster_height *= 0.9f;
		scalematrix.scale(fscale, fscale, fscale);
		rotmatrix.load_identity();
		rotmatrix.xrot(-90f);
		curlife = 10f;
		maxlife = 10f;
		direction.assign(SuperCowHelper.Cos(rotation), SuperCowHelper.Sin(rotation), 0f);
		punishtimer = 0f;
		physpunishtimer = 0f;
		forcedmove.assign(0f, 0f, 0f);
	}

	public virtual void SetBehavior(behaviors_t b)
	{
		switch (b)
		{
		case behaviors_t.BT_STAND:
			SetRotation(60f);
			behavior_time = p_stand.iStageTime;
			SetAnimStage(p_stand);
			break;
		case behaviors_t.BT_WALK:
			behavior_time = 20000;
			SetAnimStage(p_walk);
			break;
		case behaviors_t.BT_ATTACK:
			SetAnimStage(p_ataka, 200f);
			behavior_time = p_ataka.iStageTime;
			break;
		case behaviors_t.BT_DEATH:
		{
			SCGlobals.levelstat.monsters++;
			SetAnimStage(p_stand);
			float num = CRand.prand(60f);
			SetRotation(60f + num);
			behavior_time = 3000;
			break;
		}
		}
		behavior = b;
	}

	public virtual void Physics()
	{
		if (!b_alive)
		{
			return;
		}
		dir_to_main = SCGlobals.hero.position - position;
		dir_to_main.Z = 0f;
		full_dist_to_main = dir_to_main.len();
		if (behavior == behaviors_t.BT_DEATH || monsterID == 13 || monsterID == 15 || !(full_dist_to_main > 1600f))
		{
			dir_to_main.normalize();
			dist_to_main = GetDistToMainX();
			position += move;
			bound_rect.assign(position.x - monster_width / 2f, position.y + monster_height, position.x + monster_width / 2f, position.y);
			if (monsterID == 5 || monsterID == 4)
			{
				bound_rect.y0 -= (int)(monster_height / 2f);
				bound_rect.y1 -= (int)(monster_height / 2f);
			}
			switch (behavior)
			{
			case behaviors_t.BT_WALK:
				Walk();
				break;
			case behaviors_t.BT_STAND:
				Stand();
				break;
			case behaviors_t.BT_DEATH:
				Death();
				break;
			case behaviors_t.BT_ATTACK:
				Attack();
				break;
			case behaviors_t.BT_SHOCK:
				Shock();
				break;
			case behaviors_t.BT_RUN:
				Run();
				break;
			case behaviors_t.BT_FALL:
				Fall();
				break;
			}
			move += forcedmove * SCGlobals.physics_speed;
			startpos += forcedmove;
			endpos += forcedmove;
			if (physpunishtimer > 0f)
			{
				physpunishtimer -= SCGlobals.engine_frequency;
			}
			if (behavior != behaviors_t.BT_DEATH)
			{
				CheckAtaka();
			}
			if (behavior == behaviors_t.BT_DEATH && behavior_time < 3000 && behavior_time + SCGlobals.engine_frequency >= 3000)
			{
				Sound.Addsound("monsterdeath");
			}
		}
	}

	public virtual void Walk()
	{
	}

	public virtual void Stand()
	{
	}

	public virtual void Run()
	{
	}

	public virtual void Attack()
	{
	}

	public virtual void Shock()
	{
	}

	public virtual void Fall()
	{
	}

	public virtual int CheckAtaka()
	{
		return 0;
	}

	public virtual void Death()
	{
		float num = (float)(3000 - behavior_time) / 3000f;
		move.assign((1f - num) * 2f, 4f - 40f * num, 0f);
		move *= 0.1f * SCGlobals.physics_speed;
		if (behavior_time > 0)
		{
			behavior_time -= (int)SCGlobals.engine_frequency;
		}
		else
		{
			b_alive = false;
		}
	}

	public void SetDirection(vec3_t dir)
	{
		direction = dir;
		SetRotation(new vec2_t(direction.x, direction.y).angle());
	}

	public virtual void CheckForObstactle()
	{
		move = direction;
		move *= fspeed * SCGlobals.physics_speed;
		vec3_t vec3_t2 = position + move;
		vec2_t vec2_t2 = new vec2_t(vec3_t2.X, vec3_t2.Y);
		frect_t rc = default(frect_t);
		rc.assign(vec2_t2.x - monster_width / 2f, vec2_t2.y + monster_height, vec2_t2.x + monster_width / 2f, vec2_t2.y);
		bool flag = false;
		float x = 0f;
		for (int i = 0; i < SCGlobals.numobstacles; i++)
		{
			obstacle_t obstacle_t2 = SCGlobals.ar_obstacles[i];
			if (obstacle_t2.obsflag == obstacle_flags_t.RIGID_OBF && obstacle_t2.rect.isRectTouchRect(rc))
			{
				flag = true;
				x = ((obstacle_t2.pos.X > position.X) ? (-1f) : 1f);
				break;
			}
		}
		if (flag)
		{
			direction.X = x;
			b_aimcaptured = false;
			SetDirection(direction);
		}
		if (vec2_t2.X < startpos.X)
		{
			vec2_t2.X = startpos.X;
			direction.X = 1f;
			SetDirection(direction);
		}
		if (vec2_t2.X > endpos.X)
		{
			vec2_t2.X = endpos.X;
			direction.X = -1f;
			SetDirection(direction);
		}
		move.assign(vec2_t2.X - position.X, vec2_t2.Y - position.Y, 0f);
	}

	public float GetFreeDirX(vec2_t p)
	{
		frect_t rc = default(frect_t);
		rc.assign(p.x - monster_width / 2f, p.y + monster_height, p.x + monster_width / 2f, p.y);
		for (int i = 0; i < SCGlobals.numobstacles; i++)
		{
			obstacle_t obstacle_t2 = SCGlobals.ar_obstacles[i];
			if (obstacle_t2.obsflag != obstacle_flags_t.TRANS_OBF && obstacle_t2.rect.isRectIntersectRect(rc))
			{
				if (!(obstacle_t2.pos.X > position.X))
				{
					return 1f;
				}
				return -1f;
			}
		}
		if (p.X < startpos.X)
		{
			return 1f;
		}
		if (p.X > endpos.X)
		{
			return -1f;
		}
		return 0f;
	}

	public float GetDistToMainX()
	{
		float num = SCGlobals.hero.position.Y - position.y;
		if (num > monster_height * 1.5f || num < -2f)
		{
			return 1000000f;
		}
		return SCGlobals.hero.position.X - position.x;
	}

	public float GetFreeMonsterDirX(vec2_t p)
	{
		default(frect_t).assign(p.x - monster_width / 2f, p.y + monster_height, p.x + monster_width / 2f, p.y);
		return 0f;
	}

	public void SetFlag(bool b_set)
	{
	}

	public virtual void Punish(float power)
	{
		if (behavior != behaviors_t.BT_DEATH && b_alive)
		{
			punishtimer = 1000f;
			physpunishtimer = 1000f;
			int addscore = 50;
			switch (monsterID)
			{
			case 0:
				addscore = 50;
				break;
			case 2:
				addscore = 80;
				break;
			case 1:
				addscore = 100;
				break;
			case 3:
				addscore = 150;
				break;
			case 5:
				addscore = 130;
				break;
			case 4:
				addscore = 130;
				break;
			case 6:
				addscore = 150;
				break;
			case 8:
				addscore = 150;
				break;
			case 10:
				addscore = 200;
				break;
			case 11:
				addscore = 250;
				break;
			case 12:
				addscore = 80;
				break;
			case 13:
				addscore = 300;
				break;
			case 14:
				addscore = 100;
				break;
			}
			SCGlobals.supercowgame.AddScore(position, addscore);
			int num = monsterID;
			if (num == 3)
			{
				Particles.AddDynamicParticle(position, 14, 20, -1);
			}
			else
			{
				Particles.AddDynamicParticle(position, 1, 30, -1);
			}
			curlife -= power;
			if (curlife <= 0f)
			{
				SetBehavior(behaviors_t.BT_DEATH);
				AddDeathCoin();
			}
		}
	}

	public void AddDeathCoin()
	{
		obstacle_t anyObstacleByPoint = SuperCowGame.GetAnyObstacleByPoint(new vec2_t(position), b_nontriangle: false);
		float num = 0f;
		Bonus.AddCoin(endy: (anyObstacleByPoint.p_levelobj == null) ? (-1024f) : (anyObstacleByPoint.rect.y0 + 0.9f), type: (maxlife >= 30f) ? 3 : ((!(maxlife >= 20f)) ? 1 : 2), pos: new vec3_t(position.x, position.y + 0.3f, 0f).v2t(), size: 64f);
	}

	public void DrawLiveBar(int n)
	{
		if (behavior != behaviors_t.BT_DEATH && b_alive && SCGlobals.viewport_rect.isRectTouchRect(bound_rect))
		{
			float num = monster_width * 0.7f;
			vec2_t vec2_t2 = new vec2_t(curpos.x - num / 2f, curpos.z + monster_height + 1f);
			if (monsterID == 4 || monsterID == 5)
			{
				vec2_t2.y -= monster_height;
			}
			CowRender.DisableRS(0u);
			CowRender.EnableRS(4u);
			CowRender.EnableRS(5u);
			CowRender.SetBlend(0);
			CowRender.LoadMatrixNull();
			CowRender.BindTexture(SCGlobals.p_white);
			vec3_t vec3_t2 = default(vec3_t);
			vec3_t vec3_t3 = default(vec3_t);
			vec3_t vec3_t4 = default(vec3_t);
			vec3_t2.assign(vec2_t2.x, vec2_t2.y, 10f);
			vec3_t3.assign(vec2_t2.x + num * (curlife / maxlife), vec2_t2.y, 10f);
			vec3_t4.assign(vec2_t2.x + num, vec2_t2.y, 10f);
			CowRender.DrawLine(vec3_t2, vec3_t3, 0.07f, new dcolor_t(0, byte.MaxValue, 0, 155));
			CowRender.DrawLine(vec3_t3, vec3_t4, 0.07f, new dcolor_t(byte.MaxValue, 0, 0, 155));
			CowRender.DisableRS(4u);
			CowRender.EnableRS(0u);
		}
	}

	public void BaseDraw()
	{
		base.Draw();
	}

	public override void Draw()
	{
		if (!b_offscreen && b_alive)
		{
			if (ilevelobjscale > 1)
			{
				DrawLiveBar(ilevelobjscale * 2);
			}
			CowRender.EnableRS(4u);
			CowRender.EnableRS(5u);
			CowRender.SetBlend(0);
			base.Draw();
			CowRender.DisableRS(4u);
			CowRender.DisableRS(5u);
		}
	}

	public void CalculateSound()
	{
	}

	public override void CalcMath()
	{
		base.CalcMath();
	}

	public override void Calculate()
	{
		if (!b_alive)
		{
			return;
		}
		if (punishsoundtimer > 0f)
		{
			punishsoundtimer -= SCGlobals.ftickperframe;
		}
		CalculateSound();
		b_offscreen = !SCGlobals.frustum.SphereInFrustum(position.x, position.y, position.z, 5f);
		if (b_offscreen)
		{
			return;
		}
		if (punishtimer > 0f)
		{
			float num = 1f - punishtimer / 1000f;
			float num2 = 1f + (1f - num) * 0.3f * (float)Math.Sin(num * 5f * 3.1415925f);
			float num3 = 1f - (1f - num) * 0.3f * (float)Math.Sin(num * 5f * 3.1415925f);
			scalematrix.scale(fscale * num2, fscale * num2, fscale * num3);
			punishtimer -= SCGlobals.ftickperframe;
		}
		else
		{
			scalematrix.scale(fscale, fscale, fscale);
		}
		if (behavior == behaviors_t.BT_DEATH)
		{
			float num4 = (float)(3000 - behavior_time) + (float)SCGlobals.engine_frequency * SCGlobals.frameinterp;
			float num5 = num4 / 3000f;
			vec3_t vec3_t2 = default(vec3_t);
			float num6 = num5 * 8f * 3.1415925f;
			float num7 = 1f - num5 * 2f;
			byte b = (byte)(255f * num7);
			for (int i = 0; i < 12; i++)
			{
				vec3_t2.assign((float)Math.Sin(num6 + (float)i), monster_height + 0.03f * (float)i, (float)Math.Cos(num6 + (float)i));
				vec3_t2 *= 1f + 0.03f * (float)i;
				Particles.AddParticle(position - move * (1f - SCGlobals.frameinterp) + vec3_t2, 10f * (float)i, 0.6f, new dcolor_t(b, b, b, byte.MaxValue), 0, 0);
			}
		}
		if (!SCGlobals.b_pause)
		{
			base.Calculate();
		}
	}

	public void DrawShadow()
	{
	}

	public virtual void DrawDevices()
	{
	}
}
