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
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			float result = 0f;
			if (game.IsActive && ((GamePadState)(ref state)).IsConnected)
			{
				GamePadThumbSticks thumbSticks = ((GamePadState)(ref state)).ThumbSticks;
				result = ((GamePadThumbSticks)(ref thumbSticks)).Left.X;
			}
			return result;
		}
	}

	public float ThumbStickLeftY
	{
		get
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			float result = 0f;
			if (game.IsActive && ((GamePadState)(ref state)).IsConnected)
			{
				GamePadThumbSticks thumbSticks = ((GamePadState)(ref state)).ThumbSticks;
				result = ((GamePadThumbSticks)(ref thumbSticks)).Left.Y;
			}
			return result;
		}
	}

	public float ThumbStickRightX
	{
		get
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			float result = 0f;
			if (game.IsActive && ((GamePadState)(ref state)).IsConnected)
			{
				GamePadThumbSticks thumbSticks = ((GamePadState)(ref state)).ThumbSticks;
				result = ((GamePadThumbSticks)(ref thumbSticks)).Right.X;
			}
			return result;
		}
	}

	public float ThumbStickRightY
	{
		get
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			float result = 0f;
			if (game.IsActive && ((GamePadState)(ref state)).IsConnected)
			{
				GamePadThumbSticks thumbSticks = ((GamePadState)(ref state)).ThumbSticks;
				result = ((GamePadThumbSticks)(ref thumbSticks)).Right.Y;
			}
			return result;
		}
	}

	public bool LeftTriggerPressed
	{
		get
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			GamePadTriggers triggers = ((GamePadState)(ref state)).Triggers;
			return checkPressed(((GamePadTriggers)(ref triggers)).Left, ref LeftTriggerWasReleased);
		}
	}

	public bool RightTriggerPressed
	{
		get
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			GamePadTriggers triggers = ((GamePadState)(ref state)).Triggers;
			return checkPressed(((GamePadTriggers)(ref triggers)).Right, ref RightTriggerWasReleased);
		}
	}

	public bool APressed
	{
		get
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			GamePadButtons buttons = ((GamePadState)(ref state)).Buttons;
			return checkPressed(((GamePadButtons)(ref buttons)).A, ref AWasReleased);
		}
	}

	public bool BPressed
	{
		get
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			GamePadButtons buttons = ((GamePadState)(ref state)).Buttons;
			return checkPressed(((GamePadButtons)(ref buttons)).B, ref BWasReleased);
		}
	}

	public bool YPressed
	{
		get
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			GamePadButtons buttons = ((GamePadState)(ref state)).Buttons;
			return checkPressed(((GamePadButtons)(ref buttons)).Y, ref YWasReleased);
		}
	}

	public bool XPressed
	{
		get
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			GamePadButtons buttons = ((GamePadState)(ref state)).Buttons;
			return checkPressed(((GamePadButtons)(ref buttons)).X, ref XWasReleased);
		}
	}

	public bool StartPressed
	{
		get
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			GamePadButtons buttons = ((GamePadState)(ref state)).Buttons;
			return checkPressed(((GamePadButtons)(ref buttons)).Start, ref StartWasReleased);
		}
	}

	public bool BackPressed
	{
		get
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			GamePadButtons buttons = ((GamePadState)(ref state)).Buttons;
			return checkPressed(((GamePadButtons)(ref buttons)).Back, ref BackWasReleased);
		}
	}

	public bool UpPressed
	{
		get
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			GamePadDPad dPad = ((GamePadState)(ref state)).DPad;
			return checkPressed(((GamePadDPad)(ref dPad)).Up, ref UpWasReleased);
		}
	}

	public bool DownPressed
	{
		get
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			GamePadDPad dPad = ((GamePadState)(ref state)).DPad;
			return checkPressed(((GamePadDPad)(ref dPad)).Down, ref DownWasReleased);
		}
	}

	public bool LeftPressed
	{
		get
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			GamePadDPad dPad = ((GamePadState)(ref state)).DPad;
			return checkPressed(((GamePadDPad)(ref dPad)).Left, ref LeftWasReleased);
		}
	}

	public bool RightPressed
	{
		get
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			GamePadDPad dPad = ((GamePadState)(ref state)).DPad;
			return checkPressed(((GamePadDPad)(ref dPad)).Right, ref RightWasReleased);
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		this.player = player;
	}

	private bool checkPressed(ButtonState buttonState, ref bool controlWasReleased)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
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
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		state = GamePad.GetState(player);
		this.game = game;
		if (((GamePadState)(ref state)).IsConnected)
		{
			GamePadButtons buttons = ((GamePadState)(ref state)).Buttons;
			if ((int)((GamePadButtons)(ref buttons)).A == 0)
			{
				AWasReleased = true;
			}
			GamePadButtons buttons2 = ((GamePadState)(ref state)).Buttons;
			if ((int)((GamePadButtons)(ref buttons2)).B == 0)
			{
				BWasReleased = true;
			}
			GamePadButtons buttons3 = ((GamePadState)(ref state)).Buttons;
			if ((int)((GamePadButtons)(ref buttons3)).Y == 0)
			{
				YWasReleased = true;
			}
			GamePadButtons buttons4 = ((GamePadState)(ref state)).Buttons;
			if ((int)((GamePadButtons)(ref buttons4)).X == 0)
			{
				XWasReleased = true;
			}
			GamePadButtons buttons5 = ((GamePadState)(ref state)).Buttons;
			if ((int)((GamePadButtons)(ref buttons5)).Start == 0)
			{
				StartWasReleased = true;
			}
			GamePadButtons buttons6 = ((GamePadState)(ref state)).Buttons;
			if ((int)((GamePadButtons)(ref buttons6)).Back == 0)
			{
				BackWasReleased = true;
			}
			GamePadDPad dPad = ((GamePadState)(ref state)).DPad;
			if ((int)((GamePadDPad)(ref dPad)).Up == 0)
			{
				UpWasReleased = true;
			}
			GamePadDPad dPad2 = ((GamePadState)(ref state)).DPad;
			if ((int)((GamePadDPad)(ref dPad2)).Down == 0)
			{
				DownWasReleased = true;
			}
			GamePadDPad dPad3 = ((GamePadState)(ref state)).DPad;
			if ((int)((GamePadDPad)(ref dPad3)).Left == 0)
			{
				LeftWasReleased = true;
			}
			GamePadDPad dPad4 = ((GamePadState)(ref state)).DPad;
			if ((int)((GamePadDPad)(ref dPad4)).Right == 0)
			{
				RightWasReleased = true;
			}
			GamePadTriggers triggers = ((GamePadState)(ref state)).Triggers;
			if (((GamePadTriggers)(ref triggers)).Left == 0f)
			{
				LeftTriggerWasReleased = true;
			}
			GamePadTriggers triggers2 = ((GamePadState)(ref state)).Triggers;
			if (((GamePadTriggers)(ref triggers2)).Right == 0f)
			{
				RightTriggerWasReleased = true;
			}
		}
		GamePadDPad dPad5 = ((GamePadState)(ref state)).DPad;
		UpdateRepeat(GamePadKeys.Up, ((GamePadDPad)(ref dPad5)).Up, updateTime);
		GamePadDPad dPad6 = ((GamePadState)(ref state)).DPad;
		UpdateRepeat(GamePadKeys.Down, ((GamePadDPad)(ref dPad6)).Down, updateTime);
		GamePadDPad dPad7 = ((GamePadState)(ref state)).DPad;
		UpdateRepeat(GamePadKeys.Left, ((GamePadDPad)(ref dPad7)).Left, updateTime);
		GamePadDPad dPad8 = ((GamePadState)(ref state)).DPad;
		UpdateRepeat(GamePadKeys.Right, ((GamePadDPad)(ref dPad8)).Right, updateTime);
		GamePadThumbSticks thumbSticks = ((GamePadState)(ref state)).ThumbSticks;
		UpdateRepeatThumb(GamePadKeys.ThumbstickLeftXMin, ((GamePadThumbSticks)(ref thumbSticks)).Left.X, updateTime);
		GamePadThumbSticks thumbSticks2 = ((GamePadState)(ref state)).ThumbSticks;
		UpdateRepeatThumb(GamePadKeys.ThumbstickLeftXMax, ((GamePadThumbSticks)(ref thumbSticks2)).Left.X, updateTime);
		GamePadThumbSticks thumbSticks3 = ((GamePadState)(ref state)).ThumbSticks;
		UpdateRepeatThumb(GamePadKeys.ThumbstickLeftYMin, ((GamePadThumbSticks)(ref thumbSticks3)).Left.Y, updateTime);
		GamePadThumbSticks thumbSticks4 = ((GamePadState)(ref state)).ThumbSticks;
		UpdateRepeatThumb(GamePadKeys.ThumbstickLeftYMax, ((GamePadThumbSticks)(ref thumbSticks4)).Left.Y, updateTime);
	}

	private void UpdateRepeat(GamePadKeys button, ButtonState state, GameTime updateTime)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
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
