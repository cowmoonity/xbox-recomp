using Microsoft.Xna.Framework;

namespace SuperCow;

public static class XInputHelper
{
	private static GamePads gamePads = new GamePads();

	public static GamePads GamePads => gamePads;

	public static void Update(Game game, GameTime updateTime)
	{
		gamePads.Update(game, updateTime);
	}

	public static void SetDelay(int delay)
	{
		gamePads.SetDelay(delay);
	}
}
