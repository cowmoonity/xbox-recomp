using System;
using System.Globalization;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public class CLianaObj : CLevelObj
{
	public float fspeed;

	public float web_radius;

	public float web_amplitude;

	public float phi0;

	public float web_angle;

	public float prev_web_angle;

	public float flytime;

	public vec2_t lianaend = default(vec2_t);

	public vec2_t prevlianaend = default(vec2_t);

	public float prevdx;

	public override void Init()
	{
		base.Init();
		int1 = 0;
		if (extratextparam.Length > 0)
		{
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
				fspeed = float.Parse(text, CultureInfo.InvariantCulture);
			}
		}
		else
		{
			fspeed = 1f;
		}
		prevdx = 0f;
		PrepareParamsBeforeDraw();
	}

	public void PrepareParamsBeforeDraw()
	{
		flytime = 0f;
		web_radius = (endpos - startpos).len();
		float num = Math.Abs(startpos.x - endpos.x);
		web_amplitude = (float)Math.Asin(num / web_radius);
		if (web_amplitude > 0.9424779f)
		{
			web_amplitude = 0.9424779f;
		}
		phi0 = ((startpos.x > endpos.x) ? (-(float)Math.PI / 2f) : ((float)Math.PI / 2f));
		web_angle = web_amplitude * (float)Math.Sin(phi0);
		prev_web_angle = web_angle;
	}

	public override void Draw(dcolor_t dc)
	{
		if (SCGlobals.b_le_active)
		{
			PrepareParamsBeforeDraw();
		}
		dc = dcolor_t.ColorWhite;
		if (b_selected)
		{
			dc.c2 = (dc.c1 = 0);
		}
		vec2_t vec2_t2 = prevlianaend * (1f - SCGlobals.frameinterp) + lianaend * SCGlobals.frameinterp;
		vec2_t vec2_t3 = startpos;
		vec2_t vec2_t4 = ((!SCGlobals.b_le_active) ? vec2_t2 : endpos);
		frect_t rc = new frect_t(startpos, vec2_t2);
		if (SCGlobals.viewport_rect.isRectTouchRect(rc))
		{
			float num = web_angle;
			if (!SCGlobals.b_le_active)
			{
				num = prev_web_angle * (1f - SCGlobals.frameinterp) + web_angle * SCGlobals.frameinterp;
			}
			num = num * 180f / (float)Math.PI - 180f;
			CowRender.DrawDynBuffer();
			CowRender.EnableRS(7u);
			CowRender.BindTexture(p_texture);
			CowRender.DrawRotatedColorQuad2D(vec2_t3 + new vec2_t(0f, web_radius / 2f), vec2_t3, num, (float)p_texture.width / 32f, web_radius, new frect_t(0f, 0f, 1f, web_radius * 32f / (float)p_texture.height), dcolor_t.ColorWhite, 0f);
			CowRender.BindTexture(SCGlobals.p_liana_tex_up);
			CowRender.DrawColorQuad2D(vec2_t3, (float)SCGlobals.p_liana_tex_up.width / 32f, (float)SCGlobals.p_liana_tex_up.height / 32f, SCGlobals.tc_normal, dcolor_t.ColorWhite, 0f);
			CowRender.BindTexture(SCGlobals.p_liana_tex_down);
			CowRender.DrawRotatedColorQuad2D(vec2_t4, vec2_t4, num, (float)SCGlobals.p_liana_tex_down.width / 32f, (float)SCGlobals.p_liana_tex_down.height / 32f, SCGlobals.tc_normal, dcolor_t.ColorWhite, 0f);
			GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
			graphicsDevice.Textures[0] = (Texture)(object)SuperCowGame.mainTex;
			CowRender.DisableRS(7u);
		}
	}

	public override void Touch()
	{
	}

	public override void Physics()
	{
		flytime += SCGlobals.physics_speed;
		vec2_t vec2_t2 = default(vec2_t);
		prev_web_angle = web_angle;
		web_angle = web_amplitude * (float)Math.Sin(phi0 + flytime * 0.05f * fspeed);
		vec2_t2.assign((float)Math.Cos(web_angle - (float)Math.PI / 2f), (float)Math.Sin(web_angle - (float)Math.PI / 2f));
		vec2_t2 *= web_radius;
		float num = prevlianaend.x - lianaend.x;
		_ = b_switch;
		prevdx = num;
		b_switch = 0;
		prevlianaend = lianaend;
		lianaend = startpos + vec2_t2;
	}
}
