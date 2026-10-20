using System;
using Microsoft.Xna.Framework.Input;

namespace SuperCow;

public class CMainHero : CMonster
{
	public enum ENUM
	{
		VIBRATE_HIT,
		VIBRATION_ENEMY_HIT,
		VIBRATE_CHECKPOINT
	}

	public int numstands;

	public int jumpsoundindex;

	public animstage_t p_push = new animstage_t();

	public animstage_t p_kink = new animstage_t();

	public int numdeathanim;

	public int starttime;

	public animstage_t[] p_void = new animstage_t[10];

	public int numvoidanim;

	public int nvoidanim;

	public animstage_t[] p_merrydance = new animstage_t[3];

	public animstage_t p_animstart = new animstage_t();

	public animstage_t p_victory = new animstage_t();

	public animstage_t p_poklon = new animstage_t();

	public animstage_t p_jump_up = new animstage_t();

	public animstage_t p_jump_down = new animstage_t();

	public animstage_t p_jump_top = new animstage_t();

	public animstage_t p_sitdown = new animstage_t();

	public animstage_t p_banjo = new animstage_t();

	public animstage_t p_wow = new animstage_t();

	public animstage_t p_toilet = new animstage_t();

	public animstage_t p_lianaanim = new animstage_t();

	public animstage_t p_readanim = new animstage_t();

	public vec3_t safeposition = default(vec3_t);

	public float herobridgey;

	public float megajumptimer;

	public float hypershoetimer;

	public float firmnesstimer;

	public float startharmlesstimer;

	public float lianatimer;

	public float jump_value;

	public float fall_value;

	public bool b_landing;

	public bool b_sliding;

	public float jump_dir;

	public float horiz_speed;

	public float delay_time;

	public int[] numweapons = new int[32];

	public int curweapon;

	public float invisible_time;

	public bool b_invisible;

	public float make_inv_time;

	public float death_inv_counter;

	public int ponytal_index;

	public float punishtime;

	public float sparkstime;

	public bool b_setsparks;

	public bool b_eyesout;

	public int stepsoundindex;

	public int punishsoundindex;

	public int monstercombo;

	public int numbonuses;

	public CLianaObj p_lianaobj;

	public float lianay;

	public float yeye;

	public float zeye;

	public float yeyec;

	public float zeyec;

	public float yeyed;

	public float zeyed;

	public float btim;

	public float mHeroMaxSpeed;

	public float mHeroAcceleration;

	public float mHeroJumpValue;

	public float mHeroMegaJumpValue;

	public bool b_kick;

	public bool b_dance;

	public bool b_firstfall;

	public bool b_firmsurface;

	public float herocursorx;

	public float herocursorx0;

	public float herocursor_dead_zone = 1f;

	public float herocursor_full_zone = 2f;

	public int toiletcounter;

	public int deathphraseconter;

	public bool b_left;

	public bool b_right;

	public bool b_up;

	public bool b_down;

	public bool b_up0;

	public bool b_up1;

	public bool b_go;

	public bool b_herosafe = true;

	private CAnimHost animhost = new CAnimHost();

	public string[] ar_herowords = new string[5];

	public vec2_t exitpoint;

	public CMainHero()
	{
		mHeroMaxSpeed = 3.5f;
		mHeroAcceleration = 0.2f;
		mHeroJumpValue = 11f;
		mHeroMegaJumpValue = 14f;
		btim = -0.009f;
	}

	public void SetMaxSpeed(float aSpeedModifier)
	{
		mHeroMaxSpeed = aSpeedModifier * 3.5f;
	}

	public void SetAcceleration(float aAccel)
	{
		mHeroAcceleration = aAccel * 0.2f;
	}

	public void SetJumpValue(float aJumpMod)
	{
		mHeroJumpValue = aJumpMod * 11f;
		mHeroMegaJumpValue = aJumpMod * 14f;
	}

	public override bool Load(string name)
	{
		base.Load(name);
		for (int i = 0; i < 5; i++)
		{
			string text = "heroword" + (i + 1);
			ar_herowords[i] = wstrstuff.GetStr(text);
		}
		p_walk = SCGlobals.animhost.Load("cow_beg.anm");
		p_walk2 = SCGlobals.animhost.Load("cow_hop.anm");
		p_stand = SCGlobals.animhost.Load("cow_stand.anm");
		p_jump_up = SCGlobals.animhost.Load("cow_jump_up.anm");
		p_jump_down = SCGlobals.animhost.Load("cow_jump_down.anm");
		p_jump_top = SCGlobals.animhost.Load("cow_jump_top.anm");
		p_sitdown = SCGlobals.animhost.Load("cow_lookdown.anm");
		p_toilet = SCGlobals.animhost.Load("cow_sit.anm");
		p_wow = SCGlobals.animhost.Load("cow_wow.anm");
		p_lianaanim = SCGlobals.animhost.Load("cow_liana.anm");
		p_readanim = SCGlobals.animhost.Load("cow_read.anm");
		p_stage = p_stand;
		p_kink = p_stand;
		p_void[0] = SCGlobals.animhost.Load("cow_loiter1.anm");
		p_void[1] = SCGlobals.animhost.Load("cow_loiter2.anm");
		p_void[2] = SCGlobals.animhost.Load("cow_loiter3.anm");
		p_void[3] = SCGlobals.animhost.Load("cow_loiter4.anm");
		p_void[4] = SCGlobals.animhost.Load("cow_loiter5.anm");
		p_void[6] = p_stand;
		p_merrydance[0] = SCGlobals.animhost.Load("cow_dance1.anm");
		p_merrydance[1] = SCGlobals.animhost.Load("cow_dance2.anm");
		p_merrydance[2] = SCGlobals.animhost.Load("cow_dance3.anm");
		p_victory = SCGlobals.animhost.Load("cow_victory.anm");
		p_poklon = SCGlobals.animhost.Load("cow_poklon.anm");
		numvoidanim = 5;
		p_animstart = SCGlobals.animhost.Load("cow_victory.anm");
		p_push = p_stand;
		p_shock = SCGlobals.animhost.Load("cow_shock.anm");
		p_banjo = SCGlobals.animhost.Load("cow_banjo.anm");
		p_death = SCGlobals.animhost.Load("cow_death.anm");
		numdeathanim = 2;
		leye_index = GetBoneIndexByName("Box01");
		reye_index = GetBoneIndexByName("Box11");
		hohol_index = -1;
		Refresh();
		monster_width = 2f * fscale;
		monster_height = 4.5f * fscale;
		return true;
	}

