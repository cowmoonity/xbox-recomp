using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SuperCow;

internal class SplashScreen : AbstractScreen
{
	private float mAlpha = 255f;

	private float mTimer;

	private bool mFinished;

	private bool mSkipSplash;

	private float mInitialTimer = 3000f;

	private int mCurrentScreen;

	private List<texture_t> mScreens;

	public SplashScreen()
	{
		Store.SetCurrentDirectory("", theAdd: false);
		mScreens = new List<texture_t>();
		mScreens.Add(SCGlobals.texturehost.Load("splash1.jpg"));
		mScreens.Add(SCGlobals.texturehost.Load("splash2.png"));
	}

	public override void ShutDown()
	{
		base.ShutDown();
		mScreens.Clear();
	}

	public override void Draw()
	{
		CowRender.SetTextMode();
		CowRender.DisableRS(7u);
		CowRender.BindTexture(mScreens[mCurrentScreen]);
		CowRender.DrawColorQuad2D(mScreens[mCurrentScreen].mImage, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)mAlpha), 10f);
		if (mCurrentScreen + 1 < mScreens.Count)
		{
			CowRender.BindTexture(mScreens[mCurrentScreen + 1]);
			CowRender.DrawColorQuad2D(mScreens[mCurrentScreen + 1].mImage, new dcolor_t(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255 - mAlpha)), 10f);
		}
		CowRender.EnableRS(7u);
		base.Draw();
	}

	public override void Update(GameTime gameTime)
	{
		if (mFinished)
		{
			return;
		}
		if (mSkipSplash)
		{
			SuperCow.AddScreen(3, new LoadingScreen());
			mFinished = true;
			return;
		}
		if (mInitialTimer > 0f)
		{
			mInitialTimer -= gameTime.ElapsedGameTime.Milliseconds;
			return;
		}
		mTimer += gameTime.ElapsedGameTime.Milliseconds;
		float num = 1f - mTimer / 3000f;
		if (num < 0f)
		{
			num = 0f;
		}
		mAlpha = num * num * 255f;
		if (!(mTimer >= 3000f))
		{
			return;
		}
		if (mCurrentScreen + 1 < mScreens.Count)
		{
			mCurrentScreen++;
			mTimer = 0f;
			mAlpha = 255f;
			if (mCurrentScreen + 1 == mScreens.Count)
			{
				SuperCow.AddScreen(3, new LoadingScreen());
			}
		}
		else
		{
			mFinished = true;
		}
	}
}
