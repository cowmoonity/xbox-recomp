using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public class CreditsScreen : AbstractScreen
{
	public int i_gamecredits = 32;

	public float i_gamecreditscur;

	public float f_gamecredits;

	public float fcredittime;

	public float splash_timer;

	private Cue mMusic;

	public CreditsScreen()
	{
		CowRender.SetMenuLight();
	}

	public override void Draw()
	{
		CowRender.SetOrtho(800f, 600f, 1f, 300f);
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		CowRender.EnableRS(4u);
		CowRender.SetBlend(0);
		CowRender.DisableRS(1u);
		CowRender.EnableRS(5u);
		// TODO: alpha test not implemented - 1f
		SCGlobals.cam.load_identity();
		SCGlobals.cam.translate(0f, 0f, 50f);
		CowRender.LoadCamera(SCGlobals.cam);
		SCGlobals.frustum.Calculate();
		CowRender.LoadMatrixNull();
		graphicsDevice.RasterizerState = RasterizerState.CullNone;
		CowRender.DisableRS(1u);
		CowRender.DisableRS(0u);
		if (SuperCow.IsHDTV)
		{
			CowRender.SetOrtho(597.0149f, 447.76117f, 1f, 300f);
		}
		graphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
		graphicsDevice.Textures[0] = SuperCowGame.mainTex;
		SCGlobals.animbacks.DrawAnimateBacks();
		CowRender.SetOrtho(800f, 600f, 1f, 500f);
		SCGlobals.cam.load_identity();
		SCGlobals.cam.translate(0f, 0f, 50f);
		CowRender.LoadMatrixNull();
		CowRender.LoadCamera(SCGlobals.cam);
		SCGlobals.frustum.Calculate();
		DrawCredits();
		CowRender.LoadMatrixNull();
	}

	public override void Activate()
	{
		base.Activate();
		i_gamecreditscur = 0f;
		f_gamecredits = 0f;
		fcredittime = 0f;
		SCGlobals.animbacks.NewAnimateBackgroundColor(1);
		mMusic = Sound.Play(Sounds.credits);
	}

	public override void Deactivate()
	{
		base.Deactivate();
		Sound.Stop(mMusic);
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
	}

	public override void JoyPress(GamePadKeys button, int theID)
	{
		Deactivate();
		SuperCow.RemoveScreen(base.id, clear: true);
		SuperCow.AddScreen(4, activate: true);
		SuperCow.AddScreen(22, activate: true);
	}

	public void DrawCredits()
	{
		vec2_t pos = default(vec2_t);
		frect_t vc = default(frect_t);
		frect_t tc = default(frect_t);
		dcolor_t c = default(dcolor_t);
		if (f_gamecredits == 0f)
		{
			splash_timer = 0f;
		}
		f_gamecredits += SCGlobals.ftickperframe;
		CowRender.EnableRS(4u);
		CowRender.SetBlend(0);
		CowRender.DisableRS(7u);
		if (splash_timer > 0f)
		{
			float num = splash_timer / 1000f;
			if (num > 1f)
			{
				num = 1f;
			}
			c.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255f * num));
			CowRender.BindTexture(SCGlobals.p_happyend0);
			CowRender.DrawColorQuad2D(SCGlobals.p_happyend0.mImage, 0f, 0f, SCGlobals.p_happyend0.mImage.Width, SCGlobals.p_happyend0.mImage.Height, c, 0f);
			if (splash_timer < 4000f && splash_timer + SCGlobals.ftickperframe >= 4000f)
			{
				Sound.Addsound("camera");
			}
			if (splash_timer >= 4000f)
			{
				num = (splash_timer - 4000f) / 500f;
				if (num > 1f)
				{
					num = 1f;
				}
				if (num > 0.5f)
				{
					CowRender.BindTexture(SCGlobals.p_happyend1);
					CowRender.DrawColorQuad2D(SCGlobals.p_happyend1.mImage, 0f, 0f, SCGlobals.p_happyend1.mImage.Width, SCGlobals.p_happyend1.mImage.Height, c, 0f);
				}
				if (num < 1f)
				{
					c.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255f * (float)Math.Sin(num * (float)Math.PI)));
					CowRender.BindTexture(SCGlobals.p_white);
					CowRender.DrawColorQuad2D(new vec2_t(0f, 0f), 1067f, 600f, new frect_t(0f, 0f, 1f, 1f), c, 0f);
				}
			}
			if (splash_timer < 6000f)
			{
				splash_timer += SCGlobals.ftickperframe;
			}
		}
		CowRender.EnableRS(7u);
		float num2;
		if (i_gamecreditscur == 0f)
		{
			num2 = f_gamecredits / 300f;
			if (num2 > 1f)
			{
				num2 = 1f;
			}
		}
		else
		{
			num2 = 1f;
		}
		byte b = (byte)(255f - 100f * num2);
		c.assign(b, b, b, byte.MaxValue);
		if (i_gamecreditscur == (float)(i_gamecredits - 1))
		{
			if (splash_timer == 0f)
			{
				splash_timer = 1f;
			}
			num2 = ((!(f_gamecredits < 1000f)) ? 1f : (f_gamecredits / 1000f));
		}
		else
		{
			if (f_gamecredits > 5000f)
			{
				f_gamecredits = 0f;
				if (i_gamecreditscur < (float)(i_gamecredits - 1))
				{
					i_gamecreditscur += 1f;
				}
			}
			if (f_gamecredits < 1000f)
			{
				num2 = f_gamecredits / 1000f;
			}
			else if (f_gamecredits > 4000f)
			{
				num2 = (f_gamecredits - 4000f) / 1000f;
				num2 = 1f - num2;
			}
			else
			{
				num2 = 1f;
			}
		}
		num2 *= num2;
		int num3 = (int)i_gamecreditscur;
		tc.assign(0.02f, 0.02f, 1f, 1f);
		b = (byte)(255f * num2);
		if (num3 == 0)
		{
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, 0, b);
			Text.OutTextXY(0f, 30f, 24f, SCGlobals.gamecredits[num3].position, b_center: true);
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, b);
			Text.OutTextXY(0f, -30f, 22f, SCGlobals.gamecredits[num3].name, b_center: true);
		}
		else if (i_gamecreditscur == (float)(i_gamecredits - 1))
		{
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, 0, b);
			Text.OutTextXY(0f, -250f, 18f, SCGlobals.gamecredits[num3].position, b_center: true);
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, b);
			Text.OutTextXY(0f, 250f, 18f, SCGlobals.gamecredits[num3].name, b_center: true);
			CowRender.BindTexture(SCGlobals.p_a);
			CowRender.DrawColorQuad2D(SCGlobals.p_a.mImage, -30f, -250f, 21f, 21f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, b), 10f);
		}
		else if (SCGlobals.gamecredits[num3].persname != null)
		{
			c.assign(byte.MaxValue, byte.MaxValue, byte.MaxValue, b);
			if (SCGlobals.gamecredits[num3].persname == "goat" && SuperCow.IsHDTV)
			{
				pos.assign(-278f, -46f);
			}
			else
			{
				pos.assign(-144f, -46f);
			}
			vc.assign(pos, 512f, 512f);
			CowRender.DisableRS(7u);
			CowRender.BindTexture(Comics.ComicsGetPersTex(SCGlobals.gamecredits[num3].persname));
			CowRender.DrawColorQuad2D(vc, tc, c, 0f);
			CowRender.EnableRS(7u);
			float num4 = 270f;
			float num5 = -380f;
			num4 = 210f;
			num5 = 310f - Text.GetStringWidth(SCGlobals.gamecredits[num3].name, 20f) * 0.5f;
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, 0, b);
			Text.OutTextXY(num5, num4, 18f, SCGlobals.gamecredits[num3].position, b_center: true);
			num4 -= 32f;
			Text.OutTextCenter("\n");
			Text.SetFontSize(5f);
			Text.OutTextCenter("\n");
			Text.SetFontSize(18f);
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, b);
			Text.OutTextCenter(SCGlobals.gamecredits[num3].name);
		}
		else
		{
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			if (SCGlobals.gamecredits[num3].name != null)
			{
				num6 += Text.PrepareWindowText(600f, SCGlobals.gamecredits[num3].name, 18f);
			}
			num6 += Text.PrepareWindowText(600f, SCGlobals.gamecredits[num3].position, 18f);
			num4 += num6 / 2f;
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, 0, b);
			Text.OutTextXY(num5, num4, 18f, SCGlobals.gamecredits[num3].position, b_center: true);
			Text.OutTextCenter("\n");
			Text.SetFontSize(5f);
			Text.OutTextCenter("\n");
			Text.SetFontSize(18f);
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, b);
			Text.OutTextCenter(SCGlobals.gamecredits[num3].name);
		}
	}
}
