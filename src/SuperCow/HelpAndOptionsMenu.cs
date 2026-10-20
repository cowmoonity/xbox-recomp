using Microsoft.Xna.Framework;

namespace SuperCow;

internal class HelpAndOptionsMenu : MenuWindow
{
	private enum WidgetList
	{
		BUTTON_INTRO,
		BUTTON_SETTINGS,
		BUTTON_CREDITS,
		BUTTON_BACK,
		WIDGET_COUNT
	}

	private int mNextMenu = -1;

	public HelpAndOptionsMenu(Rectangle bounds, int speed, bool visible, bool dropDown)
		: base(bounds, speed, visible, dropDown)
	{
		string[] array = new string[4] { "Intro", "Settings", "Credits", "Back" };
		Button child = null;
		float num = bounds.Y + 20 - (bounds.Y + bounds.Height) - 300;
		for (int i = 0; i < 4; i++)
		{
			child = new Button(i, this)
			{
				text = array[i],
				x = bounds.X - bounds.Width / 2,
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
		Show();
	}

	public override void Deactivate()
	{
		base.Deactivate();
		Hide();
	}

	public override void ButtonPressed(int theID)
	{
		mNextMenu = -1;
		if (theID == 2)
		{
			Deactivate();
			mNextMenu = 9;
		}
		if (theID == 0)
		{
			Deactivate();
			SCGlobals.bFromMenu = true;
			mNextMenu = 8;
		}
		if (theID == 3)
		{
			Deactivate();
			mNextMenu = 16;
		}
		if (theID == 1)
		{
			Deactivate();
			mNextMenu = 15;
		}
	}

	public override void ShowCompleted()
	{
	}

	public override void HideCompleted()
	{
		if (mNextMenu == -1)
		{
			return;
		}
		if (mNextMenu == 16)
		{
			SuperCow.RemoveScreen(base.id, clear: true);
			SuperCow.PurgeInTrash(base.id);
		}
		else
		{
			SuperCow.RemoveScreen(base.id, clear: false);
		}
		if (mNextMenu == 8)
		{
			SuperCow.GetScreen(4).Deactivate();
			SuperCow.RemoveScreen(4, clear: false);
			SuperCow.AddScreen(mNextMenu, activate: true);
			return;
		}
		if (mNextMenu == 9)
		{
			SuperCow.GetScreen(4).Deactivate();
			SuperCow.RemoveScreen(4, clear: false);
			SuperCow.AddScreen(mNextMenu, activate: true);
			return;
		}
		SuperCow.AddScreen(mNextMenu, activate: false);
		MenuWindow menuWindow = (MenuWindow)SuperCow.GetScreen(mNextMenu);
		if (!menuWindow.menuVisible)
		{
			menuWindow.Activate();
		}
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
	}
}
