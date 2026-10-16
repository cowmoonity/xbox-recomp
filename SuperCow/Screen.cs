using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

public abstract class Screen
{
	protected Game game;

	public Game GameInstance => game;

	public Screen(Game game)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		this.game = game;
		if (game != null)
		{
			_ = (IGraphicsDeviceService)game.Services.GetService(typeof(IGraphicsDeviceService));
		}
	}

	public virtual GameState Update(TimeSpan time, TimeSpan elapsedTime)
	{
		return GameState.None;
	}

	public virtual void Render()
	{
	}

	public virtual void Shutdown()
	{
	}

	public virtual void OnCreateDevice()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		_ = (IGraphicsDeviceService)game.Services.GetService(typeof(IGraphicsDeviceService));
	}
}
