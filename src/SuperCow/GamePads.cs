using Microsoft.Xna.Framework;

namespace SuperCow;

public class GamePads
{
	private GamePadHelper[] gamePads = new GamePadHelper[4]
	{
		new GamePadHelper((PlayerIndex)0),
		new GamePadHelper((PlayerIndex)1),
		new GamePadHelper((PlayerIndex)2),
		new GamePadHelper((PlayerIndex)3)
	};
	
	public GamePadHelper this[PlayerIndex player] => gamePads[(int)player];

	public void Update(Game game, GameTime updateTime)
	{
		GamePadHelper[] array = gamePads;
		foreach (GamePadHelper gamePadHelper in array)
		{
			gamePadHelper.Update(game, updateTime);
		}
	}

	public void SetDelay(int delay)
	{
		GamePadHelper[] array = gamePads;
		foreach (GamePadHelper gamePadHelper in array)
		{
			gamePadHelper.repeatDelay = delay;
		}
	}
}
