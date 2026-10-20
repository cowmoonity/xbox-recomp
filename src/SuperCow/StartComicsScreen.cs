using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace SuperCow;

public class StartComicsScreen : AbstractScreen
{
	private WindowRect winRect = new WindowRect();

	private Cue mMusic;

	private Cue mSound;

	public int INTRO_COMICS_NUM_PICS = 5;

	public int INTRO_COMICS_APP_TIME = 1000;

	public float INTRO_COMICS_H;

	public int icurpicindex;

	public int iprevpicindex;

	public texture_t[] ar_comicstex = new texture_t[5];

	public float fpicapptime;

	public float fstartcomicstime;

	private int nCommand = -1;

	public texture_t p_skytex;

	public texture_t p_buttonback;

	public texture_t p_buttonbacklum;

	public StartComicsScreen()
	{
		Store.SetCurrentDirectory("levelbacks", theAdd: false);
		p_skytex = SCGlobals.texturehost.Load("backsky1.jpg");
		Store.SetCurrentDirectory("ui", theAdd: false);
		p_buttonback = SCGlobals.texturehost.Load("button_back.tga");
		p_buttonbacklum = SCGlobals.texturehost.Load("button_back_lum.tga");
		Store.SetCurrentDirectory("comics", theAdd: false);
		ref texture_t reference = ref ar_comicstex[0];
		reference = SCGlobals.texturehost.Load("1.jpg");
		ref texture_t reference2 = ref ar_comicstex[1];
		reference2 = SCGlobals.texturehost.Load("2.jpg");
		ref texture_t reference3 = ref ar_comicstex[2];
		reference3 = SCGlobals.texturehost.Load("3.jpg");
		ref texture_t reference4 = ref ar_comicstex[3];
		reference4 = SCGlobals.texturehost.Load("4.jpg");
		ref texture_t reference5 = ref ar_comicstex[4];
		reference5 = SCGlobals.texturehost.Load("5.jpg");
		winRect.SetBack(back: false);
	}

	public override void JoyPress(GamePadKeys button, int theID)
	{
		nCommand = (int)button;
		base.JoyPress(button, theID);
	}

	public override void Draw()
	{
		base.Draw();
	}

	public override void Activate()
	{
		base.Activate();
		icurpicindex = 0;
		iprevpicindex = 0;
		fpicapptime = 0f;
		fstartcomicstime = 0f;
		mSound = Sound.Play(Sounds.intro1);
		mMusic = Sound.Play(Sounds.game2);
	}

	public override void Deactivate()
	{
		base.Deactivate();
		Sound.Stop(mMusic);
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		if (nCommand >= 0 && fpicapptime >= (float)INTRO_COMICS_APP_TIME)
		{
			if (nCommand == 8)
			{
				if (icurpicindex > 0)
				{
					iprevpicindex = icurpicindex;
					icurpicindex--;
				}
				fpicapptime = INTRO_COMICS_APP_TIME;
				Sound.Addsound("menu");
				Sound.Stop(mSound);
				mSound = Sound.Play((Sounds)(371 + icurpicindex));
			}
			else
			{
				if (nCommand == 0 || nCommand == 1)
				{
					nCommand = -1;
					return;
				}
				if (nCommand == 2)
				{
					fpicapptime = 0f;
					iprevpicindex = icurpicindex;
					icurpicindex++;
					Sound.Addsound("menu");
					Sound.Stop(mSound);
					if (icurpicindex < INTRO_COMICS_NUM_PICS)
					{
						mSound = Sound.Play((Sounds)(371 + icurpicindex));
					}
				}
			}
		}
		nCommand = -1;
	}

