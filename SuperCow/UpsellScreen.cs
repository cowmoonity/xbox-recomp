using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace SuperCow;

public class UpsellScreen : AbstractScreen
{
	private Cue mMusic;

	private WindowRect winRect = new WindowRect();

	public override void Draw()
	{
		CowRender.SetOrtho(800f, 600f, 1f, 300f);
		SCGlobals.cam.load_identity();
		CowRender.LoadCamera(SCGlobals.cam);
		winRect.SetRect(new frect_t(-200f, 150f, 400f, 300f));
		winRect.Draw();
		string str = "Please unlock the game to access all 47 levels, new barnyard baddies and brutish bosses!";
		Text.PrepareWindowText(280f, str, 14f);
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		Text.DrawPreparedTextCenterXY(0f, 60f, 14f);
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
		Text.OutTextCenterXY(0f, -100f, 14f, "    Unlock full game,    Continue");
		CowRender.BindTexture(SCGlobals.p_x);
		CowRender.DrawColorQuad2D(SCGlobals.p_x.mImage, -140f, -100f, 21f, 21f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 10f);
		CowRender.BindTexture(SCGlobals.p_b);
		CowRender.DrawColorQuad2D(SCGlobals.p_b.mImage, 62f, -100f, 21f, 21f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 10f);
		CowRender.LoadMatrixNull();
	}

	public override void Activate()
	{
		base.Activate();
		SCGlobals.b_victory = false;
		SuperCow.Settings.LoadSettings();
	}

	public override void JoyPress(GamePadKeys button, int theID)
	{
		if (button == GamePadKeys.B)
		{
			Deactivate();
			SuperCow.RemoveScreen(base.id, clear: true);
		}
		if (button == GamePadKeys.X)
		{
			Deactivate();
			SuperCow.RemoveScreen(base.id, clear: true);
			SuperCow.ShowMarketPlace();
		}
		base.JoyPress(button, theID);
	}

	public override void Deactivate()
	{
		base.Deactivate();
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
	}

	public override void FromDemoToFull()
	{
		Deactivate();
		SuperCow.RemoveScreen(base.id, clear: true);
		base.FromDemoToFull();
	}
}