	public override void Refresh()
	{
		curlife = 100f;
		maxlife = 100f;
		monstercombo = 0;
		herobridgey = -10000f;
		lianatimer = 0f;
		fscale = 0.9f;
		scalematrix.load_identity();
		scalematrix.scale(fscale, fscale, fscale);
		p_stage = p_animstart;
		b_overlap = false;
		stage_timef = (stageup_timef = 0f);
		b_alive = true;
		b_invisible = false;
		invisible_time = 0f;
		b_eyesout = false;
		b_kick = false;
		b_punished = false;
		punishtime = 0f;
		numstands = 0;
		numdirection = 0;
		correct_pos = new vec3_t(0f, 0f, 0f);
		move = new vec3_t(0f, 0f, 0f);
		forcedmove = new vec3_t(0f, 0f, 0f);
		rotmatrix = default(matrix_t);
		rotmatrix.load_identity();
		rotmatrix.xrot(-90f);
		animspeed = 1f;
		delay_time = 0f;
		make_inv_time = 0f;
		numdirection = 2;
		dest_rotation = 0f;
		rotation = 0f;
		death_inv_counter = 0f;
		curweapon = 0;
		nvoidanim = 0;
		b_firstfall = false;
		jump_value = 0f;
		jump_dir = 0f;
		horiz_speed = 0f;
		pers_radius = 3f;
		b_dance = false;
		b_landing = false;
		b_sliding = false;
		b_firmsurface = false;
		b_setsparks = false;
		sparkstime = 0f;
		yeye = (zeye = (yeyec = (zeyec = (btim = 0f))));
		btim = -0.009f;
		yeyed = (zeyed = 100f);
		numbonuses = 0;
		punishsoundtimer = 0f;
		walksoundtimer = 0f;
		stepsoundindex = 0;
		punishsoundindex = 0;
		punishsoundtimer = 0f;
		behavior = behaviors_t.BT_STAND;
		SetBehavior(behaviors_t.BT_STAND);
		SetRotation(80f);
		herocursorx = 0f;
		herocursorx0 = 0f;
		megajumptimer = 0f;
		hypershoetimer = 0f;
		firmnesstimer = 0f;
		startharmlesstimer = 0f;
		starttime = 0;
		coord_z = 25f;
		toiletcounter = 0;
		b_up0 = false;
		b_up1 = false;
	}

	public override void Physics()
	{
		SetFlag(b_set: false);
		position += move;
		bound_rect.assign(position.x - monster_width / 2f, position.y + monster_height, position.x + monster_width / 2f, position.y);
		if (behavior == behaviors_t.BT_WOW)
		{
			SCGlobals.control_state = 0u;
		}
		b_up = XInputHelper.GamePads[SuperCow.lockedController].State.Buttons.A == ButtonState.Pressed;
		b_right = XInputHelper.GamePads[SuperCow.lockedController].ThumbStickLeftX > 0.2f;
		b_left = XInputHelper.GamePads[SuperCow.lockedController].ThumbStickLeftX < -0.2f;
		b_down = XInputHelper.GamePads[SuperCow.lockedController].ThumbStickLeftY < -0.2f;
		b_go = b_left || b_right || b_up;
		if (b_go)
		{
			b_down = false;
		}
		if (b_left)
		{
			if (horiz_speed > 0f && !b_sliding)
			{
				horiz_speed = 0f;
			}
			if (horiz_speed > 0f - mHeroMaxSpeed)
			{
				if (behavior == behaviors_t.BT_WALK)
				{
					horiz_speed -= mHeroAcceleration;
				}
				else
				{
					horiz_speed -= mHeroAcceleration * 2f;
				}
			}
		}
		if (b_right)
		{
			if (horiz_speed < 0f && !b_sliding)
			{
				horiz_speed = 0f;
			}
			if (horiz_speed < mHeroMaxSpeed)
			{
				if (behavior == behaviors_t.BT_WALK)
				{
					horiz_speed += mHeroAcceleration;
				}
				else
				{
					horiz_speed += mHeroAcceleration * 2f;
				}
			}
		}
		if (!b_left && !b_right && !b_sliding)
		{
			horiz_speed = 0f;
		}
		if (b_left && b_right && !b_sliding)
		{
			horiz_speed = 0f;
		}
		if (b_landing && hypershoetimer > 0f)
		{
			SCGlobals.supercowgame.SetCameraEffect(2);
		}
		b_landing = false;
		switch (behavior)
		{
		case behaviors_t.BT_WALK:
			Walk();
			break;
		case behaviors_t.BT_WOW:
			Wow();
			break;
		case behaviors_t.BT_STAND:
		case behaviors_t.BT_READ:
			Stand();
			break;
		case behaviors_t.BT_SHOCK:
			Shock();
			break;
		case behaviors_t.BT_JUMP:
			Jump();
			break;
		case behaviors_t.BT_FALL:
			Fall();
			break;
		case behaviors_t.BT_START:
			Start();
			break;
		case behaviors_t.BT_DEATH:
		case behaviors_t.BT_DEATHFALL:
			Death();
			break;
		case behaviors_t.BT_PUSH:
			Push();
			break;
		case behaviors_t.BT_VICTORY:
			Victory();
			break;
		case behaviors_t.BT_SITDOWN:
			Lookdown();
			break;
		case behaviors_t.BT_LIANA:
			Liana();
			break;
		}
		if (behavior != behaviors_t.BT_DEATH && behavior != behaviors_t.BT_DEATHFALL && behavior != behaviors_t.BT_VICTORY && behavior != behaviors_t.BT_WOW && behavior != behaviors_t.BT_LIANA)
		{
			CheckForObstactle();
		}
		if (position.y < SCGlobals.supercowgame.level_bottom - 25f && SCGlobals.hero.behavior != behaviors_t.BT_DEATHFALL && SCGlobals.hero.behavior != behaviors_t.BT_DEATH)
		{
			SetBehavior(behaviors_t.BT_DEATHFALL);
		}
		if (punishtime > 0f)
		{
			punishtime -= SCGlobals.physics_speed * (float)SCGlobals.ENGINE_FREQ;
		}
		if (megajumptimer > 0f)
		{
			megajumptimer -= SCGlobals.engine_frequency;
			if (megajumptimer < 0f)
			{
				megajumptimer = 0f;
			}
		}
		if (hypershoetimer > 0f)
		{
			hypershoetimer -= SCGlobals.engine_frequency;
			if (hypershoetimer < 0f)
			{
				hypershoetimer = 0f;
			}
		}
		if (firmnesstimer > 0f)
		{
			firmnesstimer -= SCGlobals.engine_frequency;
			if (firmnesstimer < 0f)
			{
				firmnesstimer = 0f;
			}
		}
		if (startharmlesstimer > 0f)
		{
			startharmlesstimer -= SCGlobals.engine_frequency;
			if (startharmlesstimer < 0f)
			{
				startharmlesstimer = 0f;
			}
		}
		if (lianatimer > 0f)
		{
			lianatimer -= SCGlobals.engine_frequency;
			if (lianatimer < 0f)
			{
				lianatimer = 0f;
			}
		}
		forcedmove.assign(0f, 0f, 0f);
		b_punished = false;
	}

