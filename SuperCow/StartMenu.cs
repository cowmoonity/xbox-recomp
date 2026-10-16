using Microsoft.Xna.Framework;

namespace SuperCow;

internal class StartMenu : MenuWindow
{
	private enum WidgetList
	{
		BUTTON_OK,
		WIDGET_COUNT
	}

	private int mNextMenu = -1;

	private WindowRect winRect = new WindowRect();

	private Rectangle mBounds;

	public StartMenu(Rectangle bounds, int speed, bool visible, bool dropDown)
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
			if (SCGlobals.b_gamemenu)
			{
				mNextMenu = 17;
			}
		}
	}

	public override void Draw()
	{
		matrix_t m = default(matrix_t);
		m.load_identity();
		m.scale(1f, 1f - SCGlobals.menusetk * SCGlobals.menusetk, 1f);
		CowRender.LoadMatrix(m);
		float num = Text.PrepareWindowText(290f, wstrstuff.GetStrDelta("task1", SCGlobals.curleveltask), 16f) + 200f;
		winRect.SetRect(new frect_t(-180f, num / 2f, 360f, num));
		winRect.Draw();
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		float num2 = num / 2f - 50f;
		string str = "Stage " + (PlayerProfile.curlevelstage + 1) + "  Level " + (PlayerProfile.curlevel + 1);
		Text.OutTextXY(0f, num2, 22f, str, b_center: true);
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
		num2 -= 48f;
		Text.OutTextXY(0f, num2, 15f, "Your goal is:", b_center: true);
		Text.SetTextColor(0, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		num2 -= 30f;
		Text.DrawPreparedTextCenterXY(0f, num2, 15f);
		Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		num2 = -85f;
		Text.OutTextXY(0f, num2, 15f, "Press    to continue", b_center: true);
		CowRender.BindTexture(SCGlobals.p_a);
		CowRender.DrawColorQuad2D(SCGlobals.p_a.mImage, -26f, num2, 21f, 21f, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 10f);
		CowRender.LoadMatrixNull();
	}

	public override void Uncollapsed()
	{
	}

	public override void Collapsed()
	{
		SuperCow.RemoveScreen(base.id, clear: true);
		SCGlobals.supercowgame.b_showedTask = true;
		if (!SCGlobals.b_gamemenu)
		{
			SCGlobals.b_pause = false;
		}
		if (mNextMenu != -1)
		{
			SuperCow.AddScreen(mNextMenu, activate: true);
		}
	}

	public override void FromDemoToFull()
	{
		SCGlobals.b_pause = false;
		SuperCow.RemoveScreen(base.id, clear: true);
		SCGlobals.supercowgame.FromDemoToFull();
		SCGlobals.supercowgame.FreeGame();
		SCGlobals.supercowgame = null;
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
	}
}
