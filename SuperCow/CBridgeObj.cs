using System;

namespace SuperCow;

public class CBridgeObj : CLevelObj
{
	public float[] fsegments = new float[128];

	public float[] fsegments1 = new float[128];

	public int bridge_sound_index;

	public string str_bridge_sound;

	public float bridge_sound_timer;

	public override void Init()
	{
		base.Init();
		int1 = 0;
	}

	public float GetDeltaY()
	{
		float num = (float)p_texture.width / 32f;
		float num2 = (float)SCGlobals.p_bridge1.width / 32f;
		int num3 = (int)((sizex - 2f * num) / num2);
		int1 = (int)(SCGlobals.hero.position.x - (rect.x0 + num));
		if (int1 <= 0)
		{
			int1 = 0;
		}
		if (int1 == 2)
		{
			int1 = 1;
		}
		if (int1 >= num3)
		{
			int1 = 0;
		}
		if (int1 == num3 - 1)
		{
			int1 = num3 - 2;
		}
		return fsegments[int1];
	}

	public override void DrawStatic(dcolor_t dc)
	{
	}

	public override void Draw(dcolor_t dc)
	{
		if (!b_offscreen)
		{
			dc.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			if (b_selected)
			{
				dc.assign(byte.MaxValue, 0, 0, byte.MaxValue);
			}
			frect_t vc = default(frect_t);
			float num = (float)p_texture.width / 32f;
			CowRender.BindTexture(p_texture);
			vc.assign(rect.x0, rect.y0, rect.x0 + num, rect.y1);
			CowRender.DrawColorQuad2DDynBuffer(vc, new frect_t(0f, 0f, 1f, 1f), dc, 0f);
			vc.assign(rect.x1 - num, rect.y0, rect.x1, rect.y1);
			CowRender.DrawColorQuad2DDynBuffer(vc, new frect_t(1f, 0f, 0f, 1f), dc, 0f);
			CowRender.EnableRS(7u);
			CowRender.BindTexture(SCGlobals.p_bridge1);
			float num2 = (float)SCGlobals.p_bridge1.width / 32f;
			int num3 = (int)((sizex - 2f * num) / num2);
			for (int i = 0; i < num3; i++)
			{
				float num4 = fsegments[i] * SCGlobals.frameinterp + fsegments1[i] * (1f - SCGlobals.frameinterp);
				float num5 = fsegments[i + 1] * SCGlobals.frameinterp + fsegments1[i + 1] * (1f - SCGlobals.frameinterp);
				vc.assign(rect.x0 + num + num2 * (float)i, rect.y0, rect.x0 + num + num2 * (float)(i + 1), rect.y1);
				pl_p1.assign(vc.x1, vc.y1 - num5, 0f);
				pl_p2.assign(vc.x1, vc.y0 - num5, 0f);
				pl_p3.assign(vc.x0, vc.y0 - num4, 0f);
				pl_p4.assign(vc.x0, vc.y1 - num4, 0f);
				pl_t1.assign(1f, 1f);
				pl_t2.assign(1f, 0f);
				pl_t3.assign(0f, 0f);
				pl_t4.assign(0f, 1f);
				CowRender.DrawColorQuadDynBuffer(pl_p1, pl_p2, pl_p3, pl_p4, pl_t1, pl_t2, pl_t3, pl_t4, dc);
			}
			CowRender.DisableRS(7u);
		}
	}

	public override void Physics()
	{
		if (b_offscreen)
		{
			return;
		}
		float num = (float)p_texture.width / 32f;
		float num2 = (float)SCGlobals.p_bridge1.width / 32f;
		int num3 = (int)((sizex - 2f * num) / num2);
		frect_t frect_t2 = rect;
		frect_t2.translate(0f, -1f);
		if (frect_t2.isPointInRect(new vec2_t(SCGlobals.hero.position)))
		{
			int1 = (int)(SCGlobals.hero.position.x - (rect.x0 + num));
			if (int1 <= 0)
			{
				int1 = 0;
			}
			if (int1 == 2)
			{
				int1 = 1;
			}
			if (int1 >= num3)
			{
				int1 = 0;
			}
			if (int1 == num3 - 1)
			{
				int1 = num3 - 2;
			}
			if (int1 != 0 && SCGlobals.hero.behavior != behaviors_t.BT_STAND && SCGlobals.hero.behavior != behaviors_t.BT_SITDOWN)
			{
				if (obj_time < 1000f)
				{
					obj_time2 = 0f;
				}
				obj_time = 2000f;
				if (bridge_sound_timer <= 0f)
				{
					str_bridge_sound = "bridge" + (bridge_sound_index + 1);
					bridge_sound_index = (bridge_sound_index + 1) % 2;
					bridge_sound_timer = 1000f;
					Sound.Addsound(str_bridge_sound);
				}
				int num4 = int1;
				if (num4 > num3 / 2)
				{
					num4 = num3 - num4;
				}
				float1 = (float)num4 / (float)(num3 / 2);
			}
		}
		else
		{
			int1 = 0;
		}
		float num5 = (float)num3 / 30f;
		float num6 = obj_time / 2000f;
		num5 *= 1f - num6 * float1 * (0.5f * (float)Math.Sin(obj_time2 * 0.008f) - 0.5f);
		obj_time -= SCGlobals.engine_frequency;
		if (obj_time < 0f)
		{
			obj_time = 0f;
		}
		obj_time2 += SCGlobals.engine_frequency;
		for (int i = 0; i < 128; i++)
		{
			fsegments1[i] = fsegments[i];
		}
		for (int j = 0; j < num3; j++)
		{
			float num7 = (float)j / (float)num3;
			float num8 = -4f * num5 * num7 * (num7 - 1f);
			num7 = (float)(j + 1) / (float)num3;
			float num9 = -4f * num5 * num7 * (num7 - 1f);
			fsegments[j] = (num8 + num9) / 2f;
		}
		if (bridge_sound_timer > 0f)
		{
			bridge_sound_timer -= SCGlobals.engine_frequency;
		}
	}
}
