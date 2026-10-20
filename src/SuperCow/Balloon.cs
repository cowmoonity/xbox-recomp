using System;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public static class Balloon
{
	public static int MENU_ITEM_SIZE = 64;

	public static bool visible = false;

	public static string str_balloon;

	public static string str_balloon2;

	public static string str_prevballoon;

	public static float fballoon_time = 0f;

	public static vec2_t balloon_pos;

	public static vec2_t prevballoon_pos = new vec2_t(-1000f, 1000f);

	public static float fballoon_sizex;

	public static float fballoon_angle;

	public static bool b_balloon_set = false;

	public static bool b_balloonarrow;

	public static bool b_balloongrow;

	public static bool b_balloonbutton;

	public static int balloonanimnumber;

	public static frect_t balloon_button_rect;

	public static void SetBalloon(vec2_t pos, string str, float sizex, bool b_arrow, float angle, bool b_grow)
	{
		visible = true;
		fballoon_sizex = sizex;
		balloon_pos = pos;
		str_balloon = str;
		b_balloon_set = true;
		b_balloonarrow = b_arrow;
		fballoon_angle = angle;
		b_balloongrow = b_grow;
		b_balloonbutton = false;
		balloonanimnumber = -1;
	}

	public static void SetBalloonTipWithButton(vec2_t pos, string str, float sizex, bool b_arrow, float angle, bool b_grow)
	{
		fballoon_sizex = sizex;
		balloon_pos = pos;
		str_balloon = str;
		b_balloon_set = true;
		b_balloonarrow = b_arrow;
		fballoon_angle = angle;
		b_balloongrow = b_grow;
		b_balloonbutton = true;
		balloonanimnumber = -1;
	}

	public static void DrawBalloonRect(frect_t rect)
	{
		vec2_t vec2_t2 = default(vec2_t);
		vec2_t vec2_t3 = default(vec2_t);
		vec2_t p = rect.p0;
		vec2_t2.assign(rect.x1, rect.y0);
		vec2_t3.assign(rect.x0, rect.y1);
		vec2_t p2 = rect.p1;
		CowRender.BindTexture(SCGlobals.p_white);
		dcolor_t c = new dcolor_t(75, 25, 0, 179);
		CowRender.DrawColorQuad2D((p + vec2_t2) / 2f - new vec2_t(0f, MENU_ITEM_SIZE / 4), vec2_t2.x - p.x - (float)MENU_ITEM_SIZE, MENU_ITEM_SIZE / 2, new frect_t(0f, 0f, 1f, 1f), c, 0f);
		CowRender.DrawColorQuad2D((vec2_t3 + p2) / 2f + new vec2_t(0f, MENU_ITEM_SIZE / 4), p2.x - vec2_t3.x - (float)MENU_ITEM_SIZE, MENU_ITEM_SIZE / 2, new frect_t(0f, 0f, 1f, 1f), c, 0f);
		CowRender.DrawColorQuad2D((p + vec2_t3) / 2f + new vec2_t(MENU_ITEM_SIZE / 4, 0f), MENU_ITEM_SIZE / 2, p.y - vec2_t3.y - (float)MENU_ITEM_SIZE, new frect_t(0f, 0f, 1f, 1f), c, 0f);
		CowRender.DrawColorQuad2D((vec2_t2 + p2) / 2f - new vec2_t(MENU_ITEM_SIZE / 4, 0f), MENU_ITEM_SIZE / 2, vec2_t2.y - p2.y - (float)MENU_ITEM_SIZE, new frect_t(0f, 0f, 1f, 1f), c, 0f);
		frect_t vc = rect;
		vc.assign(vc.pos(), vc.sizex() - (float)MENU_ITEM_SIZE, vc.sizey() - (float)MENU_ITEM_SIZE);
		CowRender.DrawColorQuad2D(vc, SCGlobals.tc_normal, c, 0f);
		CowRender.EnableRS(5u);
		CowRender.BindTexture(SCGlobals.p_menuballoon);
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		if (b_balloongrow)
		{
			// TODO: alpha test not implemented - 160f
		}
		CowRender.DrawColorQuad2D(p, MENU_ITEM_SIZE, MENU_ITEM_SIZE, SCGlobals.ar_wintexcoords[0], new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		CowRender.DrawColorQuad2D(vec2_t2, MENU_ITEM_SIZE, MENU_ITEM_SIZE, SCGlobals.ar_wintexcoords[1], new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		CowRender.DrawColorQuad2D(vec2_t3, MENU_ITEM_SIZE, MENU_ITEM_SIZE, SCGlobals.ar_wintexcoords[2], new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		CowRender.DrawColorQuad2D(p2, MENU_ITEM_SIZE, MENU_ITEM_SIZE, SCGlobals.ar_wintexcoords[3], new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		CowRender.DrawColorQuad2D((p + vec2_t2) / 2f, vec2_t2.x - p.x - (float)MENU_ITEM_SIZE, MENU_ITEM_SIZE, SCGlobals.ar_wintexcoords[4], new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		CowRender.DrawColorQuad2D((vec2_t3 + p2) / 2f, p2.x - vec2_t3.x - (float)MENU_ITEM_SIZE, MENU_ITEM_SIZE, SCGlobals.ar_wintexcoords[4], new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		CowRender.DrawColorQuad2D((p + vec2_t3) / 2f, MENU_ITEM_SIZE, p.y - vec2_t3.y - (float)MENU_ITEM_SIZE, SCGlobals.ar_wintexcoords[5], new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		CowRender.DrawColorQuad2D((vec2_t2 + p2) / 2f, MENU_ITEM_SIZE, vec2_t2.y - p2.y - (float)MENU_ITEM_SIZE, SCGlobals.ar_wintexcoords[5], new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		// TODO: alpha test not implemented - 1f
		CowRender.DisableRS(5u);
	}

	public static void DrawBalloonRectEx(frect_t rect)
	{
		dcolor_t dcolor_t2 = default(dcolor_t);
		float num = rect.sizex();
		float num2 = rect.sizey();
		vec2_t vec2_t2 = rect.pos();
		dcolor_t2.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		CowRender.BindTexture(SCGlobals.p_menuballoon);
		vec2_t p = vec2_t2;
		p.x -= num * 0.5f;
		CowRender.DrawColorQuad2D(p, MENU_ITEM_SIZE, num2, SCGlobals.ar_wintexcoords[5], new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		p = vec2_t2;
		p.x += num * 0.5f;
		CowRender.DrawColorQuad2D(p, MENU_ITEM_SIZE, num2, SCGlobals.ar_wintexcoords[5], new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		p = vec2_t2;
		p.y += num2 * 0.5f;
		CowRender.DrawColorQuad2D(p, num, MENU_ITEM_SIZE, SCGlobals.ar_wintexcoords[4], new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		p = vec2_t2;
		p.y -= num2 * 0.5f;
		CowRender.DrawColorQuad2D(p, num, MENU_ITEM_SIZE, SCGlobals.ar_wintexcoords[4], new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
	}

	public static void DrawCurrentBalloon()
	{
		float num = fballoon_sizex;
		matrix_t m = default(matrix_t);
		m.load_identity();
		float num2 = fballoon_time / 250f;
		if (num2 > 1f)
		{
			num2 = 1f;
		}
		float num3 = 1f;
		if (SCGlobals.supercowgame != null)
		{
			num3 = SCGlobals.supercowgame.le_scale;
		}
		m.translate(balloon_pos.x / num3, balloon_pos.y / num3, 0f);
		if (b_balloongrow)
		{
			m.scale(num2, num2, num2);
		}
		CowRender.LoadMatrix(m);
		if (!b_balloongrow)
		{
			CowRender.SetGlobal2DColor(new color_t(1f, 1f, 1f, num2));
		}
		if (balloon_pos.len() < 1f)
		{
			balloon_pos.x += 1f;
		}
		float stringWidth = Text.GetStringWidth(str_balloon, 11f);
		if (stringWidth < num - (float)MENU_ITEM_SIZE)
		{
			num = stringWidth + (float)MENU_ITEM_SIZE;
		}
		float num4 = Text.PrepareWindowText(num - (float)(MENU_ITEM_SIZE / 2), str_balloon, 11f) + (float)(MENU_ITEM_SIZE / 2);
		float num5 = num4;
		if (num5 < (float)MENU_ITEM_SIZE)
		{
			num5 = MENU_ITEM_SIZE;
		}
		if (b_balloonbutton)
		{
			num5 += (float)MENU_ITEM_SIZE * 0.7f;
		}
		vec2_t vec2_t2 = -balloon_pos;
		if (fballoon_angle > -1000f)
		{
			vec2_t2.assign(SuperCowHelper.fast_cos(fballoon_angle), SuperCowHelper.fast_sin(fballoon_angle));
		}
		vec2_t2.normalize();
		vec2_t vec2_t3 = vec2_t2 * (1.3f + 0.1f * (float)Math.Sin(fballoon_time * 0.01f)) * MENU_ITEM_SIZE;
		float angle = vec2_t2.angle() + 180f;
		vec2_t result = default(vec2_t);
		if (Collision.intersetRayLine(ref result, new vec2_t((0f - num) / 2f, num5 / 2f), new vec2_t(num / 2f, num5 / 2f), new vec2_t(0f, 0f), vec2_t2) < 0f && Collision.intersetRayLine(ref result, new vec2_t((0f - num) / 2f, num5 / 2f), new vec2_t((0f - num) / 2f, (0f - num5) / 2f), new vec2_t(0f, 0f), vec2_t2) < 0f && Collision.intersetRayLine(ref result, new vec2_t((0f - num) / 2f, (0f - num5) / 2f), new vec2_t(num / 2f, (0f - num5) / 2f), new vec2_t(0f, 0f), vec2_t2) < 0f)
		{
			Collision.intersetRayLine(ref result, new vec2_t(num / 2f, num5 / 2f), new vec2_t(num / 2f, (0f - num5) / 2f), new vec2_t(0f, 0f), vec2_t2);
		}
		float num6 = result.len();
		result.normalize();
		num6 += 1.4f * (float)MENU_ITEM_SIZE;
		if (b_balloonarrow)
		{
			CowRender.BindTexture(SCGlobals.p_menuballoon);
			CowRender.DrawRotatedColorQuad2D(vec2_t3, vec2_t3, angle, MENU_ITEM_SIZE * 2, MENU_ITEM_SIZE, new frect_t(0.501f, 0.505f, 0.999f, 0.995f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		}
		frect_t rect = default(frect_t);
		rect.assign(result * num6, num, num5);
		CowRender.EnableRS(1u);
		_ = SuperCow.DeviceManager.GraphicsDevice;
		CowRender.EnableRS(2u);
		DrawBalloonRect(rect);
		float y = ((num5 != (float)MENU_ITEM_SIZE) ? (num5 / 2f - (float)(MENU_ITEM_SIZE / 4) - 11f) : 0f);
		vec2_t3 = rect.pos() + new vec2_t(0f, y);
		Text.SetTextCoordZ(-1f);
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		Text.DrawPreparedTextCenterXY(vec2_t3.x, vec2_t3.y, 11f);
		Text.SetTextCoordZ(45f);
		CowRender.DisableRS(1u);
		CowRender.LoadMatrixNull();
		CowRender.SetGlobal2DColor(new color_t(1f, 1f, 1f, 1f));
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
	}

	public static void DrawCurrentAnimatedBalloon()
	{
		float num = fballoon_sizex;
		matrix_t m = default(matrix_t);
		m.load_identity();
		float num2 = fballoon_time / 250f;
		if (num2 > 1f)
		{
			num2 = 1f;
		}
		m.translate(balloon_pos.x, balloon_pos.y, 0f);
		if (b_balloongrow)
		{
			m.scale(num2, num2, num2);
		}
		CowRender.LoadMatrix(m);
		if (!b_balloongrow)
		{
			CowRender.SetGlobal2DColor(new color_t(1f, 1f, 1f, num2));
		}
		if (balloon_pos.len() < 1f)
		{
			balloon_pos.x += 1f;
		}
		float num3 = ((!(str_balloon2 != "")) ? 0f : (Text.PrepareWindowText(num - (float)(MENU_ITEM_SIZE / 2), str_balloon2, 11f) + (float)(MENU_ITEM_SIZE / 2)));
		float num4 = Text.PrepareWindowText(num - (float)(MENU_ITEM_SIZE / 2), str_balloon, 11f) + (float)(MENU_ITEM_SIZE / 2);
		float num5 = num4 + num3;
		num5 += (float)MENU_ITEM_SIZE * 0.7f;
		num5 += 150f;
		vec2_t vec2_t2 = default(vec2_t);
		vec2_t2.assign(0f, 0f);
		if (balloon_pos.x < 0f)
		{
			vec2_t2.x = 1f;
		}
		else
		{
			vec2_t2.x = -1f;
		}
		vec2_t vec2_t3 = vec2_t2 * (1.3f + 0.1f * (float)Math.Sin(fballoon_time * 0.01f)) * MENU_ITEM_SIZE;
		float angle = vec2_t2.angle() + 180f;
		vec2_t vec2_t4 = default(vec2_t);
		vec2_t4.assign(vec2_t2.x * num / 2f, (0f - balloon_pos.y) / 300f * (num5 / 2f - (float)MENU_ITEM_SIZE));
		float num6 = vec2_t4.len();
		vec2_t4.normalize();
		num6 += 1.4f * (float)MENU_ITEM_SIZE;
		frect_t rect = default(frect_t);
		rect.assign(vec2_t4 * num6, num, num5);
		CowRender.EnableRS(1u);
		_ = SuperCow.DeviceManager.GraphicsDevice;
		CowRender.EnableRS(2u);
		DrawBalloonRect(rect);
		if (b_balloonarrow)
		{
			CowRender.BindTexture(SCGlobals.p_menuballoon);
			CowRender.DrawRotatedColorQuad2D(vec2_t3, vec2_t3, angle, MENU_ITEM_SIZE * 2, MENU_ITEM_SIZE, new frect_t(0.501f, 0.505f, 0.999f, 0.995f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 20f);
		}
		CowRender.DisableRS(1u);
		float y = ((num5 != (float)MENU_ITEM_SIZE) ? (num5 / 2f - (float)(MENU_ITEM_SIZE / 4) - 11f) : 0f);
		vec2_t3 = rect.pos() + new vec2_t(0f, y);
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		Text.DrawPreparedTextCenterXY(vec2_t3.x, vec2_t3.y, 11f);
		if (str_balloon2 != "")
		{
			num3 = Text.PrepareWindowText(num - (float)(MENU_ITEM_SIZE / 2), str_balloon2, 11f);
			Text.DrawPreparedTextCenterXY(vec2_t3.x, rect.pos().y - num5 / 2f + num3 + 50f, 11f);
		}
		_ = balloonanimnumber;
		_ = 0;
		CowRender.LoadMatrixNull();
		CowRender.SetGlobal2DColor(new color_t(1f, 1f, 1f, 1f));
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
	}

	public static void DrawBalloonRects()
	{
		if (str_balloon == null)
		{
			return;
		}
		fballoon_time += SCGlobals.ftickperframe;
		if (str_balloon != str_prevballoon)
		{
			fballoon_time = 0f;
			prevballoon_pos = balloon_pos;
			str_prevballoon = str_balloon;
		}
		if (visible)
		{
			if (balloonanimnumber >= 0)
			{
				DrawCurrentAnimatedBalloon();
			}
			else
			{
				DrawCurrentBalloon();
			}
		}
	}

	public static void DrawBalloobArrow(vec2_t p, float ang, float sizex, float sizey)
	{
		CowRender.BindTexture(SCGlobals.p_menuballoon);
		CowRender.DrawRotatedColorQuad2D(p, p, ang, (float)MENU_ITEM_SIZE * sizex, (float)(MENU_ITEM_SIZE / 2) * sizey, new frect_t(0.501f, 0.505f, 0.999f, 0.995f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 20f);
	}
}
