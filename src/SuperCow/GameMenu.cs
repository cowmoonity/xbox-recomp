using Microsoft.Xna.Framework;

namespace SuperCow;

internal class GameMenu : MenuWindow
{
	private enum WidgetList
	{
		BUTTON_RESUME,
		BUTTON_TASK,
		BUTTON_OPTIONS,
		BUTTON_MAINMENU,
		WIDGET_COUNT
	}

	private int mNextMenu = -1;

	public GameMenu(Rectangle bounds, int speed, bool visible, bool dropDown)
		: base(bounds, speed, visible, dropDown)
	{
		string[] array = new string[4] { "Resume game", "Task", "Options", "Return to map" };
		Button button = null;
		float num = bounds.Y + 20 - (bounds.Y + bounds.Height) - 300;
		for (int i = 0; i < 4; i++)
		{
			button = new Button(i, this)
			{
				text = array[i],
				x = bounds.X - bounds.Width / 2,
				y = num,
				font = 0,
				fontSize = 20f
			};
			num += 30f;
			AddChild(button);
			if (i == 0)
			{
				base.backChild = button;
			}
		}
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
		if (theID == 0)
		{
			Deactivate();
		}
		if (theID == 1)
		{
			Deactivate();
			mNextMenu = 18;
		}
		if (theID == 2)
		{
			Deactivate();
			mNextMenu = 15;
		}
		if (theID == 3)
		{
			Deactivate();
			mNextMenu = 7;
		}
	}

	public override void ShowCompleted()
	{
	}

	public override void HideCompleted()
	{
		if (mNextMenu != -1)
		{
			SuperCow.RemoveScreen(base.id, clear: false);
			if (mNextMenu == 7)
			{
				SuperCow.RemoveScreen(11, clear: true);
				SuperCow.PurgeInTrash(base.id);
				SCGlobals.supercowgame.FreeGame();
				SCGlobals.supercowgame.StopMusic();
				SCGlobals.supercowgame = null;
				SuperCow.AddScreen(7, activate: true);
			}
			else
			{
				SuperCow.AddScreen(mNextMenu, activate: false);
				MenuWindow menuWindow = (MenuWindow)SuperCow.GetScreen(mNextMenu);
				if (!menuWindow.menuVisible)
				{
					menuWindow.Activate();
				}
			}
		}
		else
		{
			SuperCow.RemoveScreen(base.id, clear: true);
			SuperCow.PurgeInTrash(base.id);
			SCGlobals.b_gamemenu = false;
			SCGlobals.b_pause = false;
		}
	}

	public override void FromDemoToFull()
	{
		if (SCGlobals.b_gamemenu)
		{
			SuperCow.RemoveScreen(base.id, clear: true);
			SuperCow.PurgeInTrash(base.id);
			SCGlobals.b_gamemenu = false;
			SCGlobals.b_pause = false;
			SCGlobals.supercowgame.FromDemoToFull();
			SCGlobals.supercowgame.FreeGame();
			SCGlobals.supercowgame = null;
		}
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
	}
}