	public override void CheckForObstactle()
	{
		b_firmsurface = false;
		herobridgey = -100000f;
		b_sliding = false;
		b_herosafe = true;
		float newy = 0f;
		move *= 0.12f * SCGlobals.physics_speed;
		vec2_t nextpos = new vec2_t(position + move);
		frect_t rc = default(frect_t);
		rc.assign(nextpos.x - monster_width / 2f, nextpos.y + monster_height, nextpos.x + monster_width / 2f, nextpos.y);
		frect_t frect_t2 = default(frect_t);
		vec2_t vec2_t2 = new vec2_t(position);
		vec2_t2.y += monster_height - 1.35f;
		int i;
		if (lianatimer <= 0f)
		{
			for (i = 0; i < SCGlobals.level.numlevelobjs; i++)
			{
				if (SCGlobals.level.ar_levelobjs[i].objID != 27)
				{
					continue;
				}
				CLianaObj cLianaObj = (CLianaObj)SCGlobals.level.ar_levelobjs[i];
				frect_t2.p0 = cLianaObj.startpos;
				frect_t2.p1 = cLianaObj.lianaend;
				frect_t2.normalize();
				if (frect_t2.isRectTouchRect(bound_rect))
				{
					vec2_t vec2_t3 = SuperCowHelper.closestPointOnLine(cLianaObj.startpos, cLianaObj.lianaend, vec2_t2);
					vec2_t vec2_t4 = SuperCowHelper.closestPointOnLine(cLianaObj.startpos, cLianaObj.prevlianaend, vec2_t2);
					float num = (vec2_t3 - vec2_t2).len();
					if ((vec2_t3.x >= vec2_t2.x && vec2_t4.x <= vec2_t2.x) || (vec2_t4.x >= vec2_t2.x && vec2_t3.x <= vec2_t2.x) || !(num > 1f))
					{
						SetBehavior(behaviors_t.BT_LIANA);
						lianay = ((vec2_t3 - cLianaObj.startpos).len() + (vec2_t4 - cLianaObj.startpos).len()) / 2f;
						p_lianaobj = cLianaObj;
						return;
					}
				}
			}
		}
		for (i = 0; i < SCGlobals.numobstacles; i++)
		{
			obstacle_t obstacle_t2 = SCGlobals.ar_obstacles[i];
			frect_t rect = obstacle_t2.rect;
			if (obstacle_t2.obsflag == obstacle_flags_t.RIGID_OBF)
			{
				if (!rect.isRectTouchRect(rc))
				{
					continue;
				}
				if (behavior == behaviors_t.BT_FALL && obstacle_t2.p_levelobj.objID == 8)
				{
					obstacle_t2.p_levelobj.Touch();
					SetBehavior(behaviors_t.BT_JUMP);
					jump_value = 8f;
					if (megajumptimer > 0f)
					{
						jump_value = 12f;
					}
					obstacle_t2.obsflag = obstacle_flags_t.TRANS_OBF;
					SCGlobals.ar_obstacles[i] = obstacle_t2;
					continue;
				}
				float num2 = rect.y1 - rc.y0;
				float value = rect.y0 - rc.y1;
				if (move.y > 0f && num2 <= 0f && num2 > -1.5f * move.y)
				{
					nextpos.y = position.y + (move.y + num2);
				}
				else if (Math.Abs(value) < 1f)
				{
					newy = rect.y0 - 0.2f;
					if (nextpos.y < newy)
					{
						nextpos.y += 0.5f;
						if (nextpos.y > newy)
						{
							nextpos.y = newy;
						}
					}
					b_firmsurface = true;
				}
				else
				{
					nextpos.x = position.x;
					horiz_speed = 0f;
				}
			}
			else if (obstacle_t2.b_triangle)
			{
				if (obstacle_t2.rect.isPointInTriangle(nextpos, ref newy))
				{
					newy = (nextpos.y = newy - 0.1f);
					b_firmsurface = true;
				}
			}
			else
			{
				if (nextpos.x < rect.x0 || nextpos.x > rect.x1 || nextpos.y > rect.y0 || nextpos.y < rect.y1)
				{
					continue;
				}
				if ((obstacle_t2.p_levelobj.type & 0x16) != 0)
				{
					b_herosafe = false;
				}
				if (obstacle_t2.obsflag == obstacle_flags_t.TRANS_OBF)
				{
					if (obstacle_t2.p_levelobj.objID == 10 && obstacle_t2.p_levelobj.b_switch != 0 && behavior == behaviors_t.BT_STAND)
					{
						exitpoint = obstacle_t2.p_levelobj.position;
						CheckForThrophyAfterLevel();
						SetBehavior(behaviors_t.BT_VICTORY);
					}
					if (obstacle_t2.p_levelobj.objID == 16 && behavior == behaviors_t.BT_STAND)
					{
						if (obstacle_t2.p_levelobj.int1 > 0 && obstacle_t2.p_levelobj.int1 < 20)
						{
							Sound.Addsound("whistle");
						}
						toiletcounter++;
						if (toiletcounter % 10 == 0 && obstacle_t2.p_levelobj.int1 < 20)
						{
							SetAnimStage(p_toilet, 100f);
							SetRotation(obstacle_t2.p_levelobj.b_invertobj ? 20f : (-90f));
							obstacle_t2.p_levelobj.int1++;
							SCGlobals.ar_obstacles[i] = obstacle_t2;
							SCGlobals.supercowgame.AddScore(obstacle_t2.pos + new vec2_t(0f, -1f), 10, obstacle_t2.p_levelobj.b_invertobj ? 3 : 2);
						}
					}
					obstacle_t2.p_levelobj.Touch();
					continue;
				}
				if (obstacle_t2.p_levelobj.objID != 2)
				{
					newy = rect.y0 - 0.2f;
					if (nextpos.y < newy)
					{
						nextpos.y += 0.5f;
						if (nextpos.y > newy)
						{
							nextpos.y = newy;
						}
					}
				}
				if (obstacle_t2.p_levelobj.objID == 17)
				{
					float num4 = fall_value / 2f;
					num4 = ((!b_down) ? (fall_value + ((megajumptimer > 0f) ? 1.2f : 1f)) : (fall_value / 4f));
					if (megajumptimer > 0f)
					{
						if (num4 > 20f)
						{
							num4 = 20f;
						}
					}
					else if (num4 > 17f)
					{
						num4 = 17f;
					}
					if (obstacle_t2.p_levelobj.objsubtype == 3 && behavior != behaviors_t.BT_JUMP)
					{
						SetBehavior(behaviors_t.BT_JUMP);
						jump_value = 18f;
						if (megajumptimer > 0f)
						{
							jump_value = 22f;
						}
						obstacle_t2.p_levelobj.obj_time = 300f;
						SCGlobals.ar_obstacles[i] = obstacle_t2;
						Sound.Addsound("spring");
					}
					else if (num4 > 5f && behavior == behaviors_t.BT_FALL)
					{
						SetBehavior(behaviors_t.BT_JUMP);
						jump_value = num4;
						obstacle_t2.p_levelobj.obj_time = 300f;
						SCGlobals.ar_obstacles[i] = obstacle_t2;
						if (obstacle_t2.p_levelobj.objsubtype == 2)
						{
							Sound.Addsound("matrac");
						}
						else
						{
							Sound.Addsound("trampo");
						}
					}
				}
				if (obstacle_t2.p_levelobj.objID == 24)
				{
					herobridgey = rect.y0 - 0.2f;
					if (behavior != behaviors_t.BT_JUMP && behavior != behaviors_t.BT_FALL)
					{
						nextpos.y = rect.y0 - ((CBridgeObj)obstacle_t2.p_levelobj).GetDeltaY();
					}
				}
				obstacle_t2.p_levelobj.Touch();
				b_firmsurface = true;
				if (obstacle_t2.p_levelobj.name == "ice1" || obstacle_t2.p_levelobj.name == "ice2")
				{
					b_sliding = true;
				}
			}
		}
		if (b_firmsurface && behavior == behaviors_t.BT_FALL)
		{
			b_landing = true;
			if (fall_value > 32f)
			{
				SCGlobals.supercowgame.SetCameraEffect(2);
				Sound.Addsound("landinghard");
			}
			else
			{
				Sound.Addsound("landing");
			}
			if (b_go)
			{
				SetBehavior(behaviors_t.BT_WALK);
			}
			else
			{
				SetBehavior(behaviors_t.BT_STAND);
			}
		}
		if (!b_firmsurface && behavior != behaviors_t.BT_JUMP)
		{
			if (b_firstfall)
			{
				SetBehavior(behaviors_t.BT_FALL);
			}
			else
			{
				b_firstfall = true;
			}
		}
		else
		{
			b_firstfall = false;
		}
		CheckAttackMonster(ref nextpos);
		int num5 = 0;
		for (; i < SCGlobals.nummonsters; i++)
		{
			CMonster cMonster = SCGlobals.ar_monsters[num5];
			if (cMonster.b_alive && cMonster.behavior != behaviors_t.BT_DEATH && (int)Math.Abs(cMonster.dist_to_main) < 4 && SCGlobals.supercowgame.b_bossfight)
			{
				b_herosafe = false;
			}
		}
		move.assign(nextpos.x - position.x, nextpos.y - position.y, 0f);
		if (behavior == behaviors_t.BT_WALK && move.y != 0f && !b_landing)
		{
			float num6 = Math.Abs(move.x);
			move.normalize();
			move *= num6;
		}
		CheckForWowFactor();
		move += forcedmove;
		position0 = position;
	}