	public override void DrawGeometry()
	{
		base.DrawGeometry();
		matrix_t m = default(matrix_t);
		float num = 512f;
		vec2_t v = default(vec2_t);
		CowRender.SetTextMode();
		SCGlobals.cam.load_identity();
		SCGlobals.cam.translate(0f, 0f, 10f);
		CowRender.SetGlobal2DColor(new color_t(1f, 1f, 1f, 1f));
		CowRender.SetTextMode();
		CowRender.LoadCamera(SCGlobals.cam);
		CowRender.BindTexture(p_skytex);
		float num2 = SCGlobals.global_timer * 4E-05f;
		if (SuperCow.IsHDTV)
		{
			CowRender.DrawColorQuad2D(p_skytex.mImage, 0f, 0f, 1066f, 600f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		}
		else
		{
			CowRender.DrawColorQuad2D(new vec2_t(0f, 0f), 800f, 600f, new frect_t(num2, 0f, num2 + 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f);
		}
		fstartcomicstime += SCGlobals.ftickperframe;
		_ = fstartcomicstime;
		_ = 1000f;
		float num3;
		if (fpicapptime <= (float)INTRO_COMICS_APP_TIME)
		{
			if (icurpicindex >= iprevpicindex)
			{
				fpicapptime += SCGlobals.ftickperframe;
			}
			else
			{
				fpicapptime -= SCGlobals.ftickperframe;
			}
			if (fpicapptime > (float)INTRO_COMICS_APP_TIME && icurpicindex == INTRO_COMICS_NUM_PICS)
			{
				if (PlayerProfile.b_fresh && !SCGlobals.bFromMenu)
				{
					PlayerProfile.b_fresh = false;
					Deactivate();
					SuperCow.RemoveScreen(base.id, clear: true);
					SuperCow.AddScreen(7, activate: true);
					SuperCow.Settings.SaveSettings();
				}
				else
				{
					Deactivate();
					SCGlobals.bFromMenu = false;
					SuperCow.RemoveScreen(base.id, clear: true);
					SuperCow.AddScreen(4, activate: true);
					SuperCow.AddScreen(22, activate: true);
				}
			}
			if (fpicapptime < 0f)
			{
				iprevpicindex = icurpicindex;
				fpicapptime = INTRO_COMICS_APP_TIME;
			}
			num3 = fpicapptime / (float)INTRO_COMICS_APP_TIME;
			if (num3 > 1f)
			{
				num3 = 1f;
			}
		}
		else
		{
			num3 = 1f;
		}
		if (icurpicindex != iprevpicindex || iprevpicindex != 0)
		{
			int num4 = 1;
			float num5 = 1f - num3;
			num = 512f;
			byte b = byte.MaxValue;
			float z = 0.7f + 0.3f * num5;
			CowRender.SetGlobal2DColor(new color_t(1f, 1f, 1f, num5));
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(num5 * 255f));
			v.assign(-160f * num3, 0f);
			m.load_identity();
			m.scale(z, z, z);
			m.translate(v);
			m.zrot_delta(15f * (num3 + (float)num4 - 1f));
			CowRender.LoadMatrix(m);
			CowRender.BindTexture(ar_comicstex[(icurpicindex > iprevpicindex) ? iprevpicindex : icurpicindex]);
			CowRender.DrawColorQuad2D(new vec2_t(0f, INTRO_COMICS_H), num, num, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(b, b, b, b), 10f);
			winRect.SetRect(new frect_t(0f - num / 2f, INTRO_COMICS_H + num / 2f, num, num));
			winRect.Draw();
		}
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		CowRender.SetGlobal2DColor(new color_t(1f, 1f, 1f, 1f));
		if (icurpicindex != INTRO_COMICS_NUM_PICS)
		{
			num = 512f;
			m.load_identity();
			CowRender.LoadMatrixNull();
			if (fpicapptime < (float)INTRO_COMICS_APP_TIME)
			{
				m.translate(200f * (1f - num3), 0f, 0f);
				m.scale(num3, num3, num3);
				m.zrot_delta((0f - (1f - num3)) * 120f);
				CowRender.LoadMatrix(m);
			}
			else
			{
				CowRender.DisableRS(7u);
			}
			CowRender.BindTexture(ar_comicstex[(icurpicindex > iprevpicindex) ? icurpicindex : iprevpicindex]);
			CowRender.DrawColorQuad2D(new vec2_t(0f, INTRO_COMICS_H), num, num, new frect_t(0f, 0f, 1f, 1f), new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 10f);
			CowRender.EnableRS(7u);
			winRect.SetRect(new frect_t(0f - num / 2f, INTRO_COMICS_H + num / 2f, num, num));
			winRect.Draw();
			if (fpicapptime >= (float)INTRO_COMICS_APP_TIME)
			{
				float num6 = Text.PrepareWindowText(480f, wstrstuff.GetStrDelta("introcomics1", icurpicindex), 15f);
				Text.DrawPreparedTextCenterXY(0f, -250f + num6 + INTRO_COMICS_H, 15f);
			}
		}
		CowRender.LoadMatrixNull();
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, 155, byte.MaxValue);
		int num7 = icurpicindex + 1;
		if (num7 > INTRO_COMICS_NUM_PICS)
		{
			num7 = INTRO_COMICS_NUM_PICS;
		}
		string str = string.Format("{0}   {1:D}/{2:D}", wstrstuff.GetStr("introname"), num7, INTRO_COMICS_NUM_PICS);
		Text.OutTextCenterXY(0f, 255f, 20f, str);
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		Text.OutTextXY(0f, -260f, 10f, "Press    to continue", b_center: true);
		CowRender.DisableRS(7u);
		CowRender.BindTexture(SCGlobals.p_a);
		CowRender.DrawColorQuad2D(SCGlobals.p_a.mImage, -18f, -260f, 16f, 16f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 10f);
		_ = icurpicindex;
		CowRender.EnableRS(7u);
		CowRender.LoadMatrixNull();
	}
}
