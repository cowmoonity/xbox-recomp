using System;

namespace SuperCow;

public class CFruitTree : CLevelObj
{
	public vec2_t[] ar_fruitspos = new vec2_t[11];

	public vec2_t[] ar_leavespos = new vec2_t[11];

	public uint falldown;

	private int NUM_LEAVES = 11;

	public override void Init()
	{
		b_switch = 0;
		b_active = byte.MaxValue;
		iauxvar = 0;
		obj_time = 0f;
		obj_time2 = 0f;
		float1 = 0f;
		int1 = 0;
		position = startpos;
		frandom1 = CRand.frand(1f);
		frandom2 = CRand.frand(1f);
		falldown = 0u;
		CRand.randomize2((int)(Math.Abs(frandom1) * 10f));
		vec2_t vec2_t2 = default(vec2_t);
		vec2_t vec2_t3 = new vec2_t(0f, 2f);
		CRand.frand2(180f);
		for (int i = 0; i < NUM_LEAVES - 3; i++)
		{
			float angle = 360f / (float)(NUM_LEAVES - 3) * (float)i - 90f;
			vec2_t2.assign(0.8f * SuperCowHelper.fast_cos(angle), 1.5f * SuperCowHelper.fast_sin(angle));
			float num = 1.9f + CRand.prand2(1f);
			if (i == 0)
			{
				num = 0f;
			}
			vec2_t vec2_t4 = vec2_t3 + vec2_t2 * num;
			ar_leavespos[i] = vec2_t4;
			ref vec2_t reference = ref ar_fruitspos[i];
			reference = vec2_t4 + new vec2_t(CRand.frand2(0.3f), CRand.frand2(0.3f));
		}
		for (int j = 0; j < 3; j++)
		{
			ar_leavespos[j + NUM_LEAVES - 3] = default(vec2_t);
			ref vec2_t reference2 = ref ar_fruitspos[j + NUM_LEAVES - 3];
			reference2 = ar_leavespos[j] + new vec2_t(CRand.frand2(0.4f), CRand.prand2(0.4f));
		}
	}

	public override void Touch()
	{
		if (SCGlobals.hero.behavior == behaviors_t.BT_WALK)
		{
			float num = (1f - Math.Abs(position.x - SCGlobals.hero.position.x) / (sizex * 0.5f)) * 2000f;
			if (num > obj_time2)
			{
				obj_time2 = num;
			}
		}
	}

	public override void Physics()
	{
		if (!SCGlobals.hero.b_landing || !SCGlobals.ar_obstacles[p_obstacle_idx].rect.isPointInRect(SCGlobals.hero.position))
		{
			return;
		}
		if (SCGlobals.hero.fall_value < 9f)
		{
			obj_time2 = 2200f;
			return;
		}
		float num = (SCGlobals.hero.fall_value - 9f) / 10f;
		if (num > 1f)
		{
			num = 1f;
		}
		obj_time2 = 2700f;
		int num2 = (int)(num * (float)(NUM_LEAVES - 3));
		for (int i = 0; i < num2; i++)
		{
			int num3 = (int1 + i) % NUM_LEAVES;
			if ((falldown & (1 << num3)) == 0)
			{
				falldown |= (uint)(1 << num3);
				vec2_t pos = ar_fruitspos[num3] + position;
				obstacle_t anyObstacleByPoint = SuperCowGame.GetAnyObstacleByPoint(new vec2_t(position.x, position.y), b_nontriangle: true);
				float num4 = 0f;
				Bonus.AddFruit(endy: ((anyObstacleByPoint.p_levelobj == null) ? (-1024f) : anyObstacleByPoint.rect.y0) + 0.5f, pos: pos, size: (num3 >= NUM_LEAVES - 3) ? 0.6f : 1f, p_texture: SCGlobals.ar_fruittexs[objsubtype]);
			}
		}
		int1 = (int1 + num2) % NUM_LEAVES;
	}

	public override void Draw(dcolor_t dc)
	{
		if (b_offscreen)
		{
			return;
		}
		frect_t tc = new frect_t(0f, 0f, 1f, 1f);
		if (obj_time2 > 0f)
		{
			obj_time2 -= SCGlobals.ftickperframe;
		}
		obj_time += SCGlobals.ftickperframe;
		if (b_selected)
		{
			dc.assign(byte.MaxValue, 0, 0, byte.MaxValue);
		}
		CowRender.BindTexture(p_texture);
		CowRender.DrawColorQuad2DDynBuffer(rect, tc, dc, 0f);
		CowRender.EnableRS(7u);
		CowRender.EnableRS(4u);
		CowRender.BindTexture(SCGlobals.ar_fruittexs[objsubtype]);
		CRand.randomize2((int)(Math.Abs(frandom1) * 10f));
		_ = position + new vec2_t(0f, 2f);
		for (int i = 0; i < NUM_LEAVES; i++)
		{
			_ = ar_leavespos[i] + position;
			float num = 1.1f;
			float num2 = ((!(obj_time2 > 0f)) ? 2f : (2f + 20f * obj_time2 / 2000f));
			float angle = num2 * (float)Math.Sin(obj_time * 0.01f + CRand.frand2((float)Math.PI));
			vec2_t vec2_t2 = ar_fruitspos[i] + position;
			if ((falldown & (1 << i)) == 0)
			{
				if (i >= NUM_LEAVES - 3)
				{
					num *= 0.6f;
				}
				CowRender.DrawRotatedColorQuad2DDynBuffer(vec2_t2, vec2_t2 + new vec2_t(0f, num * 0.5f), angle, num, num, SCGlobals.tc_normal, dcolor_t.ColorWhite, 0f);
			}
		}
		CowRender.DisableRS(7u);
	}
}