	public void Wow()
	{
		move.assign(0f, 0f, 0f);
		behavior_time += (int)SCGlobals.engine_frequency;
		if (behavior_time < 500 && behavior_time + SCGlobals.engine_frequency >= 500)
		{
			Sound.Addsound("yahoo");
		}
		if (behavior_time > p_wow.iStageTime - 100)
		{
			SetBehavior(behaviors_t.BT_STAND);
		}
	}

	public void Lookdown()
	{
		move.assign(horiz_speed, 0f, 0f);
		horiz_speed *= 0.925f;
		if (b_down)
		{
			SCGlobals.supercowgame.cameradown += 1f;
			if (SCGlobals.supercowgame.cameradown > 5f)
			{
				SCGlobals.supercowgame.cameradown = 5f;
			}
		}
		else
		{
			SetBehavior(behaviors_t.BT_STAND);
		}
	}

	public override void Walk()
	{
		if (walksoundtimer == 0f)
		{
			string sound = ((!(hypershoetimer > 0f)) ? ("stephero" + (stepsoundindex + 1)) : ("hyperstephero" + (stepsoundindex + 1)));
			Sound.Addsound(sound);
			stepsoundindex = (stepsoundindex + CRand.irand(2) + 1) % 3;
		}
		walksoundtimer += SCGlobals.engine_frequency;
		if (walksoundtimer >= 380f / animspeed)
		{
			walksoundtimer = 0f;
		}
		move.assign(horiz_speed, 0f, 0f);
		if (!b_sliding)
		{
			SetRotation(new vec2_t(move.x, move.y).angle());
		}
		else
		{
			SetRotation(new vec2_t(b_left ? (-1f) : 1f, 0f).angle());
		}
		if (b_up)
		{
			SetBehavior(behaviors_t.BT_JUMP);
		}
		else if (b_down)
		{
			SetBehavior(behaviors_t.BT_SITDOWN);
		}
		else if (!b_go)
		{
			SetBehavior(behaviors_t.BT_STAND);
		}
	}

