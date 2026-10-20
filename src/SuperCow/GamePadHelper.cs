using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SuperCow;

public class GamePadHelper
{
	private PlayerIndex player;

	private KeyboardState keyState;

	private Game game;

	private bool[] repeatValues = new bool[20];

	private int[] repeatTimers = new int[20];

	private bool[] thumbRepeatValues = new bool[20];

	private int[] thumbRepeatTimers = new int[20];

	private int mRepeatDelay = 100;

	private bool AWasReleased;

	private bool BWasReleased;

	private bool YWasReleased;

	private bool XWasReleased;

	private bool StartWasReleased;

	private bool BackWasReleased;

	private bool UpWasReleased;

	private bool DownWasReleased;

	private bool LeftWasReleased;

	private bool RightWasReleased;

	private bool LeftTriggerWasReleased;

	private bool RightTriggerWasReleased;

	private bool kbAWasReleased;

	private bool kbBWasReleased;

	private bool kbYWasReleased;

	private bool kbXWasReleased;

	private bool kbStartWasReleased;

	private bool kbBackWasReleased;

	private bool kbUpWasReleased;

	private bool kbDownWasReleased;

	private bool kbLeftWasReleased;

	private bool kbRightWasReleased;

	private bool kbLeftTriggerWasReleased;

	private bool kbRightTriggerWasReleased;

	public GamePadState state;

	public int repeatDelay
	{
		set
		{
			mRepeatDelay = value;
		}
	}

	public GamePadState State => state;

	public float ThumbStickLeftX
	{
		get
		{
			float result = 0f;
			if (game.IsActive && state.IsConnected)
			{
				GamePadThumbSticks thumbSticks = state.ThumbSticks;
				result = thumbSticks.Left.X;
			}
			return result;
		}
	}

	public float ThumbStickLeftY
	{
		get
		{
			float result = 0f;
			if (game.IsActive && state.IsConnected)
			{
				GamePadThumbSticks thumbSticks = state.ThumbSticks;
				result = thumbSticks.Left.Y;
			}
			return result;
		}
	}

	public float ThumbStickRightX
	{
		get
		{
			float result = 0f;
			if (game.IsActive && state.IsConnected)
			{
				GamePadThumbSticks thumbSticks = state.ThumbSticks;
				result = thumbSticks.Right.X;
			}
			return result;
		}
	}

	public float ThumbStickRightY
	{
		get
		{
			float result = 0f;
			if (game.IsActive && state.IsConnected)
			{
				GamePadThumbSticks thumbSticks = state.ThumbSticks;
				result = thumbSticks.Right.Y;
			}
			return result;
		}
	}

	public bool LeftTriggerPressed
	{
		get
		{
			GamePadTriggers triggers = state.Triggers;
			return checkPressed(triggers.Left, ref LeftTriggerWasReleased);
		}
	}

	public bool RightTriggerPressed
	{
		get
		{
			GamePadTriggers triggers = state.Triggers;
			return checkPressed(triggers.Right, ref RightTriggerWasReleased);
		}
	}

	public bool APressed
	{
		get
		{
			GamePadButtons buttons = state.Buttons;
			return checkPressed(buttons.A, ref AWasReleased);
		}
	}

	public bool BPressed
	{
		get
		{
			GamePadButtons buttons = state.Buttons;
			return checkPressed(buttons.B, ref BWasReleased);
		}
	}

	public bool YPressed
	{
		get
		{
			GamePadButtons buttons = state.Buttons;
			return checkPressed(buttons.Y, ref YWasReleased);
		}
	}

	public bool XPressed
	{
		get
		{
			GamePadButtons buttons = state.Buttons;
			return checkPressed(buttons.X, ref XWasReleased);
		}
	}

	public bool StartPressed
	{
		get
		{
			GamePadButtons buttons = state.Buttons;
			return checkPressed(buttons.Start, ref StartWasReleased);
		}
	}

	public bool BackPressed
	{
		get
		{
			GamePadButtons buttons = state.Buttons;
			return checkPressed(buttons.Back, ref BackWasReleased);
		}
	}

	public bool UpPressed
	{
		get
		{
			GamePadDPad dPad = state.DPad;
			return checkPressed(dPad.Up, ref UpWasReleased);
		}
	}

	public bool DownPressed
	{
		get
		{
			GamePadDPad dPad = state.DPad;
			return checkPressed(dPad.Down, ref DownWasReleased);
		}
	}

	public bool LeftPressed
	{
		get
		{
			GamePadDPad dPad = state.DPad;
			return checkPressed(dPad.Left, ref LeftWasReleased);
		}
	}

	public bool RightPressed
	{
		get
		{
			GamePadDPad dPad = state.DPad;
			return checkPressed(dPad.Right, ref RightWasReleased);
		}
	}

	public bool UpPressedRepeat => repeatValues[6];

	public bool DownPressedRepeat => repeatValues[7];

	public bool LeftPressedRepeat => repeatValues[8];

	public bool RightPressedRepeat => repeatValues[9];

	public bool ThumbUpPressedRepeat => thumbRepeatValues[15];

	public bool ThumbDownPressedRepeat => thumbRepeatValues[14];

	public bool ThumbLeftPressedRepeat => thumbRepeatValues[12];

	public bool ThumbRightPressedRepeat => thumbRepeatValues[13];

	public GamePadHelper(PlayerIndex player)
	{
		this.player = player;
	}

