using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public class StatsScreen : AbstractScreen
{
	public static bool b_firstfirework;

	private Cue mMusic;

	private WindowRect windowRect = new WindowRect();

	private WindowRect levResults = new WindowRect();

	private WindowRect okButton = new WindowRect();

	public StatsScreen()
	{
		SCGlobals.menuscenetime = 0f;
		float num = 500f;
		float num2 = 400f;
		float num3 = 0f;
		windowRect.SetRect(new frect_t(num3 - num / 2f, num2 / 2f, num, num2));
		float num4 = 210f;
		float num5 = 390f;
		float y = 100f;
		levResults.SetRect(new frect_t((0f - num5) / 2f, 50f + num4, num5, y));
		num4 = -210f;
		okButton.SetRect(new frect_t((0f - num5) / 2f, 50f + num4, num5, y));
	}

	public override void Draw()
	{
		SCGlobals.cam.load_identity();
		SCGlobals.cam.translate(0f, 0f, 50f);
		CowRender.LoadMatrixNull();
		CowRender.LoadCamera(SCGlobals.cam);
		SCGlobals.frustum.Calculate();
		vec2_t vec2_t2 = new vec2_t(0f, 0f);
		_ = SuperCow.DeviceManager.GraphicsDevice;
		vec2_t2 = SCGlobals.camerapos;
		SCGlobals.camerapos.y = 0f;
		SCGlobals.menubacktime -= SCGlobals.ftickperframe * 0.005f;
		SCGlobals.camerapos.x = SCGlobals.menubacktime;
		CowRender.DisableRS(1u);
		CowRender.DisableRS(2u);
		SCGlobals.supercowgame.DrawBackground2();
		CowRender.DisableRS(5u);
		SCGlobals.camerapos = vec2_t2;
		if (SCGlobals.menuscenetime - SCGlobals.ftickperframe < 800f && SCGlobals.menuscenetime >= 800f)
		{
			mMusic = Sound.Play(Sounds.credits);
			SCGlobals.menustattimer = 0f;
			SCGlobals.menustatnum = 0;
		}
		if (SCGlobals.menuscenetime < 800f)
		{
			b_firstfirework = false;
			SCGlobals.firet = 2000f;
			return;
		}
		CowRender.EnableRS(7u);
		float num = 500f;
		float num2 = 400f;
		float num3 = 0f;
		float num4 = -160f;
		float num5 = -10f;
		float num6 = 125f;
		matrix_t m = default(matrix_t);
		m.load_identity();
		vec3_t pos = default(vec3_t);
		frect_t vc = default(frect_t);
		CowRender.EnableRS(2u);
		CowRender.EnableRS(1u);
		CowRender.BindTexture(default(texture_t));
		CowRender.SetBlend(2);
		float num7 = 390f;
		float num8 = 210f + SCGlobals.menusetk * 200f;
		vc.assign((0f - num7) / 2f, 50f + num8, num7 / 2f, -50f + num8);
		CowRender.DrawColorQuad2D(vc, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		num8 = -210f - SCGlobals.menusetk * 200f;
		vc.assign((0f - num7) / 2f, 50f + num8, num7 / 2f, -50f + num8);
		CowRender.DrawColorQuad2D(vc, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		CowRender.SetBlend(0);
		m.load_identity();
		m.scale(1f, 1f - SCGlobals.menusetk * SCGlobals.menusetk, 1f);
		CowRender.LoadMatrix(m);
		vc.assign(num3 - num / 2f, num2 / 2f, num, num2);
		windowRect.Draw();
		CowRender.DisableRS(1u);
		levResults.Draw();
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, 155, byte.MaxValue);
		Text.OutTextCenterXY(0f, 210f, 21f, wstrstuff.GetStr("stats"));
		okButton.Draw();
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		Text.OutTextXY(0f, -210f, 15f, "Press    to continue", b_center: true);
		CowRender.BindTexture(SCGlobals.p_a);
		CowRender.DrawColorQuad2D(SCGlobals.p_a.mImage, -26f, num8, 21f, 21f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 10f);
		if (SCGlobals.menusetk != 0f)
		{
			return;
		}
		num8 = num2 / 2f - 90f;
		SCGlobals.menustattimer += SCGlobals.ftickperframe;
		while (SCGlobals.menustattimer > 500f && SCGlobals.menustatnum < 6)
		{
			SCGlobals.menustatnum++;
			SCGlobals.menustattimer -= 500f;
			if (SCGlobals.menustatnum == 1)
			{
				Sound.Addsound("stats0");
				for (int i = 0; i < 10; i++)
				{
					Particles.AddDynamicParticle(new vec3_t(5f - (float)i, num8 / 32f, 0f), 31, 1, -1);
				}
			}
			if (SCGlobals.menustatnum > 1)
			{
				string sound = $"stats{SCGlobals.menustatnum - 1:D}";
				Sound.Addsound(sound);
				pos.assign(num4, num8 - 55f - (float)(50 * (SCGlobals.menustatnum - 2)), 0f);
				pos /= 32f;
				Particles.AddDynamicParticle(pos, 31, 1, -1);
				Particles.AddDynamicParticle(pos, 26, 1, -1);
				pos.x = num6 / 32f;
				Particles.AddDynamicParticle(pos, 31, 1, -1);
				Particles.AddDynamicParticle(pos, 26, 1, -1);
			}
		}
		dcolor_t col_selected = SCGlobals.col_selected;
		float num9 = 43f;
		if (SCGlobals.menustatnum >= 1)
		{
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, 155, byte.MaxValue);
			Text.OutTextXY(num4, num8, 21f, wstrstuff.GetStr("yourscore"), b_center: false);
			Text.SetTextColor(byte.MaxValue, 205, 0, byte.MaxValue);
			string sound = $"{SCGlobals.supercowgame.scoreatlevel:D}";
			Text.OutTextXY(num6, num8, 26f, sound, b_center: false);
		}
		num8 -= num9 + 5f;
		if (SCGlobals.menustatnum >= 2)
		{
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			SCGlobals.menugem[0].Draw(new vec2_t(num4, num8), -1, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f, 0f);
			CowRender.DrawDynBuffer();
			Text.OutTextCenterXY(num5, num8, 16f, wstrstuff.GetStr("coinstat"));
			string sound = $"{SCGlobals.levelstat.coins:D} / {SCGlobals.levelstat.numcoins:D}";
			Text.SetTextColor(col_selected);
			Text.OutTextXY(num6, num8, 16f, sound, b_center: false);
		}
		num8 -= num9;
		if (SCGlobals.menustatnum >= 3)
		{
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			SCGlobals.menugem[1].Draw(new vec2_t(num4, num8), -1, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f, 0f);
			CowRender.DrawDynBuffer();
			Text.OutTextCenterXY(num5, num8, 16f, wstrstuff.GetStr("gemstat"));
			string sound = $"{SCGlobals.levelstat.gems:D} / {SCGlobals.levelstat.numgems:D}";
			Text.SetTextColor(col_selected);
			Text.OutTextXY(num6, num8, 16f, sound, b_center: false);
		}
		num8 -= num9;
		if (SCGlobals.menustatnum >= 4)
		{
			DrawMonster2D(num4, num8 - 10f, 2, 0.33f);
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			Text.OutTextCenterXY(num5, num8, 16f, wstrstuff.GetStr("monsterstat"));
			string sound = $"{SCGlobals.levelstat.monsters:D} / {SCGlobals.levelstat.nummonsters:D}";
			Text.SetTextColor(col_selected);
			Text.OutTextXY(num6, num8, 16f, sound, b_center: false);
		}
		num8 -= num9;
		if (SCGlobals.menustatnum >= 5)
		{
			CowRender.BindTexture(SCGlobals.p_magglass);
			CowRender.DrawColorQuad2D(new vec2_t(num4, num8), 40f, 40f, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			Text.OutTextCenterXY(num5, num8, 16f, wstrstuff.GetStr("secretstat"));
			string sound = $"{SCGlobals.levelstat.secrets:D} / {SCGlobals.levelstat.numsecrets:D}";
			Text.SetTextColor(col_selected);
			Text.OutTextXY(num6, num8, 16f, sound, b_center: false);
		}
		num8 -= num9 + 5f;
		if (SCGlobals.menustatnum >= 6)
		{
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, 155, byte.MaxValue);
			Text.OutTextXY(num4, num8, 21f, wstrstuff.GetStr("completeness"), b_center: false);
			Text.SetTextColor(byte.MaxValue, 205, 0, byte.MaxValue);
			string sound = $"{SCGlobals.levelcompleteness:D}%";
			Text.OutTextXY(num6, num8, 26f + ((SCGlobals.levelcompleteness == 100) ? (3f * SuperCowHelper.psin(3f, -1000001f)) : 0f), sound, b_center: false);
		}
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		CowRender.LoadMatrixNull();
		if (SCGlobals.menustatnum >= 6 && SCGlobals.levelcompleteness == 100)
		{
			SCGlobals.firet -= SCGlobals.ftickperframe;
			if (SCGlobals.firet <= 0f)
			{
				SCGlobals.firet += 500 + CRand.irand(500);
				if (CRand.irand(2) == 0)
				{
					Sound.Addsound("ExpSalute1");
				}
			}
		}
		base.Draw();
	}

	public override void Update(GameTime gameTime)
	{
		SCGlobals.menuscenetime += SCGlobals.ftickperframe;
		base.Update(gameTime);
	}

	public void DrawMonster2D(float x, float y, int mid, float mscale)
	{
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		float num = 1f / 32f;
		CowRender.SetOrtho(800f * num, 600f * num, 1f, 300f);
		CowRender.EnableRS(0u);
		CowRender.EnableRS(1u);
		CowRender.EnableRS(2u);
		CowRender.EnableRS(7u);
		CowRender.EnableRS(5u);
		graphicsDevice.RasterizerState = RasterizerState.CullNone;
		CMonster pers = SCGlobals.pershost.GetPers(mid);
		pers.Calculate();
		float z = pers.fscale * mscale;
		pers.scalematrix.scale(z, z, z);
		pers.matrix.load_identity();
		pers.matrix.translate(x * num, 30f, y * num);
		pers.matrix.zrot_delta(160f);
		pers.BaseDraw();
		CowRender.LoadMatrixNull();
		CowRender.DisableRS(1u);
		CowRender.DisableRS(0u);
		CowRender.SetOrtho(800f, 600f, 1f, 300f);
	}

	public override void JoyPress(GamePadKeys button, int theID)
	{
		if (button == GamePadKeys.A || button == GamePadKeys.Start)
		{
			SuperCow.RemoveScreen(base.id, clear: true);
			SCGlobals.supercowgame.FreeGame();
			Sound.Stop(mMusic);
			if (SuperCow.Settings.curlevel == 0 && SCGlobals.b_victory)
			{
				SuperCow.AddScreen(14, activate: true);
			}
			else
			{
				SuperCow.AddScreen(7, activate: true);
				SCGlobals.supercowgame = null;
			}
		}
		base.JoyPress(button, theID);
	}

	public override void FromDemoToFull()
	{
		Deactivate();
		Sound.Stop(mMusic);
		SuperCow.RemoveScreen(base.id, clear: true);
		SCGlobals.supercowgame.FreeGame();
		SCGlobals.supercowgame = null;
		SuperCow.AddScreen(4, activate: true);
		SuperCow.AddScreen(16, activate: true);
		SuperCow.AddScreen(6, activate: true);
		base.FromDemoToFull();
	}
}
