using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

namespace SuperCow;

public class MainMenu : MenuWindow
{
	private enum WidgetList
	{
		BUTTON_START,
		BUTTON_ACHIEVEMENTS,
		BUTTON_HELPANDOPTIONS,
		BUTTON_UNLOCK,
		BUTTON_EXIT,
		WIDGET_COUNT
	}

	private int mNextMenu = -1;

	private int mNextScreen = -1;

	public MainMenu(Rectangle bounds, int speed, bool visible, bool dropDown)
		: base(bounds, speed, visible, dropDown)
	{
		string[] array = new string[5] { "Start", "Trophies", "Help and Options", "Unlock full game", "Exit game" };
		Button button = null;
		float num = bounds.Y + 20 - (bounds.Y + bounds.Height) - 300;
		for (int i = 0; i < 5; i++)
		{
			if (Guide.IsTrialMode || i != 3)
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
		mNextScreen = -1;
		mNextMenu = -1;
		if (theID == 3)
		{
			SuperCow.ShowMarketPlace();
		}
		if (theID == 4)
		{
			Deactivate();
			SCGlobals.b_exitGame = true;
		}
		if (theID == 1)
		{
			Deactivate();
			TrophyRoom.InitTrophyRoom(0, SuperCow.Settings.GetPlayerName(), ingame: false);
			mNextScreen = 10;
		}
		if (theID == 0)
		{
			Deactivate();
			if (PlayerProfile.b_fresh)
			{
				mNextScreen = 8;
			}
			else
			{
				mNextScreen = 7;
			}
		}
		if (theID == 2)
		{
			Deactivate();
			mNextMenu = 22;
		}
	}

	public override void ShowCompleted()
	{
	}

	public override void HideCompleted()
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
			return;
		}
		SuperCow.GetScreen(4).Deactivate();
		SuperCow.RemoveScreen(4, clear: false);
		if (mNextScreen == 7 || mNextScreen == 8)
		{
			SuperCow.PurgeInTrash(base.id);
		}
		SuperCow.AddScreen(mNextScreen, activate: true);
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
	}

	public override void FromDemoToFull()
	{
		if (GetChildByID(3).visible)
		{
			base.focusedChild = GetChildByID(0);
			base.widgetManager.focusedWidget = base.focusedChild;
			GetChildByID(3).visible = false;
			GetChildByID(4).y -= 30f;
			SetupOrder();
			base.FromDemoToFull();
		}
	}
}
