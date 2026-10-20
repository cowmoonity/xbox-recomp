using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

namespace SuperCow;

internal class StageCommics : AbstractScreen
{
	public bool bUnlocked;

	public texture_t p_happyendback;

	public float comics_splash_timer;

	public float fcomicsendtimer;

	public frect_t skip_rect;

	public StageCommics()
	{
		p_happyendback = SCGlobals.texturehost.Load("happyendback.jpg");
	}

	public override void Update(GameTime gameTime)
	{
		SCGlobals.hero.Calculate();
		base.Update(gameTime);
	}

	public void DrawGameMenuComics()
	{
		vec2_t vec2_t2 = skip_rect.pos();
		float num = vec2_t2.x;
		float num2 = vec2_t2.y;
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		float font_size = 10f;
		if (!SCGlobals.b_completegame)
		{
			Text.OutTextXY(num, num2 - 260f, font_size, "Press    to skip", b_center: true);
			CowRender.BindTexture(SCGlobals.p_start);
			CowRender.DrawColorQuad2D(SCGlobals.p_start.mImage, -3f, -260f, 16f, 16f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 10f);
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
		if (SuperCow.Settings.curlevelstage == 10)
		{
			CowRender.BindTexture(p_happyendback);
			if (SuperCow.IsHDTV)
			{
				CowRender.DrawColorQuad2D(p_happyendback.mImage, 0f, 0f, 1067f, 600f, dcolor_t.ColorWhite, 0f);
			}
			else
			{
				CowRender.DrawColorQuad2D(p_happyendback.mImage, dcolor_t.ColorWhite, 0f);
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
		dcolor_t colorWhite = dcolor_t.ColorWhite;
		if (comics_splash_timer > 0f)
		{
			float num = comics_splash_timer / 500f;
			if (num > 1f)
			{
				num = 1f;
			}
			colorWhite.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255f * num));
			CowRender.BindTexture(SCGlobals.p_happyend0);
			if (SuperCow.IsHDTV)
			{
				CowRender.DrawColorQuad2D(p_happyendback.mImage, 0f, 0f, 1067f, 600f, dcolor_t.ColorWhite, 0f);
			}
			else
			{
				CowRender.DrawColorQuad2D(p_happyendback.mImage, dcolor_t.ColorWhite, 0f);
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
						CowRender.DrawColorQuad2D(p_happyendback.mImage, 0f, 0f, 1067f, 600f, dcolor_t.ColorWhite, 0f);
					}
					else
					{
						CowRender.DrawColorQuad2D(p_happyendback.mImage, dcolor_t.ColorWhite, 0f);
					}
				}
				if (num < 1f)
				{
					colorWhite.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255f * (float)Math.Sin(num * (float)Math.PI)));
					CowRender.BindTexture(SCGlobals.p_white);
					CowRender.DrawColorQuad2D(new vec2_t(0f, 0f), 1067f, 600f, SCGlobals.tc_normal, colorWhite, 0f);
				}
			}
			CowRender.EnableRS(7u);
			if (comics_splash_timer > 4500f)
			{
				num = (comics_splash_timer - 4500f) / 500f;
				colorWhite.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255f * num));
				Text.SetTextColor(colorWhite);
				Text.OutTextCenterXY(0f, -250f, 15f + 0.1f * SuperCowHelper.psin(1f), "Press     to continue");
				CowRender.BindTexture(SCGlobals.p_a);
				CowRender.DrawColorQuad2D(SCGlobals.p_a.mImage, -26f, -250f, 21f, 21f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 10f);
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
		if (!(fcomicsendtimer > 0f))
		{
			return;
		}
		fcomicsendtimer += SCGlobals.ftickperframe;
		if (!(fcomicsendtimer > 900f))
		{
			return;
		}
		if (SCGlobals.b_completegame)
		{
			SuperCow.RemoveScreen(base.id, clear: true);
			SuperCow.AddScreen(4, activate: true);
			SuperCow.AddScreen(16, activate: true);
			return;
		}
		SuperCow.RemoveScreen(base.id, clear: true);
		if (!Guide.IsTrialMode)
		{
			if (bUnlocked)
			{
				SuperCow.AddScreen(4, activate: true);
				SuperCow.AddScreen(16, activate: true);
				SuperCow.AddScreen(6, activate: true);
			}
			else
			{
				SuperCow.AddScreen(7, activate: true);
			}
		}
		else
		{
			SuperCow.AddScreen(4, activate: true);
			SuperCow.AddScreen(16, activate: true);
			SuperCow.AddScreen(5, activate: true);
		}
		SCGlobals.supercowgame = null;
	}

	public void StageEnd()
	{
		if (SuperCow.Settings.curlevelstage == 10)
		{
			comics_splash_timer = 1f;
			return;
		}
		fcomicsendtimer = 1f;
		SuperCowGame.SetGagDisapp(900f);
	}

	public void Load()
	{
		skip_rect.assign(new vec2_t(-270f, 237f), 120f, 16f);
		Store.SetCurrentDirectory("", theAdd: false);
		Comics.ComicsLoad();
	}

	public void OnKeyDown(uint nChar)
	{
	}

	public void OnLButtonDown()
	{
	}

	public void ProcessCommand()
	{
	}

	public override void Activate()
	{
		fcomicsendtimer = 0f;
		comics_splash_timer = 0f;
		Comics.ComicsInit();
		SCGlobals.hero.SetAnimStage(SCGlobals.hero.p_void[2]);
	}

	public void SignInChanged()
	{
	}

	public override void JoyPress(GamePadKeys button, int theID)
	{
		if (button == GamePadKeys.Start && !SCGlobals.b_completegame)
		{
			Comics.ComicsSkip();
			Comics.ComicsAbort();
			SuperCow.RemoveScreen(base.id, clear: true);
			if (!Guide.IsTrialMode)
			{
				if (bUnlocked)
				{
					SuperCow.AddScreen(4, activate: true);
					SuperCow.AddScreen(16, activate: true);
					SuperCow.AddScreen(6, activate: true);
				}
				else
				{
					SuperCow.AddScreen(7, activate: true);
				}
			}
			else
			{
				SuperCow.AddScreen(4, activate: true);
				SuperCow.AddScreen(16, activate: true);
				SuperCow.AddScreen(5, activate: true);
			}
			SCGlobals.supercowgame = null;
		}
		if (button == GamePadKeys.A)
		{
			if (comics_splash_timer == 5000f)
			{
				fcomicsendtimer = 1f;
			}
			else
			{
				Comics.ComicsClick();
			}
		}
	}

	public override void FromDemoToFull()
	{
		bUnlocked = true;
		base.FromDemoToFull();
	}
}