	public override void Stand()
	{
		move.assign(horiz_speed, 0f, 0f);
		horiz_speed *= 0.925f;
		if (behavior != behaviors_t.BT_READ)
		{
			if (behavior_time > 0)
			{
				behavior_time -= (int)SCGlobals.engine_frequency;
			}
			else if (p_stage == p_shock)
			{
				SetAnimStage(p_stand);
				SetRotation(45f);
				behavior_time = p_stand.iStageTime;
			}
			else
			{
				SetAnimStage(p_void[nvoidanim], 300f);
				SetRotation(90f);
				behavior_time = p_void[nvoidanim].iStageTime;
				nvoidanim = (nvoidanim + 1) % numvoidanim;
			}
		}
		else if (SCGlobals.tip_read_timer > 0)
		{
			SCGlobals.tip_read_timer -= (int)SCGlobals.engine_frequency;
			move.assign(0f, 0f, 0f);
			return;
		}
		if (b_up)
		{
			SetBehavior(behaviors_t.BT_JUMP);
		}
		else if (b_down)
		{
			SetBehavior(behaviors_t.BT_SITDOWN);
		}
		else if (b_go)
		{
			SetBehavior(behaviors_t.BT_WALK);
		}
	}

	public override void Shock()
	{
		SetRotation(90f);
		move.assign(0f, 0f, 0f);
		horiz_speed = 0f;
		if (behavior_time > 0)
		{
			behavior_time -= (int)SCGlobals.engine_frequency;
		}
		else
		{
			SetBehavior(behaviors_t.BT_STAND);
		}
	}

	public void Jump()
	{
		move.assign(horiz_speed, 0f, 0f);
		SetRotation(new vec2_t(move.x, move.y).angle());
		move.y = jump_value * 0.8f;
		if (jump_value >= 0f && jump_value - SCGlobals.physics_speed <= 0f)
		{
			SetBehavior(behaviors_t.BT_FALL);
		}
		else
		{
			jump_value -= SCGlobals.physics_speed;
		}
	}

	public override void Fall()
	{
		move.assign(horiz_speed, 0f, 0f);
		if (move.x != 0f)
		{
			SetRotation(new vec2_t(move.x, move.y).angle());
		}
		move.y = (0f - jump_value) * 0.8f;
		fall_value += SCGlobals.physics_speed;
		if (jump_value < 8f)
		{
			jump_value += SCGlobals.physics_speed;
		}
	}

	public void CheckAttackMonster(ref vec2_t nextpos)
	{
		if (behavior != behaviors_t.BT_FALL || nextpos.y >= position.y)
		{
			return;
		}
		frect_t rc = default(frect_t);
		frect_t frect_t2 = default(frect_t);
		rc.assign(nextpos.x - monster_width / 2f, position.y, nextpos.x + monster_width / 2f, nextpos.y);
		float num = 10f;
		if (hypershoetimer > 0f)
		{
			num *= 2f;
		}
		for (int i = 0; i < SCGlobals.nummonsters; i++)
		{
			CMonster cMonster = SCGlobals.ar_monsters[i];
			if (!cMonster.b_alive || cMonster.behavior == behaviors_t.BT_DEATH)
			{
				continue;
			}
			frect_t2 = cMonster.bound_rect;
			frect_t2.y1 = frect_t2.y0 - 0.3f;
			if (!frect_t2.isRectTouchRect(rc))
			{
				continue;
			}
			if (cMonster.monsterID == 6 && SCGlobals.hero.hypershoetimer <= 0f)
			{
				if (punishtime <= 0f)
				{
					SetBehavior(behaviors_t.BT_JUMP);
					jump_value = 0.5f * mHeroJumpValue;
				}
				continue;
			}
			if ((cMonster.monsterID == 9 || cMonster.monsterID == 7 || cMonster.monsterID == 15) && cMonster.physpunishtimer > 0f)
			{
				cMonster.Punish(num);
				continue;
			}
			if (cMonster.monsterID == 6)
			{
				num /= 2f;
			}
			cMonster.Punish(num);
			Sound.Addsound("killmonster");
			SetBehavior(behaviors_t.BT_JUMP);
			monstercombo++;
			if (hypershoetimer > 0f)
			{
				SCGlobals.supercowgame.SetCameraEffect(2);
			}
		}
	}

