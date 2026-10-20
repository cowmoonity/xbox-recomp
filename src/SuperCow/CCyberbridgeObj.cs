using System;

namespace SuperCow;

public class CCyberbridgeObj : CLevelObj
{
	public float bridgelen;

	public override void Draw(dcolor_t dc)
	{
		if (b_offscreen)
		{
			return;
		}
		dc.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		if (b_selected)
		{
			dc.assign(byte.MaxValue, 0, 0, byte.MaxValue);
		}
		CowRender.BindTexture(p_texture);
		rect.p0.v3t();
		int height = (int)p_texture.height;
		float num = (float)height / 32f;
		_ = (float)p_texture.height / 32f;
		if (p_obstacle_idx != -1)
		{
			frame_rect = SCGlobals.ar_obstacles[p_obstacle_idx].rect;
		}
		else
		{
			frame_rect = rect;
		}
		frect_t vc = default(frect_t);
		int num2 = (int)Math.Ceiling((frame_rect.x1 - frame_rect.x0) * 32f / (float)height);
		int num3 = (int)p_texture.width / height;
		frect_t tc = default(frect_t);
		for (int i = 0; i < num2; i++)
		{
			if (endpos.x < position.x)
			{
				int num4 = ((i < num3 - 1) ? i : (i % (num3 - 2) + 1));
				vc.x0 = frame_rect.x0 + num * (float)i;
				vc.y0 = frame_rect.y0;
				vc.x1 = frame_rect.x0 + num * (float)(i + 1);
				vc.y1 = frame_rect.y1;
				vc.x0 = (float)num4 * 0.25f;
				vc.y0 = 0f;
				vc.x1 = (float)(num4 + 1) * 0.25f;
				vc.y1 = 1f;
				if (vc.x1 > frame_rect.x1)
				{
					tc.x1 -= 0.25f * (vc.x1 - frame_rect.x1) / num;
					vc.x1 = frame_rect.x1;
				}
			}
			else
			{
				int num4 = ((i < num3 - 1) ? i : (i % (num3 - 2) + 1));
				vc.x0 = frame_rect.x1 - num * (float)(i + 1);
				vc.y0 = frame_rect.y0;
				vc.x1 = frame_rect.x1 - num * (float)i;
				vc.y1 = frame_rect.y1;
				vc.x0 = 1f - (float)(num4 + 1) * 0.25f;
				vc.y0 = 0f;
				vc.x1 = 1f - (float)num4 * 0.25f;
				vc.y1 = 1f;
				if (vc.x0 < frame_rect.x0)
				{
					tc.x0 += 0.25f * (frame_rect.x0 - vc.x0) / num;
					vc.x0 = frame_rect.x0;
				}
			}
			CowRender.DrawColorQuad2DDynBuffer(vc, tc, dc, 0f);
		}
	}

	public override void Physics()
	{
		obj_time += SCGlobals.engine_frequency;
		bridgelen -= float1 * 0.2f;
		if (bridgelen <= 0f)
		{
			bridgelen = 0f;
			float1 *= -1f;
		}
		if (bridgelen >= sizex)
		{
			bridgelen = sizex;
			float1 *= -1f;
		}
		float num = 1f;
		if (endpos.x < position.x)
		{
			SCGlobals.ar_obstacles[p_obstacle_idx].rect.x0 = SCGlobals.ar_obstacles[p_obstacle_idx].rect.x1 - bridgelen;
		}
		else
		{
			SCGlobals.ar_obstacles[p_obstacle_idx].rect.x1 = SCGlobals.ar_obstacles[p_obstacle_idx].rect.x0 + bridgelen;
			num = -1f;
		}
		if (SCGlobals.ar_obstacles[p_obstacle_idx].rect.isPointInRect(SCGlobals.hero.position))
		{
			SCGlobals.hero.forcedmove.assign(float1 * 0.2f * num, 0f, 0f);
		}
	}
}
