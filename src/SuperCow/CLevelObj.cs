using System;
using System.Globalization;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public class CLevelObj
{
	public enum LOT
	{
		LOT_scale = 1,
		LOT_dynamic = 2,
		LOT_movable = 4,
		LOT_bonus = 8,
		LOT_unsafe = 0x10
	}

	public int myid;

	public string name;

	public vec2_t tippos = default(vec2_t);

	public vec3_t pl_p1 = default(vec3_t);

	public vec3_t pl_p2 = default(vec3_t);

	public vec3_t pl_p3 = default(vec3_t);

	public vec3_t pl_p4 = default(vec3_t);

	public vec2_t pl_t1 = default(vec2_t);

	public vec2_t pl_t2 = default(vec2_t);

	public vec2_t pl_t3 = default(vec2_t);

	public vec2_t pl_t4 = default(vec2_t);

	public bool bDrawn;

	public frect_t rect = default(frect_t);

	public int draw_plane;

	public bool b_invertobj;

	public float rotation;

	public vec2_t endpos = default(vec2_t);

	public vec2_t position = default(vec2_t);

	public float sizex;

	public float sizey;

	public vec2_t position0 = default(vec2_t);

	public frect_t rect0 = default(frect_t);

	public float rotation0;

	public int game_object_index;

	public texture_t p_texture;

	public CGeomObj p_cgo;

	public CSprite p_sprite;

	public float obj_time;

	public float obj_time2;

	public float float1;

	public float frandom1;

	public float frandom2;

	public int iauxvar;

	public int int1;

	public bool b_selected;

	public bool b_offscreen;

	public byte b_switch;

	public byte b_active;

	public vec2_t startpos = default(vec2_t);

	public float movedist;

	public obstacle_t p_obstaclez = default(obstacle_t);

	public int p_obstacle_idx = -1;

	public CLevelObj[] ar_linkedobjs = new CLevelObj[5];

	public int numlinks;

	public frect_t frame_rect = default(frect_t);

	public vec2_t frame_pos = default(vec2_t);

	public vec2_t move = default(vec2_t);

	public int objsubtype;

	public short objID;

	public byte type;

	public string extratextparam;

	public CLevelObj()
	{
		extratextparam = "";
	}

	public virtual void Init()
	{
		b_switch = 0;
		b_active = byte.MaxValue;
		b_selected = false;
		iauxvar = 0;
		obj_time = 0f;
		obj_time2 = 0f;
		float1 = 0f;
		int1 = 0;
		if (objID != 19)
		{
			position = startpos;
			rect.assign(position, rect.sizex(), rect.sizey());
		}
		frame_rect = rect;
		frandom1 = CRand.frand(1f);
		frandom2 = CRand.frand(1f);
		movedist = (endpos - startpos).len();
		move.assign(0f, 0f);
		p_obstacle_idx = -1;
		numlinks = 0;
		rotation0 = 0f;
	}

	public bool IsObjFromTask(int ntask)
	{
		switch (ntask)
		{
		case 0:
			return false;
		case 1:
			if (name == "key")
			{
				return true;
			}
			break;
		case 2:
			if (objID == 6)
			{
				return true;
			}
			break;
		case 3:
			if (objID == 31 || objID == 32)
			{
				return true;
			}
			break;
		case 4:
			if (objID == 8)
			{
				return true;
			}
			break;
		}
		return false;
	}

	public void SetExtraTextParam(string param)
	{
		if (param == null || param[0] == '\0')
		{
			if (extratextparam.Length > 0)
			{
				extratextparam = "";
			}
			return;
		}
		if (extratextparam.Length > 0)
		{
			extratextparam = "";
		}
		extratextparam = param;
	}

	public void AddLinkedObjs(CLevelObj p_obj)
	{
		if (numlinks < 5)
		{
			ar_linkedobjs[numlinks++] = p_obj;
		}
	}

	public void AddLinkedPers(CMonster p_pers)
	{
		CDynPlatformObj cDynPlatformObj = (CDynPlatformObj)this;
		if (cDynPlatformObj.nummonsterlinks < 5)
		{
			cDynPlatformObj.ar_monsterlinks[cDynPlatformObj.nummonsterlinks++] = p_pers;
		}
	}

	public void GetSaveData(commonsaveobj_t saveobj)
	{
		saveobj.rect = rect.Clone();
		saveobj.draw_plane = draw_plane;
		saveobj.b_invertobj = b_invertobj;
		saveobj.rotation = rotation;
		saveobj.endpos = endpos;
		if (extratextparam.Length > 0)
		{
			saveobj.str = extratextparam;
		}
		else
		{
			saveobj.str = "";
		}
	}

	public void SetSaveData(commonsaveobj_t saveobj)
	{
		rect = saveobj.rect.Clone();
		draw_plane = saveobj.draw_plane;
		b_invertobj = saveobj.b_invertobj;
		rotation = saveobj.rotation;
		endpos = saveobj.endpos;
		position = saveobj.rect.pos();
		position0 = saveobj.rect.pos();
		sizex = saveobj.rect.sizex();
		sizey = saveobj.rect.sizey();
		if (saveobj.str.Length > 0)
		{
			SetExtraTextParam(saveobj.str);
		}
		else if (extratextparam.Length > 0)
		{
			extratextparam = "";
		}
	}

	public void SetPos(vec2_t pos)
	{
		if (SCGlobals.b_snaptogrid)
		{
			pos.assign((float)Math.Round(pos.x), (float)Math.Round(pos.y));
		}
		position = pos;
		sizex = (float)p_texture.width / 32f * SCGlobals.le_plane_scale;
		sizey = (float)p_texture.height / 32f * SCGlobals.le_plane_scale;
		if (p_cgo != null)
		{
			float radius = p_cgo.p_meshes[0].bsphere.radius;
			if (sizex < radius)
			{
				sizex = radius;
			}
			if (sizey < radius)
			{
				sizey = radius;
			}
		}
		rect.assign(position, sizex, sizey);
		draw_plane = SCGlobals.draw_plane_index;
	}

	public void SetRectangle(frect_t new_rect)
	{
		frect_t frect_t2 = default(frect_t);
		frect_t2 = new_rect;
		vec2_t p = default(vec2_t);
		vec2_t p2 = default(vec2_t);
		if (objID == 19)
		{
			endpos = new_rect.p0;
		}
		if (objID == 25)
		{
			endpos = new_rect.p1;
		}
		if (objID == 11 || objID == 26)
		{
			float num = (float)p_texture.width / 32f;
			if (frect_t2.y0 < frect_t2.y1)
			{
				p.y = (float)Math.Floor(frect_t2.y0);
				p2.y = p.y + (float)Math.Ceiling(Math.Abs(frect_t2.y0 - frect_t2.y1) / num) * num;
			}
			else
			{
				p2.y = (float)Math.Ceiling(frect_t2.y0);
				p.y = p2.y - (float)Math.Ceiling(Math.Abs(frect_t2.y0 - frect_t2.y1) / num) * num;
			}
			if (SCGlobals.le_state == 3)
			{
				if (frect_t2.x0 > frect_t2.x1)
				{
					p.x = frect_t2.x0;
					p2.x = frect_t2.x1;
				}
				else
				{
					p2.x = frect_t2.x0;
					p.x = frect_t2.x1;
				}
			}
			else if (frect_t2.x0 > frect_t2.x1)
			{
				p.x = (float)Math.Ceiling(frect_t2.x0);
				p2.x = p.x - num;
			}
			else
			{
				p2.x = (float)Math.Floor(frect_t2.x0);
				p.x = p2.x + num;
			}
		}
		else if (objID == 0 || objID == 1 || objID == 5 || objID == 2 || objID == 25)
		{
			float num = (float)p_texture.height / 32f;
			if (frect_t2.x0 < frect_t2.x1)
			{
				p.x = (float)Math.Floor(frect_t2.x0);
				p2.x = p.x + (float)Math.Ceiling(frect_t2.sizex() / num) * num;
			}
			else
			{
				p2.x = (float)Math.Ceiling(frect_t2.x0);
				p.x = p2.x - (float)Math.Ceiling(frect_t2.sizex() / num) * num;
			}
			if (SCGlobals.le_state == 3)
			{
				if (frect_t2.y0 > frect_t2.y1)
				{
					p.y = frect_t2.y0;
					p2.y = frect_t2.y1;
				}
				else
				{
					p2.y = frect_t2.y0;
					p.y = frect_t2.y1;
				}
			}
			else if (frect_t2.y0 > frect_t2.y1)
			{
				p.y = (float)Math.Ceiling(frect_t2.y0);
				p2.y = p.y - num;
			}
			else
			{
				p2.y = (float)Math.Floor(frect_t2.y0);
				p.y = p2.y + num;
			}
		}
		else
		{
			p = new_rect.p0;
			p2 = new_rect.p1;
		}
		rect.p0 = p;
		rect.p1 = p2;
		rect.normalize();
		sizex = rect.sizex();
		sizey = rect.sizey();
		position = rect.pos();
		startpos = rect.pos();
		CowRender.DisableRS(4u);
		CowRender.DrawEmptyRect(rect);
		CowRender.EnableRS(4u);
		draw_plane = SCGlobals.draw_plane_index;
	}

	public virtual void Touch()
	{
		if (objID != 8)
		{
			return;
		}
		if (b_switch == 0)
		{
			SCGlobals.levelstat.garbage++;
			if (objsubtype == 1)
			{
				Sound.Addsound("garbage2");
			}
			else
			{
				Sound.Addsound("garbage1");
			}
		}
		b_switch = 1;
	}

	public virtual void Physics()
	{
		if ((type & 4) != 0)
		{
			if (p_obstacle_idx != -1)
			{
				SCGlobals.ar_obstacles[p_obstacle_idx].rect.translate(move);
				SCGlobals.ar_obstacles[p_obstacle_idx].pos = position;
			}
			rect.x0 = (0f - sizex) / 2f + position.x;
			rect.y0 = sizey / 2f + position.y;
			rect.x1 = sizex / 2f + position.x;
			rect.y1 = (0f - sizey) / 2f + position.y;
			position += move;
		}
		if (b_offscreen)
		{
			return;
		}
		if (objID == 13)
		{
			frect_t frect_t2 = rect;
			frect_t2.y0 += 2f;
			if (frect_t2.isPointInRect(SCGlobals.hero.position))
			{
				b_switch = 1;
				iauxvar = 1;
				if (obj_time == 0f)
				{
					Sound.Addsound("rake");
				}
				if (obj_time < 700f)
				{
					obj_time += SCGlobals.physics_speed * 150f;
					if (obj_time > 700f)
					{
						if (SCGlobals.hero.punishtime <= 0f)
						{
							SCGlobals.hero.b_setsparks = true;
							SCGlobals.hero.Punish(25f);
						}
						obj_time = 1000f;
					}
				}
				else
				{
					obj_time = 1000f;
					iauxvar = 2;
				}
			}
			else
			{
				iauxvar = -1;
				if (obj_time > 0f)
				{
					obj_time -= SCGlobals.physics_speed * 60f;
					return;
				}
				obj_time = 0f;
				iauxvar = 0;
			}
		}
		else if (myid == 4)
		{
			if (rect.isPointInRect(SCGlobals.hero.position))
			{
				SCGlobals.hero.Punish(25f);
			}
		}
		else if (myid == 5 || myid == 6)
		{
			if (rect.isPointInRect(SCGlobals.hero.position))
			{
				SCGlobals.hero.Punish(110f);
			}
		}
		else if (myid == 7)
		{
			if (rect.isPointInRect(SCGlobals.hero.position) && SCGlobals.hero.behavior != behaviors_t.BT_DEATHFALL && SCGlobals.hero.behavior != behaviors_t.BT_DEATH)
			{
				SCGlobals.hero.SetBehavior(behaviors_t.BT_DEATHFALL);
			}
		}
		else if (myid == 8)
		{
			if (b_active != 0 && rect.isRectTouchRect(SCGlobals.hero.bound_rect) && SCGlobals.hero.behavior != behaviors_t.BT_DEATHFALL && SCGlobals.hero.behavior != behaviors_t.BT_DEATH)
			{
				b_active = 0;
				Sound.Addsound("secretfound");
				Text.AddEventText(wstrstuff.GetStr("secretfound"), new vec2_t(0f, 0f), 30f, 3000f);
				SCGlobals.levelstat.secrets++;
			}
		}
		else
		{
			if (objID == 16)
			{
				return;
			}
			if (myid == 9 || myid == 10)
			{
				if (b_switch == 0 && rect.isRectTouchRect(SCGlobals.hero.bound_rect))
				{
					SCGlobals.hero.VibrateJoy(2u);
					SCGlobals.hero.safeposition.assign(position.x - sizex / 2f, rect.y1, 0f);
					b_switch = 1;
					if (myid == 9)
					{
						Sound.Addsound("checkpont");
					}
					Sound.Addsound("checkpont2");
				}
			}
			else if (myid == 11 && b_switch == 0 && rect.isRectTouchRect(SCGlobals.hero.bound_rect))
			{
				b_switch = 1;
				Sound.Addsound("pan");
			}
		}
	}

	public void Calculate()
	{
		if (b_active == 0)
		{
			b_offscreen = true;
			return;
		}
		if (myid == 3)
		{
			CalulateWaterfall();
		}
		frame_rect = rect;
		frame_pos = position;
		if ((type & 1) == 0)
		{
			frame_rect.translate(move * SCGlobals.frameinterp);
			frame_pos = position + move * SCGlobals.frameinterp;
		}
		frect_t frect_t2 = frame_rect;
		if (rotation != 0f)
		{
			if (sizex > sizey)
			{
				frect_t2.assign(frame_pos, sizex, sizex);
			}
			else
			{
				frect_t2.assign(frame_pos, sizey, sizey);
			}
		}
		if (!frect_t2.isRectTouchRect(SCGlobals.viewport_rect))
		{
			b_offscreen = true;
			return;
		}
		b_offscreen = false;
		if (extratextparam.Length <= 0 || myid != 12)
		{
			return;
		}
		frect_t frect_t3 = rect;
		frect_t3.x0 -= 2f;
		frect_t3.x1 += 2f;
		if (frect_t3.isRectTouchRect(SCGlobals.hero.bound_rect))
		{
			string str = wstrstuff.GetStr(extratextparam);
			if (str.Length > 0)
			{
				if (int1 == 0)
				{
					tippos = SCGlobals.camerapos + SCGlobals.hero.position.v2t();
					Sound.Addsound("tip");
					int1 = 1;
				}
				if (!SCGlobals.b_gamemenu && (SCGlobals.hero.behavior == behaviors_t.BT_STAND || SCGlobals.hero.behavior == behaviors_t.BT_WALK))
				{
					Balloon.SetBalloon(tippos, str, 300f, b_arrow: false, 90f, b_grow: true);
				}
				if (rect.isRectTouchRect(SCGlobals.hero.bound_rect) && b_switch == 0 && (SCGlobals.hero.behavior == behaviors_t.BT_STAND || SCGlobals.hero.behavior == behaviors_t.BT_WALK))
				{
					b_switch = 1;
					SCGlobals.tip_read_timer = 1500;
					SCGlobals.hero.SetBehavior(behaviors_t.BT_READ);
				}
				else if (SCGlobals.hero.behavior == behaviors_t.BT_STAND)
				{
					SCGlobals.hero.SetBehavior(behaviors_t.BT_READ);
				}
				if (rect.isRectTouchRect(SCGlobals.hero.bound_rect))
				{
					Balloon.visible = true;
				}
			}
		}
		else
		{
			if (SCGlobals.hero.behavior != behaviors_t.BT_READ)
			{
				Balloon.visible = false;
			}
			int1 = 0;
			SCGlobals.tip_read_timer = 0;
		}
	}

	public virtual void DrawStatic(dcolor_t dc)
	{
		if (name.Contains("dynplat") || name.Contains("guillotine") || name.Contains("barrel") || name.Contains("elevator") || name.Contains("trough"))
		{
			return;
		}
		CowRender.SetBlend(0);
		CowRender.EnableRS(5u);
		CowRender.EnableRS(4u);
		if (name == "chasm" || name == "lantern" || name == "waterfall" || name == "secret" || name == "deadzone" || name == "Light01" || (name == "tablet" && !SCGlobals.b_le_active) || name == "checkpoint" || name == "checkpoint2")
		{
			return;
		}
		switch (objID)
		{
		case 0:
		case 5:
			bDrawn = true;
			DrawPlatforms();
			return;
		case 11:
			bDrawn = true;
			DrawPosts();
			return;
		case 4:
		case 6:
		case 8:
		case 10:
		case 12:
		case 13:
		case 14:
		case 17:
		case 18:
		case 19:
		case 20:
		case 21:
			return;
		}
		if (!name.Contains("dynplat") && !name.Contains("guillotine") && !name.Contains("barrel"))
		{
			frect_t tc = (b_invertobj ? SCGlobals.tc_inverse : SCGlobals.tc_normal);
			bDrawn = true;
			if (rotation == 0f)
			{
				CowRender.BindTexture(p_texture);
				CowRender.DrawColorQuad2DStaticBuffer(frame_rect, tc, dc, 0f);
				CowRender.ObjectPerFrame++;
			}
			else
			{
				CowRender.BindTexture(p_texture);
				CowRender.DrawRotatedColorQuad2DStaticBuffer(position, position, rotation, sizex, sizey, tc, dc, 0f);
				CowRender.ObjectPerFrame++;
			}
		}
	}

	public virtual void Draw(dcolor_t dc)
	{
		if ((bDrawn || b_offscreen) && myid != 4)
		{
			return;
		}
		CowRender.SetBlend(0);
		CowRender.EnableRS(5u);
		CowRender.EnableRS(4u);
		frect_t frect_t2 = (b_invertobj ? SCGlobals.tc_inverse : SCGlobals.tc_normal);
		if (myid == 1)
		{
			DrawChasm();
		}
		else
		{
			if (myid == 2)
			{
				return;
			}
			if (myid == 3)
			{
				DrawWaterfall();
			}
			else
			{
				if (myid == 8 || myid == 7)
				{
					return;
				}
				if (myid == 13)
				{
					DrawLight();
					return;
				}
				if (myid == 12 && !SCGlobals.b_le_active)
				{
					DrawTablet();
					return;
				}
				if (myid == 9 || myid == 10)
				{
					DrawCheckpoint();
					return;
				}
				switch (objID)
				{
				case 0:
				case 5:
					DrawFire();
					return;
				case 6:
					DrawPers();
					return;
				case 11:
					return;
				case 12:
					DrawWater();
					return;
				case 8:
					DrawGarbage(dc);
					return;
				case 13:
					DrawRake();
					return;
				case 14:
					DrawStart();
					break;
				case 10:
					DrawExit();
					break;
				case 17:
					DrawTrampo();
					return;
				case 21:
					if (!SCGlobals.b_le_active)
					{
						DrawHand();
						return;
					}
					break;
				case 18:
					dc.a = 155;
					dc.g = 100;
					dc.b = 100;
					break;
				case 20:
					dc.a = 155;
					dc.r = 100;
					dc.b = 100;
					break;
				case 19:
					DrawTriangle();
					return;
				case 4:
					return;
				}
				frect_t2 = (b_invertobj ? SCGlobals.tc_inverse : SCGlobals.tc_normal);
				if (rotation == 0f)
				{
					CowRender.BindTexture(p_texture);
					CowRender.DrawColorQuad2DDynBuffer(frame_rect, frect_t2, dc, 0f);
					CowRender.ObjectPerFrame++;
				}
				else
				{
					CowRender.BindTexture(p_texture);
					CowRender.DrawRotatedColorQuad2DDynBuffer(position, position, rotation, sizex, sizey, frect_t2, dc, 0f);
					CowRender.ObjectPerFrame++;
				}
			}
		}
	}

	public static CLevelObj CreateLevelObjFromGameObj(int go_index)
	{
		gameobject_t gameobject_t2 = SCGlobals.ar_gameobjs[go_index];
		string text = SCGlobals.group_names[gameobject_t2.group].name;
		string text2 = gameobject_t2.name;
		byte b = 0;
		int num = 0;
		int num2 = 0;
		int num3 = 1;
		byte b2 = 4;
		if (text.Contains("platform"))
		{
			b2 = 1;
			b = 0;
			if (text2.Contains("fence"))
			{
				b = 5;
			}
			else if (text2.Contains("dynplatform"))
			{
				b2 |= 3;
				b = 1;
				num = text2[11] - 49;
			}
			else if (text2.Contains("elevator"))
			{
				num3 = 0;
				num = 4;
				b2 = 2;
				b = 1;
			}
			else if (text2 == "bridge")
			{
				b = 24;
			}
			else if (text2 == "cyberbridge")
			{
				b = 25;
			}
		}
		else if (text2.Contains("flowers"))
		{
			b2 = 1;
			b = 5;
		}
		else if (text.Contains("garbage"))
		{
			switch (text2)
			{
			case "barrel1":
				num = 0;
				break;
			case "bag":
				num = 1;
				break;
			case "box":
				num = 2;
				break;
			}
			b = 8;
		}
		else if (text.Contains("character"))
		{
			b = 6;
			if (text2.Contains("bird") || text2.Contains("wasp") || text2.Contains("bat") || text2.Contains("spider") || text2.Contains("frog"))
			{
				b2 |= 2;
			}
		}
		else if (text.Contains("bosses"))
		{
			b = 6;
		}
		else if (text.Contains("bonuses"))
		{
			b = 28;
			if (text2.Contains("coin1"))
			{
				b = 29;
				num = 0;
			}
			else if (text2.Contains("coin2"))
			{
				b = 29;
				num = 1;
			}
			else if (text2.Contains("coin3"))
			{
				b = 29;
				num = 2;
			}
			else if (text2.Contains("dollar"))
			{
				b = 30;
				num = 0;
			}
			else if (text2.Contains("dollar"))
			{
				b = 30;
				num = 0;
			}
			else if (text2.Contains("gold"))
			{
				b = 31;
				num = 0;
			}
			else if (text2.Contains("gem1"))
			{
				b = 32;
				num = 0;
			}
			else if (text2.Contains("gem2"))
			{
				b = 32;
				num = 1;
			}
			else if (text2.Contains("gem3"))
			{
				b = 32;
				num = 2;
			}
			else
			{
				b = 31;
				num = 0;
			}
			b2 |= 8;
		}
		else if (text2.Contains("cloud"))
		{
			b2 = 1;
			b = 2;
			if (text2 == "cloud2")
			{
				num = 1;
			}
		}
		else if (text2.Contains("-tree"))
		{
			b = 15;
			if (text2.Contains("apple"))
			{
				num = 0;
			}
			else if (text2.Contains("plum"))
			{
				num = 1;
			}
			else if (text2.Contains("pear"))
			{
				num = 2;
			}
		}
		else if (text == "power-ups")
		{
			b = 28;
			if (text2 == "life")
			{
				b = 33;
				num = 0;
			}
			b2 |= 8;
		}
		else if (text.Contains("obstacles"))
		{
			b2 = 1;
			if (text2.Contains("semirigid"))
			{
				b = 20;
			}
			else if (text2.Contains("rigid"))
			{
				b = 18;
			}
			else
			{
				switch (text2)
				{
				case "triangle":
					b = 19;
					break;
				case "secret":
				case "deadzone":
					b = 3;
					break;
				}
			}
		}
		else if (text2.Contains("ground"))
		{
			num3 = 1;
			b = 4;
			num = text2[6] - 49;
		}
		else
		{
			switch (text2)
			{
			case "exit":
				b = 10;
				break;
			case "hand1":
			case "hand3":
				b2 = 0;
				b = 21;
				break;
			default:
				if (text2.Contains("tablet"))
				{
					b = 22;
					num2 = 12;
				}
				else if (text2.Contains("chasm"))
				{
					num3 = 3;
					b2 = 1;
					b = 3;
					num2 = 1;
				}
				else if (text2.Contains("stolb"))
				{
					num3 = 2;
					b = 11;
					b2 = 1;
				}
				else if (text2 == "waterfall")
				{
					b2 = 1;
					b = 3;
					num2 = 3;
				}
				else if (text2 == "fire")
				{
					b2 = 17;
					b = 0;
					num2 = 4;
				}
				else if (text2 == "fire")
				{
					b2 = 1;
					b = 0;
					num2 = 4;
				}
				else if (text2.Contains("start"))
				{
					b = 14;
				}
				else if (text2.Contains("rake"))
				{
					b2 |= 0x10;
					b = 13;
				}
				else if (text2.Contains("guillotine"))
				{
					b2 = 1;
					b = 26;
				}
				else if (text2.Contains("toilet"))
				{
					b = 16;
				}
				else if (text2.Contains("trampo"))
				{
					b = 17;
					num = 0;
					if (text2.Length >= 6)
					{
						char c = text2[text2.Length - 1];
						if (c != 'o')
						{
							num = c - 49;
						}
					}
				}
				else if (text2 == "liana")
				{
					b = 27;
					b2 |= 2;
				}
				else
				{
					b = 3;
				}
				break;
			}
		}
		CLevelObj cLevelObj = (((b2 & 8) != 0) ? new CBonusObj() : (b switch
		{
			27 => new CLianaObj(), 
			15 => new CFruitTree(), 
			1 => new CDynPlatformObj(), 
			2 => new CCloudObj(), 
			24 => new CBridgeObj(), 
			25 => new CCyberbridgeObj(), 
			26 => new CGuillotineObj(), 
			_ => new CLevelObj(), 
		}));
		if (text2 == "secret")
		{
			num2 = 8;
		}
		if (text2 == "deadzone")
		{
			num2 = 7;
		}
		if (text2 == "Light01")
		{
			num2 = 13;
		}
		if (text2 == "checkpoint")
		{
			num2 = 9;
		}
		if (text2 == "checkpoint2")
		{
			num2 = 10;
		}
		if (text2 == "lantern")
		{
			num2 = 2;
		}
		if (text2 == "picket")
		{
			num2 = 5;
		}
		if (text2 == "picket2")
		{
			num2 = 6;
		}
		if (text2 == "badwc")
		{
			num2 = 11;
		}
		cLevelObj.name = text2;
		cLevelObj.objID = b;
		cLevelObj.type = b2;
		cLevelObj.objsubtype = num;
		cLevelObj.game_object_index = go_index;
		cLevelObj.p_texture = gameobject_t2.p_tex;
		cLevelObj.p_cgo = gameobject_t2.p_cgo;
		cLevelObj.p_sprite = gameobject_t2.p_sprite;
		cLevelObj.endpos.assign(0f, 0f);
		cLevelObj.rotation = 0f;
		cLevelObj.myid = num2;
		if (num3 != SCGlobals.draw_plane_index)
		{
			SCGlobals.draw_plane_index = num3;
		}
		cLevelObj.draw_plane = SCGlobals.draw_plane_index;
		cLevelObj.extratextparam = "";
		cLevelObj.Init();
		cLevelObj.b_invertobj = false;
		cLevelObj.numlinks = 0;
		return cLevelObj;
	}

	public static void LoadLevelObjStuff()
	{
		Store.SetCurrentDirectory("levelobjs", theAdd: false);
		ref texture_t reference = ref SCGlobals.ar_flowers[0];
		reference = SCGlobals.texturehost.Load("sunflower1.tga");
		ref texture_t reference2 = ref SCGlobals.ar_flowers[1];
		reference2 = SCGlobals.texturehost.Load("romashka1.tga");
		ref texture_t reference3 = ref SCGlobals.ar_flowers[2];
		reference3 = SCGlobals.texturehost.Load("tomato1.tga");
		ref texture_t reference4 = ref SCGlobals.ar_fruittexs[0];
		reference4 = SCGlobals.texturehost.Load("apple.tga");
		ref texture_t reference5 = ref SCGlobals.ar_fruittexs[1];
		reference5 = SCGlobals.texturehost.Load("plum.tga");
		ref texture_t reference6 = ref SCGlobals.ar_fruittexs[2];
		reference6 = SCGlobals.texturehost.Load("pear.tga");
		SCGlobals.p_electric = SCGlobals.texturehost.Load("electro.jpg");
		ref texture_t reference7 = ref SCGlobals.p_trampo[0];
		reference7 = SCGlobals.texturehost.Load("trampopressed.tga");
		ref texture_t reference8 = ref SCGlobals.p_trampo[1];
		reference8 = SCGlobals.texturehost.Load("trampopressed2.tga");
		ref texture_t reference9 = ref SCGlobals.p_trampo[2];
		reference9 = SCGlobals.texturehost.Load("trampopressed3.tga");
		ref texture_t reference10 = ref SCGlobals.p_trampo[3];
		reference10 = SCGlobals.texturehost.Load("trampopressed4.tga");
		SCGlobals.p_toilettop = SCGlobals.texturehost.Load("wc1.tga");
		SCGlobals.p_waterup = SCGlobals.texturehost.Load("waterup.tga");
		SCGlobals.p_waterfall2 = SCGlobals.texturehost.Load("waterfall2.tga");
		SCGlobals.p_bridge1 = SCGlobals.texturehost.Load("bridge1.tga");
		ref texture_t reference11 = ref SCGlobals.p_guillo_up[0];
		reference11 = SCGlobals.texturehost.Load("guillo_up.tga");
		ref texture_t reference12 = ref SCGlobals.p_guillo_down[0];
		reference12 = SCGlobals.texturehost.Load("guillo_down.tga");
		ref texture_t reference13 = ref SCGlobals.p_guillo_up[1];
		reference13 = SCGlobals.texturehost.Load("guillo_up2.tga");
		ref texture_t reference14 = ref SCGlobals.p_guillo_down[1];
		reference14 = SCGlobals.texturehost.Load("guillo_down2.tga");
		SCGlobals.p_liana_tex_up = SCGlobals.texturehost.Load("liana_up.tga");
		SCGlobals.p_liana_tex_down = SCGlobals.texturehost.Load("liana_down.tga");
		SCGlobals.p_lighttex = SCGlobals.texturehost.Load("light2.tga");
		ref texture_t reference15 = ref SCGlobals.p_checkpoint[0, 0];
		reference15 = SCGlobals.texturehost.Load("checkpoint01.tga");
		ref texture_t reference16 = ref SCGlobals.p_checkpoint[0, 1];
		reference16 = SCGlobals.texturehost.Load("checkpoint02.tga");
		ref texture_t reference17 = ref SCGlobals.p_checkpoint[0, 2];
		reference17 = SCGlobals.texturehost.Load("checkpoint03.tga");
		ref texture_t reference18 = ref SCGlobals.p_checkpoint[1, 0];
		reference18 = SCGlobals.texturehost.Load("checkpointEVIL01.tga");
		ref texture_t reference19 = ref SCGlobals.p_checkpoint[1, 1];
		reference19 = SCGlobals.texturehost.Load("checkpointEVIL02.tga");
		ref texture_t reference20 = ref SCGlobals.p_checkpoint[1, 2];
		reference20 = SCGlobals.texturehost.Load("checkpointEVIL03.tga");
		Store.SetCurrentDirectory("", theAdd: false);
		SCGlobals.p_podkladka = SCGlobals.texturehost.Load("podsvet.jpg");
		Store.SetCurrentDirectory("", theAdd: false);
	}

	public void DrawPlatforms()
	{
		dcolor_t dc = dcolor_t.ColorWhite;
		if (b_selected)
		{
			dc = SCGlobals.color_selected;
		}
		frect_t frect_t2 = rect;
		if (rotation != 0f)
		{
			CowRender.EnableRS(7u);
			SuperCowGame.DrawPlatform(frect_t2, p_texture, dc, 0, rotation, position);
			CowRender.DisableRS(7u);
		}
		else
		{
			SuperCowGame.DrawPlatform(frect_t2, p_texture, dc, 0, rotation, position);
		}
	}

	public void DrawFire()
	{
		if (myid == 4)
		{
			vec2_t vec2_t2 = rect.pos();
			float1 += SCGlobals.ftickperframe * sizex;
			obj_time += SCGlobals.ftickperframe * sizex;
			while (obj_time > 70f)
			{
				Particles.AddDynamicParticle(vec2_t2 + new vec2_t(CRand.frand(sizex / 2f), 0f), 22, 1, -1);
				obj_time -= 70f;
			}
			while (float1 > 100f)
			{
				Particles.AddDynamicParticle(vec2_t2 + new vec2_t(CRand.frand(sizex / 2f), 0.5f), 24, 1, -1);
				float1 -= 100f;
			}
		}
	}

	public void DrawHand()
	{
		float num = (SCGlobals.hero.position.v2t() - position).len();
		if (num > 12f)
		{
			return;
		}
		float num2 = ((!(num > 10f)) ? 1f : (1f - (num - 10f) / 2f));
		CowRender.EnableRS(7u);
		CowRender.BindTexture(p_texture);
		frect_t tc = (b_invertobj ? SCGlobals.tc_inverse : SCGlobals.tc_normal);
		dcolor_t colorWhite = dcolor_t.ColorWhite;
		vec2_t vec2_t2 = position;
		colorWhite.a = (byte)(255f * num2);
		vec2_t vec2_t3 = new vec2_t(SuperCowHelper.psin(2f, -1000001f) * num2, 0f);
		if (name == "hand3")
		{
			if (b_invertobj)
			{
				vec2_t3.rotate(-70f);
			}
			else
			{
				vec2_t3.rotate(70f);
			}
		}
		CowRender.DrawRotatedColorQuad2DDynBuffer(vec2_t2 + vec2_t3, vec2_t2, rotation, sizex, sizey, tc, colorWhite, 0f);
		CowRender.DisableRS(7u);
	}

	public void DrawTrampo()
	{
		dcolor_t c = dcolor_t.ColorWhite;
		if (b_selected)
		{
			c = SCGlobals.color_selected;
		}
		if (obj_time > 0f)
		{
			float num = obj_time / 300f;
			c.a = (byte)(255f * (1f - num) * (1f - num));
			CowRender.BindTexture(p_texture);
			CowRender.DrawColorQuad2DDynBuffer(frame_rect, b_invertobj ? SCGlobals.tc_inverse : SCGlobals.tc_normal, c, 0f);
			c.a = byte.MaxValue;
			if (objsubtype < SCGlobals.p_trampo.Length)
			{
				texture_t p_tex = SCGlobals.p_trampo[objsubtype];
				CowRender.BindTexture(p_tex);
				CowRender.DrawColorQuad2DDynBuffer(frame_rect, b_invertobj ? SCGlobals.tc_inverse : SCGlobals.tc_normal, c, 0f);
			}
		}
		else
		{
			CowRender.BindTexture(p_texture);
			CowRender.DrawColorQuad2DDynBuffer(frame_rect, b_invertobj ? SCGlobals.tc_inverse : SCGlobals.tc_normal, c, 0f);
		}
		obj_time -= SCGlobals.ftickperframe;
	}

	public void DrawStart()
	{
		if (!SCGlobals.b_le_active && !(obj_time >= 4000f))
		{
			CowRender.DrawDynBuffer();
			float num = obj_time / 4000f;
			float num2 = CRand.frand(1f);
			float num3 = CRand.frand(2f);
			frect_t tc = new frect_t(num2, num3, 1f + num2, 2f + num3);
			frect_t vc = frame_rect;
			vc.y0 -= 1f;
			vc.x0 += 0.1f;
			vc.x1 -= 0.1f;
			num = 1f - num * num * num;
			if (!SCGlobals.b_pause)
			{
				obj_time += SCGlobals.ftickperframe;
			}
			byte b = (byte)(255f * num);
			dcolor_t c = new dcolor_t(b, b, b, byte.MaxValue);
			CowRender.SetBlend(1);
			CowRender.BindTexture(SCGlobals.p_electric);
			CowRender.DrawColorQuad2D(vc, tc, c, 0f);
			CowRender.SetBlend(0);
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			graphicsDevice.Textures[0] = (Texture)(object)SuperCowGame.mainTex;
		}
	}

	public void DrawExit()
	{
		if (!SCGlobals.b_le_active && b_switch != 0)
		{
			CowRender.DrawDynBuffer();
			if (obj_time == 0f)
			{
				Sound.Addsound("openexit");
			}
			if (obj_time >= 3000f)
			{
				obj_time = 3000f;
			}
			float num = obj_time / 3000f;
			float num2 = CRand.frand(1f);
			float num3 = CRand.frand(2f);
			frect_t tc = new frect_t(num2, num3, 1f + num2, 2f + num3);
			frect_t vc = frame_rect;
			vc.y0 -= 1f;
			vc.x0 += 0.1f;
			vc.x1 -= 0.1f;
			num *= num;
			obj_time += SCGlobals.ftickperframe;
			byte b = (byte)(255f * num);
			dcolor_t c = new dcolor_t(b, b, b, byte.MaxValue);
			if (SCGlobals.hero.behavior == behaviors_t.BT_VICTORY)
			{
				CowRender.EnableRS(1u);
			}
			CowRender.SetBlend(1);
			CowRender.BindTexture(SCGlobals.p_electric);
			CowRender.DrawColorQuad2D(vc, tc, c, 0f);
			CowRender.SetBlend(0);
			CowRender.DisableRS(1u);
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			graphicsDevice.Textures[0] = (Texture)(object)SuperCowGame.mainTex;
		}
	}

	public void DrawToiletTop()
	{
		if (rect.isRectTouchRect(SCGlobals.viewport_rect))
		{
			CowRender.BindTexture(SCGlobals.p_toilettop);
			CowRender.DrawColorQuad2D(frame_rect, b_invertobj ? SCGlobals.tc_inverse : SCGlobals.tc_normal, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		}
	}

	public void DrawPers()
	{
		CowRender.BindTextureNull();
		gameobject_t gameobject_t2 = SCGlobals.ar_gameobjs[game_object_index];
		string text = gameobject_t2.name;
		CowRender.DisableRS(4u);
		CowRender.EnableRS(1u);
		CowRender.EnableRS(0u);
		if (b_selected)
		{
			CowRender.SetMaterial(SCGlobals.matselected);
		}
		CMonster pers = SCGlobals.pershost.GetPers(text);
		int num = 1;
		float num2 = 1f;
		if (extratextparam.Length > 0)
		{
			num = int.Parse(extratextparam, CultureInfo.InvariantCulture);
			if (num != 2 && num != 3)
			{
				num = 1;
			}
		}
		else
		{
			num = 1;
		}
		num2 = num switch
		{
			2 => 1.4f, 
			3 => 1.8f, 
			_ => 1f, 
		};
		pers.scalematrix.load_identity();
		pers.scalematrix.scale(pers.fscale * num2, pers.fscale * num2, pers.fscale * num2);
		pers.startpos = position.v3t();
		pers.position = position.v3t();
		pers.curpos = position.v3t();
		pers.curpos.z = pers.curpos.y;
		pers.curpos.y = 0f;
		pers.matrix.load_identity();
		pers.matrix.translate(pers.curpos);
		pers.matrix.zrot_delta(180f);
		pers.Draw();
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		graphicsDevice.RasterizerState = RasterizerState.CullNone;
		CowRender.LoadMatrixNull();
		CowRender.DisableRS(0u);
		CowRender.DisableRS(1u);
		CowRender.BindTexture(default(texture_t));
		CowRender.SetMaterial(SCGlobals.matcommon);
		if ((type & 2) == 0)
		{
			CowRender.EnableRS(4u);
			return;
		}
		if (text.Contains("spider"))
		{
			vec3_t p = default(vec3_t);
			vec3_t p2 = default(vec3_t);
			p.assign(startpos.x, startpos.y, 0f);
			p2.assign(endpos.x, endpos.y, 0f);
			CowRender.DrawLine(p, p2);
		}
		else
		{
			frect_t frect_t2 = new frect_t(startpos, endpos);
			CowRender.DrawEmptyRect(frect_t2);
		}
		CowRender.EnableRS(4u);
	}

	public void DrawRake()
	{
		CowRender.EnableRS(1u);
		CowRender.EnableRS(0u);
		float num = ((iauxvar == 1) ? (obj_time + SCGlobals.frameinterp * SCGlobals.physics_speed * 150f) : ((iauxvar == 2) ? 1000f : ((iauxvar >= 0) ? 0f : (obj_time - SCGlobals.frameinterp * SCGlobals.physics_speed * 60f))));
		float num2 = num / 1000f;
		num2 = ((iauxvar < 0) ? (1f - (1f - num2) * (1f - num2)) : ((float)Math.Sin(2.1991148f * num2) / 0.809f));
		matrix_t matrix = default(matrix_t);
		matrix.load_identity();
		matrix.translate(frame_pos);
		matrix.zrot(180f);
		if (b_invertobj)
		{
			matrix.xrot_delta(20f + num2 * 70f);
			matrix.yrot_delta(-60f + 60f * num2);
		}
		else
		{
			matrix.xrot_delta(20f + num2 * 70f);
			matrix.yrot_delta(60f - 60f * num2);
		}
		p_cgo.Draw(matrix);
		CowRender.LoadMatrixNull();
		CowRender.DisableRS(1u);
		CowRender.DisableRS(0u);
		CowRender.EnableRS(4u);
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		graphicsDevice.Textures[0] = (Texture)(object)SuperCowGame.mainTex;
	}

	public void DrawWater()
	{
		dcolor_t c = new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, 100);
		if (b_selected)
		{
			c.assign(byte.MaxValue, 0, 0, 100);
		}
		obj_time += SCGlobals.ftickperframe;
		CowRender.BindTexture(SCGlobals.ar_watertex[(int)(obj_time * 0.01f) % SCGlobals.NUM_WATER_TEX]);
		float x = rect.sizex() / (float)p_texture.width * 32f;
		float y = rect.sizey() / (float)p_texture.width * 32f;
		CowRender.DrawColorQuad2DDynBuffer(rect, new frect_t(0f, 0f, x, y), c, 0f);
	}

	public void DrawTopWater()
	{
		CowRender.BindTexture(SCGlobals.p_white);
		float num = rect.sizex() / (float)p_texture.width * 32f;
		float num2 = rect.sizey() / (float)p_texture.width * 32f;
		float num3 = (0f - obj_time) * 5E-05f;
		float num4 = (0f - obj_time) * 0.0001f;
		CowRender.EnableRS(1u);
		CowRender.DisableRS(2u);
		dcolor_t c = default(dcolor_t);
		c.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, 120);
		CowRender.DrawColorQuad2D(rect, new frect_t(num3, num4, num + num3, num2 + num4), c, 2f);
		CowRender.DisableRS(1u);
		CowRender.EnableRS(2u);
	}

	public void DrawPosts()
	{
		dcolor_t dc = dcolor_t.ColorWhite;
		if (b_selected)
		{
			dc = SCGlobals.color_selected;
		}
		if (rotation != 0f)
		{
			CowRender.EnableRS(7u);
			DrawPost(rect, p_texture, dc, rotation, position);
			CowRender.DisableRS(7u);
		}
		else
		{
			DrawPost(rect, p_texture, dc, rotation, position);
		}
	}

	public void DrawPost(frect_t rect0, texture_t p_tex, dcolor_t dc, float rotation, vec2_t position)
	{
		CowRender.BindTexture(p_tex);
		vec3_t vec3_t2 = rect0.p0.v3t();
		vec3_t2.y += 1f;
		vec3_t2.x -= 0.3f;
		int width = (int)p_tex.width;
		int num = (int)Math.Round((rect0.y0 - rect0.y1) * 32f / (float)width);
		int num2 = (int)p_tex.height / width;
		float num3 = rect0.sizex();
		float num4 = (float)width / 32f;
		frect_t frect_t2 = default(frect_t);
		for (int i = 0; i < num; i++)
		{
			int num5 = ((i != num - 1) ? ((i <= 2 || num <= 2) ? i : (1 + i % (num2 - 2))) : (num2 - 1));
			frect_t2.assign(vec3_t2.x, vec3_t2.y - num4 * (float)i, vec3_t2.x + num3, vec3_t2.y - num4 * (float)(i + 1));
			float num6 = 1f / (float)num2 * (float)num5;
			float num7 = 1f / (float)num2 * (float)(num5 + 1);
			float num8 = 0f;
			float num9 = 1f;
			pl_p1.assign(frect_t2.x1, frect_t2.y1, 0f);
			pl_p2.assign(frect_t2.x1, frect_t2.y0, 0f);
			pl_p3.assign(frect_t2.x0, frect_t2.y0, 0f);
			pl_p4.assign(frect_t2.x0, frect_t2.y1, 0f);
			pl_t1.assign(num9, num7);
			pl_t2.assign(num9, num6);
			pl_t3.assign(num8, num6);
			pl_t4.assign(num8, num7);
			CowRender.DrawRotatedColorQuad2DStaticBuffer(pl_p1.v2t(), position, rotation, num3, num4, new frect_t(num8, num6, num9, num7), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		}
	}

	public void DrawGarbage(dcolor_t dc)
	{
		if (b_switch != 0 && obj_time == 0f)
		{
			obj_time = 1f;
			frandom1 = ((CRand.irand(2) == 0) ? (-1f) : 1f) * (1f + CRand.prand(1f));
			if (objsubtype == 0)
			{
				frandom2 = 1f + CRand.prand(0.5f);
			}
			else
			{
				frandom2 = 1.5f;
			}
			iauxvar = 0;
			vec3_t vec3_t2 = default(vec3_t);
			vec3_t2.assign(position.x, position.y, 0f);
			SCGlobals.supercowgame.AddScore(vec3_t2, 50);
			Particles.AddDynamicParticle(vec3_t2 - new vec3_t(CRand.frand(sizex * 0.5f), sizey / 2f, 0f), 3, (objsubtype == 0) ? 7 : 10, -1);
		}
		if (obj_time == 0f)
		{
			CowRender.BindTexture(p_texture);
			CowRender.DrawColorQuad2DDynBuffer(frame_rect, new frect_t(0f, 0f, 1f, 1f), dc, 0f);
			return;
		}
		vec2_t vec2_t2 = default(vec2_t);
		float num = obj_time / 1000f;
		if (obj_time < 1000f)
		{
			obj_time += SCGlobals.ftickperframe;
		}
		else
		{
			num = 1f;
		}
		float num2 = sizex;
		float num3 = sizey;
		if (num2 > 4f)
		{
			num2 = 4f;
		}
		CowRender.BindTexture(SCGlobals.ar_flowers[objsubtype]);
		float num4 = 1f - Math.Abs((1f - num) * (1f - num) * (float)Math.Cos((float)Math.PI * 4f * num * num * num));
		num4 = frandom2 / 1.5f * num4;
		frect_t vc = frame_rect;
		vc.y0 = vc.y1 + num3 * num4;
		CowRender.DrawColorQuad2DDynBuffer(vc, new frect_t((frandom1 > 0f) ? 0f : 1f, 0f, (frandom1 > 0f) ? 1f : 0f, frandom2 / 1.5f), dc, 0f);
		if (num != 1f)
		{
			if (num < 0.5f && (int)(obj_time - SCGlobals.ftickperframe) % 100 < 50 && (int)obj_time % 100 > 50)
			{
				Particles.AddDynamicParticle(position + new vec3_t(CRand.frand(sizex * 0.5f), sizey * (num4 - 0.6f), 0f).v2t(), 7 + objsubtype, 2, -1);
			}
			vec2_t2.assign(frame_pos.x, frame_pos.y);
			vec2_t2.y -= 17f * frandom2 * num * num;
			CowRender.BindTexture(p_texture);
			CowRender.DrawRotatedColorQuad2DDynBuffer(vec2_t2, vec2_t2, num * 220f * frandom1, sizex, sizey, SCGlobals.tc_normal, dcolor_t.ColorWhite, 0f);
		}
	}

	public void DrawChasm()
	{
		CowRender.DrawDynBuffer();
		obj_time2 += SCGlobals.ftickperframe;
		float num = obj_time2 * 0.001f;
		float num2 = 32f * rect.sizex() / (float)p_texture.width;
		frect_t tc = default(frect_t);
		tc.assign(num, 0f, num + num2, 1f);
		CowRender.SetBlend(1);
		CowRender.BindTexture(p_texture);
		CowRender.DrawColorQuad2D(frame_rect, tc, dcolor_t.ColorWhite, 0f);
		tc.assign(0f - num, 0f, 0f - num + num2, 1f);
		CowRender.DrawColorQuad2D(frame_rect, tc, dcolor_t.ColorWhite, 0f);
		CowRender.SetBlend(0);
		obj_time -= SCGlobals.ftickperframe;
		if (obj_time < 0f)
		{
			Particles.AddDynamicParticle(new vec2_t(position.x + CRand.frand(sizex / 3f), SCGlobals.viewport_rect.y1), 25, 1, 8);
			obj_time += 50f;
		}
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		graphicsDevice.Textures[0] = (Texture)(object)SuperCowGame.mainTex;
	}

	private void DrawLantern()
	{
	}

	public void DrawWaterfall()
	{
		CowRender.DrawDynBuffer();
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		obj_time += SCGlobals.ftickperframe;
		CowRender.BindTexture(p_texture);
		dcolor_t c = default(dcolor_t);
		CowRender.SetBlend(0);
		CowRender.EnableRS(4u);
		CowRender.EnableRS(7u);
		c.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, 160);
		frect_t tc = (b_invertobj ? SCGlobals.tc_inverse : SCGlobals.tc_normal);
		float num = obj_time * 0.0012f;
		tc.assign(0f, 0f - num, 1f, 0f - num + sizey / ((float)p_texture.height / 32f));
		CowRender.DrawColorQuad2D(rect, tc, c, 100f);
		c.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, 200);
		CowRender.BindTexture(SCGlobals.p_waterfall2);
		num = obj_time * 0.0025f;
		tc.assign(0f, 0f - num, 1f, 0f - num + sizey / ((float)p_texture.height / 32f));
		CowRender.DrawColorQuad2D(rect, tc, c, 100f);
		CowRender.BindTexture(SCGlobals.p_waterup);
		frect_t vc = default(frect_t);
		c.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		vc.assign(rect.x0, rect.y0 + 0.5f, rect.x1, rect.y0 - 1.5f);
		CowRender.DrawColorQuad2D(vc, SCGlobals.tc_normal, c, 100f);
		byte b = (byte)(70f + 50f * (float)Math.Sin(obj_time * 0.003f));
		c.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255 - b));
		vc.translate(0.05f * (float)Math.Sin(obj_time * 0.0021f), 0.05f * (float)Math.Cos(obj_time * 0.0038f));
		CowRender.DrawColorQuad2D(vc, SCGlobals.tc_inverse, c, 100f);
		CowRender.DisableRS(7u);
		graphicsDevice.Textures[0] = (Texture)(object)SuperCowGame.mainTex;
	}

	public void CalulateWaterfall()
	{
		if (SCGlobals.b_le_active || (extratextparam.Length > 0 && extratextparam[0] == 's'))
		{
			return;
		}
		vec2_t vec2_t2 = -SCGlobals.camerapos;
		float num = ((vec2_t2.x <= rect.x1 && vec2_t2.x >= rect.x0) ? 0f : ((!(vec2_t2.x < rect.x0)) ? (vec2_t2.x - rect.x1) : (rect.x0 - vec2_t2.x)));
		float num2 = ((!(num > 18f)) ? (100f * (1f - num / 18f)) : 0f);
		float num3 = ((vec2_t2.y <= rect.y0 && vec2_t2.y >= rect.y1) ? 0f : ((!(vec2_t2.y > rect.y0)) ? (rect.y1 - vec2_t2.y) : (vec2_t2.y - rect.y0)));
		num2 = ((!(num3 > 9f)) ? (num2 * (1f - num3 / 9f)) : 0f);
		if (num2 == 0f)
		{
			int1 = 0;
			return;
		}
		if (rect.sizex() > 3.5f)
		{
			if (int1 > 3000)
			{
				int1 = 0;
			}
			if (int1 == 0)
			{
				Sound.Addsound("waterfall");
			}
		}
		else
		{
			if (int1 > 3100)
			{
				int1 = 0;
			}
			if (int1 == 0)
			{
				Sound.Addsound("brook");
			}
		}
		int1 += (int)SCGlobals.tickperframe;
	}

	public void DrawTriangle()
	{
		dcolor_t dcolor_t2 = new dcolor_t
		{
			a = 155,
			r = 100,
			b = 100,
			g = byte.MaxValue
		};
		CowRender.BindTexture(p_texture);
		CowRender.BindTextureNull();
		vec3_t vec3_t2 = default(vec3_t);
		vec3_t vec3_t3 = default(vec3_t);
		vec3_t vec3_t4 = default(vec3_t);
		float x = endpos.x;
		float y = endpos.y;
		float x2 = rect.p1.x;
		if (x2 == x)
		{
			x2 = rect.p0.x;
		}
		float y2 = rect.p1.y;
		if (y2 == y)
		{
			y2 = rect.p0.y;
		}
		vec3_t2.assign(x, y, 0f);
		vec3_t3.assign(x2, y2, 0f);
		vec3_t4.assign(x, y2, 0f);
		CowRender.DrawColorQuad(vec3_t2, vec3_t3, vec3_t4, vec3_t4, dcolor_t2, dcolor_t2, dcolor_t2, dcolor_t2);
		CowRender.DrawLine(vec3_t2, vec3_t3);
		CowRender.DrawLine(vec3_t3, vec3_t4);
		CowRender.DrawLine(vec3_t4, vec3_t2);
	}

	public void DrawSecret()
	{
		if (SCGlobals.b_le_active)
		{
			dcolor_t c = new dcolor_t
			{
				a = 100,
				r = 100,
				g = 100,
				b = 200
			};
			CowRender.BindTexture(p_texture);
			CowRender.DrawColorQuad2DDynBuffer(frame_rect, SCGlobals.tc_normal, c, 0f);
			CowRender.DrawEmptyRect(frame_rect);
			CowRender.EnableRS(7u);
			Text.OutTextXY(position.x, position.y, 0.5f, "secret", b_center: true);
			CowRender.DisableRS(7u);
		}
	}

	public void DrawDeadZone()
	{
		if (SCGlobals.b_le_active)
		{
			dcolor_t c = new dcolor_t
			{
				a = 195,
				r = 100,
				g = 100,
				b = 100
			};
			CowRender.BindTexture(p_texture);
			CowRender.DrawColorQuad2DDynBuffer(frame_rect, SCGlobals.tc_normal, c, 0f);
			CowRender.DrawEmptyRect(frame_rect);
			CowRender.EnableRS(7u);
			Text.OutTextXY(position.x, position.y, 0.5f, "dead zone", b_center: true);
			CowRender.DisableRS(7u);
		}
	}

	public void DrawTablet()
	{
		float num = 1f;
		CowRender.EnableRS(7u);
		CowRender.BindTexture(p_texture);
		frect_t tc = (b_invertobj ? SCGlobals.tc_inverse : SCGlobals.tc_normal);
		dcolor_t colorWhite = dcolor_t.ColorWhite;
		vec2_t vec2_t2 = position;
		colorWhite.a = (byte)(255f * num);
		CowRender.DrawRotatedColorQuad2DDynBuffer(vec2_t2 + new vec2_t(0f, SuperCowHelper.psin(2f, -1000001f) * 0.3f), vec2_t2, rotation, sizex, sizey, tc, colorWhite, 0f);
		CowRender.DisableRS(7u);
	}

	public void DrawLight()
	{
		dcolor_t colorWhite = dcolor_t.ColorWhite;
		frect_t tc_normal = SCGlobals.tc_normal;
		float num = SuperCowHelper.psin(0.15f, -1000001f);
		CowRender.EnableRS(7u);
		colorWhite.a = (byte)(num * 255f);
		CowRender.BindTexture(p_texture);
		CowRender.DrawRotatedColorQuad2DDynBuffer(position, position, rotation, sizex, sizey, tc_normal, colorWhite, 0f);
		colorWhite.a = (byte)((1f - num) * 255f);
		CowRender.BindTexture(SCGlobals.p_lighttex);
		CowRender.DrawRotatedColorQuad2DDynBuffer(position, position, rotation, sizex, sizey, tc_normal, colorWhite, 0f);
		CowRender.DisableRS(7u);
	}

	public void DrawCheckpoint()
	{
		dcolor_t c = dcolor_t.ColorWhite;
		if (b_selected)
		{
			c = SCGlobals.color_selected;
		}
		float num = -1f;
		if (myid == 9)
		{
			objsubtype = 0;
		}
		else
		{
			objsubtype = 1;
		}
		frect_t frect_t2 = rect;
		float num2 = 0.6f;
		float num3 = 0f;
		float num4 = 0f;
		frect_t2.x0 += num2;
		frect_t2.x1 -= num2;
		frect_t2.y0 -= 4f * num2;
		float1 += SCGlobals.ftickperframe;
		vec2_t src = new vec2_t(position.x, frect_t2.y0);
		vec2_t dst = new vec2_t(position.x, frect_t2.y1);
		if (b_switch != 0)
		{
			if (obj_time == 0f)
			{
				Text.AddLInfoText(wstrstuff.GetStr("checkpoint"));
				frandom2 = 0f;
			}
			obj_time += SCGlobals.ftickperframe;
			num3 = obj_time / 2000f;
			if (obj_time < 2000f)
			{
				while (frandom2 > 0f)
				{
					float num5 = 0.2f + CRand.frand(0.2f);
					GlowFXHost.gfxhAddNaturalObject(src, dst, new vec2_t(num5, num5), 0.03f, 0.4f, 2.5f, new dcolor_t(byte.MaxValue, (byte)(100 + CRand.irand(155)), (byte)CRand.irand(255), byte.MaxValue));
					frandom2 -= 30f;
				}
				frandom2 += SCGlobals.ftickperframe;
			}
			if (num3 > 1f)
			{
				num3 = 1f;
			}
			num4 = num3 / 0.2f;
			if (num4 > 1f)
			{
				num4 = 1f;
			}
			num = (1f - num3) * (float)Math.Sin((float)Math.PI * 11f * num3);
			if (num3 == 1f)
			{
				obj_time2 -= SCGlobals.ftickperframe;
				if (obj_time2 <= 0f)
				{
					if (CRand.irand(3) == 0)
					{
						int1 = 0;
						obj_time2 += 100f;
					}
					else if (CRand.irand(9) == 0)
					{
						int1 = 2;
						obj_time2 += 500f;
					}
					else
					{
						obj_time2 += 500f;
						int1 = 1;
					}
				}
			}
			else
			{
				int1 = 2;
			}
			CowRender.BindTexture(SCGlobals.p_checkpoint[objsubtype, int1]);
		}
		else
		{
			CowRender.BindTexture(SCGlobals.p_checkpoint[objsubtype, 0]);
		}
		if (!SCGlobals.b_le_active && (b_switch == 0 || obj_time > 2000f))
		{
			while (frandom2 > 0f)
			{
				float num6 = 0.2f + CRand.frand(0.1f);
				vec2_t vec2_t2 = position - new vec2_t(0f, 2f);
				GlowFXHost.gfxhAddNaturalObject(new vec2_t(CRand.frand(4f), CRand.frand(1f)) + vec2_t2, new vec2_t(CRand.frand(4f), CRand.frand(1f)) + vec2_t2, new vec2_t(num6, num6), 0.03f, 0.4f, 2.5f, new dcolor_t(byte.MaxValue, (byte)(100 + CRand.irand(155)), (byte)CRand.irand(255), byte.MaxValue));
				frandom2 -= 50f;
			}
			frandom2 += SCGlobals.ftickperframe;
		}
		vec3_t p = new vec3_t(frect_t2.x1, frect_t2.y0, 0f);
		vec3_t vec3_t2 = default(vec3_t);
		vec3_t p2 = new vec3_t(frect_t2.x0, frect_t2.y1, 0f);
		vec3_t vec3_t3 = default(vec3_t);
		vec3_t2 = frect_t2.p1.v3t();
		vec3_t3 = frect_t2.p0.v3t();
		float num7 = 0.3f * SuperCowHelper.psin(0.1f, frandom1 + float1);
		p.x += num7;
		p.y += num;
		vec3_t3.x += num7;
		vec3_t3.y += num;
		byte b = (byte)(185 + (int)(70f * num4));
		c.assign(b, b, b, byte.MaxValue);
		CowRender.EnableRS(7u);
		CowRender.DrawColorQuadDynBuffer(p, vec3_t2, p2, vec3_t3, new vec2_t(1f, 0f), new vec2_t(1f, 1f), new vec2_t(0f, 1f), new vec2_t(0f, 0f), c);
		CowRender.DisableRS(7u);
	}

	public void DrawBadwc()
	{
	}
}
