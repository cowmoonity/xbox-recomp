using System;
using Microsoft.Xna.Framework;

namespace SuperCow;

internal class ComicsScreen : AbstractScreen
{
	public float comics_splash_timer;

	public float fcomicsendtimer;

	public override void Draw()
	{
		base.Draw();
	}

	public override void Activate()
	{
		base.Activate();
	}

	public override void Deactivate()
	{
		base.Deactivate();
	}

	public override void Update(GameTime gameTime)
	{
		SCGlobals.hero.Calculate();
		base.Update(gameTime);
	}

	public void DrawGameMenuComics()
	{
		vec2_t vec2_t2 = default(vec2_t);
		float num = vec2_t2.x;
		float num2 = vec2_t2.y;
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		float font_size = 10f;
		if (SuperCow.IsHDTV)
		{
			Text.OutTextXY(num - 110f, num2 + 10f, font_size, wstrstuff.GetStr("comicsskip"), b_center: true);
		}
		else
		{
			Text.OutTextXY(num, num2, font_size, wstrstuff.GetStr("comicsskip"), b_center: true);
		}
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
	}

	public override void DrawGeometry()
	{
		CowRender.SetTextMode();
		SCGlobals.cam.load_identity();
		SCGlobals.cam.translate(0f, 0f, 10f);
		CowRender.LoadCamera(SCGlobals.cam);
		SCGlobals.supercowgame.DrawBackground2();
		CowRender.SetTextMode();
		CowRender.LoadCamera(SCGlobals.cam);
		CowRender.DisableRS(7u);
		if (PlayerProfile.curlevelstage == 10)
		{
			CowRender.BindTexture(SCGlobals.p_happyendback);
			if (SuperCow.IsHDTV)
			{
				CowRender.DrawColorQuad2D(SCGlobals.p_happyendback.mImage, 0f, 0f, 1067f, 600f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
			}
			else
			{
				CowRender.DrawColorQuad2D(SCGlobals.p_happyendback.mImage, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
			}
		}
		CowRender.EnableRS(7u);
		Comics.CalculateComics();
		Comics.DrawComics();
		DrawGameMenuComics();
		CowRender.LoadMatrixNull();
		CowRender.EnableRS(4u);
		CowRender.SetBlend(0);
		CowRender.DisableRS(7u);
		dcolor_t dcolor_t2 = default(dcolor_t);
		if (comics_splash_timer > 0f)
		{
			float num = comics_splash_timer / 500f;
			if (num > 1f)
			{
				num = 1f;
			}
			dcolor_t2.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255f * num));
			CowRender.BindTexture(SCGlobals.p_happyend0);
			if (SuperCow.IsHDTV)
			{
				CowRender.DrawColorQuad2D(SCGlobals.p_happyendback.mImage, 0f, 0f, 1067f, 600f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
			}
			else
			{
				CowRender.DrawColorQuad2D(SCGlobals.p_happyendback.mImage, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
			}
			if (comics_splash_timer < 4000f && comics_splash_timer + SCGlobals.ftickperframe >= 4000f)
			{
				Sound.Addsound("camera");
			}
			if (comics_splash_timer >= 4000f)
			{
				num = (comics_splash_timer - 4000f) / 500f;
				if (num > 1f)
				{
					num = 1f;
				}
				if (num > 0.5f)
				{
					CowRender.BindTexture(SCGlobals.p_happyend1);
					if (SuperCow.IsHDTV)
					{
						CowRender.DrawColorQuad2D(SCGlobals.p_happyendback.mImage, 0f, 0f, 1067f, 600f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
					}
					else
					{
						CowRender.DrawColorQuad2D(SCGlobals.p_happyendback.mImage, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
					}
				}
				if (num < 1f)
				{
					dcolor_t2.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255f * (float)Math.Sin(num * (float)Math.PI)));
					CowRender.BindTexture(SCGlobals.p_white);
					CowRender.DrawColorQuad2D(new vec2_t(0f, 0f), 800f, 600f, new frect_t(0f, 0f, 1f, 1f), dcolor_t2, 0f);
				}
			}
			CowRender.EnableRS(7u);
			if (comics_splash_timer > 4500f)
			{
				num = (comics_splash_timer - 4500f) / 500f;
				dcolor_t2.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255f * num));
				Text.SetTextColor(dcolor_t2);
				Text.OutTextCenterXY(0f, -250f, 15f + 0.1f * SuperCowHelper.psin(1f), wstrstuff.GetStr("pressatocontinue"));
			}
			if (comics_splash_timer < 5000f)
			{
				comics_splash_timer += SCGlobals.ftickperframe;
			}
			if (comics_splash_timer > 5000f)
			{
				comics_splash_timer = 5000f;
			}
		}
		CowRender.EnableRS(7u);
		if (fcomicsendtimer > 0f)
		{
			fcomicsendtimer += SCGlobals.ftickperframe;
			if (fcomicsendtimer > 900f)
			{
				_ = SCGlobals.b_completegame;
			}
		}
	}

	public void StageEnd()
	{
		if (PlayerProfile.curlevelstage == 10)
		{
			comics_splash_timer = 1f;
		}
		else
		{
			fcomicsendtimer = 1f;
		}
	}
}
