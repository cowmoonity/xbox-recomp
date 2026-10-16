using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace SuperCow;

public class UnlockedScreen : AbstractScreen
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
		string str = "Thank you for buying our game! Now you help Supercow clear every level and save your progress as you go!";
		Text.PrepareWindowText(280f, str, 14f);
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		Text.DrawPreparedTextCenterXY(0f, 60f, 14f);
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
		Text.OutTextCenterXY(0f, -100f, 14f, "Press    to continue");
		CowRender.BindTexture(SCGlobals.p_a);
		CowRender.DrawColorQuad2D(SCGlobals.p_a.mImage, -26f, -100f, 21f, 21f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 10f);
		CowRender.LoadMatrixNull();
	}

	public override void Activate()
	{
		base.Activate();
	}

	public override void JoyPress(GamePadKeys button, int theID)
	{
		if (button == GamePadKeys.A || button == GamePadKeys.B)
		{
			Deactivate();
			SuperCow.RemoveScreen(base.id, clear: true);
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
}