	public void CheckForThrophyAfterLevel()
	{
		if (SCGlobals.levelstat.monsters >= 10 && SCGlobals.b_trophy4was_not_punished)
		{
			SCGlobals.supercowgame.AddGameTrophy(8);
		}
	}

	public void CheckForWowFactor()
	{
		if (!b_firmsurface || behavior == behaviors_t.BT_FALL)
		{
			return;
		}
		if (numbonuses >= 4)
		{
			int num = ((numbonuses - 4 > 2) ? 2 : (numbonuses - 4));
			Text.AddLInfoText(wstrstuff.GetStrDelta("praise1", num));
			string sound = "praise" + (num + 1);
			Sound.Addsound(sound);
		}
		numbonuses = 0;
		if (monstercombo >= 2)
		{
			Text.RemovEventText();
			int num = ((monstercombo - 2 > 3) ? 3 : (monstercombo - 2));
			string str = wstrstuff.GetStrDelta("combo1", num) + " x " + monstercombo;
			Text.AddEventText(str, new vec2_t(0f, -120f), 25f, 2200f);
			int addscore = 50 * monstercombo * monstercombo;
			str = "+ " + addscore;
			Text.AddEventText(str, new vec2_t(0f, -190f), 30f, 2200f);
			SCGlobals.supercowgame.AddScore(new vec2_t(0f, -100000f), addscore);
			string sound2 = "praise" + ((num + 1 > 3) ? 3 : (num + 1));
			Sound.Addsound(sound2);
			if (monstercombo >= 5)
			{
				SCGlobals.supercowgame.AddGameTrophy(3);
			}
			else if (monstercombo >= 3)
			{
				SCGlobals.supercowgame.AddGameTrophy(0);
			}
		}
		monstercombo = 0;
	}

	public void Victory()
	{
		behavior_time -= (int)SCGlobals.engine_frequency;
		move.assign(0f, 0f, 0f);
		if (behavior_time < 0)
		{
			coord_z -= 0.135f;
			move.X = (exitpoint.X - position.X) / 10f;
		}
		if (behavior_time < 0 && behavior_time + (int)SCGlobals.engine_frequency >= 0)
		{
			SetAnimStage(p_walk2);
			SetRotation(-90f, 1000f);
		}
	}

	public override void Death()
	{
		if (behavior != behaviors_t.BT_DEATHFALL)
		{
			move.assign(0f, 0f, 0f);
			int num = 0;
			float num2 = 1f - (float)behavior_time / 2000f;
			float num3 = num2 * (-0.2f + num2) * 5f;
			num++;
			if (num % 7 == 0)
			{
				Particles.AddDynamicText(position, ar_herowords[2], 1);
			}
			move.assign(num2 * 0.1f, 0f - num3, 0f);
			move *= SCGlobals.physics_speed;
		}
		behavior_time -= (int)SCGlobals.engine_frequency;
		if (behavior_time < (int)SCGlobals.engine_frequency)
		{
			animspeed = 0f;
			SCGlobals.supercowgame.b_maindeath = true;
		}
	}

	public void UseThing()
	{
	}

	public void Push()
	{
	}

	public void Start()
	{
		move.assign(0f, 0f, 0f);
		SetRotation(100f);
		behavior_time -= (int)SCGlobals.engine_frequency;
		Particles.AddDynamicParticle(position + new vec3_t(0f, CRand.prand(3f), 0f), 9, 1, -1);
		if (behavior_time < 0)
		{
			SetBehavior(behaviors_t.BT_STAND);
		}
		if (b_go)
		{
			SetBehavior(behaviors_t.BT_WALK);
		}
	}

	public void Liana()
	{
		if (p_lianaobj != null)
		{
			CLianaObj cLianaObj = p_lianaobj;
			cLianaObj.b_switch = 1;
			vec2_t vec2_t2 = new vec2_t(position);
			vec2_t2.y += monster_height - 1.35f;
			frect_t frect_t2 = new frect_t(cLianaObj.startpos, cLianaObj.lianaend);
			SuperCowHelper.closestPointOnLine(frect_t2.p0, frect_t2.p1, vec2_t2);
			vec2_t vec2_t3 = cLianaObj.lianaend - cLianaObj.startpos;
			vec2_t3.normalize();
			vec2_t vec2_t4 = vec2_t3 * lianay + cLianaObj.startpos;
			move = (vec2_t4 - vec2_t2).v3t();
			if (lianay < cLianaObj.web_radius)
			{
				lianay += 0.3f;
			}
			if (lianay > cLianaObj.web_radius)
			{
				lianay = cLianaObj.web_radius;
			}
			if (b_up)
			{
				SetBehavior(behaviors_t.BT_JUMP);
			}
			if (b_down)
			{
				SetBehavior(behaviors_t.BT_FALL);
			}
			if (b_left)
			{
				cLianaObj.web_amplitude += (0f - (float)Math.Sign(move.x)) * 0.009f;
			}
			if (b_right)
			{
				cLianaObj.web_amplitude += (float)Math.Sign(move.x) * 0.009f;
			}
			if (cLianaObj.web_amplitude > 0.9424779f)
			{
				cLianaObj.web_amplitude = 0.9424779f;
			}
			if (cLianaObj.web_amplitude < (float)Math.PI / 100f)
			{
				cLianaObj.web_amplitude = (float)Math.PI / 100f;
			}
			lianatimer = 300f;
		}
	}

	public override void CalcMath()
	{
		base.CalcMath();
	}

