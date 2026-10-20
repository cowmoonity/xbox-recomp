using System;

namespace SuperCow;

internal static class Weapons
{
	public enum BULLID
	{
		FROG_BULLID,
		GOAT_BULLID,
		EGG_BULLID,
		MOLE_BULLID,
		PROF_BULLID,
		NUM_WEAPONS
	}

	public struct bullet_t
	{
		public bool b_work;

		public vec2_t pos;

		public vec2_t dir;

		public float power;

		public float r;

		public float t;

		public int njumps;

		public int type;

		public int subtype;

		public float ftimer;
	}

	private static int NUM_BULLETS = 64;

	private static float FROG_BULLET_POWER = 20f;

	public static bullet_t[] ar_bullets = new bullet_t[NUM_BULLETS];

	public static int numbullets = 0;

	public static void InitBullets()
	{
		Array.Clear(ar_bullets, 0, NUM_BULLETS);
	}

	public static void AddBullet(vec2_t pos, vec2_t dir, float power, int type, int subtype)
	{
		bullet_t bullet_t = ar_bullets[numbullets];
		bullet_t.dir = dir * 3f;
		bullet_t.pos = pos;
		bullet_t.b_work = true;
		bullet_t.t = 0f;
		bullet_t.power = power;
		bullet_t.type = type;
		bullet_t.r = 1f;
		bullet_t.ftimer = 0f;
		bullet_t.njumps = 0;
		bullet_t.subtype = subtype;
		numbullets = (numbullets + 1) % NUM_BULLETS;
		vec2_t vec2_t2 = new vec2_t(SCGlobals.hero.position);
		if ((vec2_t2 - pos).len() < 25f)
		{
			switch (type)
			{
			case 0:
				Sound.Addsound("frog");
				break;
			case 1:
				Sound.Addsound("cabbage");
				break;
			case 3:
				Sound.Addsound("molestone");
				break;
			case 4:
				Sound.Addsound("profretort");
				break;
			}
		}
		ar_bullets[numbullets] = bullet_t;
	}

	public static void DrawBullets()
	{
		dcolor_t color = default(dcolor_t);
		vec2_t vec2_t2 = default(vec2_t);
		vec2_t vec2_t3 = default(vec2_t);
		new vec3_t(0f, 0f, 1f);
		for (int i = 0; i < NUM_BULLETS; i++)
		{
			if (ar_bullets[i].b_work)
			{
				bullet_t bullet_t = ar_bullets[i];
				vec2_t2 = bullet_t.pos + bullet_t.dir * SCGlobals.frameinterp;
				float num = bullet_t.t * 0.01f;
				color.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				switch (bullet_t.type)
				{
				case 0:
				{
					vec2_t3.assign((float)Math.Sin(num), (float)Math.Cos(2f * num));
					float size = 1.3f + 0.3f * (float)Math.Sin(num);
					Particles.AddParticle(vec2_t2 + vec2_t3 * 0.2f, (0f - num) * 9f, size, color, 2, 21);
					vec2_t3.assign((float)Math.Sin(2f * num), (float)Math.Cos(num));
					float size2 = 1.3f + 0.3f * (float)Math.Sin(num + (float)Math.PI);
					Particles.AddParticle(vec2_t2 + vec2_t3 * 0.2f, (0f - num) * 9f, size2, color, 2, 21);
					color.assign(225, byte.MaxValue, byte.MaxValue, byte.MaxValue);
					Particles.AddParticle(vec2_t2, (0f - num) * 14f, 4f, color, 0, 12);
					break;
				}
				case 1:
					Particles.AddParticle(vec2_t2, (0f - num) * 30f, 2f, color, 2, 5);
					break;
				case 2:
					Particles.AddParticle(vec2_t2, num * 130f, 1f, color, 2, 35);
					break;
				case 3:
					Particles.AddParticle(vec2_t2, num * 30f, 1.4f, color, 2, 41);
					break;
				case 4:
					Particles.AddParticle(vec2_t2, num * 30f + 90f, 1.4f, color, 2, 39);
					break;
				}
				bullet_t.t += SCGlobals.ftickperframe;
			}
		}
	}

