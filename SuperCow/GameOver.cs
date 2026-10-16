using Microsoft.Xna.Framework;

namespace SuperCow;

internal class GameOver : MenuWindow
{
	private enum WidgetList
	{
		BUTTON_OK,
		WIDGET_COUNT
	}

	private int mNextMenu = -1;

	private WindowRect winRect = new WindowRect();

	private Rectangle mBounds;

	public GameOver(Rectangle bounds, int speed, bool visible, bool dropDown)
		: base(bounds, speed, visible, dropDown)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		string[] array = new string[1] { "Continue" };
		Button child = null;
		mBounds = bounds;
		float num = bounds.Height - 70 - 240;
		for (int i = 0; i < 1; i++)
		{
			child = new Button(i, this)
			{
				text = array[i],
				x = 0f,
				y = 100f,
				font = 0,
				fontSize = 20f
			};
			num += 30f;
			AddChild(child);
		}
		base.backChild = child;
		SetupOrder();
	}

	public override void Activate()
	{
		base.Activate();
		Uncollapse();
		SCGlobals.b_pause = true;
	}

	public override void Deactivate()
	{
		base.Deactivate();
		Collapse();
	}

	public override void ButtonPressed(int theID)
	{
		mNextMenu = -1;
		if (theID == 0)
		{
			Deactivate();
			mNextMenu = 23;
		}
	}

	public override void Draw()
	{
		matrix_t m = default(matrix_t);
		m.load_identity();
		m.scale(1f, 1f - SCGlobals.menusetk * SCGlobals.menusetk, 1f);
		CowRender.LoadMatrix(m);
		long num = 400L;
		float num2 = Text.PrepareWindowText((float)num - 70f, wstrstuff.GetStr("dontbesad"), 15f);
		float num3 = Text.PrepareWindowText((float)num - 70f, wstrstuff.GetStr("loselastlife"), 15f);
		float num4 = num2 + num3 + 180f;
		winRect.SetRect(new frect_t(-200f, 130f, 400f, 260f));
		winRect.Draw();
		float num5 = num4 / 2f - 50f;
		Text.SetTextColor(0, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		Text.OutTextXY(0f, num5, 22f, wstrstuff.GetStr("gameover"), b_center: true);
		num5 -= 45f;
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		Text.DrawPreparedTextCenterXY(0f, num5, 15f);
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
		num5 -= num3 + 20f;
		Text.PrepareWindowText((float)num - 70f, wstrstuff.GetStr("dontbesad"), 15f);
		Text.DrawPreparedTextCenterXY(0f, num5, 15f);
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		num5 = -85f;
		Text.OutTextXY(0f, num5, 15f, "Press    to continue", b_center: true);
		CowRender.BindTexture(SCGlobals.p_a);
		CowRender.DrawColorQuad2D(SCGlobals.p_a.mImage, -26f, num5, 21f, 21f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 10f);
		CowRender.LoadMatrixNull();
	}

	public override void Uncollapsed()
	{
	}

	public override void Collapsed()
	{
		SuperCow.RemoveScreen(base.id, clear: true);
		if (mNextMenu != -1)
		{
			SCGlobals.b_pause = false;
			SCGlobals.b_gamemenu = false;
			SCGlobals.b_stopphysics = false;
			SuperCow.RemoveScreen(11, clear: true);
			SCGlobals.supercowgame.FreeGame();
			SCGlobals.supercowgame.StopMusic();
			SCGlobals.supercowgame = null;
			SuperCow.AddScreen(7, activate: true);
		}
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
	}

	public override void FromDemoToFull()
	{
		SCGlobals.b_pause = false;
		SuperCow.RemoveScreen(base.id, clear: true);
		SCGlobals.supercowgame.FromDemoToFull();
		SCGlobals.supercowgame.FreeGame();
		SCGlobals.supercowgame = null;
	}
}
