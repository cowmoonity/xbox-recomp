using System;
using System.Globalization;

namespace SuperCow;

public class CGuillotineObj : CLevelObj
{
	public frect_t strike_rect;

	public override void Init()
	{
		base.Init();
		int1 = 0;
		strike_rect = rect;
		strike_rect.y1 = strike_rect.y0;
		string text = "";
		for (int i = 0; i < extratextparam.Length; i++)
		{
			if (extratextparam[i] >= '0' && extratextparam[i] <= '9')
			{
				text += extratextparam[i];
			}
		}
		if (text.Length > 0)
		{
			float1 = float.Parse(text, CultureInfo.InvariantCulture);
		}
		else
		{
			float1 = 1f;
		}
	}

	public override void Draw(dcolor_t dc)
	{
		CalulateSound();
		if (b_offscreen)
		{
			return;
		}
		dc = dcolor_t.ColorWhite;
		if (b_selected)
		{
			dc.c1 = 0;
			dc.c2 = 0;
		}
		CowRender.BindTexture(p_texture);
		int width = (int)p_texture.width;
		float num = (float)p_texture.width / 32f;
		_ = (float)width / 32f;
		if (SCGlobals.b_le_active)
		{
			frame_rect = rect;
		}
		else if (obj_time2 == 0f)
		{
			frame_rect = rect;
			frame_rect.y1 = frame_rect.y0;
		}
		else
		{
			frame_rect = rect;
			frame_rect.y1 = strike_rect.y1;
		}
		frect_t vc = default(frect_t);
		int num2 = (int)Math.Ceiling((frame_rect.y0 - frame_rect.y1) * 32f / (float)width);
		int num3 = (int)p_texture.height / width;
		frect_t tc = default(frect_t);
		for (int i = 0; i < num2; i++)
		{
			int num4 = ((i < num3 - 1) ? i : (i % (num3 - 2) + 1));
			vc.assign(frame_rect.x0, frame_rect.y1 + num * (float)(i + 1), frame_rect.x1, frame_rect.y1 + num * (float)i);
			tc.assign(0f, 1f - (float)(num4 + 1) * 0.25f, 1f, 1f - (float)num4 * 0.25f);
			if (vc.y0 > frame_rect.y0)
			{
				tc.y0 += 0.25f * (vc.y0 - frame_rect.y0) / num;
				vc.y0 = frame_rect.y0;
			}
			CowRender.DrawColorQuad2DDynBuffer(vc, tc, dc, 0f);
		}
		int num5 = ((!(name == "guillotine")) ? 1 : 0);
		CowRender.BindTexture(SCGlobals.p_guillo_up[num5]);
		CowRender.DrawColorQuad2DDynBuffer(new vec2_t(position.x, rect.y0), 2f, 2f, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		CowRender.BindTexture(SCGlobals.p_guillo_down[num5]);
		CowRender.DrawColorQuad2DDynBuffer(new vec2_t(position.x, rect.y1), 2f, 2f, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
	}

	public override void Physics()
	{
		if (obj_time2 > 0f)
		{
			float num = obj_time2 / 700f;
			strike_rect = rect;
			strike_rect.x0 += sizex * 0.2f;
			strike_rect.x1 -= sizex * 0.2f;
			strike_rect.y1 = strike_rect.y0 - rect.sizey() * (float)Math.Sin(num * (float)Math.PI);
			if (strike_rect.isRectTouchRect(SCGlobals.hero.bound_rect))
			{
				SCGlobals.hero.Punish(50f);
			}
			obj_time2 += SCGlobals.engine_frequency;
			if (!(obj_time2 > 700f))
			{
				return;
			}
			obj_time2 = 0f;
		}
		obj_time += SCGlobals.engine_frequency;
		if (!(obj_time > 1000f * float1))
		{
			return;
		}
		if (!b_offscreen)
		{
			if (name == "guillotine")
			{
				Sound.Addsound("guillo1");
			}
			else
			{
				Sound.Addsound("guillo2");
			}
		}
		obj_time = 0f;
		obj_time2 = 1f;
	}

	public float CalulateSound()
	{
		if (SCGlobals.b_le_active)
		{
			return 0f;
		}
		float num = ((SCGlobals.hero.position.x <= rect.x1 && SCGlobals.hero.position.x >= rect.x0) ? 0f : ((!(SCGlobals.hero.position.x < rect.x0)) ? (SCGlobals.hero.position.x - rect.x1) : (rect.x0 - SCGlobals.hero.position.x)));
		float num2 = ((!(num > 18f)) ? (100f * (1f - num / 18f)) : 0f);
		float num3 = ((SCGlobals.hero.position.y <= rect.y0 && SCGlobals.hero.position.y >= rect.y1) ? 0f : ((!(SCGlobals.hero.position.y > rect.y0)) ? (rect.y1 - SCGlobals.hero.position.y) : (SCGlobals.hero.position.y - rect.y0)));
		num2 = ((!(num3 > 12f)) ? (num2 * (1f - num3 / 12f)) : 0f);
		if (num2 == 0f)
		{
			return 0f;
		}
		_ = name == "guillotine";
		return num2;
	}
}