	public static void CrashBullet(ref bullet_t pb)
	{
		pb.b_work = false;
		if (SCGlobals.viewport_rect.isPointInRect(pb.pos))
		{
			switch (pb.type)
			{
			case 0:
				Sound.Addsound("frogcrash");
				Particles.AddDynamicParticle(pb.pos, 4, 7, 21);
				break;
			case 1:
				Sound.Addsound("cabbagecrash");
				Particles.AddDynamicParticle(pb.pos, 4, 7, 21);
				break;
			case 2:
				Sound.Addsound("birdeggcrash");
				Particles.AddDynamicParticle(pb.pos, 4, 7, 36);
				break;
			case 3:
				Sound.Addsound("molestonecrash");
				Particles.AddDynamicParticle(pb.pos, 4, 7, 42);
				break;
			case 4:
				Particles.AddDynamicParticle(pb.pos, 4, 7, 40);
				break;
			}
		}
	}

	public static void PhysicsBullets()
	{
		vec3_t vec3_t2 = default(vec3_t);
		for (int i = 0; i < NUM_BULLETS; i++)
		{
			if (!ar_bullets[i].b_work)
			{
				continue;
			}
			bullet_t pb = ar_bullets[i];
			vec2_t vec2_t2 = pb.pos + pb.dir;
			pb.ftimer += SCGlobals.engine_frequency;
			for (int j = 0; j < SCGlobals.numobstacles; j++)
			{
				obstacle_t obstacle_t2 = SCGlobals.ar_obstacles[j];
				if (obstacle_t2.obsflag == obstacle_flags_t.TRANS_OBF || !obstacle_t2.rect.isPointInRect(vec2_t2))
				{
					continue;
				}
				switch (pb.type)
				{
				case 0:
					if (pb.njumps < 3)
					{
						pb.njumps++;
						pb.dir.y *= -0.5f;
						if (SCGlobals.viewport_rect.isPointInRect(pb.pos))
						{
							Sound.Addsound("frogspitjump");
						}
						Particles.AddDynamicParticle(vec2_t2 + new vec2_t(0f, 1f), 21, 5, 21);
					}
					else
					{
						CrashBullet(ref pb);
					}
					break;
				case 2:
					CrashBullet(ref pb);
					break;
				case 4:
					if (pb.ftimer > 600f)
					{
						CrashBullet(ref pb);
						AddMonsterFromBullet(pb.subtype, pb.pos);
					}
					break;
				}
			}
			if (SCGlobals.hero.bound_rect.isPointInRect(vec2_t2) && SCGlobals.hero.IsPunishable())
			{
				SCGlobals.hero.Punish(pb.power);
				CrashBullet(ref pb);
			}
			pb.pos += pb.dir;
			switch (pb.type)
			{
			case 0:
				pb.dir.y -= 0.025f * SCGlobals.physics_speed;
				break;
			case 1:
				pb.dir.y -= 0.02f * SCGlobals.physics_speed;
				break;
			case 2:
			case 3:
			case 4:
				pb.dir.y -= 0.01f * SCGlobals.physics_speed;
				break;
			}
			switch (pb.type)
			{
			case 0:
				Particles.AddDynamicParticle(vec2_t2 - pb.dir * 3f, 21, 1, 21);
				break;
			case 1:
				Particles.AddDynamicParticle(vec2_t2.v3t() + vec3_t2, 26, 1, -1);
				break;
			}
			ar_bullets[i] = pb;
		}
	}

	public static void AddMonsterFromBullet(int id, vec2_t pos)
	{
		int id2 = 0;
		obstacle_t obstacleByPoint = SuperCowGame.GetObstacleByPoint(pos, -1f);
		if (obstacleByPoint.p_levelobj == null || obstacleByPoint.b_triangle || obstacleByPoint.obsflag == obstacle_flags_t.RIGID_OBF)
		{
			id = 0;
		}
		switch (id)
		{
		case 0:
			id2 = 11;
			break;
		case 1:
			id2 = 10;
			pos.y -= 1f;
			break;
		case 2:
			id2 = 3;
			break;
		case 3:
			id2 = 2;
			break;
		}
		CMonster cMonster = SCGlobals.pershost.AddMonster(id2);
		cMonster.position.assign(pos.x, pos.y, 0f);
		SCGlobals.supercowgame.SetupMonster(cMonster);
		SCGlobals.levelstat.nummonsters++;
	}
}
