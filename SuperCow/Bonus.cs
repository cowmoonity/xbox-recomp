using System;

namespace SuperCow;

public static class Bonus
{
	public static CSprite[] ar_coins = new CSprite[6];

	public static bonus_t[] ar_bonuses = new bonus_t[64];

	public static int fruitsindex;

	public static void LoadBonuses()
	{
		InitBonuses();
		ar_coins[0].Load("coin1.tga", 32, 8);
		ar_coins[1].Load("coin2.tga", 32, 8);
		ar_coins[2].Load("coin3.tga", 32, 8);
		ar_coins[3].Load("diamonds01.tga", 32, 8);
		ar_coins[4].Load("diamonds02.tga", 32, 8);
		ar_coins[5].Load("diamonds03.tga", 32, 8);
	}

	public static void InitBonuses()
	{
		fruitsindex = 0;
		Array.Clear(ar_bonuses, 0, ar_bonuses.Length);
		for (int i = 0; i < 6; i++)
		{
			ar_coins[i] = new CSprite();
		}
	}

	public static void AddBonusLevelCoin(vec2_t pos, int type)
	{
		SCGlobals.levelstat.numcoins++;
		bonus_t bonus_t2 = new bonus_t
		{
			b_active = true,
			b_bonuslevel = true,
			time = 0f,
			pos = pos,
			p_sprite = ar_coins[type],
			type = type,
			endy = 0f,
			speed = 0f,
			numjumps = 0,
			rot = 0f,
			rotspeed = CRand.frand(10f)
		};
		ar_bonuses[fruitsindex++] = bonus_t2;
		fruitsindex = (fruitsindex + 1) % 64;
	}

	public static void AddCoin(vec2_t pos, float size, float endy, int type)
	{
		SCGlobals.levelstat.numcoins++;
		bonus_t bonus_t2 = new bonus_t
		{
			b_active = true,
			time = 0f,
			pos = pos,
			size = size,
			p_sprite = ar_coins[type - 1],
			type = type,
			p_tex = default(texture_t),
			endy = endy,
			speed = 0.6f,
			numjumps = 0,
			rot = 0f,
			rotspeed = CRand.frand(10f)
		};
		ar_bonuses[fruitsindex++] = bonus_t2;
		fruitsindex = (fruitsindex + 1) % 64;
	}

	public static void AddFruit(vec2_t pos, float size, float endy, texture_t p_texture)
	{
		bonus_t bonus_t2 = new bonus_t
		{
			b_active = true,
			time = 0f,
			pos = pos,
			type = 0,
			size = size,
			p_tex = p_texture,
			endy = endy,
			speed = 0f,
			numjumps = 0,
			rot = 0f,
			rotspeed = CRand.frand(10f)
		};
		ar_bonuses[fruitsindex++] = bonus_t2;
		fruitsindex = (fruitsindex + 1) % 64;
		Sound.Addsound("fruitdown");
	}

	public static void CalculateBonuses()
	{
	}