	public override void Calculate()
	{
		leye_matrix.load_identity();
		if (behavior != behaviors_t.BT_SITDOWN)
		{
			btim -= SCGlobals.ftickperframe;
			if (btim < 0f)
			{
				btim = CRand.prand(1700f);
				yeye = CRand.frand(25f);
				zeye = CRand.frand(17f) + 5f;
				yeyed = 80f + CRand.prand(80f);
				zeyed = 80f + CRand.prand(80f);
			}
			yeyec += (yeye - yeyec) / yeyed * SCGlobals.ftickperframe;
			zeyec += (zeye - zeyec) / zeyed * SCGlobals.ftickperframe;
			leye_matrix.yrot(yeyec);
			leye_matrix.zrot_delta(zeyec);
		}
		reye_matrix = leye_matrix;
		base.Calculate();
		if (punishsoundtimer > 0f)
		{
			punishsoundtimer -= SCGlobals.ftickperframe;
		}
	}

	public bool IsPunishable()
	{
		if (firmnesstimer > 0f)
		{
			return false;
		}
		if (punishtime > 0f)
		{
			return false;
		}
		if (behavior == behaviors_t.BT_DEATH || behavior == behaviors_t.BT_VICTORY)
		{
			return false;
		}
		return true;
	}

	public override void Punish(float power)
	{
		if (punishtime > 0f || firmnesstimer > 0f || startharmlesstimer > 0f || behavior == behaviors_t.BT_DEATH || behavior == behaviors_t.BT_DEATHFALL || behavior == behaviors_t.BT_VICTORY)
		{
			return;
		}
		if (b_setsparks)
		{
			sparkstime = 2000f;
			b_setsparks = false;
		}
		Particles.AddDynamicParticle(position, 10, 20, -1);
		b_punished = true;
		b_eyesout = true;
		vec3_t vec3_t2 = default(vec3_t);
		if (behavior == behaviors_t.BT_STAND)
		{
			behavior_time = p_shock.iStageTime;
			SetAnimStage(p_shock, 50f);
		}
		vec3_t2.assign(0f, 5f, 0f);
		string str = ar_herowords[punishsoundindex % 2];
		Particles.AddDynamicText(position + vec3_t2, str, 1);
		string sound = "hurt" + (punishsoundindex + 1);
		punishsoundindex = (punishsoundindex + 1) % 7;
		Sound.Addsound(sound);
		punishsoundtimer = 400f;
		curlife -= power;
		if (curlife <= 0f)
		{
			curlife = 0f;
			SetBehavior(behaviors_t.BT_DEATH);
			return;
		}
		if (punishtime <= 0f)
		{
			punishtime = 3000f;
		}
		SCGlobals.b_trophy4was_not_punished = false;
	}

	public int AddHealth(float health)
	{
		curlife += health;
		int result = 0;
		if (curlife > maxlife)
		{
			result = (int)(curlife - maxlife);
			curlife = maxlife;
		}
		return result;
	}

	public void DrawArrow()
	{
		if (behavior != behaviors_t.BT_DEATH)
		{
			herocursorx0 += (herocursorx - herocursorx0) * 0.01f * SCGlobals.ftickperframe;
			if (!(Math.Abs(herocursorx0) < herocursor_dead_zone))
			{
				CowRender.BindTexture(SCGlobals.p_heroarrow);
				dcolor_t c = new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				c.a = (byte)(255f * (Math.Abs(herocursorx0) - herocursor_dead_zone) / (herocursor_full_zone - herocursor_dead_zone));
				CowRender.DrawColorQuad2DDynBuffer(new vec2_t(SCGlobals.hero.curpos.x + herocursorx0 * 1.5f, SCGlobals.hero.curpos.z + 2f), 1f, 1f, (herocursorx < 0f) ? new frect_t(0f, 0f, 1f, 1f) : new frect_t(1f, 0f, 0f, 1f), c, 0f);
			}
		}
	}

	public override void Draw()
	{
		if (SCGlobals.bInTrophyRoom)
		{
			return;
		}
		if (sparkstime > 0f)
		{
			float num = 1f - sparkstime / 2000f;
			vec3_t vec3_t2 = default(vec3_t);
			float num2 = 1f - num;
			float num3 = num * 12f * 3.1415925f;
			byte b = (byte)(255f * num2);
			for (int i = 0; i < 12; i++)
			{
				vec3_t2.assign((float)Math.Sin(num3 + (float)i), 0f, (float)Math.Cos(num3 + (float)i));
				vec3_t2 *= 0.6f + 0.03f * (float)i;
				vec3_t2.Y = 3.8f + 0.1f * (float)i;
				if (horiz_speed != 0f)
				{
					vec3_t2.X += ((horiz_speed > 0f) ? 1f : (-1f));
				}
				Particles.AddParticle(position + move * SCGlobals.frameinterp + vec3_t2, 10f * (float)i, 0.3f, new dcolor_t(b, b, b, byte.MaxValue), 0, 4);
			}
			sparkstime -= SCGlobals.ftickperframe;
		}
		if (starttime > 0)
		{
			starttime -= (int)SCGlobals.ftickperframe;
		}
		if (starttime > 0)
		{
			float num4 = 1 - starttime / 2000;
			num4 *= 2f;
			if (num4 > 1f)
			{
				num4 = 1f;
			}
			material_t matcommon = SCGlobals.matcommon;
			matcommon.Diffuse.a = num4;
			CowRender.SetBlend(0);
			CowRender.EnableRS(4u);
			CowRender.SetMaterial(matcommon);
		}
		if (punishtime > 0f)
		{
			material_t matcommon = SCGlobals.matcommon;
			float num5 = 1f - (punishtime - SCGlobals.frameinterp * (float)SCGlobals.engine_frequency) / 3000f;
			if (num5 < 0f)
			{
				num5 = 0f;
			}
			float num6 = 1f - Math.Abs(1f + (float)Math.Cos((float)Math.PI * 30f * num5)) * 0.5f * (1f - 0.7f * num5);
			matcommon.Diffuse.assign(1f, 1f, 1f, 0.3f * num6 + 0.7f);
			num5 *= 3f;
			if (num5 > 1f)
			{
				num5 = 1f;
			}
			Math.Sin((double)(num5 * 5f) * 3.1415926);
			scalematrix.scale(fscale, fscale, fscale);
			CowRender.SetBlend(0);
			CowRender.EnableRS(4u);
			CowRender.SetMaterial(matcommon);
		}
		if (firmnesstimer > 0f)
		{
			float num7 = 1f;
			if (firmnesstimer < 2000f)
			{
				num7 = firmnesstimer / 2000f;
			}
			material_t matcommon = SCGlobals.matcommon;
			float num8 = 0.5f * (float)Math.Cos(Math.PI * (double)SCGlobals.global_timer * 0.00800000037997961) * 0.5f;
			matcommon.Diffuse.assign(1f, 1f, 1f, 0.4f + num8 * 0.6f + (1f - num7) * (1f - num8) * 0.6f);
			CowRender.SetBlend(0);
			CowRender.EnableRS(4u);
			CowRender.SetMaterial(matcommon);
		}
		else if (startharmlesstimer > 0f)
		{
			float num9 = 1f;
			if (startharmlesstimer < 2000f)
			{
				num9 = startharmlesstimer / 2000f;
			}
			material_t matcommon = SCGlobals.matcommon;
			float num10 = 0.5f + (float)Math.Cos(Math.PI * (double)SCGlobals.global_timer * 0.0008) * 0.5f;
			matcommon.Diffuse.assign(1f, 1f, 1f, 0.4f + num10 * 0.6f + (1f - num9) * (1f - num10) * 0.6f);
			CowRender.SetBlend(0);
			CowRender.EnableRS(4u);
			CowRender.SetMaterial(matcommon);
		}
		base.Draw();
		CowRender.SetMaterial(SCGlobals.matcommon);
		CowRender.DisableRS(4u);
		scalematrix.scale(fscale, fscale, fscale);
	}

