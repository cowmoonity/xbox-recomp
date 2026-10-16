using Microsoft.Xna.Framework;

namespace SuperCow;

internal class LeaderboardsMenu : MenuWindow
{
	private enum WidgetList
	{
		BUTTON_BACK,
		WIDGET_COUNT
	}

	private int mNextMenu = -1;

	public LeaderboardsMenu(Rectangle bounds, int speed, bool visible, bool dropDown)
		: base(bounds, speed, visible, dropDown)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		string[] array = new string[1] { "Back" };
		Button child = null;
		float num = bounds.Height - 70 - 300;
		for (int i = 0; i < 1; i++)
		{
			child = new Button(i, this)
			{
				text = array[i],
				x = 0f,
				y = num,
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
			mNextMenu = 16;
		}
	}

	public override void Uncollapsed()
	{
	}

	public override void Collapsed()
	{
		SuperCow.RemoveScreen(base.id, clear: false);
		if (mNextMenu != -1)
		{
			SuperCow.AddScreen(mNextMenu, activate: false);
			MenuWindow menuWindow = (MenuWindow)SuperCow.GetScreen(mNextMenu);
			if (!menuWindow.menuVisible)
			{
				menuWindow.Activate();
			}
		}
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
	}
}