	public static void DrawBonuses()
	{
		CowRender.EnableRS(4u);
		CowRender.SetBlend(0);
		for (int i = 0; i < 64; i++)
		{
			bonus_t bonus_t2 = ar_bonuses[i];
			if (bonus_t2.b_active)
			{
				if (bonus_t2.type == 0 && !bonus_t2.b_bonuslevel)
				{
					CowRender.BindTexture(bonus_t2.p_tex);
					vec2_t pos = bonus_t2.pos;
					if (bonus_t2.pos.y > bonus_t2.endy + 0.1f)
					{
						pos.y += SCGlobals.physics_speed * bonus_t2.speed * SCGlobals.frameinterp;
						bonus_t2.rot += bonus_t2.rotspeed * SCGlobals.ftickperframe * 0.01f;
					}
					CowRender.DrawRotatedColorQuad2DDynBuffer(pos, pos, bonus_t2.rot, bonus_t2.size, bonus_t2.size, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
				}
				else
				{
					vec2_t pos = bonus_t2.pos;
					pos.y += SCGlobals.physics_speed * bonus_t2.speed * SCGlobals.frameinterp;
					bonus_t2.p_sprite.Draw(pos, -1, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f, 0f);
				}
			}
			ar_bonuses[i] = bonus_t2;
		}
		CowRender.DrawDynBuffer();
	}

	public static void PhysicsBonuses()
	{
		for (int i = 0; i < 64; i++)
		{
			bonus_t bonus_t2 = ar_bonuses[i];
			if (!bonus_t2.b_active)
			{
				continue;
			}
			bonus_t2.time += SCGlobals.engine_frequency;
			if (bonus_t2.b_bonuslevel)
			{
				bonus_t2.pos.y += SCGlobals.physics_speed * bonus_t2.speed;
				if (bonus_t2.speed > -0.5f)
				{
					bonus_t2.speed -= 0.04f;
				}
				if (bonus_t2.pos.y < -50f)
				{
					bonus_t2.b_active = false;
				}
				for (int j = 0; j < SCGlobals.numobstacles; j++)
				{
					obstacle_t obstacle_t2 = SCGlobals.ar_obstacles[j];
					if ((obstacle_t2.obsflag == obstacle_flags_t.RIGID_OBF || obstacle_t2.obsflag == obstacle_flags_t.SEMITRANS_OBF) && obstacle_t2.rect.isPointInRect(bonus_t2.pos))
					{
						Sound.Addsound("fruitdown");
						if (bonus_t2.numjumps < 1)
						{
							bonus_t2.numjumps++;
							bonus_t2.speed = (0f - bonus_t2.speed) * 0.9f;
							bonus_t2.pos.y = obstacle_t2.rect.y0 + 0.001f;
						}
					}
				}
			}
			else if (bonus_t2.numjumps < 4)
			{
				bonus_t2.pos.y += SCGlobals.physics_speed * bonus_t2.speed;
				bonus_t2.speed -= 0.04f;
				if (bonus_t2.pos.y < -1024f)
				{
					bonus_t2.b_active = false;
				}
				else if (bonus_t2.pos.y <= bonus_t2.endy && bonus_t2.speed < 0f)
				{
					Sound.Addsound("fruitdown");
					if (bonus_t2.numjumps < 4)
					{
						bonus_t2.numjumps++;
						bonus_t2.speed = (0f - bonus_t2.speed) * 0.45f;
						bonus_t2.pos.y = bonus_t2.endy + 0.001f;
					}
					else
					{
						bonus_t2.pos.y += (bonus_t2.endy - bonus_t2.pos.y) * 0.2f;
					}
				}
			}
			if (bonus_t2.type >= 1 || bonus_t2.b_bonuslevel)
			{
				if (bonus_t2.time >= 200f && SCGlobals.hero.bound_rect.isPointInRect(bonus_t2.pos))
				{
					bonus_t2.b_active = false;
					Particles.AddDynamicParticle(bonus_t2.pos, 26, 1, -1);
					Particles.AddParticleSource(new vec3_t(bonus_t2.pos.x, bonus_t2.pos.y, 0f), 0);
					AddScoreFromBonus(bonus_t2.pos, bonus_t2.type - 1);
					string sound = "bonuscoin" + bonus_t2.type;
					Sound.Addsound(sound);
				}
			}
			else if (bonus_t2.numjumps >= 1 && SCGlobals.hero.bound_rect.isPointInRect(bonus_t2.pos))
			{
				bonus_t2.b_active = false;
				Particles.AddDynamicParticle(bonus_t2.pos, 12, 5, -1);
				SCGlobals.supercowgame.AddScore(bonus_t2.pos, (bonus_t2.size == 1f) ? 10 : 5);
				SCGlobals.supercowgame.AddHeroHealth(5f);
				Sound.Addsound("fruiteat");
			}
			ar_bonuses[i] = bonus_t2;
		}
	}

	public static void AddScoreFromBonus(vec2_t pos, int type)
	{
		int num = 0;
		int type2 = type;
		switch (type)
		{
		case 0:
			num = 20;
			break;
		case 1:
			num = 30;
			break;
		case 2:
			num = 50;
			break;
		case 3:
			num = 350;
			break;
		case 4:
			num = 300;
			break;
		case 5:
			num = 250;
			break;
		case 6:
			num = 200;
			type2 = 2;
			break;
		default:
			num = 50;
			type2 = 2;
			break;
		}
		if (type <= 2)
		{
			SCGlobals.levelstat.coins++;
		}
		else if (type <= 6)
		{
			SCGlobals.levelstat.gems++;
		}
		if (num > 0)
		{
			SCGlobals.supercowgame.AddScore(pos, num);
		}
		Particles.AddParticleSource(new vec3_t(pos.x, pos.y, 0f), type2);
		Particles.AddDynamicParticle(pos, 26, 1, -1);
	}
}
