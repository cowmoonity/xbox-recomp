using Microsoft.Xna.Framework;
using SuperCow.Properties;

namespace SuperCow;

internal class GameSettingsMenu : MenuWindow
{
	private enum WidgetList
	{
		SOUND_SLIDER,
		MUSIC_SLIDER,
		BUTTON_BACK,
		WIDGET_COUNT
	}

	private int mNextMenu = -1;

	public GameSettingsMenu(Rectangle bounds, int speed, bool visible, bool dropDown)
		: base(bounds, speed, visible, dropDown)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		string[] array = new string[1] { "Back" };
		Button button = null;
		float num = bounds.Height - 70;
		button = new Button(2, this)
		{
			text = array[0],
			x = 0f,
			y = bounds.Y - 50,
			font = 0,
			fontSize = 20f
		};
		num += 30f;
		AddChild(button);
		base.backChild = button;
		AddChild(new Slider(0, this)
		{
			text = "Sound volume",
			x = 0f,
			y = bounds.Y - 60,
			font = 0,
			fontSize = 15f,
			sliderWidth = 200f,
			sliderValue = (int)(GameSettings.Default.SoundVolume * 10f)
		});
		AddChild(new Slider(1, this)
		{
			text = "Music volume",
			x = 0f,
			y = bounds.Y - 60 - 60,
			font = 0,
			fontSize = 15f,
			sliderWidth = 200f,
			sliderValue = (int)(GameSettings.Default.MusicVolume * 10f)
		});
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
		SuperCow.Settings.SaveSettings();
		Collapse();
	}

	public override void ButtonPressed(int theID)
	{
		if (theID == 2)
		{
			Deactivate();
			if (SCGlobals.b_gamemenu)
			{
				mNextMenu = 17;
			}
			else
			{
				mNextMenu = 22;
			}
		}
	}

	public override void JoyMove(GamePadKeys button, int theID)
	{
		if (theID == 0)
		{
			Slider slider = (Slider)GetChildByID(0);
			GameSettings.Default.SoundVolume = (float)slider.sliderValue / 10f;
			Sound.SetSoundVolume(GameSettings.Default.SoundVolume);
			Sound.Addsound("menu");
		}
		if (theID == 1)
		{
			Slider slider2 = (Slider)GetChildByID(1);
			GameSettings.Default.MusicVolume = (float)slider2.sliderValue / 10f;
			Sound.SetMusicVolume(GameSettings.Default.MusicVolume);
		}
		base.JoyMove(button, theID);
	}

	public override void Uncollapsed()
	{
	}

	public override void Collapsed()
	{
		SuperCow.RemoveScreen(base.id, clear: true);
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

	public override void FromDemoToFull()
	{
		if (SCGlobals.b_gamemenu)
		{
			SuperCow.RemoveScreen(base.id, clear: true);
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