	public virtual void CheckForBonus()
	{
	}

	public override void SetBehavior(behaviors_t b)
	{
		if (behavior == b)
		{
			return;
		}
		switch (b)
		{
		case behaviors_t.BT_READ:
			SetRotation(-80f);
			SetAnimStage(p_readanim);
			break;
		case behaviors_t.BT_STAND:
			SetRotation(45f);
			SetAnimStage(p_stand);
			behavior_time = 5000;
			break;
		case behaviors_t.BT_WALK:
			walksoundtimer = 0f;
			SetAnimStage(p_walk);
			break;
		case behaviors_t.BT_JUMP:
		{
			SCGlobals.supercowgame.b_jumpkey = false;
			string sound = "herojump" + (jumpsoundindex + 1);
			jumpsoundindex = (jumpsoundindex + 1) % 3;
			Sound.Addsound(sound);
			walksoundtimer = 0f;
			jump_value = mHeroJumpValue;
			if (megajumptimer > 0f)
			{
				jump_value = mHeroMegaJumpValue;
			}
			jump_dir = 1f;
			SetAnimStage(p_jump_up);
			behavior_time = p_jump_up.iStageTime;
			break;
		}
		case behaviors_t.BT_FALL:
			walksoundtimer = 0f;
			fall_value = 0f;
			jump_value = 0f;
			SetAnimStage(p_jump_down, 200f);
			behavior_time = p_jump_down.iStageTime;
			break;
		case behaviors_t.BT_PUSH:
			move.assign(0f, 0f, 0f);
			SetAnimStage(p_push);
			break;
		case behaviors_t.BT_SHOCK:
			SetAnimStage(p_shock, 50f);
			behavior_time = p_shock.iStageTime;
			break;
		case behaviors_t.BT_DEATH:
		{
			string sound = "death" + (deathphraseconter + 1);
			Sound.Addsound(sound);
			deathphraseconter = (deathphraseconter + 1) % 6;
			behavior_time = 2000;
			SetAnimStage(p_death);
			if (SCGlobals.numlives != 0)
			{
				Text.AddFunnyText(wstrstuff.GetStr("loselife"), new vec2_t(0f, 30f), 0, 25f, 4000f);
			}
			break;
		}
		case behaviors_t.BT_DEATHFALL:
			behavior_time = 2000;
			if (SCGlobals.numlives != 0)
			{
				Text.AddFunnyText(wstrstuff.GetStr("loselife"), new vec2_t(0f, 30f), 0, 25f, 4000f);
			}
			break;
		case behaviors_t.BT_SITDOWN:
			SetAnimStage(p_sitdown, 500f);
			SetRotation(70f + (float)CRand.irand(40));
			break;
		case behaviors_t.BT_START:
			starttime = 2000;
			SetAnimStage(p_animstart);
			behavior_time = p_animstart.iStageTime;
			SetRotation(80f);
			break;
		case behaviors_t.BT_WOW:
			SetAnimStage(p_wow);
			behavior_time = 0;
			SetRotation(0f, 100f);
			break;
		case behaviors_t.BT_LIANA:
			SetAnimStage(p_lianaanim);
			SetRotation(-90f);
			break;
		case behaviors_t.BT_VICTORY:
			if (behavior == behaviors_t.BT_WOW || SCGlobals.trophytimer > 0f)
			{
				return;
			}
			Sound.Addsound("levelcomplete");
			Sound.Addsound("applause");
			SetAnimStage(p_poklon);
			SetRotation(90f);
			behavior_time = p_poklon.iStageTime;
			Text.AddFunnyText(wstrstuff.GetStr("levelcomplete"), new vec2_t(0f, 0f), 0, 30f, 6000f);
			SCGlobals.supercowgame.SetCameraEffect(1);
			break;
		}
		behavior = b;
	}

	public void VibrateJoy(uint theID)
	{
	}
}