	private bool checkPressed(ButtonState buttonState, ref bool controlWasReleased)
	{
		return checkPressed((int)buttonState == 1, ref controlWasReleased);
	}

	private bool checkPressed(float triggerState, ref bool controlWasReleased)
	{
		return checkPressed(triggerState > 0f, ref controlWasReleased);
	}

	private bool checkPressed(bool pressed, ref bool controlWasReleased)
	{
		bool flag = controlWasReleased && pressed;
		if (game.IsActive)
		{
			if (flag)
			{
				controlWasReleased = false;
			}
			return flag;
		}
		return false;
	}

	public void Update(Game game, GameTime updateTime)
	{
		state = GamePad.GetState(player);
		this.game = game;
		if (state.IsConnected)
		{
			GamePadButtons buttons = state.Buttons;
			if (buttons.A == 0)
			{
				AWasReleased = true;
			}
			GamePadButtons buttons2 = state.Buttons;
			if (buttons2.B == 0)
			{
				BWasReleased = true;
			}
			GamePadButtons buttons3 = state.Buttons;
			if (buttons3.Y == 0)
			{
				YWasReleased = true;
			}
			GamePadButtons buttons4 = state.Buttons;
			if (buttons4.X == 0)
			{
				XWasReleased = true;
			}
			GamePadButtons buttons5 = state.Buttons;
			if (buttons5.Start == 0)
			{
				StartWasReleased = true;
			}
			GamePadButtons buttons6 = state.Buttons;
			if (buttons6.Back == 0)
			{
				BackWasReleased = true;
			}
			GamePadDPad dPad = state.DPad;
			if (dPad.Up == 0)
			{
				UpWasReleased = true;
			}
			GamePadDPad dPad2 = state.DPad;
			if (dPad2.Down == 0)
			{
				DownWasReleased = true;
			}
			GamePadDPad dPad3 = state.DPad;
			if (dPad3.Left == 0)
			{
				LeftWasReleased = true;
			}
			GamePadDPad dPad4 = state.DPad;
			if (dPad4.Right == 0)
			{
				RightWasReleased = true;
			}
			GamePadTriggers triggers = state.Triggers;
			if (triggers.Left == 0f)
			{
				LeftTriggerWasReleased = true;
			}
			GamePadTriggers triggers2 = state.Triggers;
			if (triggers2.Right == 0f)
			{
				RightTriggerWasReleased = true;
			}
		}
		GamePadDPad dPad5 = state.DPad;
		UpdateRepeat(GamePadKeys.Up, dPad5.Up, updateTime);
		GamePadDPad dPad6 = state.DPad;
		UpdateRepeat(GamePadKeys.Down, dPad6.Down, updateTime);
		GamePadDPad dPad7 = state.DPad;
		UpdateRepeat(GamePadKeys.Left, dPad7.Left, updateTime);
		GamePadDPad dPad8 = state.DPad;
		UpdateRepeat(GamePadKeys.Right, dPad8.Right, updateTime);
		GamePadThumbSticks thumbSticks = state.ThumbSticks;
		UpdateRepeatThumb(GamePadKeys.ThumbstickLeftXMin, thumbSticks.Left.X, updateTime);
		GamePadThumbSticks thumbSticks2 = state.ThumbSticks;
		UpdateRepeatThumb(GamePadKeys.ThumbstickLeftXMax, thumbSticks2.Left.X, updateTime);
		GamePadThumbSticks thumbSticks3 = state.ThumbSticks;
		UpdateRepeatThumb(GamePadKeys.ThumbstickLeftYMin, thumbSticks3.Left.Y, updateTime);
		GamePadThumbSticks thumbSticks4 = state.ThumbSticks;
		UpdateRepeatThumb(GamePadKeys.ThumbstickLeftYMax, thumbSticks4.Left.Y, updateTime);
	}

	private void UpdateRepeat(GamePadKeys button, ButtonState state, GameTime updateTime)
	{
		if ((int)state == 1)
		{
			repeatTimers[(int)button] += updateTime.ElapsedGameTime.Milliseconds;
			if (repeatTimers[(int)button] > mRepeatDelay)
			{
				repeatTimers[(int)button] = 0;
				repeatValues[(int)button] = true;
			}
			else
			{
				repeatValues[(int)button] = false;
			}
		}
		else
		{
			repeatTimers[(int)button] = 0;
			repeatValues[(int)button] = false;
		}
	}

	private void UpdateRepeatThumb(GamePadKeys button, float state, GameTime updateTime)
	{
		if ((button == GamePadKeys.ThumbstickLeftXMin && state < -0.35f) || (button == GamePadKeys.ThumbstickLeftXMax && state > 0.35f) || (button == GamePadKeys.ThumbstickLeftYMax && state > 0.35f) || (button == GamePadKeys.ThumbstickLeftYMin && state < -0.35f))
		{
			thumbRepeatTimers[(int)button] += updateTime.ElapsedGameTime.Milliseconds;
			if (thumbRepeatTimers[(int)button] > mRepeatDelay)
			{
				thumbRepeatTimers[(int)button] = 0;
				thumbRepeatValues[(int)button] = true;
			}
			else
			{
				thumbRepeatValues[(int)button] = false;
			}
		}
		else
		{
			thumbRepeatTimers[(int)button] = 0;
			thumbRepeatValues[(int)button] = false;
		}
	}
}
