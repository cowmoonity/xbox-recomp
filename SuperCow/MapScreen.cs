using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCow;

internal class MapScreen : AbstractScreen, ThreadInterface
{
	private enum WidgetList
	{
		BUTTON_CONTINUE,
		BUTTON_BACK,
		WIDGET_COUNT
	}

	private texture_t mBackground;

	private bool mLoaded;

	private bool mLoadingInProgress;

	private WindowRect windowRect = new WindowRect();

	private Cue mMusic;

	public void ThreadFinished()
	{
	}

	public void ThreadFinished(int taskId)
	{
		SCGlobals.supercowgame.InitLevel(0);
		mLoadingInProgress = false;
		mLoaded = true;
		SCGlobals.gLevelLoading = false;
		GameMap.b_gm_start = false;
		Deactivate();
		SuperCow.RemoveScreen(base.id, clear: false);
		SuperCow.AddScreen(11, activate: true);
		SuperCow.KillLevelLoadingThread();
	}

	public void ThreadProc()
	{
		mLoadingInProgress = true;
		SCGlobals.supercowgame = new SuperCowGame();
		SCGlobals.supercowgame.GameStartup();
		SCGlobals.supercowgame.InitLevelThread();
	}

	public MapScreen()
	{
		Store.SetCurrentDirectory("gamemap", theAdd: false);
		mBackground = SCGlobals.texturehost.Load("megamap");
	}

	public override void Draw()
	{
		if (mLoadingInProgress)
		{
			CowRender.SetBlend(0);
			windowRect.Draw();
			Text.SetTextColor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			CowRender.SetTextMode();
			string str = "Loading...";
			Text.OutTextXY(0f, 0f, 16f, str, b_center: true);
		}
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
	}

	public override void Activate()
	{
		GameMap.gmActivate();
		base.Activate();
		mMusic = Sound.Play(Sounds.map);
		windowRect.SetRect(new frect_t(-100f, 50f, 200f, 100f));
	}

	public override void Deactivate()
	{
		base.Deactivate();
		Sound.Stop(mMusic);
	}

	public override void ButtonPressed(int theID)
	{
		switch (theID)
		{
		case 1:
			Deactivate();
			SuperCow.RemoveScreen(base.id, clear: true);
			SuperCow.AddScreen(4, activate: true);
			SuperCow.AddScreen(16, activate: true);
			break;
		case 0:
			Deactivate();
			SuperCow.RemoveScreen(base.id, clear: true);
			SuperCow.AddScreen(11, activate: true);
			break;
		}
	}

	public override void JoyPress(GamePadKeys button, int theID)
	{
		if (!mLoadingInProgress)
		{
			GameMap.gmSetCommand((uint)button);
			base.JoyPress(button, theID);
		}
	}

	public override void JoyMoveRepeat(GamePadKeys button, int theID)
	{
		if (!mLoadingInProgress)
		{
			GameMap.gmSetCommand((uint)button);
		}
	}

	public override void DrawGeometry()
	{
		GraphicsDevice graphicsDevice = SuperCow.DeviceManager.GraphicsDevice;
		CowRender.EnableRS(4u);
		CowRender.SetBlend(0);
		CowRender.DisableRS(1u);
		CowRender.EnableRS(5u);
		graphicsDevice.RenderState.ReferenceAlpha = 1;
		CowRender.SetLighting(theEnable: false);
		CowRender.SetOrtho(800f, 600f, 1f, 300f);
		SCGlobals.cam.load_identity();
		SCGlobals.cam.translate(0f, 0f, 150f);
		CowRender.LoadMatrixNull();
		CowRender.LoadCamera(SCGlobals.cam);
		SCGlobals.frustum.Calculate();
		graphicsDevice.RenderState.CullMode = (CullMode)1;
		GameMap.gmCalculate();
		GameMap.gmDraw();
		Balloon.DrawBalloonRects();
		Particles.CalculateParticles();
		Particles.DrawParticles();
		CowRender.DisableRS(5u);
		CowRender.SetBlend(0);
		base.DrawGeometry();
	}

	public override void FromDemoToFull()
	{
		Deactivate();
		SuperCow.RemoveScreen(base.id, clear: true);
		SuperCow.AddScreen(4, activate: true);
		SuperCow.AddScreen(16, activate: true);
		SuperCow.AddScreen(6, activate: true);
		base.FromDemoToFull();
	}
}
